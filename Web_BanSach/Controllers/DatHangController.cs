using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_BanSach.Models;
using static Web_BanSach.Models.CartItem;

namespace Web_BanSach.Controllers
{
    public class DatHangController : Controller
    {
        private QL_SACHEntities db = new QL_SACHEntities();

        // 🔥 Lấy hóa đơn tạm hoặc tạo mới
        private HOADON GetOrCreateHoaDon()
        {
            int makhachhang = 1; // Tạm thời — sau dùng session đăng nhập

            var hd = db.HOADONs.FirstOrDefault(h =>
                h.MAKHACHHANG == makhachhang && h.MATINHTRANG == 6);

            if (hd == null)
            {
                hd = new HOADON
                {
                    MAKHACHHANG = makhachhang,
                    MANHANVIEN = null,
                    NGAYLAP = DateTime.Now,
                    DIACHI = "",
                    TONGTIEN = 0,
                    MATINHTRANG = 6 // trạng thái giỏ hàng (chờ thanh toán)
                };

                db.HOADONs.Add(hd);
                db.SaveChanges();
            }

            return hd;
        }

        // ==========================
        // GIỎ HÀNG HIỂN THỊ
        // ==========================
        public ActionResult Index()
        {
            var gioHang = Session["GioHang"] as CartItem.GioHang;
            if (gioHang == null)
            {
                gioHang = new CartItem.GioHang();
            }

            var model = new GioHangViewModel
            {
                Items = gioHang.items,
                TongTien = gioHang.TongTien()
            };

            return View(model);
        }

        // ==========================
        // THÊM SẢN PHẨM + LƯU SQL
        // ==========================
        public ActionResult ThemMatHang(int masach)
        {
            // ─ GIỮ SESSION GIỐNG BẢN GỐC ─
            CartItem.GioHang gioHang = Session["GioHang"] as CartItem.GioHang;
            if (gioHang == null) gioHang = new CartItem.GioHang();

            var sach = db.BANGSACHes.FirstOrDefault(s => s.MASACH == masach);
            if (sach != null)
            {
                gioHang.ThemSP(sach, 1);
                Session["GioHang"] = gioHang;
            }

            // ─ LƯU SQL ─
            var hd = GetOrCreateHoaDon();

            var ct = db.CHITIET_HOADON.FirstOrDefault(c =>
                c.MAHOADON == hd.MAHOADON && c.MASACH == masach);

            if (ct == null)
            {
                ct = new CHITIET_HOADON
                {
                    MAHOADON = hd.MAHOADON,
                    MASACH = masach,
                    SOLUONG = 1,
                    DONGIA = sach.GIABAN
                };
                db.CHITIET_HOADON.Add(ct);
            }
            else
            {
                ct.SOLUONG += 1;
            }

            hd.TONGTIEN = db.CHITIET_HOADON
                .Where(c => c.MAHOADON == hd.MAHOADON)
                .Sum(c => c.SOLUONG * c.DONGIA);

            db.SaveChanges();

            return RedirectToAction("Index", "DatHang");
        }

        // ==========================
        // CẬP NHẬT SỐ LƯỢNG
        // ==========================
        [HttpPost]
        public ActionResult CapNhatMatHang(int masach, int soluong)
        {
            CartItem.GioHang gioHang = Session["GioHang"] as CartItem.GioHang;
            if (gioHang == null) return RedirectToAction("Index");

            var item = gioHang.items.FirstOrDefault(i => i.sach.MASACH == masach);

            if (item != null)
            {
                if (soluong > 0)
                    item.soluong = soluong;
                else
                    gioHang.items.Remove(item);

                Session["GioHang"] = gioHang;
            }

            // ─ LƯU SQL ─
            var hd = GetOrCreateHoaDon();
            var ct = db.CHITIET_HOADON.FirstOrDefault(c => c.MAHOADON == hd.MAHOADON && c.MASACH == masach);

            if (ct != null)
            {
                if (soluong > 0)
                {
                    ct.SOLUONG = soluong;
                }
                else
                {
                    db.CHITIET_HOADON.Remove(ct);
                }

                hd.TONGTIEN = db.CHITIET_HOADON
                    .Where(c => c.MAHOADON == hd.MAHOADON)
                    .Sum(c => c.SOLUONG * c.DONGIA);

                db.SaveChanges();
            }

            return RedirectToAction("Index", "DatHang");
        }

        // ==========================
        // XÓA SẢN PHẨM
        // ==========================
        public ActionResult XoaMatHang(int masach)
        {
            CartItem.GioHang gioHang = Session["GioHang"] as CartItem.GioHang;
            if (gioHang == null) return RedirectToAction("Index");

            var item = gioHang.items.FirstOrDefault(i => i.sach.MASACH == masach);
            if (item != null)
            {
                gioHang.items.Remove(item);
                Session["GioHang"] = gioHang;
            }

            // ─ LƯU SQL ─
            var hd = GetOrCreateHoaDon();
            var ct = db.CHITIET_HOADON.FirstOrDefault(c => c.MAHOADON == hd.MAHOADON && c.MASACH == masach);

            if (ct != null)
            {
                db.CHITIET_HOADON.Remove(ct);

                hd.TONGTIEN = db.CHITIET_HOADON
                    .Where(c => c.MAHOADON == hd.MAHOADON)
                    .Sum(c => c.SOLUONG * c.DONGIA);

                db.SaveChanges();
            }

            return RedirectToAction("Index", "DatHang");
        }

        // ==========================
        // THANH TOÁN THẬT SỰ
        // ==========================
        [HttpPost]
        public ActionResult ThanhCong(string diachi)
        {
            var hd = GetOrCreateHoaDon();

            hd.DIACHI = diachi;
            hd.MATINHTRANG = 5; // ĐÃ XÁC NHẬN ĐƠN
            db.SaveChanges();

            Session["GioHang"] = new CartItem.GioHang();

            return View();
        }
    }
}

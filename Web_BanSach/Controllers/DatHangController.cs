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
        // GET: DatHang
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
        public ActionResult ThanhCong() {
            return View();
        }
        public ActionResult ThemMatHang(int masach)
        {
            // Lấy giỏ hàng hiện tại từ Session
            CartItem.GioHang gioHang = Session["GioHang"] as CartItem.GioHang;

            // Nếu chưa có giỏ hàng thì tạo mới
            if (gioHang == null)
            {
                gioHang = new CartItem.GioHang();
            }

            var sach = db.BANGSACHes.FirstOrDefault(s => s.MASACH == masach);
            if (sach != null)
            {
                gioHang.ThemSP(sach, 1);
            }

            // Cập nhật lại session
            Session["GioHang"] = gioHang;
            return RedirectToAction("Index", "DatHang");
        }
        public ActionResult XemMatHang()
        {
            CartItem.GioHang gh = (CartItem.GioHang)Session["GioHang"];
            return View(gh);
        }
        //Cập nhật số lượng
        [HttpPost]
        public ActionResult CapNhatMatHang(int masach, int soluong)
        {
            GioHang gioHang = Session["GioHang"] as GioHang;
            if (gioHang == null)
            {
                return RedirectToAction("Index", "DatHang");
            }
            var item = gioHang.items.FirstOrDefault(i => i.sach != null && i.sach.MASACH == masach);
            if (item != null)
            {
                if (soluong > 0)
                {
                    item.soluong = soluong;
                }
                else
                {
                    gioHang.items.Remove(item);
                }
                Session["GioHang"] = gioHang;
            }

            return RedirectToAction("Index", "DatHang");
        }
        //Xóa 1 sản phẩm khỏi giỏ
        public ActionResult XoaMatHang(int masach)
        {
            GioHang gioHang = Session["GioHang"] as GioHang;
            if (gioHang == null)
            {
                return RedirectToAction("Index", "DatHang");
            }

            var item = gioHang.items.FirstOrDefault(i => i.sach != null && i.sach.MASACH == masach);
            if (item != null)
            {
                gioHang.items.Remove(item);
                Session["GioHang"] = gioHang;
            }

            return RedirectToAction("Index", "DatHang");
        }
    }
}
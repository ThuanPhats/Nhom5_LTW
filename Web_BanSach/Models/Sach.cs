using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_BanSach.Controllers;

namespace Web_BanSach.Models
{
    public class Sach
    {
        public virtual LOAISACH LOAISACH { get; set; }
        public virtual NHAXUATBAN NHAXUATBAN { get; set; }
    }

    public class Saches : BANGSACH
    {
        public List<LOAISACH> lstloai { get; set; }
        public List<NHAXUATBAN> lstNXB { get; set; }
    }
    public class GioHangViewModel
    {
        public List<CartItem> Items { get; set; }
        public double TongTien { get; set; }
    }

    public class CartItem
    {
        public BANGSACH sach { get; set; }
        public int soluong { get; set; }
        public double thanhtien
        {
            get
            {
                return sach != null ? soluong * Convert.ToDouble(sach.GIABAN) : 0;
            }
        }
        public CartItem(BANGSACH s, int sl)
        {
            this.sach = s;
            this.soluong = sl;
        }

        public class GioHang
        {
            public List<HOADON> lst;
            public List<CartItem> items = new List<CartItem>();

            //Tạo giỏ hàng
            public GioHang()
            {
                lst = new List<HOADON>();
            }

            //Thêm sản phẩm
            public void ThemSP(BANGSACH s, int soluong)
            {
                var item = items.Find(m => m.sach.MASACH == s.MASACH);
                if (item == null)
                {
                    items.Add(new CartItem(s, soluong));
                }
                else
                {
                    item.soluong += soluong;
                }
            }

            //Lấy 1 dòng hàng theo mã
            public CartItem LayItem(int masach)
            {
                return items.FirstOrDefault(m => m.sach != null && m.sach.MASACH == masach);
            }

            //Cập nhật số lượng (soluong <= 0 => xóa)
            public void CapNhat(int masach, int soluong)
            {
                var it = LayItem(masach);
                if (it == null) return;

                if (soluong <= 0)
                    items.Remove(it);
                else
                    it.soluong = soluong;
            }

            //Xóa sản phẩm
            public void Xoa(int masach)
            {
                var it = LayItem(masach);
                if (it != null) items.Remove(it);
            }

            //Tính tổng tiền
            public double TongTien()
            {
                return items.Sum(m => m.thanhtien);
            }

            //Tính số mặt hàng (số dòng)
            public int TongSoLuong()
            {
                return items.Count;
            }

            //Tổng số lượng (cộng dồn)
            public int TongSoLuongHang()
            {
                return items.Sum(m => m.soluong);
            }
        }
    }
}
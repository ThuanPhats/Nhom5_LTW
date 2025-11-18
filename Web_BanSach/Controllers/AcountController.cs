using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Web_BanSach.Controllers
{
    public class AcountController : Controller
    {
        QL_SACHEntities ql = new QL_SACHEntities();
        public ActionResult Login()
        {
            ViewBag.Url = !String.IsNullOrEmpty(Request.UrlReferrer.ToString()) ? Request.UrlReferrer.ToString() : "/";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XuLyFormDN(FormCollection f, string duongdan)
        {
            string email = f["email"];
            string pass = f["password"];

            // Kiểm tra khách hàng
            var kh = ql.KHACHHANGs.FirstOrDefault(k => k.EMAIL == email && k.MATKHAU == pass);
            if (kh != null)
            {
                FormsAuthentication.SetAuthCookie(kh.TENKHACHHANG, true);
                Session["TEN"] = kh.TENKHACHHANG;
                Session["LOAI_TK"] = "KHACHHANG";
                Session["MA"] = kh.MAKHACHHANG;
                return Redirect(duongdan);
            }

            // Kiểm tra nhân viên
            var nv = ql.NHANVIENs.FirstOrDefault(n => n.EMAIL == email && n.MATKHAU == pass);
            if (nv != null)
            {
                FormsAuthentication.SetAuthCookie(nv.TENNHANVIEN, true);
                Session["TEN"] = nv.TENNHANVIEN;
                Session["LOAI_TK"] = "NHANVIEN";
                Session["MA"] = nv.MANHANVIEN;
                Session["VAITRO"] = nv.MAVAITRO; // lưu vai trò vào session
                if (nv.MAVAITRO == 1) // Admin
                {
                    // Redirect vào Area Admin, controller Index
                    return RedirectToAction("Index", "Sach", new { area = "Admin" });
                }
                else
                {
                    // URL chuẩn của trang Register
                    string registerUrl = Url.Action("Register", "Account");
                    // Kiểm tra nếu đường dẫn cũ là trang đăng ký
                    if (!string.IsNullOrEmpty(duongdan) && string.Equals(duongdan, registerUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        // Nếu đường dẫn cũ là đăng ký thì redirect về Index sách
                        return RedirectToAction("Index", "Sach");
                    }
                    else
                    {
                        // Ngược lại vẫn redirect về đường dẫn cũ
                        return Redirect(duongdan);
                    }
                }    
            }

            // Sai thông tin
            ViewBag.Loi = "Email hoặc mật khẩu không đúng";
            return View("Login");
        }

        // GET: Hiển thị form đăng ký
        public ActionResult Register()
        {
            ViewBag.Url = !String.IsNullOrEmpty(Request.UrlReferrer.ToString()) ? Request.UrlReferrer.ToString() : "/";
            return View();
        }

        // POST: Xử lý form đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(FormCollection f)
        {
            string ten = f["TENKHACHHANG"];
            string pass = f["MATKHAU"];
            string sdt = f["SDT"];
            string email = f["EMAIL"];
            string diachi = f["DIACHI"];

            // Kiểm tra email đã tồn tại chưa
            var check = ql.KHACHHANGs.FirstOrDefault(k => k.EMAIL == email);
            if (check != null)
            {
                ViewBag.Loi = "Email này đã được sử dụng!";
                return View();
            }

            // Tạo đối tượng khách hàng mới
            KHACHHANG kh = new KHACHHANG();
            kh.TENKHACHHANG = ten;
            kh.EMAIL = email;
            kh.MATKHAU = pass;
            kh.SDT = sdt;
            kh.DIACHI = diachi;

            ql.KHACHHANGs.Add(kh);
            ql.SaveChanges();

            ViewBag.ThongBao = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";
            return RedirectToAction("Login");
        }



        public ActionResult DangXuat()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Sach", new { area = "" });
            //có thể dùng cách dưới
            //return Redirect("/");
        }

        // GET: Acount

        //////////



    }
}
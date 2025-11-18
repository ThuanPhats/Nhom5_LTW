
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_BanSach.Areas.Admin.Controllers
{
    public class LienHeController : Controller
    {
        // GET: Admin/LienHe
        public ActionResult Index()
        {
            ViewBag.TieuDe = "Thông tin liên Hệ của nhóm 5";
            ViewBag.Message = "Web bán sách trực tuyến được thực hiện bởi nhóm 5.";
            List<string> list = new List<string>();
            list.Add("1. Nguyễn Tuấn Anh- 20120001 - 0123456789");
            list.Add("2. La thuận Phát- 20120002 - 0987654321");
            list.Add("3. Nguyễn Phúc - 20120003 - 0912345678");
            ViewBag.Nhom5 = list;
            return View();
        }
    }
}
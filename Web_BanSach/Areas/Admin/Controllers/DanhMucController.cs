using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_BanSach.Areas.Admin.Controllers
{
    public class DanhMucController : Controller
    {
        // GET: Admin/DanhMuc
        QL_SACHEntities sach = new QL_SACHEntities();
        public PartialViewResult _DanhMuc()
        {
            return PartialView();
        }

        public PartialViewResult _DanhMucTL()
        {
            List<LOAISACH> tl = sach.LOAISACHes.ToList();
            return PartialView(tl);
        }

        public PartialViewResult _DanhMucNXB()
        {
            List<NHAXUATBAN> nxb = sach.NHAXUATBANs.ToList();
            return PartialView(nxb);
        }
        public PartialViewResult _DanhMucTimKiem()
        {
            List<LOAISACH> tk = sach.LOAISACHes.ToList();
            return PartialView(tk);
        }
    }
}
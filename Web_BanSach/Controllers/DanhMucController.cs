using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_BanSach.Controllers
{
    public class DanhMucController : Controller
    {
        // GET: DanhMuc
        QL_SACHEntities sach = new QL_SACHEntities();
        public ActionResult _DanhMuc()
        {
            return PartialView();
        }

        public ActionResult _DanhMucTL()
        {
            List<LOAISACH> tl = sach.LOAISACHes.ToList();
            return PartialView(tl);
        }
 
        public ActionResult _DanhMucNXB()
        {
            List<NHAXUATBAN> nxb = sach.NHAXUATBANs.ToList();
            return PartialView(nxb);
        }
        public ActionResult _DanhMucTimKiem()
        {
            List<LOAISACH> tk = sach.LOAISACHes.ToList();
            return PartialView(tk);
        }
    }
}
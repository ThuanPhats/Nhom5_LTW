using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_BanSach.Areas.Admin.Controllers
{
    public class NhaXuatBanController : Controller
    {
        // GET: Admin/NhaXuatBan
        QL_SACHEntities nxb = new QL_SACHEntities();

        // GET: NhaXuatBan
        public ActionResult Index()
        {
            List<NHAXUATBAN> dsnxb = nxb.NHAXUATBANs.ToList();
            return View(dsnxb);
        }
        //GET: NhaXuatBan/thêm NXB
        public ActionResult ThemNXB()
        {
            return View();
        }
        public ActionResult ViewThemNXB(NHAXUATBAN nhaxuatban)
        {
            nxb.NHAXUATBANs.Add(nhaxuatban);
            nxb.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: NhaXuatBan/xoá loại sách
        public ActionResult ViewDelete(int nxb_id)
        {
            var xoa = nxb.NHAXUATBANs.FirstOrDefault(m => m.MANXB == nxb_id);
            return View(xoa);
        }

        [HttpPost]
        public ActionResult DeleteSubmit(NHAXUATBAN nhaxuatban)
        {
            // kiểm tra dữ liệu có tồn tại trong SQL không
            if (ModelState.IsValid)
            {
                // lấy ID nếu giá trị ID chọn đúng vs giá trị ID muốn xoá
                var id = nxb.NHAXUATBANs.FirstOrDefault(m => m.MANXB == nhaxuatban.MANXB);
                nxb.NHAXUATBANs.Remove(id);
                nxb.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //GET: NhaXuatBan/sửa loại sách

        public ActionResult ViewEdit(int nxb_id)
        {
            var sua = nxb.NHAXUATBANs.FirstOrDefault(m => m.MANXB == nxb_id);
            return View(sua);
        }

        [HttpPost]
        public ActionResult EditSubmit(NHAXUATBAN nhaxuatban)
        {
            if (ModelState.IsValid)
            {
                var id = nxb.NHAXUATBANs.FirstOrDefault(m => m.MANXB == nhaxuatban.MANXB);
                id.TENNXB = nhaxuatban.TENNXB;
                id.DIACHI = nhaxuatban.DIACHI;
                nxb.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
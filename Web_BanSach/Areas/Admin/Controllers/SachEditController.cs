using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_BanSach.Models;

namespace Web_BanSach.Areas.Admin.Controllers
{
    public class SachEditController : Controller
    {
        // GET: Admin/SachEdit
        QL_SACHEntities sach = new QL_SACHEntities();

        // GET: SachEdit
        public ActionResult Index()
        {
            List<BANGSACH> dssach = sach.BANGSACHes.ToList();
            return View(dssach);
        }

        public ActionResult ThemSach()
        {
            ViewBag.SelectLoai = new SelectList(sach.LOAISACHes, "MALOAI", "TENLOAI");
            ViewBag.SelectNXB = new SelectList(sach.NHAXUATBANs, "MANXB", "TENNXB");
            return View();
        }

        [HttpPost]
        public ActionResult XuLySach(Saches s, HttpPostedFileBase AnhBiaUpload)
        {
            BANGSACH sachess = new BANGSACH();
            if (ModelState.IsValid)
            {
                // thêm ảnh
                if (AnhBiaUpload != null && AnhBiaUpload.ContentLength > 0)
                {
                    var filename = Path.GetFileName(AnhBiaUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnh"), filename);
                    if (!System.IO.File.Exists(path))
                    {
                        AnhBiaUpload.SaveAs(path);
                        sachess.ANHBIA = filename;
                    }
                }

                //thêm thông tin khác
                sachess.TENSACH = s.TENSACH;
                sachess.GIABAN = s.GIABAN;
                sachess.NOIDUNG = s.NOIDUNG;
                sachess.NAMXUATBAN = s.NAMXUATBAN;
                sachess.MALOAI = s.MALOAI;
                sachess.MANXB = s.MANXB;
                sach.BANGSACHes.Add(sachess);
                sach.SaveChanges();
                return RedirectToAction("Index", "SachEdit");
            }
            ViewBag.SelectLoai = new SelectList(sach.LOAISACHes, "MALOAI", "TENLOAI", s.MALOAI);
            ViewBag.SelectNXB = new SelectList(sach.NHAXUATBANs, "MANXB", "TENNXB", s.MANXB);
            return View("Index");
        }


        //GET: Sach/xoá loại sách
        public ActionResult ViewDelete(int? id)
        {
            var saches = sach.BANGSACHes.Find(id);
            return View(saches);
        }

        [HttpPost]
        public ActionResult DeleteSubmit(int? id)
        {
            // kiểm tra dữ liệu có tồn tại trong SQL không
            if (ModelState.IsValid)
            {
                var xoa = sach.BANGSACHes.Find(id);
                sach.BANGSACHes.Remove(xoa);
                sach.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //GET: Sách/sửa loại sách
        public ActionResult ViewEdit(int id)
        {
            ViewBag.SelectLoai = new SelectList(sach.LOAISACHes, "MALOAI", "TENLOAI");
            ViewBag.SelectNXB = new SelectList(sach.NHAXUATBANs, "MANXB", "TENNXB");
            var saches = sach.BANGSACHes.Find(id);
            return View(saches);
        }

        [HttpPost]
        public ActionResult SuaSach(BANGSACH s, HttpPostedFileBase AnhBiaUpload)
        {
            if (ModelState.IsValid)
            {
                var sachess = sach.BANGSACHes.Find(s.MASACH);
                // thêm ảnh
                if (AnhBiaUpload != null && AnhBiaUpload.ContentLength > 0)
                {

                    var filename = Path.GetFileName(AnhBiaUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnh"), filename);
                    if (!System.IO.File.Exists(path))
                    {
                        AnhBiaUpload.SaveAs(path);
                        sachess.ANHBIA = filename;
                    }
                }

                //thêm thông tin khác
                sachess.TENSACH = s.TENSACH;
                sachess.GIABAN = s.GIABAN;
                sachess.NOIDUNG = s.NOIDUNG;
                sachess.NAMXUATBAN = s.NAMXUATBAN;
                sachess.MALOAI = s.MALOAI;
                sachess.MANXB = s.MANXB;
                UpdateModel(sachess);
                sach.SaveChanges();
                return RedirectToAction("Index", "SachEdit");
            }
            ViewBag.SelectLoai = new SelectList(sach.LOAISACHes, "MALOAI", "TENLOAI", s.MALOAI);
            ViewBag.SelectNXB = new SelectList(sach.NHAXUATBANs, "MANXB", "TENNXB", s.MANXB);
            return View("Index");
        }



    }
}
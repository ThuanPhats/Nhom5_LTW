using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_BanSach.Areas.Admin.Controllers
{
    public class LoaiSachController : Controller
    {
        // GET: Admin/LoaiSach
        QL_SACHEntities loai = new QL_SACHEntities();
        // GET: LoaiSach
        public ActionResult Index()
        {
            List<LOAISACH> dsLoai = loai.LOAISACHes.ToList();
            return View(dsLoai);
        }

        //GET: LoaiSach/thêm loại sách
        public ActionResult ThemLoai()
        {

            return View();
        }
        public ActionResult ViewThemLoai(LOAISACH loaisach)
        {
            loai.LOAISACHes.Add(loaisach);
            loai.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: LoaiSach/xoá loại sách
        public ActionResult ViewDelete(int Loaisach_id)
        {
            var xoa = loai.LOAISACHes.FirstOrDefault(m => m.MALOAI == Loaisach_id);
            return View(xoa);
        }

        [HttpPost]
        public ActionResult DeleteSubmit(LOAISACH theloai)
        {
            // kiểm tra dữ liệu có tồn tại trong SQL không
            if (ModelState.IsValid)
            {
                // lấy ID nếu giá trị ID chọn đúng vs giá trị ID muốn xoá
                var id = loai.LOAISACHes.FirstOrDefault(m => m.MALOAI == theloai.MALOAI);
                loai.LOAISACHes.Remove(id);
                loai.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //GET: LoaiSach/sửa loại sách
        public ActionResult ViewEdit(int Loaisach_id)
        {
            var sua = loai.LOAISACHes.FirstOrDefault(m => m.MALOAI == Loaisach_id);
            return View(sua);
        }

        [HttpPost]
        public ActionResult EditSubmit(LOAISACH theloai)
        {
            if (ModelState.IsValid)
            {
                var id = loai.LOAISACHes.FirstOrDefault(m => m.MALOAI == theloai.MALOAI);
                id.TENLOAI = theloai.TENLOAI;
                loai.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
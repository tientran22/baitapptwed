using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;
using PagedList;
using System.Net;

namespace Test.Controllers
{
    public class Tien_61134498Controller : Controller
    {
        TestEntities db = new TestEntities();
        // GET: Test
        string LayMaSV()
        {
            var maMax = db.SinhViens.ToList().Select(n => n.MaSV).Max();
            int maSV = int.Parse(maMax.Substring(2)) + 1;
            string SV = String.Concat("0", maSV.ToString());
            return "SV" + SV.Substring(maSV.ToString().Length - 1);
        }
        public ActionResult Index(int? page)
        {
            
            if (page == null) page = 1;

            
            var dsSV = (from l in db.SinhViens
                        select l).OrderBy(x => x.MaSV);          
            int pageSize = 2;           
            int pageNumber = (page ?? 1);         
            return View(dsSV.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult Create()
        {

            ViewBag.MaSV = LayMaSV();
            ViewBag.MaP = new SelectList(db.Lops, "MaLop", "TenLop");
            return View();
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSV,HoSV,TenSV,NgaySinh,GioiTinh,,AnhSV,DiaChi,MaLop")] SinhVien Sv)
        {
            //System.Web.HttpPostedFileBase Avatar;
            var imgSV = Request.Files["Avatar"];
            //Lấy thông tin từ input type=file có tên Avatar
            string postedFileName = System.IO.Path.GetFileName(imgSV.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/" + postedFileName);
            imgSV.SaveAs(path);

            if (ModelState.IsValid)
            {
                Sv.MaSV = LayMaSV();
                Sv.AnhSV = postedFileName;
                db.SinhViens.Add(Sv);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLop = new SelectList(db.Lops, "MaLop", "TenLop", Sv.MaLop);
            return View(Sv);
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVien sv = db.SinhViens.Find(id);
            if (sv == null)
            {
                return HttpNotFound();
            }
            return View(sv);
        }
        public ActionResult TimKiem(string maSV = "", string HoTen = "")
        {
            ViewBag.MASV = maSV;
            ViewBag.HOTEN = HoTen;

            var sINHVIENs = db.SinhViens.SqlQuery("SinhVien_TimKiem'" + maSV + "','" + HoTen + "'");
            if (sINHVIENs.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";
            return View(sINHVIENs.ToList());

        }
    }
}
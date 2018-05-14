using OnYourWay.Models;
using OnYourWay.Models.DBTables;
using OnYourWay.Models.Extensisons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnYourWay.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    [AuthorizeUser(AccessLevel = "Category")]
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index(int? id)
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                var categories = db.Categories.Where(a => a.CategoryID ==id&&a.Delete!=true).ToList();
                return View( categories);
            }
        }
        public ActionResult Add(string nameAr, string nameEn, string nameOr,int? Categoryid)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Models.DBTables.Category Category = new Models.DBTables.Category() {NameAR=nameAr,NameEN=nameEn,NameOR=nameOr,Block=false,Delete=false,CategoryID= Categoryid };
                db.Categories.Add(Category);
                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult Edit(string nameAr, string nameEn, string nameOr, int id)
        {
            using (ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                Models.DBTables.Category Category = db.Categories.Where(a => a.ID == id).FirstOrDefault();
                Category.NameAR = nameAr;
                Category.NameEN = nameEn;
                Category.NameOR = nameOr;
                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                db.Categories.Where(a => a.ID == id).FirstOrDefault().Delete = true;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Block(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Category = db.Categories.Where(a => a.ID == id).FirstOrDefault();
                if (Category.Block == true)
                {

                    Category.Block = false;

                }
                else
                {
                    Category.Block = true;

                }
                db.SaveChanges();
                return Json(new { del = true, id = Category.ID, status = Category.Block }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
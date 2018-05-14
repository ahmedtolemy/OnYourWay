using OnYourWay.Models;
using OnYourWay.Models.DBTables;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnYourWay.Models.Extensisons;

namespace OnYourWay.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    [AuthorizeUser(AccessLevel = "City")]
    public class CityController : Controller
    {
        // GET: City
        public ActionResult Index()
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                var Cities = db.Cities.Where(a => a.Delete != true).Include(a=>a.Clients).Include(a=>a.Customers).OrderBy(a => a.NameAR).ToList();
                return View(Cities);
            }
               
        }
        public ActionResult Add(string nameAr, string nameEn, string nameOr)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                City City = new City();
                City.NameAR = nameAr;
                City.NameEN = nameEn;
                City.NameOR = nameOr;
                City.Block = false;
                City.Delete = false;
                db.Cities.Add(City);

                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult Edit(string nameAr, string nameEn, string nameOr, int id)
        {
            using (ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                City City = db.Cities.Where(a=>a.ID==id).FirstOrDefault();
                City.NameAR = nameAr;
                City.NameEN = nameEn;
                City.NameOR = nameOr;
                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                db.Cities.Where(a=>a.ID==id).FirstOrDefault().Delete=true;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Block(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var city = db.Cities.Where(a=>a.ID==id).FirstOrDefault();
                if (city.Block==true)
                {

                    city.Block = false;

                }
                else
                {
                    city.Block = true;

                }
                db.SaveChanges();
                return Json(new { del = true, id = city.ID, status = city.Block }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
using OnYourWay.Models;
using OnYourWay.Models.DBTables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnYourWay.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class CarController : Controller
    {
        // GET: Car
        public ActionResult Index()
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                var Cars = db.CarCompanies.Where(a => a.Delete != true)
                    .OrderBy(a=>a.NameAR)
                    .ToList();
                return View(Cars);
            }
                
        }
        public ActionResult Add(string nameAr, string nameEn, string nameOr)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                CarCompany Car = new CarCompany();
                Car.NameAR = nameAr;
                Car.NameEN = nameEn;
                Car.NameOR = nameOr;
                Car.Block = false;
                Car.Delete = false;
                db.CarCompanies.Add(Car);

                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult Edit(string nameAr, string nameEn, string nameOr, int id)
        {
            using (ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                CarCompany Car = db.CarCompanies.Where(a => a.ID == id).FirstOrDefault();
                Car.NameAR = nameAr;
                Car.NameEN = nameEn;
                Car.NameOR = nameOr;
                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                db.CarCompanies.Where(a => a.ID == id).FirstOrDefault().Delete = true;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Block(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var car = db.CarCompanies.Where(a => a.ID == id).FirstOrDefault();
                if (car.Block == true)
                {

                    car.Block = false;

                }
                else
                {
                    car.Block = true;

                }
                db.SaveChanges();
                return Json(new { del = true, id = car.ID, status = car.Block }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ClientCar()
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                var cars = db.Clients.Where(a => a.CarTypeID != null).Include(a=>a.CarType.CarCompany).ToList();
                return View(cars);
            }
        }
        #region cartypesactions
        public ActionResult CarTypes(int? id)
        {
            if (id!=null)
            {

           
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var CarTypes = db.CarTypes.Where(a => a.Delete != true&&a.CarCompanyID==id).OrderBy(a => a.NameAR).ToList();
                return View(CarTypes);
            }
            }
            return RedirectToAction("Index", "Home");

        }
        public ActionResult AddType(string nameAr, string nameEn, string nameOr,string carid)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                CarType Car = new CarType();
                Car.NameAR = nameAr;
                Car.NameEN = nameEn;
                Car.NameOR = nameOr;
                Car.CarCompanyID = int.Parse(carid);
                Car.Block = false;
                Car.Delete = false;
                db.CarTypes.Add(Car);

                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult EditType(string nameAr, string nameEn, string nameOr, int id)
        {
            using (ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                CarType Car = db.CarTypes.Where(a => a.ID == id).FirstOrDefault();
                Car.NameAR = nameAr;
                Car.NameEN = nameEn;
                Car.NameOR = nameOr;
                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteType(int id)
        {
            using (ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                db.CarTypes.Where(a => a.ID == id).FirstOrDefault().Delete = true;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult BlockType(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var car = db.CarTypes.Where(a => a.ID == id).FirstOrDefault();
                if (car.Block == true)
                {

                    car.Block = false;

                }
                else
                {
                    car.Block = true;

                }
                db.SaveChanges();
                return Json(new { del = true, id = car.ID, status = car.Block }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
        
    }
}
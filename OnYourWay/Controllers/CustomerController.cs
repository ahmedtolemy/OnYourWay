using OnYourWay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnYourWay.Controllers
{
    public class CustomerController : BaseController
    {
        // GET: Customer
        public ActionResult Index()
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                var Customers = db.Customers.Where(a => a.Delete != true)
                    .OrderByDescending(a => a.RegisterDate)
                    .ToList();
                return View(Customers);
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Customers.Where(a => a.ID == id).FirstOrDefault().Delete = true;
                db.SaveChanges();
            }
            return Json(new { del = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Block(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Customer = db.Customers.Where(a => a.ID == id).FirstOrDefault();
                if (Customer.Block == true)
                {
                    Customer.Block = false;
                }
                else
                {
                    Customer.Block = true;
                }
                db.SaveChanges();
                return Json(new { del = true, id = Customer.ID, status = Customer.Block }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult CustomerTrips(int? id)
        {
            if (id!=null)
            {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                var trips = db.Trips.Where(a => a.CustomerID == id).Include(a=>a.Customer).Include(a=>a.Offers)
                    .Include(a=>a.Offer.Client.CarType.CarCompany).Include(a=>a.MainCategory).ToList();
                return View(trips);
            }
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
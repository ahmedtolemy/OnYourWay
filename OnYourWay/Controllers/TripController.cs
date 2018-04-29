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
    public class TripController : BaseController
    {
        // GET: Trip
        public ActionResult Active()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var trips = db.Trips.Where(a => a.Status != (int)TripStatus.Completed && a.Status != (int)TripStatus.Canceled)
                    .Include(a => a.Customer).Include(a=>a.MainCategory).Include(a => a.Offer.Client.CarType.CarCompany).Include(a => a.Offers)
                    .OrderByDescending(a => a.Created_at).ToList();
                     return View("Trips",trips);
            }
           
        }
        public ActionResult Finish()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var trips = db.Trips.Where(a => a.Status == (int)TripStatus.Completed || a.Status == (int)TripStatus.Canceled)
                    .Include(a => a.Customer).Include(a=>a.MainCategory).Include(a => a.Offer.Client.CarType.CarCompany).Include(a => a.Offers)
                    .OrderByDescending(a => a.Created_at).ToList();
                return View("Trips", trips);
            }
        }
    }
}
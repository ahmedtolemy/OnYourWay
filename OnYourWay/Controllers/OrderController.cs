using OnYourWay.Models;
using OnYourWay.Models.DBTables;
using OnYourWay.Models.Extensisons;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnYourWay.Controllers
{

    [AuthorizeUser(AccessLevel = "Order")]
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult Active()
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                var orders = db.Offers.Where(a=>a.Approved!=true&&a.Trip.OfferStatus!=(int)TripOffferStatus.AcceptedOffer)
                    .Include(a=>a.Trip.Customer).Include(a=>a.Trip.MainCategory).Include(a=>a.Client.CarType.CarCompany).Include(a=>a.AcceptedTrips)
                    .OrderByDescending(a => a.Created_at).ToList();
                return View("Offers",orders);
            }
        }
        public ActionResult Finish()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var orders = db.Offers.Where(a => a.Approved==true)
                    .Include(a => a.Trip.Customer).Include(a=>a.Trip.MainCategory).Include(a => a.Client.CarType.CarCompany).Include(a=>a.AcceptedTrips)
                    .OrderByDescending(a => a.Created_at).ToList();
                return View("Offers",orders);
            }
        }
    }
}
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

    [Authorize(Roles = "Manager,Admin,User")]
    public class ClientController : BaseController
    {
        // GET: Client
        public ActionResult Index()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<Client> Clients = new List<Client>(); 
                if (!User.IsInRole("User"))
                {

                
                 Clients = db.Clients.Where(a => a.Delete != true).Include(a => a.Ratings)
                    .OrderByDescending(a => a.RegisterDate)
                    .ToList();
                }
                else
                {
                     Clients = db.Clients.Where(a => a.Delete != true&&a.User!=null&&a.User.UserName==User.Identity.Name).Include(a => a.Ratings)
                   .OrderByDescending(a => a.RegisterDate)
                   .ToList();
                }
                return View(Clients);
            }

        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Clients.Where(a => a.ID == id).FirstOrDefault().Delete = true;
                db.SaveChanges();
            }
            return Json(new { del = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Block(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Client = db.Clients.Where(a => a.ID == id).FirstOrDefault();
                if (Client.Block == true)
                {
                    Client.Block = false;
                }
                else
                {
                    Client.Block = true;
                }
                db.SaveChanges();
                return Json(new { del = true, id = Client.ID, status = Client.Block }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult ClientOffers(int? id)
        {
            if (id != null)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var offers = db.Offers.Where(a => a.ClientID == id).Include(a=>a.Client).Include(a=>a.Trip.Customer).Include(a=>a.Trip.MainCategory)
                        .Include(a=>a.AcceptedTrips)
                        .OrderByDescending(a => a.Created_at).ToList();
                    return View(offers);
                }

            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ClientTrips(int? id)
        {
            if (id != null)
            {
                
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var trips = db.Trips.Where(a => a.Offer.ClientID == id).Include(a=>a.Offer.Client).Include(a=>a.Customer)
                        .OrderByDescending(a=>a.Created_at).Include(a=>a.MainCategory).ToList();
                    return View(trips);
                }
               
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
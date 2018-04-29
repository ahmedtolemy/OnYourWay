using OnYourWay.Models;
using OnYourWay.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnYourWay.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                DashBoard dashbord = (from Clients in db.Clients
                                      let clients = db.Clients.Where(a => a.Delete != true).Count()
                                      let customers = db.Customers.Where(a => a.Delete != true).Count()
                                      let admins = db.Users.Where(a => a.Deleted != true).Count()
                                      let Donetrips = db.Trips.Where(a => a.Status == (int)TripStatus.Completed || a.Status == (int)TripStatus.Canceled).Count()
                                      let trips = db.Trips.Where(a => a.Status != (int)TripStatus.Completed && a.Status != (int)TripStatus.Canceled).Count()
                                      let offers = db.Offers.Count()
                                      let maincat=db.Categories.Where(a=>a.CategoryID==null).Count()
                                      let cat=db.Categories.Where(a=>a.CategoryID!=null).Count()
                                      let complains = db.Complains.Count()
                                      select new DashBoard
                                      {
                                          customers = customers,
                                          clients = clients,
                                          donetrips=Donetrips,
                                          trips=trips,
                                          offers=offers,
                                          complains=complains,
                                          admins = admins,
                                          main_category=maincat,
                                          category=cat
                                      }).Take(1).FirstOrDefault();
                return View(dashbord);
            }

        }
    }
}
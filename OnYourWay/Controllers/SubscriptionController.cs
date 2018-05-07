using OnYourWay.Models;
using OnYourWay.Models.DBTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnYourWay.Models.Extensisons;
using System.Data.Entity;

namespace OnYourWay.Controllers
{
    public class SubscriptionController : BaseController
    {
        // GET: Subscription
        public ActionResult Index()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var userid = User.Identity.GetUserID();
                var clients = db.Clients.Where(a => a.Delete != true && (a.UserID == null || a.UserID == userid)).ToList();
                var subscrib = db.Subscriptions.Where(a => a.AdminID == null && a.userID == userid&&a.Delete!=true).Include(a => a.Client).ToList();

                return View(new Tuple<List<Client>, List<Subscription>>(clients, subscrib));
            }
        }
        public ActionResult Add(int clientid, int nameAr, string nameEn, string nameOr)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var userid = User.Identity.GetUserID();
                var months = int.Parse(nameEn);
                Subscription City = new Subscription()
                {
                    userID = userid,
                    ClientID = clientid,
                    Type = nameAr,
                    Start = DateTime.Now,
                    End = DateTime.Now.AddMonths(months),
                    Balance = nameOr,
                    Block=false,
                    Delete=false
                };
                db.Subscriptions.Add(City);

                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }


        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                db.Subscriptions.Where(a => a.ID == id).FirstOrDefault().Delete = true;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Block(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var city = db.Subscriptions.Where(a => a.ID == id).FirstOrDefault();
                if (city.Block == true)
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
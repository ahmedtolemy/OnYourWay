using OnYourWay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace OnYourWay.Controllers
{
    [Authorize(Roles ="Manager")]
    public class PanelController : Controller
    {
        // GET: Panel
        public ActionResult Index()
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                var admins = db.Users.Where(a => a.Roles.Select(b => b.RoleId).Contains("1")&& a.Roles.Count<2).ToList();
                return View(admins);
            }
           
        }
                            
        public ActionResult Edit(string clientid, string[] accesslevels)
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                var admin = db.Users.Where(a => a.Id == clientid).FirstOrDefault();
                if (admin!=null)
                {
                    admin.AccessLevel = string.Join(",", accesslevels!=null?accesslevels:new[] {"" ,""});
                }
                db.SaveChanges();
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
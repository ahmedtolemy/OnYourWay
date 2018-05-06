using OnYourWay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnYourWay.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class ComplainController : BaseController
    {
        // GET: Complain
        public ActionResult Complains()
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                var complains = db.Complains.Where(a=>a.Delete!=true).Include(a=>a.Client).Include(a=>a.Customer).OrderByDescending(a=>a.Created_at).ToList();
                return View(complains);
            }
           
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                db.Complains.Where(a => a.ID == id).FirstOrDefault().Delete = true;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
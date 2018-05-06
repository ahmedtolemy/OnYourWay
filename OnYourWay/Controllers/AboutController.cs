using OnYourWay.Models;
using OnYourWay.Models.DBTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnYourWay.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class AboutController : BaseController
    {
        // GET: About
        public ActionResult Index()
        {
            ApplicationDbContext DB = new ApplicationDbContext();
            var about = DB.AppSettings.FirstOrDefault();
            return View(about!=null?about :new AppSetting());
        }
        [HttpPost]
        public ActionResult EditSettings(AppSetting model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var setting = db.AppSettings.FirstOrDefault();
                if (setting!=null)
                {
                    setting.AboutAr = model.AboutAr;
                    setting.AboutEn = model.AboutEn;
                    setting.AboutOr = model.AboutOr;
                    setting.StepsAr = model.StepsAr;
                    setting.StepsEn = model.StepsEn;
                    setting.StepsOr = model.StepsOr;
                    setting.Phone = model.Phone;
                    setting.Email = model.Email;
                    setting.FaceBook = model.FaceBook;
                    setting.Twitter = model.Twitter;
                    setting.Instagram = model.Instagram;
                }
                else
                {
                    db.AppSettings.Add(model);
                }
                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
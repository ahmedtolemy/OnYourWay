using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using OnYourWay.Models;
using OnYourWay.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnYourWay.Controllers
{
    [Authorize(Roles ="Manager,Admin")]
    public class AdminController : BaseController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Admin
        public ActionResult Index()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Admins = db.Users.Where(s => !s.Deleted).Include(a => a.Roles).ToList();
                return View(Admins);
            }

        }
        public ActionResult AddAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BlockUser(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.Find(id);
                if (user.Active)
                {
                    user.Active = false;
                }
                else
                {
                    user.Active = true;
                }

                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { del = true, id = user.Id, status = user.Active }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult DeleteUser(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.Find(id);
                user.Deleted = true;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new { del = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditAdmin(string id)
        {
            if (id != null)
            {


                using (ApplicationDbContext db = new Models.ApplicationDbContext())
                {
                    var user = db.Users.Find(id);
                    AdminModel model = new AdminModel();
                    model.id = id;
                    model.mail = user.Email;
                    model.name = user.UserName;
                    model.phone = user.PhoneNumber;
                    model.pass = user.ImageUrl;
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public ActionResult EditAdminData(AdminModel model)
        {
            using (ApplicationDbContext db = new Models.ApplicationDbContext())
            {

                var user = db.Users.Find(model.id);
                PasswordHasher ph = new PasswordHasher();
                var validate = db.Users.Where(a => a.Id != model.id).Select(s => new { email = s.Email, username = s.UserName }).ToList();
                if (model.oldpass != null)
                {
                    if (ph.VerifyHashedPassword(user.PasswordHash, model.oldpass) == PasswordVerificationResult.Failed)
                    {
                        return Json(new { success = false, error = "من فضلك ادخل الرقم السري القديم صحيحا" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (model.pass == null)
                        {
                            return Json(new { success = false, error = "من فضلك ادخل الرقم السري الجديد صحيحا" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    if (model.pass != null)
                    {
                        return Json(new { success = false, error = "من فضلك ادخل الرقم السري القديم" }, JsonRequestBehavior.AllowGet);

                    }
                }
                if (validate.Any(a => a.email == model.mail))
                {
                    return Json(new { success = false, error = "الايميل مستخدم من قبل" }, JsonRequestBehavior.AllowGet);
                }
                if (validate.Any(a => a.username == model.name))
                {
                    return Json(new { success = false, error = "أسم المستخدم مستخدم من قبل" }, JsonRequestBehavior.AllowGet);
                }

                var file = Request.Files["user"];
                string date = DateTime.Now.Ticks.ToString();
                if (file != null)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/userImages/"), date + fileName);
                    file.SaveAs(path);
                    user.ImageUrl = date + fileName;
                }

                user.Email = model.mail;
                user.UserName = model.name;
                user.PhoneNumber = model.phone;
                user.PasswordHash = model.pass != null && model.oldpass != null ? ph.HashPassword(model.pass) : user.PasswordHash;
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterAdmin(RegisterViewModel model)
        {
            List<string> lstErrors = new List<string>();
            if (!ModelState.IsValid)
            {
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        lstErrors.Add(error.ErrorMessage);
                    }
                }
                return Json(new { success = false, errors = lstErrors }, JsonRequestBehavior.AllowGet);

            }

            var user = new ApplicationUser
            {
                UserName = model.FullName,
                Email = model.Email,
                ImageUrl = "defualt.png",
                Active = true,
                Deleted = false,
                PhoneNumber = model.phone,
                RegisterDate = DateTime.Now
            };



            var file = Request.Files["user"];
            string date = DateTime.Now.Ticks.ToString();
            if (file != null)
            {
                string fileName = DateTime.Now.Ticks.ToString() + ".png";
                var path = Path.Combine(Server.MapPath("~/Content/userImages"), date + fileName);
                file.SaveAs(path);
                user.ImageUrl = date + fileName;
            }
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                string Userid = "";
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Userid = db.Users.Where(x => x.Email == model.Email)
                       .Select(x => x.Id).FirstOrDefault();
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                    if (!roleManager.RoleExists("Admin"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("Admin"));
                    }
                    await UserManager.AddToRoleAsync(Userid, "Admin");
                    db.SaveChanges();
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            foreach (var error in result.Errors)
            {
                lstErrors.Add(error);
            }
            return Json(new { success = false, errors = lstErrors }, JsonRequestBehavior.AllowGet);


        }

    }
}
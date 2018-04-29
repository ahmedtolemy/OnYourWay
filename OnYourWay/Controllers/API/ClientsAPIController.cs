using OnYourWay.Models;
using OnYourWay.Models.DBTables;
using OnYourWay.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnYourWay.Controllers.API
{
    [AllowAnonymous]
    public class ClientsAPIController : BaseController
    {
        // GET: Clients
        //sms//notify//change car data
        [HttpPost]
        public ActionResult Login(ClientLogin client)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Client CurrentClient = db.Clients.Where(a => a.Password == client.password && a.Mobile == client.phone)
                    .Include(a => a.City).Include(a => a.Company)
                    .Include(a => a.CarType).Include(a => a.CarType.CarCompany).FirstOrDefault();
                if (CurrentClient != null)
                {
                    if (CurrentClient.Delete != true && CurrentClient.Block != true)
                    {
                        if (CurrentClient.Activation == true)
                        {
                            CurrentClient.Token = client.token;
                            db.SaveChanges();
                            return Json(new
                            {
                                key = 1,
                                msg = "Access",
                                data = new
                                {
                                    driver_id = CurrentClient.ID,
                                    name = CurrentClient.Name,
                                    img_url = Url.Content("~") + "Content/clientImages/" + CurrentClient.ImgUrl,
                                    phone = CurrentClient.Mobile,
                                    active = CurrentClient.Active,
                                    balance = CurrentClient.Balance == null ? "0" : CurrentClient.Balance,
                                    city_id = CurrentClient.CityID,
                                    city_name = CurrentClient.City != null ? massege(client.language, CurrentClient.City.NameAR, CurrentClient.City.NameEN, CurrentClient.City.NameOR) : "",
                                    companyname = CurrentClient.Company != null ? CurrentClient.Company.Name : "",

                                }
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Random rnd = new Random();
                            //send sms
                            return Json(new
                            {
                                key = 1,
                                msg = "Active",
                                data = rnd.Next(100000, 999999)
                            }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        return Json(new
                        {
                            key = 0,
                            msg = massege(client.language, "لقد تم حظرك من قبل الموقع", "You Not available to access", "لقد تم حظرك من قبل الموقع")
                        }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new
                    {
                        key = 0,
                        msg = massege(client.language, "خطا في البيانات", "Wrong Data", "خطا في البيانات")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpGet]
        public ActionResult Register(string language)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Data = (from city in db.Cities
                            let cars = db.CarCompanies
                            .Select(a => new
                            {
                                id = a.ID,
                                namear = a.NameAR,
                                nameen = a.NameEN,
                                nameor = a.NameOR,
                                cartypes = a.CarTypes.Select(s => new { id = s.ID, namear = s.NameAR, nameen = s.NameEN, nameor = s.NameOR })
                            })
                            let cities = db.Cities.Select(a => new { id = a.ID, namear = a.NameAR, nameen = a.NameEN, nameor = a.NameOR })
                            let companies = db.Companies.Select(a => new { id = a.ID, namear = a.Name })
                            select new { cars = cars.ToList(), cities = cities.ToList(), companies = companies.ToList() }).Take(1).FirstOrDefault();

                var category = new[] { new { id = 1, name = CategoryName(language, 1) } }.ToList();
                for (int i = 2; i < 8; i++)
                {
                    category.Add(new { id = i, name = CategoryName(language, i) });
                }
                return Json(new
                {
                    Key = 1,
                    msg = "access",
                    data = new
                    {
                        city = Data.cities.Select(a => new { id = a.id, name = massege(language, a.namear, a.nameen, a.nameor) }).ToList(),
                        cars = Data.cars.Select(a => new
                        {
                            id = a.id,
                            name = massege(language, a.namear, a.nameen, a.nameor),
                            cartype = a.cartypes.Select(s => new { id = s.id, name = massege(language, s.namear, s.nameen, s.nameor) }).ToList()
                        }).ToList(),
                        category_people = category.Where(a => a.id == 1 || a.id == 2 || a.id == 6).ToList(),
                        category_goods = category.Where(a => a.id == 3 || a.id == 4 || a.id == 7).ToList(),
                        main_category = category.Where(a => a.id == 5 || a.id == 6 || a.id == 7).ToList(),
                        car_models = Enumerable.Range(DateTime.Now.Year - 39, 40).OrderByDescending(a => a).ToList(),
                        companies = Data.companies

                    }
                }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult Register(ClientRegister register)
        {
            //still hadwork
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var check = db.Clients.Where(a => a.Mobile == register.phone).ToList().Count();
                if (check > 0)
                {
                    return Json(new
                    {
                        key = 0,
                        msg = massege(register.language, "الرقم مستخدم", "Number Used", "الرقم مستخدم")
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Client client = new Client()
                    {
                        CarColor = register.car_color,
                        CarSeats = int.Parse(register.car_seats),
                        CarUrl = ImgUrl(register.car_img, "clientImages"),
                        Created_at = DateTime.Now,
                        ImgUrl = ImgUrl(register.driver_img, "clientImages"),
                        InsuranceUrl = ImgUrl(register.insurance_img, "clientImages"),
                        Language = register.language,
                        LicenseImgUrl = ImgUrl(register.license_img, "clientImages"),
                        Mobile = register.phone,
                        Name = register.name,
                        Activation = false,
                        Password = register.password,
                        CityID = int.Parse(register.city_id),
                        Token = register.token,
                        PlateChars = register.car_char,
                        PlateNumber = register.car_number
                    };
                    switch (int.Parse(register.tape))
                    {
                        case (int)Category.People_Box:
                            client.CarTypeID = int.Parse(register.select_2);
                            client.CarModel = register.select_3;
                            client.Type = int.Parse(register.select_4);
                            break;
                        case (int)Category.Goods_Animals:
                            client.Type = int.Parse(register.select_1);
                            client.CarTypeID = int.Parse(register.select_3);
                            client.CarModel = register.select_4;
                            break;
                        case (int)Category.Shipping:
                            Company company = new Company()
                            {
                                CompanyNumber = register.select_4,
                                Name = register.select_1,
                                RecordNumber = register.select_2,
                                Phone = register.select_3,
                                Created_at = DateTime.Now
                            };
                            db.Companies.Add(company);
                            db.SaveChanges();
                            client.CompanyID = company.ID;
                            break;
                        default:
                            break;
                    }
                    db.Clients.Add(client);
                    db.SaveChanges();
                    return Json(new
                    {
                        key = 1,
                        msg = "success"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult Myorders(string language, int clientid)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var orders = db.Trips.Where(a => a.AcceptedOfferID != null && a.Offer.ClientID == clientid).ToList();
                return Json(new
                {
                    key = 1,
                    msg = "access",
                    data = orders.Select(a => new
                    {
                        id = a.ID,
                        cars_count = a.CarsCount,
                        car_type = a.CarType,
                        category = CategoryName(language, a.CategoryID),
                        people_count = a.PeopleCount
                      ,
                        time = a.Start,
                        start = a.StartPoint,
                        end = a.EndPoint,
                        car_count = a.CarsCount,
                        box_type = a.BoxType,
                        weight = a.Weight
                    }).ToList()
                }
                , JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Orders(string language, int clientid)
        {
            ///stillwork
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var Data = (from city in db.Cities
                            let client = db.Clients.Where(a => a.ID == clientid).FirstOrDefault()
                            let trips = db.Trips.Where(a => a.AcceptedOfferID == null && client.Categories.Select(b => (int?)b.ID).Contains(a.CategoryID))
                            select new { orders = trips }).Take(1).FirstOrDefault();
                return Json(new
                {
                    key = 1,
                    msg = "access",
                    data = Data.orders.Select(a => new
                    {
                        id = a.ID,
                        cars_count = a.CarsCount,
                        car_type = a.CarType,
                        category =a.MainCategory!=null?massege(language,a.MainCategory.NameAR,a.MainCategory.NameEN,a.MainCategory.NameOR):"" ,
                        people_count = a.PeopleCount,
                        time = a.Start,
                        start = a.StartPoint,
                        end = a.EndPoint,
                        car_count = a.CarsCount,
                        box_type = a.BoxType,
                        weight = a.Weight
                    }).ToList()
                }
                , JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Sendoffer(string language, int? triptime, string time, int? tripid, int? clientid, string money, string note)
        {

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var trip = db.Trips.Where(a => a.ID == tripid).Include(a => a.Offers).FirstOrDefault();
                if (trip != null)
                {
                    if (trip.Offers.Count() > 0 && trip.Offers.Where(a => a.ClientID == clientid).Count() > 2)
                    {
                        return Json(new
                        {
                            key = 0,
                            msg = massege(language, "لقد وصلت للحد الاقصي للعروض لهذه الرحلة",
                            "You send Three times before", "لقد وصلت للحد الاقصي للعروض لهذه الرحلة")
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else if (trip.Offers.Count() > 0 &&
                        trip.Offers.Where(a => a.ClientID == clientid).Select(a => a.Money.ToString()).Any(a => a == money))
                    {
                        return Json(new
                        {
                            key = 0,
                            msg = massege(language, "لقد ارسلت هذا السعر من قبل",
                            "You send this amount before", "لقد ارسلت هذا السعر من قبل")
                        }, JsonRequestBehavior.AllowGet);
                    }
                    var date = trip.RequestDate;

                    if (triptime == (int)TripTime.Later)
                    {
                        double ticks = double.Parse(time);
                        TimeSpan span = TimeSpan.FromMilliseconds(ticks);
                        date = ((new DateTime(1970, 1, 1)) + span).ToLocalTime();
                    }
                    Offer offer = new Offer() { ClientID = clientid, Money = double.Parse(money), OfferDate = date, TripID = trip.ID, Note = note };
                    db.Offers.Add(offer);
                    db.SaveChanges();
                    return Json(new
                    {
                        key = 1,
                        msg = massege(language, "لقد تم ارسال العرض",
                            "Offer Send", "لقد تم ارسال العرض")
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        key = 1,
                        msg = massege(language, "خطا في البيانات حاول في وقت اخر",
                            "try again later", "خطا في البيانات حاول في وقت اخر")
                    }, JsonRequestBehavior.AllowGet);
                }

            }



        }
        public ActionResult aboutus(string language)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var aboutus = db.AppSettings.FirstOrDefault();
                return Json(new
                {
                    key = 1,
                    msg = "access",
                    data = new
                    {
                        about = massege(language, aboutus.AboutAr, aboutus.AboutEn, aboutus.AboutOr),
                        step = massege(language, aboutus.StepsAr, aboutus.StepsEn, aboutus.StepsOr),
                        twitter = aboutus.Twitter,
                        facebook = aboutus.FaceBook,
                        instgram = aboutus.Instagram,
                        phone = aboutus.Phone,
                        address = aboutus.Address,
                        email = aboutus.Email
                    }
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult setting(string language, int? clientid)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var client = db.Clients.Where(a => a.ID == clientid).FirstOrDefault();
                return Json(new
                {
                    key = 1,
                    msg = "Success",
                    data = new
                    {
                        language = client.Language,
                        notify = client.Notification,
                        alarm = client.Alarm
                    }
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult setting(string language, int? clientid, bool? notify, bool? alarm)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var client = db.Clients.Where(a => a.ID == clientid).FirstOrDefault();
                client.Language = language; client.Alarm = alarm; client.Notification = notify;
                db.SaveChanges();
                return Json(new
                {
                    key = 1,
                    msg = "Success"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ratting(string language, int? clientid)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var rating = db.Ratings.Where(a => a.ClientID == clientid).OrderByDescending(a => a.Created_at)
                    .Select(a => new { rate = a.Rate, name = a.Customer.Name, coment = a.Comment }).ToList();
                if (rating.Count() > 0)
                {
                    return Json(new
                    {
                        key = 1,
                        msg = "Success",
                        data = new
                        {

                            rate = string.Format("{0:0.0}", ((double)rating.Select(a => a.rate).Sum() / (double)(rating.Count() * 5)) * 5)
                   ,
                            rating = rating
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        key = 0,
                        msg = massege(language, "لم يتم تقييم حتي الان", "No Rating", "لم يتم تقييم حتي الان"),

                    }, JsonRequestBehavior.AllowGet);
                }


            }
        }
        [HttpPost]
        public ActionResult ChangePassword(string language, int? clientid, string oldpass, string newpass)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var client = db.Clients.Where(a => a.ID == clientid && a.Password == oldpass).FirstOrDefault();
                if (client != null)
                {
                    client.Password = newpass;
                    client.Updated_at = DateTime.Now;
                    db.SaveChanges();
                    return Json(new
                    {

                        Key = 0,
                        msg = massege(language, "تم تغيير كلمة السر", "PassWord Change", "تم تغيير كلمة السر")
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {

                        Key = 0,
                        msg = massege(language, "كلمة السر غير صحيحة", "PassWord Not Valid", "كلمة السر غير صحيحة")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult ChangeProfile(string language, int? clientid)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var data = (from Clients in db.Clients
                            let client = db.Clients.Where(a => (a.Delete != true || a.Block != true) && a.ID == clientid)
                            .Select(a => new { name = a.Name, phone = a.Mobile, cityid = a.CityID, img_url = a.ImgUrl })
                            let cities = db.Cities.Where(a => a.Delete != true || a.Block != true)
                            .Select(a => new { namear = a.NameAR, nameen = a.NameEN, nameor = a.NameOR, id = a.ID })

                            select new { client = client.FirstOrDefault(), cities = cities.ToList() }).Take(1).FirstOrDefault();
                return Json(new
                {
                    key = 1,
                    msg = "Success",
                    data = new
                    {
                        client = new
                        {
                            name = data.client.name,
                            phone = data.client.phone,
                            city_id = data.client.cityid,
                            img_url = Url.Content("~") + "Content/clientImages/" + data.client.img_url
                        },
                        cities = data.cities.Select(a => new { name = massege(language, a.namear, a.nameen, a.nameor), id = a.id }).ToList()
                    }
                }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public ActionResult ChangeProfile(string language, int? clientid, string phone, string name, int? cityid)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var client = db.Clients.Where(a => a.ID == clientid).FirstOrDefault();
                client.Mobile = phone;
                client.Name = name;
                client.Updated_at = DateTime.Now;
                client.CityID = cityid != 0 ? cityid : null;
                db.SaveChanges();
                return Json(new
                {
                    key = 1,
                    msg = massege(language, "تم تغيير البيانات", "Data Changed", "تم تغيير البيانات")
                }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult SendComplain(string language, int? clientid, string name, string email, string comp)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Complain complain = new Complain()
                {
                    PersonType = (int)PersonType.Client,
                    ClientID = clientid,
                    Created_at = DateTime.Now,
                    complain = comp,
                    Email = email,
                    name = name
                };
                db.Complains.Add(complain);
                db.SaveChanges();
                return Json(new
                {
                    key = 1,
                    msg = massege(language, "تم ارسال", "Done", "تم الارسال")
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
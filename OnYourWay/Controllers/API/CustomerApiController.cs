using OnYourWay.Models;
using OnYourWay.Models.DBTables;
using OnYourWay.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OnYourWay.Controllers.API
{
    [AllowAnonymous]
    public class CustomerApiController : BaseController
    {
        // GET: CustomerApi
        [HttpPost]
        public ActionResult Register(string language, string name, string phone, string password, string token, HttpPostedFileBase img)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                string fileName = null;
                var check = db.Customers.Where(a => a.Mobile == phone).ToList();
                if (check.Count() > 0)
                {
                    return Json(new
                    {
                        key = 0,
                        msg = massege(language, "الرقم مستخدم من قبل", "Number Already Used", "الرقم مستخدم من قبل")
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    fileName = ImgUrl(img, "customerimages");

                    Customer customer = new Customer() { Mobile = phone, Password = password, Name = name, Language = language, Activation = false, ImgUrl = fileName, Token = token, Created_at = DateTime.Now };
                    db.SaveChanges();
                    //send sms to activate
                    Random rnd = new Random();
                    var code = rnd.Next(100000, 999999);
                    SendMessage("verfication Code:'"+code+"'",phone);
                    return Json(new
                    {
                        key = 1,
                        msg = "success",
                        code =code.ToString() ,
                        id = customer.ID
                    }, JsonRequestBehavior.AllowGet);
                }
            }

        }
        [HttpPost]
        public ActionResult Login(string language, string phone, string password, string token)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Customer customer = db.Customers.Where(a => a.Password == password && a.Mobile == phone)
                    .Include(a => a.City)
                    .FirstOrDefault();
                if (customer != null)
                {
                    if (customer.Activation != true)
                    {
                        Random rnd = new Random();
                        var code = rnd.Next(100000,999999);
                        //send sms
                        SendMessage("verificationcode:'" + code + ",", phone);
                        return Json(new
                        {
                            key = 0,
                            active=false,
                            msg = massege(language, "سيتم ارسال كود التفعيل", "Verification Code Send", "سيتم ارسال كود التفعيل")
                        }, JsonRequestBehavior.AllowGet);

                    }
                    if (customer.Delete == true || customer.Block == true)
                    {
                        return Json(new
                        {
                            key = 0,
                            msg = massege(language, "لقد تم حظرك من الموقع", "you Deleted from Admin", "لقد تم حظرك من الموقع")
                        }, JsonRequestBehavior.AllowGet);
                    }
                    customer.Token = token;
                    db.SaveChanges();
                    return Json(new
                    {
                        key = 1,
                        msg = "Access",
                        data = new
                        {
                            id = customer.ID,
                            img_url = Url.Content("~") + "Content/customerimages/" + customer.ImgUrl,
                            name = customer.Name,
                            phone = customer.Mobile,
                            active = customer.Active,
                            city_id = customer.CityID,
                            city_name = customer.City != null ? massege(language, customer.City.NameAR, customer.City.NameEN, customer.City.NameOR) : ""
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        key = 0,
                        msg = massege(language, "خطا في البيانات", "Wrong Data", "خطا في البيانات")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        public ActionResult Trip(BasicOrder bo)
        {
            try
            {
                Trip trip = new Trip()
                {
                    StartLat = bo.start_lat,
                    StartLng = bo.start_lng,
                    StartPoint = bo.start_city,
                    EndLat = bo.end_lat,
                    EndLng = bo.end_lng,
                    EndPoint = bo.end_city,
                    Start = bo.now == true ? DateTime.Now : ((new DateTime(1970, 1, 1)) + (TimeSpan.FromMilliseconds(double.Parse(bo.time)))).ToLocalTime(),
                    RequestDate = DateTime.Now,
                    Note = bo.note,
                    CategoryID = bo.type,
                    Created_at = DateTime.Now,
                    TripTimeStatus = bo.now == true ? (int)TripTime.Now : (int)TripTime.Later,
                    Status = (int)TripStatus.NotStarted,
                    OfferStatus = (int)TripOffferStatus.CheckOffer,
                    CustomerID = bo.customer_id
                };
                switch (bo.type)
                {
                    case (int)Category.People:
                        trip.PeopleType = bo.sub_type;
                        trip.PeopleCount = bo.people_count;
                        break;
                    case (int)Category.Animal:
                        trip.AnimalsType = bo.sub_type;
                        trip.ImgUrl = string.Join(",", bo.img.Select(a => ImgUrl(a, "Animal")));
                        trip.CarType = bo.car_size;
                        trip.CarsCount = bo.car_count;
                        break;
                    case (int)Category.Goods:
                        trip.GoodsType = bo.sub_type;
                        trip.ImgUrl = string.Join(",", bo.img.Select(a => ImgUrl(a, "Goods")));
                        trip.CarType = bo.car_size;
                        trip.CarsCount = bo.car_count;
                        break;
                    case (int)Category.Box:
                        trip.BoxType = bo.sub_type;
                        trip.BoxWay = bo.box_way;
                        trip.Weight = bo.weight;
                        trip.ImgUrl = string.Join(",", bo.img.Select(a => ImgUrl(a, "Box")));
                        break;
                    case (int)Category.Shipping:
                        trip.ShippingType = bo.sub_type;
                        trip.Weight = bo.weight;
                        trip.ImgUrl = string.Join(",", bo.img.Select(a => ImgUrl(a, "Shipping")));
                        break;
                    default:
                        break;
                }
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    db.Trips.Add(trip);
                    db.SaveChanges();
                }
                return Json(new { key = 1, msg = "Done" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { key = 0, msg = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult MyTrips(string language, int? customer_id)
        {
            //ss
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var trips = db.Trips.Where(a => a.Customer.ID == customer_id && (a.Status != (int)TripStatus.Completed || a.Status != (int)TripStatus.Canceled))
                        .Include(a => a.Offer).Include(a => a.Offer.Client).Include(a => a.Offer.Client.CarType).Include(a => a.Offer.Client.CarType.CarCompany).ToList();
                    return Json(new
                    {
                        key = 1,
                        msg = "success",
                        data = trips.Select(a => new
                        {
                            id = a.ID,
                            date = a.Start,
                            trip_type = CategoryName(language, a.CategoryID),
                            status = OfferName(language, a.OfferStatus),
                            car_name = a.AcceptedOfferID == null ? "" :a.Offer.Client.CarType==null?"":
                            massege(language, a.Offer.Client.CarType.CarCompany.NameAR, a.Offer.Client.CarType.CarCompany.NameEN, a.Offer.Client.CarType.CarCompany.NameOR)
                        }).ToList()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                return Json(new {key=0,msg=" Error" },JsonRequestBehavior.AllowGet);
            }
           
          
        }
        public ActionResult DoneTrips(string language,int? customer_id)
        {//ss
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var trips = db.Trips.Where(a => a.Customer.ID == customer_id && (a.Status == (int)TripStatus.Completed || a.Status == (int)TripStatus.Canceled))
                        .Include(a => a.Offer).Include(a => a.Offer.Client).Include(a => a.Offer.Client.CarType).Include(a => a.Offer.Client.CarType.CarCompany).ToList();
                    return Json(new
                    {
                        key = 1,
                        msg = "success",
                        data = trips.Select(a => new
                        {
                            id = a.ID,
                            date = a.Start,
                            trip_type = CategoryName(language, a.CategoryID),
                            status = OfferName(language, a.OfferStatus),
                            car_name = a.AcceptedOfferID == null ? "" : a.Offer.Client.CarType == null ? "" :
                            massege(language, a.Offer.Client.CarType.CarCompany.NameAR, a.Offer.Client.CarType.CarCompany.NameEN, a.Offer.Client.CarType.CarCompany.NameOR)
                        }).ToList()
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                return Json(new { key = 0, msg = " Error" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ChangeProfile(string language, int? customer_id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var data = (from Clients in db.Customers
                            let client = db.Customers.Where(a => (a.Delete != true || a.Block != true) && a.ID == customer_id)
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
                            img_url = Url.Content("~") + "Content/customerimages/" + data.client.img_url
                        },
                        cities = data.cities.Select(a => new { name = massege(language, a.namear, a.nameen, a.nameor), id = a.id }).ToList()
                    }
                }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public ActionResult ChangeProfile(string language, int? customer_id, string phone, string name, int? cityid,HttpPostedFileBase img)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var client = db.Customers.Where(a => a.ID == customer_id).FirstOrDefault();
                client.Mobile = phone;
                client.Name = name;
                client.Updated_at = DateTime.Now;
                client.CityID = cityid != 0 ? cityid : null;
                if (img != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Content/customerimages/"), client.ImgUrl);
                    img.SaveAs(path);
                }
                db.SaveChanges();
                return Json(new
                {
                    key = 1,
                    msg = massege(language, "تم تغيير البيانات", "Data Changed", "تم تغيير البيانات")
                }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult ChangePassword(string language, int? customer_id, string oldpass, string newpass)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var client = db.Customers.Where(a => a.ID == customer_id && a.Password == oldpass).FirstOrDefault();
                if (client != null&&newpass.Count()>=6)
                {
                    client.Password = newpass;
                    client.Updated_at = DateTime.Now;
                    db.SaveChanges();
                    return Json(new
                    {

                        Key = 1,
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
        public ActionResult setting(string language, int? customer_id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var client = db.Customers.Where(a => a.ID == customer_id).FirstOrDefault();
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
        public ActionResult setting(string language, int? customer_id, bool? notify, bool? alarm)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var client = db.Customers.Where(a => a.ID == customer_id).FirstOrDefault();
                client.Language = language; client.Alarm = alarm; client.Notification = notify;
                db.SaveChanges();
                return Json(new
                {
                    key = 1,
                    msg = "Success"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SendComplain(string language, int?customer_id, string name, string email, string comp)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Complain complain = new Complain()
                {
                    PersonType = (int)PersonType.Customer,
                    CustomerID = customer_id,
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
        [HttpPost]
        public ActionResult SendPushMsg(string deviceID, string msg)
        {
            try
            {
                string applicationID = "AAAA3RgimmI:APA91bHZ_q2FjaSqeK2mYj7AVbB28fJN2yixm2WyHTksTjwnroW4SxH90_f88-lyXDQFU80eyZ2BtP5cFXw0K314MGV7GiEoOc1e6Jd34D9S2t99e9MTQHutlr6LmaToQRS8pkHIHShc";
                string senderId = "949592693346";
                string deviceId = deviceID;
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                var data = new
                {
                    to = deviceId,

                    data = new
                    {
                        body = msg,
                        title = "القطيف",
                        sound = "Enabled"

                    }
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }

                return Json(new
                {
                    key = 1,
                    msgs = msg
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return Json(new
                {
                    key = 0,
                    msgs = "فشل الارسال"
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
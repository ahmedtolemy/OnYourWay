using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OnYourWay.Controllers
{
    public class BaseController : Controller
    {
        #region Enums
        public enum SubscribType {Monthly=0,Money=1 }
        public enum Category {People=1,Box=2,Goods=3,Animal=4,Shipping=5,People_Box=6,Goods_Animals=7};
        public enum peopleType {Person=1,Family=2 }
        public enum AnimalType { Animal = 1, Feed = 2 }
        public enum ToolType { Tools = 1, Goods = 2 }

        public enum Language
        {
            AR = 1, EN = 2, OR = 3

        };
        public enum TripTime
        { Now = 1, Later = 2 };
        public enum TripOffferStatus
        { CheckOffer = 1, AcceptedOffer = 2 };
        public enum TripStatus
        {NotStarted=1,InProgress=2,Completed=3,Canceled=4 };
        public enum PersonType {
            Client =1,Customer=2,Company=3 }
        #endregion
        public string TripStatusName(string lang,int? status)
        {
            switch (status)
            {
                case (int)TripStatus.NotStarted:
                    return massege(lang, "لم تبدا", "Not started", "لم تبدا");
                case (int)TripStatus.InProgress:
                    return massege(lang, "فعال", "Active", "فعال");
                case (int)TripStatus.Completed:
                    return massege(lang, "أنتهت", "Done", "أنتهت");
                case (int)TripStatus.Canceled:
                    return massege(lang, "الغت", "}Canceled", "الغت");
                default:
                    return "";
            }
        }
        public string OfferName(string lang,int? status)
        {
            switch (status)
            {
                case (int)TripOffferStatus.CheckOffer:
                    return massege(lang, "الاطلاع علي العروض", "Check Offer", "الاطلاع علي العروض");
                case (int)TripOffferStatus.AcceptedOffer:
                    return massege(lang,"موافقة علي العرض","Accepted Offer", "موافقة علي العرض");
                default:
                    return "";
                    
            }
        }
        public string massege(string lang,string ar,string en,string or)
        {
            if (lang==Language.AR.ToString())
            {
                return ar;
            }
            else if (lang.ToUpper() == Language.EN.ToString())
            {
                return en;
            }
            else
            {
                return or;
            }
        }
        public string CategoryName(string lang,int? category)
        {
            

            switch (category)
            {
                case (int)Category.People:
                    return massege(lang,"أشخاص","people","اشخاص");
                case (int)Category.Box:
                    return massege(lang, "طرود", "Box", "طرود");
                case (int)Category.Goods:
                    return massege(lang, "بضائع", "Goods", "بضائع");
                case (int)Category.Animal:
                    return massege(lang, "حيوانات واعلاف", "animals", "حيوانات واعلاف");
                case (int)Category.Shipping:
                    return massege(lang, "شحن دولي", "Shipping", "شحن دولي");
                case (int)Category.People_Box:
                    return massege(lang, "طرود وتوصيل ركاب", "People & Box", "طرود وتوصيل ركاب");
                case (int)Category.Goods_Animals:
                    return massege(lang, "نقل عام", "Goods & Animals", "نقل عام");
                default:
                    return "";
            }
        }
        public string ImgUrl(HttpPostedFileBase img,string foldername)
        {
            if (img != null)
            {
                var fileName = Guid.NewGuid() + ".png";
                var path = Path.Combine(Server.MapPath("~/Content/"+foldername+"/"), fileName);
                img.SaveAs(path);
                return fileName;
            }
            else
            {
                return "";
            }
        }
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public string SendMessage(string msg, string numbers)
        {
            //int temp = '0';

            HttpWebRequest req = (HttpWebRequest)
            WebRequest.Create("http://www.mobily.ws/api/msgSend.php");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            string postData = "mobile=966595550509" + "&password=" + "S1m2k3r4h5" + "&numbers=" + numbers + "&sender=" + "HRS-ME.COM" + "&msg=" + msg + "&applicationType=68&lang=3";
            req.ContentLength = postData.Length;

            StreamWriter stOut = new
            StreamWriter(req.GetRequestStream(),
            System.Text.Encoding.ASCII);
            stOut.Write(postData);
            stOut.Close();
            // Do the request to get the response
            string strResponse;
            StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
            strResponse = stIn.ReadToEnd();
            stIn.Close();
            return strResponse;
        }
    }
}
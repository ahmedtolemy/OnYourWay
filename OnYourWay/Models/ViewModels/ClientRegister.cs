using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.ViewModels
{
    public class ClientRegister
    {
        public string language { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string city_id { get; set; }
        public string password { get; set; }
        public string token { get; set; }
        public HttpPostedFileBase license_img { get; set; }
        public HttpPostedFileBase car_img { get; set; }
        public HttpPostedFileBase driver_img { get; set; }
        public HttpPostedFileBase insurance_img { get; set; }
        public string car_char { get; set; }
        public string car_seats { get; set; }
        public string car_color { get; set; }
        public string car_number { get; set; }
        public string tape { get; set; }
        public string select_1 { get; set; }
        public string select_2 { get; set; }
        public string select_3 { get; set; }
        public string select_4 { get; set; }
    }
}
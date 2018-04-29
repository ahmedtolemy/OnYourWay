using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.ViewModels
{
    public class BasicOrder
    {
        public string language { get; set; }
        public int? customer_id { get; set; }
        public int? type { get; set; }
        public string start_lat { get; set; }
        public string start_lng { get; set; }
        public string start_city { get; set; }
        public string end_lat { get; set; }
        public string end_lng { get; set; }
        public string end_city { get; set; }
        public int? sub_type { get; set; }
        public bool? now { get; set; }
        public string time { get; set; }
        public string note { get; set; }
        public int? people_count { get; set; }
        public int? weight { get; set; }
        public int? box_way { get; set; }
        public List<HttpPostedFileBase> img { get; set; }
        public int? car_count  { get; set; }
        public int? car_size { get; set; }
    }
}
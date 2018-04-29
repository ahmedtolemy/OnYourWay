using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.DBTables
{
    public class AppSetting
    {
        [Key]
        public int ID { get; set; }
        public string AboutAr { get; set; }
        public string AboutEn { get; set; }
        public string AboutOr { get; set; }
        public string StepsAr { get; set; }
        public string StepsEn { get; set; }
        public string StepsOr { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string FaceBook { get; set; }
        public string Twitter { get; set; }
        public string Web { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Instagram { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.DBTables
{
    public class CarCompany
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(50)]
        public string NameAR { get; set; }
        [MaxLength(50)]
        public string NameEN { get; set; }
        [MaxLength(50)]
        public string NameOR { get; set; }
        public bool? Block { get; set; }
        public bool? Delete { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public List<CarType> CarTypes { get; set; }
    }
}
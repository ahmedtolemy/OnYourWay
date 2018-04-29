using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.DBTables
{
    public class CarType
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
        [Index]
        public int? CarCompanyID { get; set; }
        [ForeignKey("CarCompanyID")]
        public CarCompany CarCompany { get; set; }
        public List<Client> Clients { get; set; }
    }
}
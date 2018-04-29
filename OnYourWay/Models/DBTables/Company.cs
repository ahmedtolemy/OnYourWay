using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.DBTables
{
    public class Company
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50),Phone]
        public string Phone { get; set; }
        public DateTime? RegisterDate { get; set; }
        public bool? Block { get; set; }
        public bool? Delete { get; set; }
        public string CompanyNumber  { get; set; }
        public string ImgUrl { get; set; }
        public string RecordNumber { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public List<Client> Clients { get; set; }

    }
}
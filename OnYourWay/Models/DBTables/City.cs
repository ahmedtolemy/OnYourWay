using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.DBTables
{
    public class City
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(50)]
        public string NameAR { get; set; }
        [MaxLength(50)]
        public string NameEN { get; set; }
        [MaxLength(50)]
        public string NameOR { get; set; }
        public bool? Delete { get; set; }
        public bool? Block { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public List<Client> Clients { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
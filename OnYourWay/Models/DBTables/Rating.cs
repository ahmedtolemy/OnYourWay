using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.DBTables
{
    public class Rating
    {
        [Key]
        public int ID { get; set; }
        public DateTime? Created_at { get; set; }
        public int? ClientID { get; set; }
        public int? CustomerID { get; set; }
        public int? Rate { get; set; }
        public string Comment { get; set; }
        [ForeignKey("ClientID")]
        public Client Client { get; set; }
        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.DBTables
{
    public class Complain
    {
        [Key]
        public int ID { get; set; }
        public string complain { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public bool? Delete { get; set; }
        public string name { get; set; }
        public int? PersonType  { get; set; }
        public int? ClientID { get; set; }
        public int? CustomerID { get; set; }
        [ForeignKey("ClientID")]
        public Client Client { get; set; }
        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
        public DateTime? Created_at { get; set; }

    }
}
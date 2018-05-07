using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace OnYourWay.Models.DBTables
{
    public class Subscription
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(128)]
        [ForeignKey("user")]
        public string userID { get; set; }
        [MaxLength(128)]
        [ForeignKey("Admin")]
        public string AdminID { get; set; }
        public int? ClientID { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        [MaxLength(100)]
        public string Balance { get; set; }
        public bool? Delete { get; set; }
        public bool? Block { get; set; }
        public int? Type { get; set; }
        [MaxLength(30)]
        public string Subscription_Count { get; set; }
        [ForeignKey("ClientID")]
        public Client Client { get; set; }

        public ApplicationUser user { get; set; }

        public ApplicationUser Admin { get; set; }
    }
}
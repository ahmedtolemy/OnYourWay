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
        public string UserID { get; set; }
        public int? ClientID { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        [MaxLength(100)]
        public string Balance { get; set; }
        [ForeignKey("ClientID")]
        public Client Client { get; set; }
        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; }
    }
}
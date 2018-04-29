using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.DBTables
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [EmailAddress]
        [MaxLength(60)]
        public string Email { get; set; }
        public string Token { get; set; }
        [Phone]
        [MaxLength(50)]
        public string Mobile { get; set; }
        public string ImgUrl { get; set; }
        public bool? Active { get; set; }
        [MaxLength(50),MinLength(6)]
        public string Password { get; set; }
        [MaxLength(20)]
        public string Language { get; set; }
        public bool? Delete { get; set; }
        public int? CityID { get; set; }
        public bool? Block { get; set; }
        public bool? Activation { get; set; }
        public bool? Notification { get; set; }
        public bool? Alarm { get; set; }
        public DateTime? RegisterDate { get; set; }
        [ForeignKey("CityID")]
        public City City { get; set; }
        public List<Trip> Trips { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<Complain> Complains { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
    }
}
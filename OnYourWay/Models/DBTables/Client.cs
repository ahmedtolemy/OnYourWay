using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.DBTables
{
    public class Client
    {
        public Client()
        {
            this.Categories = new HashSet<Category>();
        }
        [Key]
        public int ID { get; set; }
        public int? CompanyID { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        public string Token { get; set; }
        public bool? Delete { get; set; }
        public bool? Block { get; set; }
        [Phone]
        [MaxLength(50)]
        public string Mobile { get; set; }
        public int? CityID { get; set; }
        [MaxLength(50),MinLength(6)]
        public string Password { get; set; }
        public string ImgUrl { get; set; }
        public bool? Active { get; set; }
        public bool? Activation { get; set; }
        public string CarUrl { get; set; }
        public string LicenseImgUrl { get; set; }
        public string PlateNumber { get; set; }
        public string PlateChars { get; set; }
        public string InsuranceUrl { get; set; }
        public string Balance { get; set; }
        public DateTime? RegisterDate { get; set; }
        [MaxLength(20)]
        public string Language { get; set; }
        public bool? Notification { get; set; }
        public bool? Alarm { get; set; }

        public int? CarSeats { get; set; }
        [MaxLength(30)]
        public string CarColor { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public int? Type { get; set; }
        [MaxLength(20)]
        public string CarModel { get; set; }
        [MaxLength(128)]
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; }
        public int? CarTypeID { get; set; }
        [ForeignKey("CompanyID")]
        public Company Company { get; set; }
        [ForeignKey("CarTypeID")]
        public CarType CarType { get; set; }
        [ForeignKey("CityID")]
        public City City { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<Offer> Offers { get; set; }
        public List<Complain> Complains { get; set; }
        public ICollection<Category> Categories { get; set; }
        public List<Subscription> subscriptions { get; set; }

    }
}
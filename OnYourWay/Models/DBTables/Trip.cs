using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.DBTables
{
    public class Trip
    {
        [Key]
        public int ID { get; set; }
        public int? CustomerID { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string StartLng { get; set; }
        public string EndLng { get; set; }
        public string StartLat { get; set; }
        public string EndLat { get; set; }
        public int? TripTimeStatus { get; set; }
        public int? OfferStatus { get; set; }
        public bool? Active { get; set; }
        public int? Status { get; set; }
        public string Note { get; set; }
        #region category
        public int? CategoryID { get; set; }
        public int? CarType { get; set; }
        public int? CarsCount { get; set; }
        public int? Weight { get; set; }
        public string ImgUrl { get; set; }
        public int? PeopleType { get; set; }
        public int? PeopleCount { get; set; }
        public int? BoxType { get; set; }
        public int? BoxWay { get; set; }
        public int? AnimalsType { get; set; }
        public int? GoodsType { get; set; }
        public int? ShippingType { get; set; }
        #endregion
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        [ForeignKey("Offer")]
        public int? AcceptedOfferID { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        [InverseProperty("Trip")]
        public ICollection<Offer> Offers { get; set; }
        [ForeignKey("CategoryID")]
        public Category MainCategory { get; set; }
        [ForeignKey("PeopleType")]
        public Category PeopleCategory { get; set; }
        [ForeignKey("BoxType")]
        public Category BoxCategory { get; set; }
        [ForeignKey("AnimalsType")]
        public Category AnimalCategory { get; set; }
        [ForeignKey("ShippingType")]
        public Category ShippingCategory { get; set; }
        [ForeignKey("GoodsType")]
        public Category GoodsCategory { get; set; }
        public Offer Offer { get; set; }
        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
    }
}
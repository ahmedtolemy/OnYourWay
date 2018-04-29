using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.DBTables
{
    public class Offer
    {
        [Key]
        public int ID { get; set; }
        public int? ClientID { get; set; }
        public Double? Money { get; set; }
        public string Note { get; set; }
        public DateTime? OfferDate { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public bool? Approved { get; set; }
        [ForeignKey("Trip")]
        public int? TripID { get; set; }
        public Trip Trip { get; set; }
        [ForeignKey("ClientID")]
        public Client Client { get; set; }
        [InverseProperty("Offer")]
        public ICollection<Trip> AcceptedTrips { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnYourWay.Models.DBTables;

namespace OnYourWay.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string ImageUrl { get; set; }
        public bool Deleted { get; set; }
        public bool Active { get; set; }
        public string Balance { get; set; }
        public int? Subscription_count { get; set; }
        public DateTime RegisterDate { get; set; }
        public List<Client> Clients { get; set; }
        [InverseProperty("user")]
        public List<Subscription> Subscriptions { get; set; }
        [InverseProperty("Admin")]
        public List<Subscription> AdminSubscriptions { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("ImageUrl", this.ImageUrl.ToString()));
            userIdentity.AddClaim(new Claim("UserID", this.Id.ToString()));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<AppSetting> AppSettings { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<City>  Cities { get; set; }
        public virtual DbSet<CarCompany> CarCompanies { get; set; }
        public virtual DbSet<CarType> CarTypes { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Complain> Complains { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
       
        public virtual DbSet<Subscription> Subscriptions { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

       // public System.Data.Entity.DbSet<OnYourWay.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}
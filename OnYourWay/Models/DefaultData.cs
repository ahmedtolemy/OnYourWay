using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using OnYourWay.Models.DBTables;


namespace OnYourWay.Models
{
    public class DefaultData: DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var role1 = new IdentityRole() { Id = "1", Name = "Admin" };
                context.Roles.Add(role1 );
            var role2 = new IdentityRole() { Id = "2", Name = "Manager" };
                context.Roles.Add(role2);
                context.SaveChanges();
            var user = new ApplicationUser()
            {
                Id = "ce132fc9-4f7a-4156-a118-dc647da67b13",
                Email = "Admin@aait.com",
                EmailConfirmed = false,
                PasswordHash = "AIfLm9o1xIltnSYi6OLVGbagJASigUA7xBfrntv5VB79lf5NacxVk9fIGcYwRAhXRw==",
                SecurityStamp = "7b633a76-c473-4ced-b585-c610edb76bd8",
                PhoneNumber = "01000000000",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                LockoutEndDateUtc = null,
                AccessFailedCount = 0,
                UserName = "admin@@",
                ImageUrl = "636588059971872818636588059971872818.png",
                Deleted = false,
                Active = true,
                RegisterDate = DateTime.Now,

            };
            user.Roles.Add(new IdentityUserRole() { RoleId="1"});
            user.Roles.Add(new IdentityUserRole() { RoleId = "2" });
            context.Users.Add(user);
            List<Category> categories = new List<Category>()
            {
                new Category(){ID=1,NameEN="People"},
                new Category(){ID=1,NameEN="Box"},
                new Category(){ID=1,NameEN="Goods"},
                new Category(){ID=1,NameEN="Animals"},
                new Category(){ID=1,NameEN="Shipping"}
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();



        }
    }
}
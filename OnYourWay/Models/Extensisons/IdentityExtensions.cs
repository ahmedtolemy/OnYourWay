using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace OnYourWay.Models.Extensisons
{
    public static class IdentityExtensions
    {
        public static string GetImgUrl(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ImageUrl");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetAccessLevel(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("AccessLevel");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetUserID(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("UserID");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }
            else if (httpContext.User.IsInRole("Manager"))
            {
                return true;
            }
            var x = httpContext.User.Identity.GetAccessLevel().ToLower();
            // string privilegeLevels = string.Join("", GetUserRights(httpContext.User.Identity.Name.ToString())); // Call another method to get rights of the user from DB
            return httpContext.User.Identity.GetAccessLevel().ToLower().Contains(this.AccessLevel.ToLower());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

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
        public static string GetUserID(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("UserID");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}
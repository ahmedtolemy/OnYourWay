using System.Web;
using System.Web.Mvc;

namespace OnYourWay
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            filters.Add(new AuthorizeAttribute() { Roles = "Admin, Manager" });
          
        }
       
    }
    
}

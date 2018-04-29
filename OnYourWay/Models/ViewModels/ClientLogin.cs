using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.ViewModels
{
    
    public class ClientLogin
    {
        public string phone { get; set; }
        public string password { get; set; }
        public string language { get; set; }
        public string token { get; set; }
    }
}
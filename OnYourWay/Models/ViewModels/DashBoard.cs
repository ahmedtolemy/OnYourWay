using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.ViewModels
{
    public class DashBoard
    {
        public int customers { get; set; }
        public int clients { get; set; }
        public int admins { get; set; }
        public int donetrips { get; set; }
        public int trips { get; set; }
        public int offers { get; set; }
        public int complains { get; set; }
        public int main_category { get; set; }
        public int category { get; set; }

    }
}
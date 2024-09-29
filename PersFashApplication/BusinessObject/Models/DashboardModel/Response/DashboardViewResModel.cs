using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.DashboardModel.Response
{
    public class DashboardViewResModel
    {
        public int TotalUser { get; set; }
        public decimal RevenueToday { get; set; }
        public decimal RevenueThisWeek { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public decimal TotalPremiumSubscription { get; set; }
        public decimal RevenueInDayRange { get; set; }
    }
}

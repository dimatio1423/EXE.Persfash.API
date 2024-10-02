using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.DashboardModel.Response
{
    public class DashboardViewDateRange 
    {
        public decimal TotalRevenueDateRange { get; set; }

        public Dictionary<DateOnly, decimal> RevenueDateRange { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.DashboardModel.Request
{
    public class DashboardReqModel
    {
        public DateOnly? startDate { get; set; }
        public DateOnly? endDate { get; set; }
    }
}

using BusinessObject.Models.DashboardModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AdminServices
{
    public interface IAdminService
    {
        Task<DashboardViewResModel> ViewDashboard(string token);
        Task<DashboardViewDateRange> ViewTotalRevenueByDateRange(string token, DateOnly? startDate, DateOnly? endDate);
    }
}

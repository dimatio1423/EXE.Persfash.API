using BusinessObject.Enums;
using BusinessObject.Models.DashboardModel.Response;
using Repositories.PaymentRepos;
using Repositories.UserRepos;
using Repositories.UserSubscriptionRepos;
using Services.AdminServices;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICustomerSubscriptionRepository _customerSubscriptionRepository;
        private readonly IDecodeTokenHandler _decodeToken;

        public AdminService(ICustomerRepository customerRepository,
            IPaymentRepository paymentRepository, 
            ICustomerSubscriptionRepository customerSubscriptionRepository,
            IDecodeTokenHandler decodeToken)
        {
            _customerRepository = customerRepository;
            _paymentRepository = paymentRepository;
            _customerSubscriptionRepository = customerSubscriptionRepository;
            _decodeToken = decodeToken;
        }
        public async Task<DashboardViewResModel> ViewDashboard(string token)
        {

            var decodeToken = _decodeToken.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Admin.ToString()))
            {
                throw new ApiException(System.Net.HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var totalCustomer = await _customerRepository.GetAll();

            var totalPremiumCustomer = await _customerSubscriptionRepository.GetTotalPremiumCustomer();

            var revenueToday = await _paymentRepository.GetTotalRevenueToday(DateOnly.FromDateTime(DateTime.Now));

            var revenueWeek = await _paymentRepository.GetTotalRenenueForWeek(DateTime.Now);

            var revenueMonth = await _paymentRepository.GetTotalRevenueForMonth(DateTime.Now);


            return new DashboardViewResModel
            {
                TotalUser = totalCustomer.Count,
                TotalPremiumSubscription = totalPremiumCustomer,
                RevenueToday = revenueToday,
                RevenueThisWeek = revenueWeek,
                RevenueThisMonth = revenueMonth,
            };
        }

        public async Task<DashboardViewDateRange> ViewTotalRevenueByDateRange(string token, DateOnly? startDate, DateOnly? endDate)
        {
            DateOnly start = (startDate.HasValue) ? startDate.Value : DateOnly.FromDateTime(DateTime.Now);
            DateOnly end = (endDate.HasValue) ? endDate.Value : start.AddDays(6);

            if (start > end)
            {
                DateOnly tmp = start;
                start = end;
                end = tmp;
            }

            var revenueDateRange = await _paymentRepository.GetTotalRevenueForDayRange(startDate, endDate);

            Dictionary<DateOnly, decimal> revenueDateRangeList = new Dictionary<DateOnly, decimal>();

            DateOnly currDate = start;

            while (currDate <= end)
            {
                var totalRevenueByDate = await _paymentRepository.GetTotalRevenueToday(currDate);

                revenueDateRangeList.Add(currDate, totalRevenueByDate);

                currDate = currDate.AddDays(1);
            }

            return new DashboardViewDateRange
            {
                TotalRevenueDateRange = revenueDateRange,
                RevenueDateRange = revenueDateRangeList
            };

        }
    }
}

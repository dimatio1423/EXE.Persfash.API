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
        public async Task<DashboardViewResModel> ViewDashboard(string token, DateOnly? startDate, DateOnly? endDate)
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

            var revenueInDateRange = await _paymentRepository.GetTotalRevenueForDayRange(startDate, endDate);


            return new DashboardViewResModel
            {
                TotalUser = totalCustomer.Count,
                TotalPremiumSubscription = totalPremiumCustomer,
                RevenueToday = revenueToday,
                RevenueThisWeek = revenueWeek,
                RevenueThisMonth = revenueMonth,
                RevenueInDayRange = revenueInDateRange
            };
        }
    }
}

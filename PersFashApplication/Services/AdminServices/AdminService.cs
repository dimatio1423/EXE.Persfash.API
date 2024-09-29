using BusinessObject.Models.DashboardModel.Response;
using Repositories.PaymentRepos;
using Repositories.UserRepos;
using Repositories.UserSubscriptionRepos;
using Services.AdminServices;
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

        public AdminService(ICustomerRepository customerRepository, IPaymentRepository paymentRepository, ICustomerSubscriptionRepository customerSubscriptionRepository)
        {
            _customerRepository = customerRepository;
            _paymentRepository = paymentRepository;
            _customerSubscriptionRepository = customerSubscriptionRepository;
        }
        public async Task<DashboardViewResModel> ViewDashboard(DateOnly? startDate, DateOnly? endDate)
        {
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

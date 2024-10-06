using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UserSubscriptionRepos
{
    public interface ICustomerSubscriptionRepository : IGenericRepository<CustomerSubscription>
    {
        Task<List<CustomerSubscription>> GetCustomerSubscriptionBySubscriptionId(int subscriptionId);
        Task<List<CustomerSubscription>> GetCustomerSubscriptionByCustomerId(int customerId);
        Task<CustomerSubscription> GetCustomerSubscription(int customerSubscriptionId);
        Task<CustomerSubscription> GetCustomerSubscriptionByCustomerIdAndSubscriptionId(int customerId, int subscriptionId);
        Task<List<CustomerSubscription>> GetCustomerSubscriptionWithDateAndStatusActive();
        Task<int> GetTotalPremiumCustomer();
    }
}

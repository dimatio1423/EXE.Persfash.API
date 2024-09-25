using BusinessObject.Entities;
using BusinessObject.Enums;
using Repositories.SubscriptionRepos;
using Repositories.UserCourseRepos;
using Repositories.UserSubscriptionRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UserSubscriptionServices
{
    public class CustomerSubscriptionService : ICustomerSubscriptionService
    {
        private readonly ICustomerSubscriptionRepository _customerSubscriptionRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public CustomerSubscriptionService(ICustomerSubscriptionRepository customerSubscriptionRepository,
            ISubscriptionRepository subscriptionRepository)
        {
            _customerSubscriptionRepository = customerSubscriptionRepository;
            _subscriptionRepository = subscriptionRepository;
        }
        public async Task<string> AutoUpdatingCustomerSubscriptionStatus()
        {
            var activeCustomerSubscription = await _customerSubscriptionRepository.GetCustomerSubscriptionWithDateAndStatusActive();

            var updatedCustomerSubscription = new List<CustomerSubscription>();

            var updatedCustomerFreeSubscription = new List<CustomerSubscription>();

            foreach (var item in activeCustomerSubscription)
            {
                if (item.EndDate < DateTime.Now)
                {
                    item.IsActive = false;

                    updatedCustomerSubscription.Add(item);

                    var freeSubscription = await _subscriptionRepository.GetSubscriptionsByName(SubscriptionTypeEnums.Free.ToString());

                    var currCustomerFreeSubscription = await _customerSubscriptionRepository.GetCustomerSubscriptionByCustomerIdAndSubscriptionId(item.CustomerId, freeSubscription.SubscriptionId);

                    if (currCustomerFreeSubscription == null) continue;

                    currCustomerFreeSubscription.IsActive = true;

                    updatedCustomerFreeSubscription.Add(currCustomerFreeSubscription);
                }
            }

            if (updatedCustomerSubscription.Count > 0)
            {
                await _customerSubscriptionRepository.UpdateRange(updatedCustomerSubscription);
            }

            if (updatedCustomerFreeSubscription.Count > 0)
            {
                await _customerSubscriptionRepository.UpdateRange(updatedCustomerFreeSubscription);
            }

            return "Updating...";
        }
    }
}

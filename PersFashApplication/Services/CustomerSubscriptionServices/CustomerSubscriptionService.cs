using BusinessObject.Entities;
using BusinessObject.Enums;
using Repositories.SubscriptionRepos;
using Repositories.UserCourseRepos;
using Repositories.UserRepos;
using Repositories.UserSubscriptionRepos;
using Services.EmailService;
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
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailService _emailService;

        public CustomerSubscriptionService(ICustomerSubscriptionRepository customerSubscriptionRepository,
            ISubscriptionRepository subscriptionRepository, 
            ICustomerRepository customerRepository,
            IEmailService emailService)
        {
            _customerSubscriptionRepository = customerSubscriptionRepository;
            _subscriptionRepository = subscriptionRepository;
            _customerRepository = customerRepository;
            _emailService = emailService;
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

                    var currCustomer = await _customerRepository.Get(item.CustomerId);

                    var currCustomerFreeSubscription = await _customerSubscriptionRepository.GetCustomerSubscriptionByCustomerIdAndSubscriptionId(item.CustomerId, freeSubscription.SubscriptionId);

                    if (currCustomerFreeSubscription == null) continue;

                    currCustomerFreeSubscription.IsActive = true;

                    updatedCustomerFreeSubscription.Add(currCustomerFreeSubscription);

                    await _emailService.SendEmailForExpireSubscription(currCustomer.FullName, currCustomer.Email);
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

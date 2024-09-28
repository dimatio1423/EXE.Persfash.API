using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Models.CustomerModels.Response;
using Repositories.UserRepos;
using Repositories.UserSubscriptionRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helper.Resolver.CustomerResolver
{
    public class GetCustomerSubscriptionResolver : IValueResolver<Customer, CustomerInformationViewModel, List<string>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerSubscriptionRepository _customerSubscriptionRepository;

        public GetCustomerSubscriptionResolver(ICustomerRepository customerRepository, ICustomerSubscriptionRepository customerSubscriptionRepository)
        {
            _customerRepository = customerRepository;
            _customerSubscriptionRepository = customerSubscriptionRepository;
        }
        public List<string> Resolve(Customer source, CustomerInformationViewModel destination, List<string> destMember, ResolutionContext context)
        {
            var currCustomer = _customerRepository.Get(source.CustomerId).GetAwaiter().GetResult();

            var currCustomerSubscriptions = _customerSubscriptionRepository.GetCustomerSubscriptionByCustomerId(currCustomer.CustomerId).GetAwaiter().GetResult();

            return currCustomerSubscriptions.Where(x => x.IsActive == true).Select(x => x.Subscription.SubscriptionTitle).ToList();
        }
    }
}

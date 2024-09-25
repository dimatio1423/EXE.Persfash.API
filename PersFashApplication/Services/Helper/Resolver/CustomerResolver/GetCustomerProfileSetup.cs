using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Models.CustomerModels.Response;
using Repositories.UserProfilesRepos;
using Repositories.UserRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helper.Resolver.CustomerResolver
{
    internal class GetCustomerProfileSetup : IValueResolver<Customer, CustomerInformationViewModel, bool?>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerProfileRepository _customerProfileRepository;

        public GetCustomerProfileSetup(ICustomerRepository customerRepository, ICustomerProfileRepository customerProfileRepository)
        {
            _customerRepository = customerRepository;
            _customerProfileRepository = customerProfileRepository;
        }
        public bool? Resolve(Customer source, CustomerInformationViewModel destination, bool? destMember, ResolutionContext context)
        {
            var currCustomer = _customerRepository.Get(source.CustomerId).GetAwaiter().GetResult();

            var currCustomerProfileSetup = _customerProfileRepository.GetCustomerProfileByCustomerId(currCustomer.CustomerId).GetAwaiter().GetResult();

            return currCustomerProfileSetup != null ? true : false;
        }
    }
}

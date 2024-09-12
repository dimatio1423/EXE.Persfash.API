using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.CustomerModels.Request;
using BusinessObject.Models.CustomerModels.Response;
using BusinessObject.Models.FashionItemsModel.Request;
using Repositories.FashionInfluencerRepos;
using Repositories.PartnerRepos;
using Repositories.SystemAdminRepos;
using Repositories.UserProfilesRepos;
using Repositories.UserRepos;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.UserServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerProfileRepository _customerProfileRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IFashionInfluencerRepository _fashionInfluencerRepository;
        private readonly IMapper _mapper;
        private readonly IDecodeTokenHandler _decodeTokenHandler;
        private readonly ISystemAdminRepository _systemAdminRepository;

        public CustomerService(ICustomerRepository customerRepository, 
            ICustomerProfileRepository customerProfileRepository,
            IPartnerRepository partnerRepository,
            IFashionInfluencerRepository fashionInfluencerRepository,
            IMapper mapper,
            IDecodeTokenHandler decodeTokenHandler,
            ISystemAdminRepository systemAdminRepository)
        {
            _customerRepository = customerRepository;
            _customerProfileRepository = customerProfileRepository;
            _partnerRepository = partnerRepository;
            _fashionInfluencerRepository = fashionInfluencerRepository;
            _mapper = mapper;
            _decodeTokenHandler = decodeTokenHandler;
            _systemAdminRepository = systemAdminRepository;
        }
        public Task CustomerProfileSetup(string token, CustomerProfileSetupReqModel customerProfileSetupReqModel)
        {
            throw new NotImplementedException();
        }

        public async Task CustomerReigster(CustomerRegisterReqModel customerRegisterReqModel)
        {
            if ( await checkUsernameExisted(customerRegisterReqModel.Username))
            {
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Username has already been used by another user");
            }

            if (await checkEmailExisted(customerRegisterReqModel.Email))
            {
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Email has already been used by another user");
            }

            if (!Enum.IsDefined(typeof(GenderEnums), customerRegisterReqModel.Gender))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Invalid gender target type");
            }

            Customer customer = new Customer
            {
                Username = customerRegisterReqModel.Username,
                Email = customerRegisterReqModel.Email,
                Password = PasswordHasher.HashPassword(customerRegisterReqModel.Password),
                FullName = customerRegisterReqModel.FullName,
                Gender = customerRegisterReqModel.Gender,
                DateOfBirth = customerRegisterReqModel.DateOfBirth,
                DateJoined = DateTime.Now,
                Status = StatusEnums.Active.ToString(),
            };

            await _customerRepository.Add(customer);
        }

        public Task<CustomerProfileViewModel> GetCustomerProfile(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> checkUsernameExisted(string username)
        {
            return (await _customerRepository.IsExistedByUsername(username) ||
                await _partnerRepository.IsExistedByUsername(username) ||
                await _fashionInfluencerRepository.IsExistedByUsername(username) ||
                await _systemAdminRepository.IsExistedByUsername(username));
        }

        public async Task<bool> checkEmailExisted(string email)
        {
            return (await _customerRepository.IsExistedByEmail(email) ||
                await _partnerRepository.IsExistedByEmail(email) ||
                await _fashionInfluencerRepository.IsExistedByEmail(email));
        }
    }
}

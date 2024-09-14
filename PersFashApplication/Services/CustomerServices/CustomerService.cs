using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.CustomerModels.Request;
using BusinessObject.Models.CustomerModels.Response;
using BusinessObject.Models.FashionItemsModel.Request;
using Repositories.FashionInfluencerRepos;
using Repositories.PartnerRepos;
using Repositories.SubscriptionRepos;
using Repositories.SystemAdminRepos;
using Repositories.UserProfilesRepos;
using Repositories.UserRepos;
using Repositories.UserSubscriptionRepos;
using Services.EmailService;
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
        private readonly ICustomerSubscriptionRepository _customerSubscriptionRepository;
        private readonly IMapper _mapper;
        private readonly IDecodeTokenHandler _decodeTokenHandler;
        private readonly ISystemAdminRepository _systemAdminRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IEmailService _emailService;

        public CustomerService(ICustomerRepository customerRepository, 
            ICustomerProfileRepository customerProfileRepository,
            IPartnerRepository partnerRepository,
            IFashionInfluencerRepository fashionInfluencerRepository,
            IMapper mapper,
            IDecodeTokenHandler decodeTokenHandler,
            ISystemAdminRepository systemAdminRepository,
            ISubscriptionRepository subscriptionRepository,
            ICustomerSubscriptionRepository customerSubscriptionRepository,
            IEmailService emailService) 
        {
            _customerRepository = customerRepository;
            _customerProfileRepository = customerProfileRepository;
            _partnerRepository = partnerRepository;
            _fashionInfluencerRepository = fashionInfluencerRepository;
            _customerSubscriptionRepository = customerSubscriptionRepository;
            _mapper = mapper;
            _decodeTokenHandler = decodeTokenHandler;
            _systemAdminRepository = systemAdminRepository;
            _subscriptionRepository = subscriptionRepository;
            _emailService = emailService;
        }
        public async Task CustomerProfileSetup(string token, CustomerProfileSetupReqModel customerProfileSetupReqModel)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currCustomerProfile = await _customerProfileRepository.GetCustomerProfileByCustomerId(currCustomer.CustomerId);

            if (currCustomerProfile != null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, $"Customer has already finish profile setup");
            }

            CustomerProfile customerProfile = new CustomerProfile
            {
                CustomerId = currCustomer.CustomerId,
                BodyType = customerProfileSetupReqModel.BodyType,
                FashionStyle = string.Join(", ", customerProfileSetupReqModel.FashionStyle),
                FitPreferences = string.Join(", ", customerProfileSetupReqModel.FitPreferences),
                PreferredSize = string.Join(", ", customerProfileSetupReqModel.PreferredSize),
                PreferredColors = string.Join(", ", customerProfileSetupReqModel.PreferredColors),
                PreferredMaterials = string.Join(", ", customerProfileSetupReqModel.PreferredMaterials),
                Occasion = string.Join(", ", customerProfileSetupReqModel.Occasion),
                Lifestyle = string.Join(", ", customerProfileSetupReqModel.Lifestyle),
                ProfileSetupComplete = true,
            };

            await _customerProfileRepository.Add(customerProfile);
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

            var customerId = await _customerRepository.AddCustomer(customer);

            var subscription = await _subscriptionRepository.GetSubscriptionsByName(SubscriptionTypeEnums.Free.ToString());

            if (subscription == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Subscription does not exist");
            }

            CustomerSubscription customerSubscription = new CustomerSubscription
            {
                SubscriptionId = subscription.SubscriptionId,
                CustomerId = customerId,
                StartDate = null,
                EndDate = null,
                IsActive = true,
            };

            await _customerSubscriptionRepository.Add(customerSubscription);

            await _emailService.SendRegistrationEmail(customer.FullName, customer.Email);
        }

        public async Task<CustomerProfileViewModel> GetCustomerProfile(string token)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currCustomerProfile = await _customerProfileRepository.GetCustomerProfileByCustomerId(currCustomer.CustomerId);

            if (currCustomerProfile == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Profile of customer with id {currCustomer.CustomerId} does not exist");
            }

            return _mapper.Map<CustomerProfileViewModel>(currCustomerProfile);
        }

        public async Task CustomerProfileSetupUpdate(string token, CustomerProfileSetupUpdateReqModel customerProfileSetupUpdateReqModel)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currCustomerProfile = await _customerProfileRepository.Get(customerProfileSetupUpdateReqModel.ProfileId);

            if (currCustomerProfile == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer profile does not exist");
            }

            if (currCustomer.CustomerId != currCustomerProfile.CustomerId)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify other customer profile");
            }


            currCustomerProfile.BodyType = !string.IsNullOrEmpty(customerProfileSetupUpdateReqModel.BodyType) ? customerProfileSetupUpdateReqModel.BodyType : currCustomerProfile.BodyType;

            currCustomerProfile.FashionStyle = (customerProfileSetupUpdateReqModel.FashionStyle != null) 
                ? string.Join(", ", customerProfileSetupUpdateReqModel.FashionStyle) : currCustomerProfile.FashionStyle;

            currCustomerProfile.FitPreferences = (customerProfileSetupUpdateReqModel.FitPreferences != null)
                ? string.Join(", ", customerProfileSetupUpdateReqModel.FitPreferences) : currCustomerProfile.FitPreferences;

            currCustomerProfile.PreferredSize = (customerProfileSetupUpdateReqModel.PreferredSize != null)
                ? string.Join(", ", customerProfileSetupUpdateReqModel.PreferredSize) : currCustomerProfile.PreferredSize;

            currCustomerProfile.PreferredColors = (customerProfileSetupUpdateReqModel.PreferredColors != null)
                ? string.Join(", ", customerProfileSetupUpdateReqModel.PreferredColors) : currCustomerProfile.PreferredColors;

            currCustomerProfile.PreferredMaterials = (customerProfileSetupUpdateReqModel.PreferredMaterials != null)
                ? string.Join(", ", customerProfileSetupUpdateReqModel.PreferredMaterials) : currCustomerProfile.PreferredMaterials;

            currCustomerProfile.Occasion = (customerProfileSetupUpdateReqModel.Occasion != null)
                ? string.Join(", ", customerProfileSetupUpdateReqModel.Occasion) : currCustomerProfile.Occasion;

            currCustomerProfile.Lifestyle = (customerProfileSetupUpdateReqModel.Lifestyle != null)
                ? string.Join(", ", customerProfileSetupUpdateReqModel.Lifestyle) : currCustomerProfile.Lifestyle;

            currCustomerProfile.FacebookLink = !string.IsNullOrEmpty(customerProfileSetupUpdateReqModel.FacebookLink) ? customerProfileSetupUpdateReqModel.FacebookLink : currCustomerProfile.FacebookLink;
            currCustomerProfile.InstagramLink = !string.IsNullOrEmpty(customerProfileSetupUpdateReqModel.InstagramLink) ? customerProfileSetupUpdateReqModel.InstagramLink : currCustomerProfile.InstagramLink;
            currCustomerProfile.TikTokLink = !string.IsNullOrEmpty(customerProfileSetupUpdateReqModel.TikTokLink) ? customerProfileSetupUpdateReqModel.TikTokLink : currCustomerProfile.TikTokLink;


            await _customerProfileRepository.Update(currCustomerProfile);
        }

        public async Task<CustomerInformationViewModel> GetCustomerInformation(int customerId)
        {
            var currCustomer = await _customerRepository.Get(customerId);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            return _mapper.Map<CustomerInformationViewModel>(currCustomer);
        }

        public async Task UpdateCustomerInformation(string token, CustomerInformationUpdateReqModel customerInformationUpdateReqModel)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            if (currCustomer.CustomerId != customerInformationUpdateReqModel.CustomerId)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify other customer information");
            }


            if (!string.IsNullOrEmpty(customerInformationUpdateReqModel.Gender) 
                && !Enum.IsDefined(typeof(GenderEnums), customerInformationUpdateReqModel.Gender))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Invalid gender type");
            }

            if (!string.IsNullOrEmpty(customerInformationUpdateReqModel.Email)
                && !customerInformationUpdateReqModel.Email.Equals(currCustomer.Email)
                && await checkEmailExisted(customerInformationUpdateReqModel.Email))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Email has already been used by another user");
            }

            currCustomer.Email = !string.IsNullOrEmpty(customerInformationUpdateReqModel.Email) ? customerInformationUpdateReqModel.Email : currCustomer.Email;
            currCustomer.FullName = !string.IsNullOrEmpty(customerInformationUpdateReqModel.FullName) ? customerInformationUpdateReqModel.FullName : currCustomer.FullName;
            currCustomer.Gender = !string.IsNullOrEmpty(customerInformationUpdateReqModel.Gender) ? customerInformationUpdateReqModel.Gender : currCustomer.Gender;
            currCustomer.DateOfBirth = customerInformationUpdateReqModel.DateOfBirth != null ? customerInformationUpdateReqModel.DateOfBirth : currCustomer.DateOfBirth;


            await _customerRepository.Update(currCustomer);
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

        public async Task<bool> CheckCustomerProfileExisted(string token)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currCustomerProfile = await _customerProfileRepository.GetCustomerProfileByCustomerId(currCustomer.CustomerId);

            return currCustomerProfile != null ? true : false;
        }
    }
}

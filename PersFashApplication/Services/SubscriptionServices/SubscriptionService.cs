using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.CustomerSubscriptionModel.Response;
using BusinessObject.Models.PaymentModel.Request;
using BusinessObject.Models.SubscriptionModels.Request;
using BusinessObject.Models.SubscriptionModels.Response;
using BusinessObject.Models.VnPayModel.Request;
using Microsoft.AspNetCore.Http;
using Repositories.PaymentRepos;
using Repositories.SubscriptionRepos;
using Repositories.SystemAdminRepos;
using Repositories.UserRepos;
using Repositories.UserSubscriptionRepos;
using Services.EmailService;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using Services.VnPayService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.SubscriptionServices
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDecodeTokenHandler _decodeToken;
        private readonly IMapper _mapper;
        private readonly ICustomerSubscriptionRepository _customerSubscriptionRepository;
        private readonly ISystemAdminRepository _systemAdminRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IVnPayService _vnPayService;
        private readonly IEmailService _emailService;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, 
            IDecodeTokenHandler decodeToken, 
            IMapper mapper,
            ISystemAdminRepository systemAdminRepository,
            ICustomerRepository customerRepository,
            ICustomerSubscriptionRepository customerSubscriptionRepository,
            IPaymentRepository paymentRepository,
            IVnPayService vnPayService,
            IEmailService emailService)
        {
            _subscriptionRepository = subscriptionRepository;
            _customerRepository = customerRepository;
            _decodeToken = decodeToken;
            _mapper = mapper;
            _customerSubscriptionRepository = customerSubscriptionRepository;
            _systemAdminRepository = systemAdminRepository;
            _paymentRepository = paymentRepository;
            _vnPayService = vnPayService;
            _emailService = emailService;
        }

        public async Task CreateNewSubscription(string token, SubscriptionCreateReqModel subscriptionCreateReqModel)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.Admin.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currAdmin = await _systemAdminRepository.GetAdminByUsername(decodedToken.username);

            if (currAdmin == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Admin does not exist");
            }

            var currSubscription = await _subscriptionRepository.GetSubscriptionsByName(subscriptionCreateReqModel.SubscriptionTitle);

            if (currSubscription != null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Subscription with title has already been used");
            }

            Subscription subscription = new Subscription
            {
                SubscriptionTitle = subscriptionCreateReqModel.SubscriptionTitle,
                Price = subscriptionCreateReqModel.Price,
                DurationInDays = subscriptionCreateReqModel.DurationInDays,
                Description = subscriptionCreateReqModel.Description,
                Status = StatusEnums.Active.ToString(),
            };

            await _subscriptionRepository.Add(subscription);
        }

        public async Task RemoveSubscription(string token, int subscriptionId)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.Admin.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currAdmin = await _systemAdminRepository.GetAdminByUsername(decodedToken.username);

            if (currAdmin == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Admin does not exist");
            }

            var currSubscription = await _subscriptionRepository.Get(subscriptionId);

            if (currSubscription == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Subscription does not exist");
            }

            currSubscription.Status = StatusEnums.Inactive.ToString();

            await _subscriptionRepository.Update(currSubscription);
        }

        public async Task<int> CreateCustomerSubscriptionTransaction(string token, int subscriptionId)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currSubscription = await _subscriptionRepository.Get(subscriptionId);

            if (currSubscription == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Subscription does not exist");
            }

            var currCustomerSubscription = await _customerSubscriptionRepository.GetCustomerSubscriptionByCustomerIdAndSubscriptionId(currCustomer.CustomerId, currSubscription.SubscriptionId);

            if (currCustomerSubscription != null && currCustomerSubscription.IsActive == true)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"The customer has already subscribed for the {currSubscription.SubscriptionTitle}");
            }

            Payment newPayment = new Payment
            {
                PaymentDate = DateTime.Now,
                Price = (decimal)currSubscription.Price,
                CustomerId = currCustomer.CustomerId,
                SubscriptionId = currSubscription.SubscriptionId,
                Status = PaymentStatusEnums.Unpaid.ToString()
            };

            var paymentId = await _paymentRepository.AddPayment(newPayment);

            return paymentId;
        }

        public async Task<string> GetPaymentUrl(HttpContext context, int paymentId, string redirectUrl)
        {
            var currPayment = await _paymentRepository.Get(paymentId);

            if (currPayment == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Payment does not exist");
            }

            if (currPayment.Status.Equals(PaymentStatusEnums.Paid.ToString()))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "The payment has already been paid");
            }

            VnPayReqModel vnPayReqModel = new VnPayReqModel
            {
                OrderId = (int)currPayment.SubscriptionId,
                PaymentId = currPayment.PaymentId,
                Amount = currPayment.Price,
                CreatedDate = currPayment.PaymentDate,
                RedirectUrl = redirectUrl,
            };

            return _vnPayService.CreatePaymentUrl(context, vnPayReqModel);
        }

        public async Task<Payment> UpdateCustomerSubscriptionTransaction(PaymentUpdateReqModel paymentUpdateReqModel)
        {
            var currPayment = await _paymentRepository.Get(paymentUpdateReqModel.paymentId);

            if (currPayment == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Payment does not exist");
            }

            if (!currPayment.Status.Equals(PaymentStatusEnums.Unpaid.ToString()))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "The payment is not in unpaid status");
            }

            if (!Enum.IsDefined(typeof(PaymentStatusEnums), paymentUpdateReqModel.status))
            {
                throw new ApiException(HttpStatusCode.BadRequest,"Please choose valid status");
            }

            currPayment.Status = paymentUpdateReqModel.status;
            currPayment.PaymentDate = DateTime.Now;
            await _paymentRepository.Update(currPayment);

            return currPayment;
        }

        public async Task UpdateSubscription(string token, SubscriptionUpdateReqModel subscriptionUpdateReqModel)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.Admin.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currAdmin = await _systemAdminRepository.GetAdminByUsername(decodedToken.username);

            if (currAdmin == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Admin does not exist");
            }

            var currSubscription = await _subscriptionRepository.Get(subscriptionUpdateReqModel.subscriptionId);

            if (currSubscription == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Subscription does not exist");
            }

            currSubscription.Price = subscriptionUpdateReqModel.Price != null ? subscriptionUpdateReqModel.Price : currSubscription.Price;
            currSubscription.DurationInDays = subscriptionUpdateReqModel.DurationInDays != null ? subscriptionUpdateReqModel.DurationInDays : currSubscription.DurationInDays;
            currSubscription.Description = !string.IsNullOrEmpty(subscriptionUpdateReqModel.Description) ? subscriptionUpdateReqModel.Description : currSubscription.Description;

            await _subscriptionRepository.Update(currSubscription);
        }

        public async Task<List<CustomerSubscriptionViewResModel>> ViewCurrentSubscriptionsOfCustomer(string token)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodedToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currCustomerSubsription = await _customerSubscriptionRepository.GetCustomerSubscriptionByCustomerId(currCustomer.CustomerId);

            return _mapper.Map<List<CustomerSubscriptionViewResModel>>(currCustomerSubsription.Where(x => x.IsActive == true).ToList());
        }

        public async Task<CustomerSubscriptionViewResModel> ViewDetailsSubscriptionOfCustomer(string token, int customerSubscriptionId)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.Customer.ToString()) &&
                !decodedToken.roleName.Equals(RoleEnums.Admin.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodedToken.username);

            var currAdmin = await _systemAdminRepository.GetAdminByUsername(decodedToken.username);

            var currCustomerSubscription = await _customerSubscriptionRepository.GetCustomerSubscription(customerSubscriptionId);

            if (currCustomerSubscription == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "CustomerSubscription does not exist ");
            }

            if (currCustomer != null)
            {
                if (currCustomer.CustomerId == currCustomerSubscription.CustomerId)
                {
                    return _mapper.Map<CustomerSubscriptionViewResModel>(currCustomerSubscription);
                }
                else
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "Can not view other customer's subscriptions");
                }
            }
            else if (currAdmin != null)
            {
                return _mapper.Map<CustomerSubscriptionViewResModel>(currCustomerSubscription);
            }
            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "User does not exist");
            }
        }

        public async Task<SubscriptionViewDetailsResModel> ViewDetailsSubsription(int subscriptionId)
        {
            var currSubscription = await _subscriptionRepository.Get(subscriptionId);

            if (currSubscription == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Subscription does not exist");
            }

            return _mapper.Map<SubscriptionViewDetailsResModel>(currSubscription);
        }

        public async Task<List<CustomerSubscriptionViewResModel>> ViewSubscriptionHistoryOfCustomer(string token)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodedToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currCustomerSubsription = await _customerSubscriptionRepository.GetCustomerSubscriptionByCustomerId(currCustomer.CustomerId);

            return _mapper.Map<List<CustomerSubscriptionViewResModel>>(currCustomerSubsription);
        }

        public async Task<List<SubscriptionViewDetailsResModel>> ViewSubscriptions(int? page, int? size)
        {
            var subscriptions = await _subscriptionRepository.GetAll(page, size);

            return _mapper.Map<List<SubscriptionViewDetailsResModel>>(subscriptions.Where(x => x.Status.Equals(StatusEnums.Active.ToString())).ToList());
        }

        public async Task AddCustomerSubscription(string token, int subscriptionId)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currSubscription = await _subscriptionRepository.Get(subscriptionId);

            if (currSubscription == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Subscription does not exist");
            }

            var freeSubscription = await _subscriptionRepository.GetSubscriptionsByName(SubscriptionTypeEnums.Free.ToString());

            if (freeSubscription == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Free Subscription does not exist");
            }

            var customerFreeSubscription = await _customerSubscriptionRepository.GetCustomerSubscriptionByCustomerIdAndSubscriptionId(currCustomer.CustomerId, freeSubscription.SubscriptionId);

            if (customerFreeSubscription == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer Free Subscription does not exist");
            }

            customerFreeSubscription.IsActive = false;

            await _customerSubscriptionRepository.Update(customerFreeSubscription);

            var currCustomerSubscription = await _customerSubscriptionRepository.GetCustomerSubscriptionByCustomerIdAndSubscriptionId(currCustomer.CustomerId, currSubscription.SubscriptionId);

            if (currCustomerSubscription == null)
            {
                CustomerSubscription customerSubscription = new CustomerSubscription
                {
                    CustomerId = currCustomer.CustomerId,
                    SubscriptionId = currSubscription.SubscriptionId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays((double)currSubscription.DurationInDays),
                    IsActive = true,
                };

                await _customerSubscriptionRepository.Add(customerSubscription);

                await _emailService.SendUpgradeToPremiumEmail(currCustomer.FullName, currCustomer.Email, currSubscription.SubscriptionTitle);

            }else if (currCustomerSubscription != null && currCustomerSubscription.IsActive == false)
            {
                currCustomerSubscription.StartDate = DateTime.Now;
                currCustomerSubscription.EndDate = DateTime.Now.AddDays((double)currSubscription.DurationInDays);
                currCustomerSubscription.IsActive = true;

                await _customerSubscriptionRepository.Update(currCustomerSubscription);

                await _emailService.SendUpgradeToPremiumEmail(currCustomer.FullName, currCustomer.Email, currSubscription.SubscriptionTitle);
            }
        }
    }
}

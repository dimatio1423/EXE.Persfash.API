using BusinessObject.Entities;
using BusinessObject.Models.CustomerModels.Request;
using BusinessObject.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.CourseServices;
using Services.FashionItemsServices;
using Services.PartnerServices;
using Services.SubscriptionServices;
using Services.UserServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IFashionItemService _fashionItemService;
        private readonly ICourseService _courseService;

        public CustomerController(ICustomerService customerService, 
            IFashionItemService fashionItemService,
            ISubscriptionService subscriptionService,
            ICourseService courseService)
        {
            _customerService = customerService;
            _subscriptionService = subscriptionService;
            _fashionItemService = fashionItemService;
            _courseService = courseService;
        }

        /// <summary>
        /// View current subscriptions of customer
        /// </summary>
        [HttpGet]
        [Authorize(Roles ="Customer")]
        [Route("current-subscription")]
        public async Task<IActionResult> ViewCurrentSubscriptionOfUser()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _subscriptionService.ViewCurrentSubscriptionsOfCustomer(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View current subscription of user successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View subscriptions history of customer
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("history-subscription")]
        public async Task<IActionResult> ViewSubscriptionHistoryOfUser()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _subscriptionService.ViewSubscriptionHistoryOfCustomer(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View subscription history of user successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View details subscriptions of customer
        /// </summary>
        [HttpGet]
        [Route("subscription/{customerSubscriptionId}")]
        public async Task<IActionResult> ViewDetailCustomerSubscription([FromRoute] int customerSubscriptionId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _subscriptionService.ViewDetailsSubscriptionOfCustomer(token, customerSubscriptionId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View details customer subscription successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View current courses of customer
        /// </summary>
        [HttpGet]
        [Route("current-course")]
        [Authorize]
        public async Task<IActionResult> GetCourseOfCustomer()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _courseService.GetCourseOfCustomer(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View current customer course successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Register new customer
        /// </summary>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterCustomer(CustomerRegisterReqModel customerRegisterReqModel)
        {

            await _customerService.CustomerReigster(customerRegisterReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Register new customer successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View customer information
        /// </summary>
        [HttpGet]
        [Route("information")]
        public async Task<IActionResult> GetCustomerInformation(int customerId)
        {

            var result = await _customerService.GetCustomerInformation(customerId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get customer information successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Update customer information
        /// </summary>
        [HttpPut]
        [Route("information")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerInformation([FromBody] CustomerInformationUpdateReqModel customerInformationUpdateReqModel)
        {

            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _customerService.UpdateCustomerInformation(token, customerInformationUpdateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Update customer information successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View customer profile
        /// </summary>
        [HttpGet]
        [Route("profile")]
        [Authorize]
        public async Task<IActionResult> GetCustomerProfileSetup()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _customerService.GetCustomerProfile(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get customer profile successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }


        /// <summary>
        /// Setup customer profile
        /// </summary>
        [HttpPost]
        [Route("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerProfileSetup(CustomerProfileSetupReqModel customerProfileSetupReq)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _customerService.CustomerProfileSetup(token, customerProfileSetupReq);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Create customer profile successfully",
            };

            return StatusCode(response.Code, response);
        }


        /// <summary>
        /// Update customer profile
        /// </summary>
        [HttpPut]
        [Route("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerProfileSetup(CustomerProfileSetupUpdateReqModel customerProfileSetupUpdateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

             await _customerService.CustomerProfileSetupUpdate(token, customerProfileSetupUpdateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Update customer profile successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Check if the customer has done the customer profile setup or not
        /// </summary>
        [HttpGet]
        [Route("profile/checkProfile")]
        [Authorize]
        public async Task<IActionResult> CheckCustomerProfile()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result =  await _customerService.CheckCustomerProfileExisted(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = result ? "The customer has already done profile setup" : "The customer has not done profile setup yet",
                Data = result ? true : false,
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Get recommendation fashion items for customer base on the profile setup
        /// </summary>
        [HttpGet]
        [Route("recommendation/fashion-item")]
        [Authorize]
        public async Task<IActionResult> GetCustomerRecommendationFashionItem(int? page = 1, int? size = 10)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _fashionItemService.RecommendFashionItemForCustomer(token, page, size);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get recommended fashion items for customer successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Get recommendation fashion items for customer filter
        /// </summary>
        [HttpGet]
        [Route("recommendation/fashion-item-filter")]
        [Authorize]
        public async Task<IActionResult> GetCustomerRecommendationFashionItemFilter(string filter = "Occasion" ,int? page = 1, int? size = 10)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _fashionItemService.RecommendFashionItemForCustomerFilter(token, page, size, filter);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get recommended fashion items filter for customer successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Get recommendation store for customer base on the profile setup
        /// </summary>
        //[HttpGet]
        //[Route("recommendation/store")]
        //[Authorize]
        //public async Task<IActionResult> GetCustomerRecommendationStores(int? page = 1, int? size = 10)
        //{
        //    var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

        //    var result = await _partnerService.RecommendPartnerForCustomer(token, page, size);

        //    ResultModel response = new ResultModel
        //    {
        //        IsSuccess = true,
        //        Code = (int)HttpStatusCode.OK,
        //        Message = "Get recommended stores for customer successfully",
        //        Data = result,
        //    };

        //    return StatusCode(response.Code, response);
        //}

        /// <summary>
        /// Get customer list for ADMIN
        /// </summary>
        [HttpGet]
        [Route("view")]
        [Authorize]
        public async Task<IActionResult> GetCustomerForAmin(int? page = 1, int? size = 10)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _customerService.GetCustomerListForAdmin(page, size);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get customer list successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Activate-deactivate customer for ADMIN
        /// </summary>
        [HttpPut]
        [Route("activate-deactivate/{customerId}")]
        [Authorize]
        public async Task<IActionResult> ActivateDeactivateUser(int customerId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _customerService.ActivateDeactivateCustomerForAdmin(token, customerId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = result ? "Activate customer successfully" : "Deactivate customer successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }
    }
}

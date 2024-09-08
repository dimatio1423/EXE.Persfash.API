using BusinessObject.Entities;
using BusinessObject.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public CustomerController(ICustomerService customerService, ISubscriptionService subscriptionService)
        {
            _customerService = customerService;
            _subscriptionService = subscriptionService;
        }

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
    }
}

using BusinessObject.Models.ResultModel;
using BusinessObject.Models.SubscriptionModels.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.SubscriptionServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> ViewSubscriptions(int? page = 1, int? size = 10)
        {

            var result = await _subscriptionService.ViewSubscriptions(page, size);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View subscriptions successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("{subscriptionId}")]
        public async Task<IActionResult> ViewDetailsSubscription([FromRoute] int subscriptionId)
        {

            var result = await _subscriptionService.ViewDetailsSubsription(subscriptionId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View details subscription successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> CreateNewSubscription([FromBody] SubscriptionCreateReqModel subscriptionCreateReqModel )
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _subscriptionService.CreateNewSubscription(token, subscriptionCreateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Create new subscription successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSubscription([FromBody] SubscriptionUpdateReqModel subscriptionUpdateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _subscriptionService.UpdateSubscription(token, subscriptionUpdateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Update subscription successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpDelete]
        [Route("/{subscriptionId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveSubscription([FromRoute] int subscriptionId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _subscriptionService.RemoveSubscription(token, subscriptionId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Remove subscription successfully",
            };

            return StatusCode(response.Code, response);
        }
    }
}

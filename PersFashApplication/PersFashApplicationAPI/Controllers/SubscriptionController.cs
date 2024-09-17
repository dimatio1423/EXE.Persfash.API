using BusinessObject.Enums;
using BusinessObject.Models.PaymentModel.Request;
using BusinessObject.Models.PaymentModel.Response;
using BusinessObject.Models.ResultModel;
using BusinessObject.Models.SubscriptionModels.Request;
using BusinessObject.Models.SubscriptionModels.Response;
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

        /// <summary>
        /// View all subscriptions
        /// </summary>
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


        /// <summary>
        /// View details subscriptions
        /// </summary>
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

        /// <summary>
        /// Create new subscription for admin
        /// </summary>
        //[HttpPost]
        //[Authorize (Roles = "Admin")]
        //public async Task<IActionResult> CreateNewSubscription([FromBody] SubscriptionCreateReqModel subscriptionCreateReqModel )
        //{
        //    var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

        //    await _subscriptionService.CreateNewSubscription(token, subscriptionCreateReqModel);

        //    ResultModel response = new ResultModel
        //    {
        //        IsSuccess = true,
        //        Code = (int)HttpStatusCode.OK,
        //        Message = "Create new subscription successfully",
        //    };

        //    return StatusCode(response.Code, response);
        //}

        /// <summary>
        /// Update subscription for admin
        /// </summary>

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


        /// <summary>
        /// Remove subscriptions
        /// </summary>
        //[HttpDelete]
        //[Route("/{subscriptionId}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> RemoveSubscription([FromRoute] int subscriptionId)
        //{
        //    var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

        //    await _subscriptionService.RemoveSubscription(token, subscriptionId);

        //    ResultModel response = new ResultModel
        //    {
        //        IsSuccess = true,
        //        Code = (int)HttpStatusCode.OK,
        //        Message = "Remove subscription successfully",
        //    };

        //    return StatusCode(response.Code, response);
        //}

        /// <summary>
        /// Subscribe subscription, create payment url transaction for customer subscription
        /// </summary>
        [HttpPost]
        [Route("subscribe")]
        [Authorize]
        public async Task<IActionResult> SubscribeSubscription(SubscriptionPaymentReqModel subscriptionSubscribeReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var paymentId = await _subscriptionService.CreateCustomerSubscriptionTransaction(token, subscriptionSubscribeReqModel.subscriptionId);

            var paymentUrl = await _subscriptionService.GetPaymentUrl(HttpContext, paymentId, subscriptionSubscribeReqModel.redirectUrl);

            var result = new PaymentResModel
            {
                paymentId = paymentId,
                paymentUrl = paymentUrl
            };

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get subscription paymentUrl successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Update status of payment customer subscription
        /// </summary>
        [HttpPut]
        [Route("subscribe")]
        [Authorize]
        public async Task<IActionResult> UpdateSubscribeSubscription(PaymentUpdateReqModel paymentUpdateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var payment = await _subscriptionService.UpdateCustomerSubscriptionTransaction(paymentUpdateReqModel);

            if (payment.Status.Equals(PaymentStatusEnums.Paid.ToString()))
            {
                await _subscriptionService.AddCustomerSubscription(token, (int)payment.SubscriptionId);

                ResultModel response = new ResultModel
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Update customer subscription successfully",
                };

                return StatusCode(response.Code, response);

            }else
            {
                 ResultModel response = new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Update customer subscription failed",
                };

                return StatusCode(response.Code, response);
            }
        }
    }
}

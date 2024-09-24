using BusinessObject.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.PaymentServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Get payments CUSTOMER, ADMIN
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPayments(int? page = 1, int? size = 10)
        {
            var result = await _paymentService.ViewPaymentForAdmin(page, size);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get payments successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        } 
    }
}

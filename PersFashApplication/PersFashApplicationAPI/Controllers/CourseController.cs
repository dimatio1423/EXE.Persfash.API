using BusinessObject.Enums;
using BusinessObject.Models.CourseModel.Request;
using BusinessObject.Models.PaymentModel.Request;
using BusinessObject.Models.PaymentModel.Response;
using BusinessObject.Models.ResultModel;
using BusinessObject.Models.SubscriptionModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Services.CourseServices;
using System.ComponentModel;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// View courses
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCourses(string? sortBy, int? page = 1, int? size = 10)
        {

            var token = !string.IsNullOrEmpty(Request.Headers["Authorization"].ToString()) ? Request.Headers["Authorization"].ToString().Split(" ")[1] : null ;

            var result = await _courseService.GetCourses(token, page, size, sortBy);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View courses successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchCourse(string? sortBy, string? searchValue, int? page = 1, int? size = 10)
        {

            var token = !string.IsNullOrEmpty(Request.Headers["Authorization"].ToString()) ? Request.Headers["Authorization"].ToString().Split(" ")[1] : null;

            var result = await _courseService.SearchCourses(token, page, size, searchValue, sortBy);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Search courses successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View courses by influencer
        /// </summary>
        [HttpGet]
        [Route("influencer/{influencerId}")]
        public async Task<IActionResult> GetCoursesByInfluencerId([FromRoute] int influencerId, int? page = 1, int? size = 10)
        {
            var result = await _courseService.GetCoursesByInfluencerId(influencerId, page, size);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View courses of influencer successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View courses of the current logging influencer
        /// </summary>
        [HttpGet]
        [Route("influencer")]
        [Authorize (Roles = "FashionInfluencer")]
        public async Task<IActionResult> GetCoursesOfCurrentInfluencerId(int? page = 1, int? size = 10)
        {

            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _courseService.GetCoursesOfCurrentInfluencerId(token, page, size);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View courses of current influencer successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Add new course
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "FashionInfluencer")]
        public async Task<IActionResult> CreateNewCourses(CourseCreateReqModel courseCreateReqModel)
        {

            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _courseService.CreateNewCourse(token, courseCreateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Create new course successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Update course information
        /// </summary>
        [HttpPut]
        [Authorize(Roles = "FashionInfluencer")]
        public async Task<IActionResult> UpdateCourse(CourseUpdateReqModel courseUpdateReqModel)
        {

            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _courseService.UpdateCourse(token, courseUpdateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Update course successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Change status of the current course
        /// </summary>
        [HttpPost]
        [Route ("activate-decativate/{courseId}")]
        [Authorize(Roles = "FashionInfluencer")]
        public async Task<IActionResult> ActivateDeactivateCourse([FromRoute] int courseId)
        {

            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _courseService.ActivateDeactivateCourse(token, courseId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = result ? "Activate course successfully" : "Deactivate course successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Check if the current customer has already owned the course
        /// </summary>
        [HttpPost]
        [Route("check-customer")]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> CheckCustomerCourse(int courseId)
        {

            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _courseService.CheckCustomerCourse(token, courseId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = result ? "Customer owns course" : "Customer does not own course ",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View details course
        /// </summary>
        [HttpPost]
        [Route("{courseId}")]
        public async Task<IActionResult> ViewDetailsCourse([FromRoute] int courseId)
        {
            var result = await _courseService.GetCourseDetails(courseId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View course details successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Purchase course, create payment url transaction for customer course
        /// </summary>
        [HttpPost]
        [Route("purchase")]
        [Authorize]
        public async Task<IActionResult> PurchaseCourse(CoursePaymentReqModel coursePaymentReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var paymentId = await _courseService.CreateCustomerCourseTransaction(token, coursePaymentReqModel.courseId);

            var paymentUrl = await _courseService.GetPaymentUrl(HttpContext, paymentId, coursePaymentReqModel.redirectUrl);

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
        [Route("purchase")]
        [Authorize]
        public async Task<IActionResult> UpdateCourseTransaction(PaymentUpdateReqModel paymentUpdateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var payment = await _courseService.UpdateCustomerCourseTransaction(paymentUpdateReqModel);

            if (payment.Status.Equals(PaymentStatusEnums.Paid.ToString()))
            {
                await _courseService.AddCustomerCourse(token, (int)payment.CourseId);

                ResultModel response = new ResultModel
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Update customer course successfully",
                };

                return StatusCode(response.Code, response);
            }
            else
            {
                ResultModel response = new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = "Update customer course failed",
                };

                return StatusCode(response.Code, response);
            }
        }
    }
}

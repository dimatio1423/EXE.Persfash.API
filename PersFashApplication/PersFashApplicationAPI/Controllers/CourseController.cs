using BusinessObject.Models.CourseModel.Request;
using BusinessObject.Models.ResultModel;
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
        public async Task<IActionResult> GetCourses(int? page = 1, int? size = 10)
        {
            var result = await _courseService.GetCourses(page, size);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View courses successfully",
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
    }
}

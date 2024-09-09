using BusinessObject.Models.CourseContentModel.Request;
using BusinessObject.Models.CourseModel.Request;
using BusinessObject.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Services.CourseContentServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseContentController : ControllerBase
    {
        private readonly ICourseContentService _courseContentService;

        public CourseContentController(ICourseContentService courseContentService)
        {
            _courseContentService = courseContentService;
        }

        [HttpGet]
        [Route ("course/{courseId}")] 
        
        public async Task<IActionResult> GetCourseContentByCourseId ([FromRoute ]int courseId, int? page = 1, int? size = 10)
        {
            var result = await _courseContentService.GetCourseContentByCourseId(courseId, page, size);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View course contents of current course successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Authorize (Roles = "FashionInfluencer")]

        public async Task<IActionResult> CreateNewContent([FromBody] ContentCreateReqModel contentCreateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _courseContentService.CreateNewCourseContent(token, contentCreateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Create new course contents successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpPut]
        [Authorize(Roles = "FashionInfluencer")]

        public async Task<IActionResult> UpdateContent([FromBody] CourseContentUpdateReqModel contentUpdateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _courseContentService.UpdateCourseContent(token, contentUpdateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Update course content successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpDelete]
        [Route ("{courseContentId}")]
        [Authorize(Roles = "FashionInfluencer")]

        public async Task<IActionResult> RemoveContent([FromRoute] int courseContentId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _courseContentService.RemoveCourseContent(token, courseContentId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Remove course contents successfully",
            };

            return StatusCode(response.Code, response);
        }
    }
}

using BusinessObject.Models.CourseMaterialModel.Request;
using BusinessObject.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.CourseMaterialServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseMaterialController : ControllerBase
    {
        private readonly ICourseMaterialService _courseMaterialService;

        public CourseMaterialController(ICourseMaterialService courseMaterialService)
        {
            _courseMaterialService = courseMaterialService;
        }

        [HttpGet]
        [Route("course-content/{courseContentId}")]

        public async Task<IActionResult> GetCourseMaterialByCourseContentId([FromRoute] int courseContentId, int? page = 1, int? size = 10)
        {
            var result = await _courseMaterialService.GetCourseMaterialByCourseContentId(courseContentId, page, size);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View course materials of current course content successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewMaterial([FromBody] MaterialCreateReqModel materialCreateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _courseMaterialService.CreateNewCourseMaterial(token, materialCreateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Create course material successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateMaterial([FromBody] CourseMaterialUpdateReqModel courseMaterialUpdateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _courseMaterialService.UpdateCourseMaterial(token, courseMaterialUpdateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Update course material successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpDelete]
        [Route ("{materialId}")]
        [Authorize]
        public async Task<IActionResult> RemoveMaterial([FromRoute] int materialId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _courseMaterialService.RemoveCourseMaterial(token, materialId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Remove course material successfully",
            };

            return StatusCode(response.Code, response);
        }
    }
}

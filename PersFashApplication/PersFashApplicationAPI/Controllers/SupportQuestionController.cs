using BusinessObject.Models.CourseMaterialModel.Request;
using BusinessObject.Models.ResultModel;
using BusinessObject.Models.SupportQuestion.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.SupportQuestionServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportQuestionController : ControllerBase
    {
        private readonly ISupportQuestionService _supportQuestionService;

        public SupportQuestionController(ISupportQuestionService supportQuestionService)
        {
            _supportQuestionService = supportQuestionService;
        }

        /// <summary>
        /// Create support question for CUSTOMER
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateSupportQuestion([FromBody] SupportQuestionCreateReqModel supportQuestionCreateReq)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _supportQuestionService.CreateNewSupportQuestion(token, supportQuestionCreateReq);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Create support question successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Update support question for CUSTOMER
        /// </summary>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateSupportQuestion([FromBody] SupportQuestionUpdateReqModel supportQuestionUpdateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _supportQuestionService.UpdateSupportQuestion(token, supportQuestionUpdateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Update support question successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Remove support question for CUSTOMER
        /// </summary>
        [HttpDelete]
        [Route("{supportId}")]
        [Authorize]
        public async Task<IActionResult> RemoveSupportQuestion([FromRoute] int supportId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _supportQuestionService.RemoveSupportQuestion(token, supportId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Remove support question successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View support question for CUSTOMER, ADMIN/ filter by Open, Answer
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewSupportQuestion(string? filter, int? page = 1, int? size = 10)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _supportQuestionService.ViewSupports(page, size, filter);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Remove support question successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }
    }
}

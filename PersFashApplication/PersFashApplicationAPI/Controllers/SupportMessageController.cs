using BusinessObject.Models.ResultModel;
using BusinessObject.Models.SupportMessage.Request;
using BusinessObject.Models.SupportQuestion.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.SupportMessageServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportMessageController : ControllerBase
    {
        private readonly ISupportMessageService _supportMessageService;

        public SupportMessageController(ISupportMessageService supportMessageService)
        {
            _supportMessageService = supportMessageService;
        }

        /// <summary>
        /// Create support message to reply to support question for ADMIN
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateSupportMessage([FromBody] SupportMessageCreateReqModel supportMessageCreateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _supportMessageService.CreateNewSupportMessage(token, supportMessageCreateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Create support message successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Update of a specific support message to reply to support question for ADMIN
        /// </summary>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateSupportMessage([FromBody] SupportMessageUpdateReqModel supportMessageUpdateReq)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _supportMessageService.UpdateSupportMessage(token, supportMessageUpdateReq);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Update support message successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Remove a specific support message for ADMIN
        /// </summary>
        [HttpDelete]
        [Route("{messageId}")]
        [Authorize]
        public async Task<IActionResult> DeleteSupportMessage([FromRoute] int messageId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _supportMessageService.RemoveSupportMessage(token, messageId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Remove support message successfully",
            };

            return StatusCode(response.Code, response);
        }
    }
}

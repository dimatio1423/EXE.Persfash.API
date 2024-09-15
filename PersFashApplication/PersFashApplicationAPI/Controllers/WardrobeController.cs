using BusinessObject.Models.ResultModel;
using BusinessObject.Models.WardrobeModel.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.WardrobeServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WardrobeController : ControllerBase
    {
        private readonly IWardrobeService _wardrobeService;

        public WardrobeController(IWardrobeService wardrobeService)
        {
            _wardrobeService = wardrobeService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetWardrobe()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _wardrobeService.ViewWardrobeOfCustomer(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View wardrobe of customer successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("{wardrobeId}")]
        [Authorize]
        public async Task<IActionResult> ViewDetailsWardrobe([FromRoute] int wardrobeId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _wardrobeService.ViewDetailsWardrobeOfCustomer(token, wardrobeId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View details wardrobe of customer successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddWardrobe([FromBody] WardrobeCreateReqModel wardrobeCreateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

             await _wardrobeService.AddNewWardrobe(token, wardrobeCreateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Add new wardrobe of customer successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateWardrobe([FromBody] WardrobeUpdateReqModel wardrobeUpdateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _wardrobeService.UpdateWardrobe(token, wardrobeUpdateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Update wardrobe of customer successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpDelete]
        [Route("{wardrobeId}")]
        [Authorize]
        public async Task<IActionResult> DeleteWardrobe([FromRoute] int wardrobeId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _wardrobeService.RemoveWardrobe(token, wardrobeId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Remove wardrobe of customer successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("item")]
        [Authorize]
        public async Task<IActionResult> AddItemToWardrobe(WardrobeItemCreateReqModel wardrobeItemCreateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _wardrobeService.AddNewItemToWardrobe(token, wardrobeItemCreateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Add new item to wardrobe of customer successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpDelete]
        [Route("item/{wardrobeItemId}")]
        [Authorize]
        public async Task<IActionResult> DeleteItemFromWardrobe([FromRoute] int wardrobeItemId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _wardrobeService.RemoveItemFromWardrobe(token, wardrobeItemId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Remove item to wardrobe of customer successfully",
            };

            return StatusCode(response.Code, response);
        }

    }
}

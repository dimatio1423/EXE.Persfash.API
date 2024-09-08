using Azure;
using BusinessObject.Enums;
using BusinessObject.Models.FashionItemsModel.Request;
using BusinessObject.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.FashionItemsServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FashionItemController : ControllerBase
    {
        private readonly IFashionItemService _fashionItemService;

        public FashionItemController(IFashionItemService fashionItemService)
        {
            _fashionItemService = fashionItemService;
        }

        [HttpGet]
        public async Task<IActionResult> ViewFashionItems(int? page = 1, int? size = 10)
        {
            var result = await _fashionItemService.ViewFashionItems(page, size);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View fashion items successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("{itemId}")]
        public async Task<IActionResult> ViewDetailsFashionItem(int itemId)
        {
            var result = await _fashionItemService.ViewDetailsFashionItem(itemId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View details fashion items successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> CreateNewFashionItem([FromForm] FashionItemCreateReqModel fashionItemCreateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _fashionItemService.CreateNewFashionItem(token, fashionItemCreateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Create new fashion item successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpPut]
        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> UpdateFashionItem([FromForm] FashionItemUpdateReqModel fashionItemUpdateReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _fashionItemService.UpdateFashionItem(token, fashionItemUpdateReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Update fashion item successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpDelete]
        [Route("{itemId}")]
        [Authorize(Roles ="Partner")]
        public async Task<IActionResult> RemoveFashionItem(int itemId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _fashionItemService.DeleteFashionItem(token, itemId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Remove fashion item successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("partner")]
        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> ViewFashionItemsOfCurrentPartner(int? page = 1, int? size = 10)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _fashionItemService.ViewFashionItemsOfCurrentPartner(token, page, size);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View fashion items of partner successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }
    }
}

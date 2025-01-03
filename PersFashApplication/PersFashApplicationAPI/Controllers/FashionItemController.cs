﻿using Azure;
using BusinessObject.Enums;
using BusinessObject.Models.FashionItemsModel.Request;
using BusinessObject.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.FashionItemsServices;
using System.Drawing;
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


        /// <summary>
        /// View all fashion items for CUSTOMER
        /// </summary>
        [HttpGet]
        [Route("view")]
        public async Task<IActionResult> ViewFashionItems([FromQuery]FashionItemFilterReqModel? fashionItemFilterReqModel, int? pageIndex = 1, int? sizeIndex = 10
            , string? sortBy = "name_asc")
        {

            var result = await _fashionItemService.ViewFashionItems(pageIndex, sizeIndex, fashionItemFilterReqModel, sortBy);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View fashion items successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View all fashion items for CUSTOMER
        /// </summary>
        [HttpGet]
        [Route("view/admin")]
        public async Task<IActionResult> ViewFashionItems(int? pageIndex = 1, int? sizeIndex = 10)
        {

            var result = await _fashionItemService.ViewFashionItemsForAdmin(pageIndex, sizeIndex);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View fashion items successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Search fashion items CUSTOMER, ADMIN
        /// </summary>
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchFashionItems([FromQuery]FashionItemFilterReqModel? fashionItemFilterReqModel, string? searchValue, int? pageIndex = 1, int? sizeIndex = 10
            , string? sortBy = "name_asc")
        {

            var result = await _fashionItemService.SearchFashionItems(pageIndex, sizeIndex, fashionItemFilterReqModel, sortBy, searchValue);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Search fashion items successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View details fashion items
        /// </summary>
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

        /// <summary>
        /// Create new fashion item for partner
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateNewFashionItem([FromBody] FashionItemCreateReqModel fashionItemCreateReqModel)
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

        /// <summary>
        /// Update fashion items for partner
        /// </summary>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFashionItem([FromBody] FashionItemUpdateReqModel fashionItemUpdateReqModel)
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


        /// <summary>
        /// Activate deactivate fashion items for partner
        /// </summary>
        [HttpPost]
        [Route("{itemId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> RemoveFashionItem(int itemId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _fashionItemService.ActivateDeactivateFashionItem(token, itemId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = result ? "Activate fashion item successfully" : "Deactivate fashion item successfully",
                Data = result, 
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View fashion items of the current logging partner
        /// </summary>
        //[HttpGet]
        //[Route("partner")]
        //[Authorize(Roles = "Partner")]
        //public async Task<IActionResult> ViewFashionItemsOfCurrentPartner(int? page = 1, int? size = 10)
        //{
        //    var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

        //    var result = await _fashionItemService.ViewFashionItemsOfCurrentPartner(token, page, size);

        //    ResultModel response = new ResultModel
        //    {
        //        IsSuccess = true,
        //        Code = (int)HttpStatusCode.OK,
        //        Message = "View fashion items of current partner successfully",
        //        Data = result
        //    };

        //    return StatusCode(response.Code, response);
        //}

        /// <summary>
        /// View fashion items by partner for customer to view by store of partner
        /// </summary>
        //[HttpGet]
        //[Route("partner/{partnerId}")]
        //public async Task<IActionResult> ViewFashionItemsByPartner([FromRoute] int partnerId, int? page = 1, int? size = 10)
        //{
        //    //var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

        //    var result = await _fashionItemService.ViewFashionItemsByPartnerId(partnerId, page, size);

        //    ResultModel response = new ResultModel
        //    {
        //        IsSuccess = true,
        //        Code = (int)HttpStatusCode.OK,
        //        Message = "View fashion items by partner successfully",
        //        Data = result
        //    };

        //    return StatusCode(response.Code, response);
        //}
    }
}

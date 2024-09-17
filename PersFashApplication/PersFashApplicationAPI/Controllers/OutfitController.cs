using BusinessObject.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.OutfitServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutfitController : ControllerBase
    {
        private readonly IOutfitService _outfitService;

        public OutfitController(IOutfitService outfitService)
        {
            _outfitService = outfitService;
        }

        /// <summary>
        /// Generate outfit recommendation for customer
        /// </summary>
        [HttpPost]
        [Route("recommendation")]
        [Authorize]
        public async Task<IActionResult> GenerateOutfit()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _outfitService.GenerateOutfitForCustomer(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Generate outfit successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View outfit recommendation for customer
        /// </summary>
        [HttpGet]
        [Route("recommendation")]
        [Authorize]
        public async Task<IActionResult> ViewRecommendationForCustomer()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _outfitService.ViewRecommendationOutfitForCustomer(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View recommended outfit successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// View favorite outfit for customer
        /// </summary>
        [HttpGet]
        [Route("favorite")]
        [Authorize]
        public async Task<IActionResult> ViewFavoriteForCustomer()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _outfitService.ViewFavoriteOutfits(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "View favorite outfit successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Add to favorite outfit for customer
        /// </summary>
        [HttpPost]
        [Route("favorite")]
        [Authorize]
        public async Task<IActionResult> AddOutfitToFavoriteOfCustomer(int outfitId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _outfitService.AddOutfitToFavoriteList(token, outfitId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Add outfit to favorite outfit successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Remove favorite outfit of customer
        /// </summary>
        [HttpDelete]
        [Route("favorite/{outfitId}")]
        [Authorize]
        public async Task<IActionResult> RemoveOutfitFromFavoriteOfCustomer([FromRoute] int outfitId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _outfitService.RemoveOutfitFromFavoriteList(token, outfitId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Remove outfit from favorite outfit successfully",
            };

            return StatusCode(response.Code, response);
        }


        /// <summary>
        /// View details outfit for customer
        /// </summary>
        //[HttpGet]
        //[Route("{outfitId}")]
        //[Authorize]
        //public async Task<IActionResult> ViewFavoriteForCustomer([FromRoute] int outfitId )
        //{
        //    var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

        //    var result = await _outfitService.ViewDetailsOutfit(token, outfitId);

        //    ResultModel response = new ResultModel
        //    {
        //        IsSuccess = true,
        //        Code = (int)HttpStatusCode.OK,
        //        Message = "View details outfit successfully",
        //        Data = result
        //    };

        //    return StatusCode(response.Code, response);
        //}
    }
}

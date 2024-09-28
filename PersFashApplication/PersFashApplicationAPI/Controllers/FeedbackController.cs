using BusinessObject.Entities;
using BusinessObject.Models.FeedbackModel.Request;
using BusinessObject.Models.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.FeedbackServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        /// <summary>
        /// Get feedbacks by fashion item
        /// </summary>
        [HttpGet]
        [Route("fashion-item")]
        public async Task<IActionResult> GetFeedbacksByItemId(int itemId)
        {
            var result = await _feedbackService.GetFeedbackByItemId(itemId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get feedback by fashion item successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Give feedbacks for fashion item
        /// </summary>
        [HttpPost]
        [Route("fashion-item")]
        [Authorize]
        public async Task<IActionResult> GiveFeedbackForFashionItem(GiveFeedbackItemReqModel giveFeedbackItemReqModel)
        {

            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _feedbackService.GiveFeedbackForItem(token, giveFeedbackItemReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Give feedback for fashion item successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Get feedbacks by course
        /// </summary>
        //[HttpGet]
        //[Route("course")]
        //public async Task<IActionResult> GetFeedbacksByCourseId(int courseId)
        //{
        //    var result = await _feedbackService.GetFeedbackByCourseId(courseId);

        //    ResultModel response = new ResultModel
        //    {
        //        IsSuccess = true,
        //        Code = (int)HttpStatusCode.OK,
        //        Message = "Get feedback by course successfully",
        //        Data = result
        //    };

        //    return StatusCode(response.Code, response);
        //}

        /// <summary>
        /// Give feedbacks for course
        /// </summary>
        //[HttpPost]
        //[Route("course")]
        //[Authorize]
        //public async Task<IActionResult> GiveFeedbackForCourse(GiveFeedbackCourseReqModel giveFeedbackCourseReqModel)
        //{

        //    var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

        //    await _feedbackService.GiveFeedbackForCourse(token, giveFeedbackCourseReqModel);

        //    ResultModel response = new ResultModel
        //    {
        //        IsSuccess = true,
        //        Code = (int)HttpStatusCode.OK,
        //        Message = "Give feedback for course successfully",
        //    };

        //    return StatusCode(response.Code, response);
        //}

        /// <summary>
        /// Get feedbacks by fashion influencer
        /// </summary>
        //[HttpGet]
        //[Route("fashion-influencer")]
        
        //public async Task<IActionResult> GetFeedbacksByInfluencerId(int influencerId)
        //{
        //    var result = await _feedbackService.GetFeedbackByFashionInfluenerId(influencerId);

        //    ResultModel response = new ResultModel
        //    {
        //        IsSuccess = true,
        //        Code = (int)HttpStatusCode.OK,
        //        Message = "Get feedback by fashion influencer successfully",
        //        Data = result
        //    };

        //    return StatusCode(response.Code, response);
        //}


        /// <summary>
        /// Give feedbacks for fashion influencer
        /// </summary>
        //[HttpPost]
        //[Route("fashion-influencer")]
        //[Authorize]
        //public async Task<IActionResult> GiveFeedbackForFashionInfluencer(GiveFeedbackInfluencerReqModel giveFeedbackInfluencerReq)
        //{

        //    var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

        //    await _feedbackService.GiveFeedbackForFashionInfluencer(token, giveFeedbackInfluencerReq);

        //    ResultModel response = new ResultModel
        //    {
        //        IsSuccess = true,
        //        Code = (int)HttpStatusCode.OK,
        //        Message = "Give feedback for fashion influencer successfully",
        //    };

        //    return StatusCode(response.Code, response);
        //}

        /// <summary>
        /// Remove feedback for CUSTOMER, ADMIN
        /// </summary>
        [HttpDelete]
        [Route("{feedbackId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFeedback( [FromRoute] int feedbackId)
        {

            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _feedbackService.RemoveFeedback(token, feedbackId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Remove feedback successfully",
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Get all feedback for ADMIN
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllFeedbacks(int ? page = 1, int? size = 10)
        {

            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _feedbackService.GetAllFeedbackForAdmin(page, size);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Remove feedback successfully",
                Data = result
            };

            return StatusCode(response.Code, response);
        }
    }
}

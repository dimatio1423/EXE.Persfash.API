using BusinessObject.Models.FeedbackModel.Request;
using BusinessObject.Models.FeedbackModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FeedbackServices
{
    public interface IFeedbackService
    {
        Task GiveFeedbackForItem(string token, GiveFeedbackItemReqModel giveFeedbackItemReqModel);
        Task GiveFeedbackForCourse(string token, GiveFeedbackCourseReqModel giveFeedbackCourseReqModel);
        Task GiveFeedbackForFashionInfluencer(string token, GiveFeedbackInfluencerReqModel giveFeedbackInfluencerReqModel);
        Task<SummaryFeedbackResModel> GetFeedbackByItemId(int itemId);
        Task<SummaryFeedbackResModel> GetFeedbackByCourseId(int courseId);
        Task<SummaryFeedbackResModel> GetFeedbackByFashionInfluenerId(int fashionItemId);
        Task RemoveFeedback(string token, int feedbackId);

        Task<SummaryFeedbackResModel> GetAllFeedbackForAdmin(int? page, int? size);
    }
}

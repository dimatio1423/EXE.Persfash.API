using BusinessObject.Models.SupportQuestion.Request;
using BusinessObject.Models.SupportQuestion.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SupportQuestionServices
{
    public interface ISupportQuestionService
    {
        Task CreateNewSupportQuestion(string token, SupportQuestionCreateReqModel supportQuestionCreateReqModel);
        Task UpdateSupportQuestion(string token, SupportQuestionUpdateReqModel supportQuestionUpdateReq);
        Task<List<SupportQuestionViewListResModel>> ViewSupports(int? page, int? size, string? filterStatus);
        Task RemoveSupportQuestion(string token, int supportQuestionId);
    }
}

using BusinessObject.Models.SupportMessage.Request;
using BusinessObject.Models.SupportQuestion.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SupportMessageServices
{
    public interface ISupportMessageService
    {
        Task CreateNewSupportMessage(string token, SupportMessageCreateReqModel supportMessageCreateReqModel);
        Task UpdateSupportMessage(string token, SupportMessageUpdateReqModel updateReqModel);
        Task RemoveSupportMessage(string token, int messsageId);
    }
}

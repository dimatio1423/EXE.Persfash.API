using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.SupportMessage.Request;
using BusinessObject.Models.SupportQuestion.Request;
using Repositories.SupportMessageRepo;
using Repositories.SupportQuestionRepo;
using Repositories.SystemAdminRepos;
using Repositories.UserRepos;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.SupportMessageServices
{
    public class SupportMessageService : ISupportMessageService
    {
        private ISupportQuestionRepository _supportQuestionRepository;
        private ISupportMessageRepository _supportMessageRepository;
        private IDecodeTokenHandler _decodeToken;
        private readonly ISystemAdminRepository _systemAdminRepository;
        private IMapper _mapper;
        private ICustomerRepository _customerRepository;

        public SupportMessageService(ISupportQuestionRepository supportQuestionRepository,
           ISupportMessageRepository supportMessageRepository,
           ISystemAdminRepository systemAdminRepository,
           IDecodeTokenHandler decodeToken, IMapper mapper,
           ICustomerRepository customerRepository)
        {
            _supportQuestionRepository = supportQuestionRepository;
            _supportMessageRepository = supportMessageRepository;
            _decodeToken = decodeToken;
            _systemAdminRepository = systemAdminRepository;
            _mapper = mapper;
            _customerRepository = customerRepository;
        }
        public async Task CreateNewSupportMessage(string token, SupportMessageCreateReqModel supportMessageCreateReqModel)
        {
            var decodeToken = _decodeToken.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Admin.ToString()))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "You do not have permission to perform this function");
            }

            var currAdmin = await _systemAdminRepository.GetAdminByUsername(decodeToken.username);

            if (currAdmin == null) throw new ApiException(HttpStatusCode.NotFound, "Admin does not exist");

            var currSupportQuestion = await _supportQuestionRepository.Get(supportMessageCreateReqModel.SupportId);

            if (currSupportQuestion == null) throw new ApiException(HttpStatusCode.NotFound, "Support question does not exist");

            SupportMessage supportMessage = new SupportMessage
            {
                SupportId = currSupportQuestion.SupportId,
                AdminId = currAdmin.AdminId,
                MessageText = supportMessageCreateReqModel.MessageText,
                DateSent = DateTime.Now
            };

            await _supportMessageRepository.Add(supportMessage);

            currSupportQuestion.Status = SupportStatusEnums.Answered.ToString();

            await _supportQuestionRepository.Update(currSupportQuestion);
        }

        public async Task UpdateSupportMessage(string token, SupportMessageUpdateReqModel updateReqModel)
        {
            var decodeToken = _decodeToken.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Admin.ToString()))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "You do not have permission to perform this function");
            }

            var currAdmin = await _systemAdminRepository.GetAdminByUsername(decodeToken.username);

            if (currAdmin == null) throw new ApiException(HttpStatusCode.NotFound, "Admin does not exist");

            var currSupportMessage = await _supportMessageRepository.Get(updateReqModel.MessageId);

            if (currSupportMessage == null) throw new ApiException(HttpStatusCode.NotFound, "Message does not exist");

            currSupportMessage.MessageText = !string.IsNullOrEmpty(updateReqModel.MessageText) ? updateReqModel.MessageText : currSupportMessage.MessageText;

            await _supportMessageRepository.Update(currSupportMessage);

        }

        public async Task RemoveSupportMessage(string token, int messsageId)
        {
            var decodeToken = _decodeToken.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Admin.ToString()))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "You do not have permission to perform this function");
            }

            var currAdmin = await _systemAdminRepository.GetAdminByUsername(decodeToken.username);

            if (currAdmin == null) throw new ApiException(HttpStatusCode.NotFound, "Admin does not exist");

            var currSupportMessage = await _supportMessageRepository.Get(messsageId);

            if (currSupportMessage == null) throw new ApiException(HttpStatusCode.NotFound, "Message does not exist");

            var supportIdOfCurrentMessage = (int)currSupportMessage.SupportId;

            await _supportMessageRepository.Remove(currSupportMessage);

            var currMessageOfQuestion = await _supportMessageRepository.GetSupportMessagesBySupportQuestionId(supportIdOfCurrentMessage);

            if (currMessageOfQuestion.Count <= 0)
            {
                var currSupportQuestion = await _supportQuestionRepository.Get((int)currSupportMessage.SupportId);

                if (currSupportQuestion == null) throw new ApiException(HttpStatusCode.NotFound, "Support question does not exist");

                currSupportQuestion.Status = SupportStatusEnums.Open.ToString();

                await _supportQuestionRepository.Update(currSupportQuestion);
            }
        }
    }
}

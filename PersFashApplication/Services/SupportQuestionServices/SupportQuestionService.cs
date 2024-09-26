using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.SupportQuestion.Request;
using BusinessObject.Models.SupportQuestion.Response;
using Org.BouncyCastle.Asn1.X509;
using Repositories.SupportMessageRepo;
using Repositories.SupportQuestionRepo;
using Repositories.UserRepos;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.SupportQuestionServices
{
    public class SupportQuestionService : ISupportQuestionService
    {
        private readonly ISupportQuestionRepository _supportQuestionRepository;
        private readonly ISupportMessageRepository _supportMessageRepository;
        private readonly IDecodeTokenHandler _decodeToken;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;

        public SupportQuestionService(ISupportQuestionRepository supportQuestionRepository, 
            ISupportMessageRepository supportMessageRepository,
            IDecodeTokenHandler decodeToken, IMapper mapper, 
            ICustomerRepository customerRepository)
        {
            _supportQuestionRepository = supportQuestionRepository;
            _supportMessageRepository = supportMessageRepository;
            _decodeToken = decodeToken;
            _mapper = mapper;
            _customerRepository = customerRepository;
        }
        public async Task CreateNewSupportQuestion(string token, SupportQuestionCreateReqModel supportQuestionCreateReqModel)
        {
            var decodeToken = _decodeToken.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null) throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");

            SupportQuestion supportQuestion = new SupportQuestion
            {
                CustomerId = currCustomer.CustomerId,
                Question = supportQuestionCreateReqModel.Question,
                DateCreated = DateTime.Now,
                Status = SupportStatusEnums.Open.ToString()
            };

            await _supportQuestionRepository.Add(supportQuestion);
        }

        public async Task UpdateSupportQuestion(string token, SupportQuestionUpdateReqModel supportQuestionUpdateReq)
        {
            var decodeToken = _decodeToken.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null) throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");

            var currSupportQuestion = await _supportQuestionRepository.Get(supportQuestionUpdateReq.SupportId);

            if (currSupportQuestion == null) throw new ApiException(HttpStatusCode.NotFound, "Support question does exist");

            currSupportQuestion.Question = !string.IsNullOrEmpty(supportQuestionUpdateReq.Question) ? supportQuestionUpdateReq.Question : currSupportQuestion.Question;

            await _supportQuestionRepository.Update(currSupportQuestion);
        }

        public async Task<List<SupportQuestionViewListResModel>> ViewSupports(string? token, int? page, int? size, string? filterStatus)
        {

            if (token != null)
            {
                var decodeToken = _decodeToken.decode(token);
                if (decodeToken.roleName.Equals(RoleEnums.Admin.ToString()))
                {
                    return _mapper.Map<List<SupportQuestionViewListResModel>>(await _supportQuestionRepository.GetSupportQuestions());
                }
            }

            var supports = await _supportQuestionRepository.GetSupportQuestions(page, size);

            if (!string.IsNullOrEmpty(filterStatus))
            {
                supports = FilterFeature(supports, filterStatus);
            }

            return _mapper.Map<List<SupportQuestionViewListResModel>>(supports.OrderByDescending(x => x.DateCreated).ToList());
        }

        public List<SupportQuestion> FilterFeature(List<SupportQuestion> supportQuestion, string filterStatus)
        {
            switch(filterStatus)
            {
                case "Open":
                    supportQuestion = supportQuestion.Where(x => x.Status.Equals(SupportStatusEnums.Open.ToString())).ToList();
                    break;

                case "Answered":
                    supportQuestion = supportQuestion.Where(x => x.Status.Equals(SupportStatusEnums.Answered.ToString())).ToList();
                    break;
                default:
                    supportQuestion = supportQuestion.Where(x => x.Status.Equals(SupportStatusEnums.Open.ToString())).ToList();
                    break;
            }

            return supportQuestion;
        }

        public async Task RemoveSupportQuestion(string token, int supportQuestionId)
        {
            var decodeToken = _decodeToken.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null) throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");

            var currSupportQuestion = await _supportQuestionRepository.Get(supportQuestionId);

            if (currSupportQuestion == null) throw new ApiException(HttpStatusCode.NotFound, "Support question does exist");

            if (currCustomer.CustomerId != currSupportQuestion.CustomerId) throw new ApiException(HttpStatusCode.BadRequest, "Can not modify other customer question");

            var currMessageOfQuestion = await _supportMessageRepository.GetSupportMessagesBySupportQuestionId(currSupportQuestion.SupportId);

            foreach (var item in currMessageOfQuestion)
            {
                await _supportMessageRepository.Remove(item);
            }

            await _supportQuestionRepository.Remove(currSupportQuestion);
        }
    }
}

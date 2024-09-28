using AutoMapper;
using BusinessObject.Enums;
using BusinessObject.Models.FashionItemsModel.Response;
using BusinessObject.Models.PartnerModel.Response;
using Repositories.FashionItemsRepos;
using Repositories.PartnerRepos;
using Repositories.UserProfilesRepos;
using Repositories.UserRepos;
using Services.EmailService;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.PartnerServices
{
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerProfileRepository _customerProfileRepository;
        private readonly IFashionItemRepository _fashionItemRepository;
        private readonly IDecodeTokenHandler _decodeToken;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public PartnerService(IPartnerRepository partnerRepository, 
            ICustomerRepository customerRepository, 
            ICustomerProfileRepository customerProfileRepository,
            IFashionItemRepository fashionItemRepository,
            IDecodeTokenHandler decodeToken, 
            IEmailService emailService, 
            IMapper mapper)
        {
            _partnerRepository = partnerRepository;
            _customerRepository = customerRepository;
            _customerProfileRepository = customerProfileRepository;
            _fashionItemRepository = fashionItemRepository;
            _decodeToken = decodeToken;
            _emailService = emailService;
            _mapper = mapper;
        }
        //public async Task<List<PartnerViewModel>> RecommendPartnerForCustomer(string token, int? page, int? size)
        //{

        //    List<int> partnerIds = new List<int>();

        //    var decodedToken = _decodeToken.decode(token);

        //    if (!decodedToken.roleName.Equals(RoleEnums.Customer.ToString()))
        //    {
        //        throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
        //    }

        //    var currCustomer = await _customerRepository.GetCustomerByUsername(decodedToken.username);

        //    if (currCustomer == null)
        //    {
        //        throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
        //    }

        //    var currCustomerProfile = await _customerProfileRepository.GetCustomerProfileByCustomerId(currCustomer.CustomerId);

        //    if (currCustomerProfile == null)
        //    {
        //        throw new ApiException(HttpStatusCode.BadRequest, "Please complete the profile setup");
        //    }

        //    var items = await _fashionItemRepository.GetRecommendationFashionItemForCustomer(currCustomer.CustomerId, page, size);

        //    foreach (var item in items)
        //    {
        //        if (!partnerIds.Contains((int)item.PartnerId))
        //        {
        //            partnerIds.Add((int)item.PartnerId);
        //        }
        //    }

        //    var partners = await _partnerRepository.GetPartnersByIds(partnerIds);

        //    return _mapper.Map<List<PartnerViewModel>>(partners);
        //}
    }
}

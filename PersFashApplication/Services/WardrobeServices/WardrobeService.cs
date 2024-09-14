using AutoMapper;
using BusinessObject.Models.WardrobeModel.Request;
using Repositories.UserRepos;
using Repositories.UserSubscriptionRepos;
using Repositories.WardrobeItemRepos;
using Repositories.WardrobeRepos;
using Services.Helpers.Handler.DecodeTokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.WardrobeServices
{
    public class WardrobeService : IWardrobeService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IWardrobeRepository _wardrobeRepository;
        private readonly IWardrobeItemRepository _wardrobeItemRepository;
        private readonly ICustomerSubscriptionRepository _customerSubscriptionRepository;
        private readonly IDecodeTokenHandler _decodeTokenHandler;
        private readonly IMapper _mapper;

        public WardrobeService(ICustomerRepository customerRepository,
            IWardrobeRepository wardrobeRepository,
            IWardrobeItemRepository wardrobeItemRepository, 
            ICustomerSubscriptionRepository customerSubscriptionRepository,
            IDecodeTokenHandler decodeTokenHandler,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _wardrobeRepository = wardrobeRepository;
            _wardrobeItemRepository = wardrobeItemRepository;
            _customerSubscriptionRepository = customerSubscriptionRepository;
            _decodeTokenHandler = decodeTokenHandler;
            _mapper = mapper;
        }
        public Task AddNewItemToWardrobe(string token, WardrobeItemCreateReqModel wardrobeItemCreateReqModel)
        {
            throw new NotImplementedException();
        }

        public Task RemoveItemFromWardrobe(string token, int wardrobeItemId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<object, string>> ViewWardrobeOfCustomer(string token)
        {
            throw new NotImplementedException();
        }
    }
}

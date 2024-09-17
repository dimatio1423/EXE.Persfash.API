using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Models.OutfitModel.Response;
using Repositories.FashionItemsRepos;
using Repositories.OutfitCombinationRepos;
using Repositories.OutfitFavoriteRepos;
using Repositories.SubscriptionRepos;
using Repositories.UserRepos;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.OutfitServices
{
    public class OutfitService : IOutfitService
    {
        private readonly IOutfitCombinationRepository _outfitCombinationRepository;
        private readonly IOutfitFavoriteRepository _outfitFavoriteRepository;
        private readonly IDecodeTokenHandler _decodeToken;
        private readonly IMapper _mapper;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IFashionItemRepository _fashionItemRepository;

        public OutfitService(IOutfitCombinationRepository outfitCombinationRepository, IOutfitFavoriteRepository outfitFavoriteRepository,
            IDecodeTokenHandler decodeToken, IMapper mapper, ISubscriptionRepository subscriptionRepository,
            ICustomerRepository customerRepository, IFashionItemRepository fashionItemRepository)
        {
            _outfitCombinationRepository = outfitCombinationRepository;
            _outfitFavoriteRepository = outfitFavoriteRepository;
            _decodeToken = decodeToken;
            _mapper = mapper;
            _subscriptionRepository = subscriptionRepository;
            _customerRepository = customerRepository;
            _fashionItemRepository = fashionItemRepository;
        }

        public async Task AddOutfitToFavoriteList(string token, int outfitId)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currOutfitRecommendation = await _outfitCombinationRepository.GetOutfitCombinationById(outfitId);

            if (currOutfitRecommendation == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Outfit does not exist");
            }

            OutfitFavorite outfitFavorite = new OutfitFavorite
            {
                CustomerId = currOutfitRecommendation.CustomerId,
                TopItemId = currOutfitRecommendation.TopItemId,
                BottomItemId = currOutfitRecommendation.BottomItemId,
                AccessoriesItemId = currOutfitRecommendation.AccessoriesItemId,
                ShoesItemId = currOutfitRecommendation.ShoesItemId,
                DressItemId = currOutfitRecommendation.DressItemId
            };

            await _outfitFavoriteRepository.Add(outfitFavorite);
        }

        public async Task GenerateOutfitForCustomer(string token)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var fashionItemRecommendation = await _fashionItemRepository.GetRecommendationFashionItemForCustomer(currCustomer.CustomerId);

            await _outfitCombinationRepository.GenerateRecommendationOutfits(currCustomer, 5, fashionItemRecommendation);
        }

        public async Task RemoveOutfitFromFavoriteList(string token, int outfitId)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);
            
            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currFavoriteOutfit = await _outfitFavoriteRepository.GetOutfitFavoriteById(outfitId);

            if (currFavoriteOutfit == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Outfit does not exist");
            }

            await _outfitFavoriteRepository.Remove(currFavoriteOutfit);
        }

        public async Task<OutfitViewDetailsResModel> ViewDetailsOutfit(string token, int outfitId)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currOutfitRecommendation = await _outfitCombinationRepository.GetOutfitCombinationById(outfitId);

            var currOutfitFavorite = await _outfitFavoriteRepository.GetOutfitFavoriteById(outfitId);

            if (currOutfitRecommendation != null)
            {
                return _mapper.Map<OutfitViewDetailsResModel>(currOutfitRecommendation);
            }else if (currOutfitFavorite != null)
            {
                return _mapper.Map<OutfitViewDetailsResModel>(currOutfitFavorite);
            }else
            {
                throw new ApiException(HttpStatusCode.NotFound, "Outfit does not exist");
            }
        }

        public async Task<List<OutfitViewListResModel>> ViewFavoriteOutfits(string token)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var outfitFavorites = await _outfitFavoriteRepository.GetOutfitFavoriteForCustomer(currCustomer.CustomerId);

            return _mapper.Map<List<OutfitViewListResModel>>(outfitFavorites);
        }

        public async Task<List<OutfitViewListResModel>> ViewRecommendationOutfitForCustomer(string token)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);
            
            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var outfitRecommendation = await _outfitCombinationRepository.GetRecommendationOutfitForCustomer(currCustomer);

            return _mapper.Map<List<OutfitViewListResModel>>(outfitRecommendation);
        }
    }
}

using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.OutfitModel.Response;
using Repositories.FashionItemsRepos;
using Repositories.OutfitCombinationRepos;
using Repositories.OutfitFavoriteRepos;
using Repositories.SubscriptionRepos;
using Repositories.UserRepos;
using Repositories.UserSubscriptionRepos;
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
        private readonly ICustomerSubscriptionRepository _customerSubscriptionRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IFashionItemRepository _fashionItemRepository;

        public OutfitService(IOutfitCombinationRepository outfitCombinationRepository, IOutfitFavoriteRepository outfitFavoriteRepository,
            IDecodeTokenHandler decodeToken, IMapper mapper, ISubscriptionRepository subscriptionRepository,
            ICustomerRepository customerRepository, IFashionItemRepository fashionItemRepository, ICustomerSubscriptionRepository customerSubscriptionRepository)
        {
            _outfitCombinationRepository = outfitCombinationRepository;
            _outfitFavoriteRepository = outfitFavoriteRepository;
            _decodeToken = decodeToken;
            _mapper = mapper;
            _subscriptionRepository = subscriptionRepository;
            _customerSubscriptionRepository = customerSubscriptionRepository;
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

            var premiumSubscription = await _subscriptionRepository.GetSubscriptionsByName(SubscriptionTypeEnums.Premium.ToString());

            if (premiumSubscription == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Premium subscription does not exist");
            }

            var currCustomerSubscription = await _customerSubscriptionRepository.GetCustomerSubscriptionByCustomerIdAndSubscriptionId(currCustomer.CustomerId, premiumSubscription.SubscriptionId);


            if (currCustomerSubscription == null || currCustomerSubscription.IsActive == false)
            {
                throw new ApiException(HttpStatusCode.Forbidden, "Please subscribe the premium subscription or extend the subscription to perform this function");
            }

            var currOutfitRecommendation = await _outfitCombinationRepository.GetOutfitCombinationById(outfitId);

            if (currOutfitRecommendation == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Outfit does not exist");
            }

            var currCustomerFavoriteOutfit = await _outfitFavoriteRepository.GetOutfitFavoriteForCustomer(currCustomer.CustomerId);

            if (currCustomerFavoriteOutfit.Any(x =>
            AreIdsEqual(x.TopItemId, currOutfitRecommendation.TopItemId) &&
            AreIdsEqual(x.BottomItemId, currOutfitRecommendation.BottomItemId) &&
            AreIdsEqual(x.ShoesItemId, currOutfitRecommendation.ShoesItemId) &&
            AreIdsEqual(x.AccessoriesItemId, currOutfitRecommendation.AccessoriesItemId) &&
            AreIdsEqual(x.DressItemId, currOutfitRecommendation.DressItemId)))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Outfit already exist in the favorite list");
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

        private bool AreIdsEqual(int? id1, int? id2)
        {
            return id1 == id2; // Handles null values
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

            var premiumSubscription = await _subscriptionRepository.GetSubscriptionsByName(SubscriptionTypeEnums.Premium.ToString());

            if (premiumSubscription == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Premium subscription does not exist");
            }

            var currCustomerSubscription = await _customerSubscriptionRepository.GetCustomerSubscriptionByCustomerIdAndSubscriptionId(currCustomer.CustomerId, premiumSubscription.SubscriptionId);


            if (currCustomerSubscription == null || currCustomerSubscription.IsActive == false)
            {
                throw new ApiException(HttpStatusCode.Forbidden, "Please subscribe the premium subscription or extend the subscription to perform this function");
            }

            var currFavoriteOutfit = await _outfitFavoriteRepository.GetOutfitFavoriteById(outfitId);

            if (currFavoriteOutfit == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Outfit does not exist");
            }

            await _outfitFavoriteRepository.Remove(currFavoriteOutfit);
        }

        public async Task<OutfitViewDetailsResModel> ViewDetailsFavoriteOutfit(string token, int outfitFavoriteId)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currOutfitFavorite = await _outfitFavoriteRepository.GetOutfitFavoriteById(outfitFavoriteId);

            if (currOutfitFavorite != null)
            {
                return _mapper.Map<OutfitViewDetailsResModel>(currOutfitFavorite);
            }
            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "Favorite Outfit does not exist");
            }
        }

        public async Task<OutfitViewDetailsResModel> ViewDetailsRecommendationOutfit(string token, int outfitRecommendationId)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currOutfitRecommendation = await _outfitCombinationRepository.GetOutfitCombinationById(outfitRecommendationId);

            if (currOutfitRecommendation != null)
            {
                return _mapper.Map<OutfitViewDetailsResModel>(currOutfitRecommendation);
            }else
            {
                throw new ApiException(HttpStatusCode.NotFound, "Recommendation Outfit does not exist");
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

            var premiumSubscription = await _subscriptionRepository.GetSubscriptionsByName(SubscriptionTypeEnums.Premium.ToString());

            if (premiumSubscription == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Premium subscription does not exist");
            }

            var currCustomerSubscription = await _customerSubscriptionRepository.GetCustomerSubscriptionByCustomerIdAndSubscriptionId(currCustomer.CustomerId, premiumSubscription.SubscriptionId);


            if (currCustomerSubscription == null || currCustomerSubscription.IsActive == false)
            {
                throw new ApiException(HttpStatusCode.Forbidden, "Please subscribe the premium subscription or extend the subscription to perform this function");
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

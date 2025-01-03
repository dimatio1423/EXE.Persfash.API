﻿using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.CustomerModels.Response;
using BusinessObject.Models.FashionItemsModel.Response;
using BusinessObject.Models.Pagination;
using BusinessObject.Models.WardrobeModel.Request;
using BusinessObject.Models.WardrobeModel.Response;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using Repositories.FashionItemsRepos;
using Repositories.SubscriptionRepos;
using Repositories.UserRepos;
using Repositories.UserSubscriptionRepos;
using Repositories.WardrobeItemRepos;
using Repositories.WardrobeRepos;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IFashionItemRepository _fashionItemRepository;
        private readonly IDecodeTokenHandler _decodeTokenHandler;
        private readonly IMapper _mapper;

        public WardrobeService(ICustomerRepository customerRepository,
            IWardrobeRepository wardrobeRepository,
            IWardrobeItemRepository wardrobeItemRepository, 
            ICustomerSubscriptionRepository customerSubscriptionRepository,
            IDecodeTokenHandler decodeTokenHandler,
            ISubscriptionRepository subscriptionRepository,
            IFashionItemRepository fashionItemRepository,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _wardrobeRepository = wardrobeRepository;
            _wardrobeItemRepository = wardrobeItemRepository;
            _customerSubscriptionRepository = customerSubscriptionRepository;
            _subscriptionRepository = subscriptionRepository;
            _fashionItemRepository = fashionItemRepository;
            _decodeTokenHandler = decodeTokenHandler;
            _mapper = mapper;
        }
        public async Task AddNewItemToWardrobe(string token, WardrobeItemCreateReqModel wardrobeItemCreateReqModel)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

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

            var currWardrobe = await _wardrobeRepository.Get(wardrobeItemCreateReqModel.WardrobeId);

            if (currWardrobe == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Wardrobe of the customer does not exist");
            }

            if (currCustomer.CustomerId != currWardrobe.CustomerId)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify other customers wardrobe");
            }

            var currFashionItem = await _fashionItemRepository.GetFashionItemsById(wardrobeItemCreateReqModel.ItemId);

            if (currFashionItem == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Fashion item does not exist");
            }

            var currWardrobeItem = await _wardrobeItemRepository.GetWardrobeItemsByWardrobeIdAndItemId(currWardrobe.WardrobeId, currFashionItem.ItemId);

            if (currWardrobeItem != null) throw new ApiException(HttpStatusCode.BadRequest, "You already added it to the wardrobe");

            WardrobeItem wardrobeItem = new WardrobeItem
            {
                WardrobeId = currWardrobe.WardrobeId,
                ItemId = currFashionItem.ItemId
            };

            await _wardrobeItemRepository.Add(wardrobeItem);

        }

        public async Task AddNewWardrobe(string token, WardrobeCreateReqModel wardrobeCreateReqModel)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

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

            var totalWardrobeOfCustomer = await _wardrobeRepository.GetWardrobesByCustomerId(currCustomer.CustomerId);

            if (totalWardrobeOfCustomer.Count == 10)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "You have reach the limit for wardrobe, \nPlease remove unused wardrobe to use this feature");
            }

            Wardrobe wardrobe = new Wardrobe
            {
                CustomerId = currCustomer.CustomerId,
                DateAdded = DateTime.Now,
                Notes = wardrobeCreateReqModel.Title
            };

            await _wardrobeRepository.Add(wardrobe);

        }

        public async Task RemoveItemFromWardrobe(string token, int wardrobeItemId)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

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

            var currWardrobeItem = await _wardrobeItemRepository.Get(wardrobeItemId);

            if (currWardrobeItem == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Wardrobe item does not exist");
            }

            var currWardrobe = await _wardrobeRepository.Get((int)currWardrobeItem.WardrobeId);

            if (currWardrobe == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Wardrobe of the customer does not exist");
            }

            if (currCustomer.CustomerId != currWardrobe.CustomerId)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify other customers wardrobe");
            }

            await _wardrobeItemRepository.Remove(currWardrobeItem);

        }

        public async Task RemoveWardrobe(string token, int wardrobeId)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

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

            var currWardrobe = await _wardrobeRepository.Get(wardrobeId);

            if (currWardrobe == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Wardrobe of the customer does not exist");
            }

            if (currCustomer.CustomerId != currWardrobe.CustomerId)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify other customers wardrobe");
            }

            var wardrobeItems = await _wardrobeItemRepository.GetWardrobeItemsByWardrobeId(currWardrobe.WardrobeId);

            foreach (var item in wardrobeItems)
            {
                await _wardrobeItemRepository.Remove(item);
            }

            await _wardrobeRepository.Remove(currWardrobe);
        }

        public async Task UpdateWardrobe(string token, WardrobeUpdateReqModel wardrobeUpdateReqModel)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

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

            var currWardrobe = await _wardrobeRepository.Get(wardrobeUpdateReqModel.WardrobeId);

            if (currWardrobe == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Wardrobe of the customer does not exist");
            }

            if (currCustomer.CustomerId != currWardrobe.CustomerId)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify other customers wardrobe");
            }

            currWardrobe.Notes = !string.IsNullOrEmpty(wardrobeUpdateReqModel.Title) ? wardrobeUpdateReqModel.Title : currWardrobe.Notes;

            await _wardrobeRepository.Update(currWardrobe);
        }

        public async Task<WardrobeViewDetailsResModel> ViewDetailsWardrobeOfCustomer(string token, int wardrobeId)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

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

            var currWardrobe = await _wardrobeRepository.GetWardrobeById(wardrobeId);

            if (currWardrobe == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Wardrobe of the customer does not exist");
            }

            if (currCustomer.CustomerId != currWardrobe.CustomerId)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not view other customers wardrobe");
            }

            Dictionary<string, object> WardrobeItems = new Dictionary<string, object>();

            var customerWardrobe = await _wardrobeItemRepository.GetWardrobeItemsByWardrobeId(currWardrobe.WardrobeId);

            var topsItem = _mapper.Map<List<WardrobeItemViewListResModel>>(customerWardrobe.Where(x => x.Item.Category.Equals(CategoryEnums.Tops.ToString())).ToList());

            var bottomItems = _mapper.Map<List<WardrobeItemViewListResModel>>(customerWardrobe.Where(x => x.Item.Category.Equals(CategoryEnums.Bottoms.ToString())).ToList());

            var accessoriesItems = _mapper.Map<List<WardrobeItemViewListResModel>>(customerWardrobe.Where(x => x.Item.Category.Equals(CategoryEnums.Accessories.ToString())).ToList());

            var shoesItem = _mapper.Map<List<WardrobeItemViewListResModel>>(customerWardrobe.Where(x => x.Item.Category.Equals(CategoryEnums.Shoes.ToString())).ToList());

            var DressItem = _mapper.Map<List<WardrobeItemViewListResModel>>(customerWardrobe.Where(x => x.Item.Category.Equals(CategoryEnums.Dresses.ToString())).ToList());

            WardrobeItems.Add("Tops", topsItem);
            WardrobeItems.Add("Bottoms", bottomItems); 
            WardrobeItems.Add("Accessories", accessoriesItems);
            WardrobeItems.Add("Shoes", shoesItem);
            WardrobeItems.Add("Dresses", DressItem);

            var detailWardrobe = new WardrobeViewDetailsResModel
            {
                WardrobeId = currWardrobe.WardrobeId,
                Customer = _mapper.Map<CustomerViewModel>(currWardrobe.Customer),
                DateAdded = currWardrobe.DateAdded,
                Notes = currWardrobe.Notes,
                WardrobeItems = WardrobeItems,
            };

            detailWardrobe.WardrobeItems = WardrobeItems;

            return detailWardrobe;
        }

        public async Task<Pagination<FashionItemViewListRes>> ViewDetailsWardrobeOfCustomerFilter(string token, int wardrobeId, string filter, int?page, int? size)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

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

            var currWardrobe = await _wardrobeRepository.GetWardrobeById(wardrobeId);

            if (currWardrobe == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Wardrobe of the customer does not exist");
            }

            if (currCustomer.CustomerId != currWardrobe.CustomerId)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not view other customers wardrobe");
            }

            var customerWardrobe = await _wardrobeItemRepository.GetWardrobeItemsByWardrobeId(currWardrobe.WardrobeId);

            switch(filter)
            {
                case "tops":
                    var topsItemIds = customerWardrobe.Where(x => x.Item.Category.Equals(CategoryEnums.Tops.ToString())).Select(x => x.ItemId).ToList();

                    var topsItems = _mapper.Map<List<FashionItemViewListRes>>(await _fashionItemRepository.GetFashionItemsByIds(topsItemIds));

                    var tops = topsItems.Skip(((page ?? 1) - 1) * (size ?? 10))
                    .Take(size ?? 10).ToList();

                    return new Pagination<FashionItemViewListRes>
                    {
                        TotalItems = topsItems.Count,
                        PageSize = size ?? 10,
                        CurrentPage = page ?? 1,
                        Data = tops
                    };

                case "bottoms":
                    var bottomItemsIds = customerWardrobe.Where(x => x.Item.Category.Equals(CategoryEnums.Bottoms.ToString())).Select(x => x.ItemId).ToList();

                    var bottomItems = _mapper.Map<List<FashionItemViewListRes>>(await _fashionItemRepository.GetFashionItemsByIds(bottomItemsIds));

                    var bottoms = bottomItems.Skip(((page ?? 1) - 1) * (size ?? 10))
                    .Take(size ?? 10).ToList();

                    return new Pagination<FashionItemViewListRes>
                    {
                        TotalItems = bottomItems.Count,
                        PageSize = size ?? 10,
                        CurrentPage = page ?? 1,
                        Data = bottoms
                    };

                case "accessories":
                    var accessoriesItemIds = customerWardrobe.Where(x => x.Item.Category.Equals(CategoryEnums.Accessories.ToString())).Select(x => x.ItemId).ToList();

                    var accessoriesItem = _mapper.Map<List<FashionItemViewListRes>>(await _fashionItemRepository.GetFashionItemsByIds(accessoriesItemIds));

                    var accessories = accessoriesItem.Skip(((page ?? 1) - 1) * (size ?? 10))
                    .Take(size ?? 10).ToList();

                    return new Pagination<FashionItemViewListRes>
                    {
                        TotalItems = accessoriesItem.Count,
                        PageSize = size ?? 10,
                        CurrentPage = page ?? 1,
                        Data = accessories
                    };

                case "shoes":
                    var shoesItemId = customerWardrobe.Where(x => x.Item.Category.Equals(CategoryEnums.Shoes.ToString())).Select(x => x.ItemId).ToList();

                    var shoesItem = _mapper.Map<List<FashionItemViewListRes>>(await _fashionItemRepository.GetFashionItemsByIds(shoesItemId));

                    var shoes = shoesItem.Skip(((page ?? 1) - 1) * (size ?? 10))
                    .Take(size ?? 10).ToList();

                    return new Pagination<FashionItemViewListRes>
                    {
                        TotalItems = shoesItem.Count,
                        PageSize = size ?? 10,
                        CurrentPage = page ?? 1,
                        Data = shoes
                    };

                case "dresses":
                    var dressItemIds = customerWardrobe.Where(x => x.Item.Category.Equals(CategoryEnums.Dresses.ToString())).Select(x => x.ItemId).ToList();

                    var dressItems = _mapper.Map<List<FashionItemViewListRes>>(await _fashionItemRepository.GetFashionItemsByIds(dressItemIds));

                    var dresses = dressItems.Skip(((page ?? 1) - 1) * (size ?? 10))
                    .Take(size ?? 10).ToList();

                    return new Pagination<FashionItemViewListRes>
                    {
                        TotalItems = dressItems.Count,
                        PageSize = size ?? 10,
                        CurrentPage = page ?? 1,
                        Data = dresses
                    };
                default:
                     topsItemIds = customerWardrobe.Where(x => x.Item.Category.Equals(CategoryEnums.Tops.ToString())).Select(x => x.ItemId).ToList();

                     topsItems = _mapper.Map<List<FashionItemViewListRes>>(await _fashionItemRepository.GetFashionItemsByIds(topsItemIds));

                     tops = topsItems.Skip(((page ?? 1) - 1) * (size ?? 10))
                    .Take(size ?? 10).ToList();

                    return new Pagination<FashionItemViewListRes>
                    {
                        TotalItems = topsItems.Count,
                        PageSize = size ?? 10,
                        CurrentPage = page ?? 1,
                        Data = tops
                    };
            }
        }

        public async Task<List<WardrobeViewListResModel>> ViewWardrobeOfCustomer(string token)
        {
            var decodeToken = _decodeTokenHandler.decode(token);

            if (!decodeToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

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

            var customerWardrobe = await _wardrobeRepository.GetWardrobesByCustomerId(currCustomer.CustomerId);

            return _mapper.Map<List<WardrobeViewListResModel>>(customerWardrobe);
        }
    }
}

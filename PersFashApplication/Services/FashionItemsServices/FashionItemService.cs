﻿using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.FashionItemsModel.Request;
using BusinessObject.Models.FashionItemsModel.Response;
using Newtonsoft.Json.Linq;
using Repositories.FashionItemImageRepos;
using Repositories.FashionItemsRepos;
using Repositories.PartnerRepos;
using Services.AWSService;
using Services.AWSServices;
using Services.FileServices;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TagLib.Ape;

namespace Services.FashionItemsServices
{
    public class FashionItemService : IFashionItemService
    {
        private readonly IFashionItemRepository _fashionItemRepository;
        private readonly IFashionItemImageRepository _fashionItemImageRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IFileService _fileService;
        private readonly IAWSService _aWSService;
        private readonly IMapper _mapper;
        private readonly IDecodeTokenHandler _decodeToken;

        public FashionItemService(IDecodeTokenHandler decodeToken, 
            IFashionItemRepository fashionItemRepository,
            IFashionItemImageRepository fashionItemImageRepository,
            IPartnerRepository partnerRepository,
            IFileService fileService,
            IAWSService aWSService,
            IMapper mapper)
        {
            _fashionItemRepository = fashionItemRepository;
            _fashionItemImageRepository = fashionItemImageRepository;
            _partnerRepository = partnerRepository;
            _fileService = fileService;
            _aWSService = aWSService;
            _mapper = mapper;
            _decodeToken = decodeToken;
        }
        public async Task CreateNewFashionItem(string token, FashionItemCreateReqModel fashionItemCreateReqModel)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.Partner.ToString()))
            {
                throw new ApiException(System.Net.HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currPartner = await _partnerRepository.GetPartnerByUsername(decodedToken.username);

            if (currPartner == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "Partner does not exist");

            }

            //_fileService.CheckImageFile(fashionItemCreateReqModel.Thumbnail);

            //foreach (var item in fashionItemCreateReqModel.ItemImages)
            //{
            //    _fileService.CheckImageFile(item);
            //}

            if (!Enum.IsDefined(typeof(FitTypeEnums), fashionItemCreateReqModel.FitType))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Invalid Fit type");
            }

            if (!Enum.IsDefined(typeof(GenderTargertEnums), fashionItemCreateReqModel.GenderTarget))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Invalid gender target type");
            }


            //var thumbnailLink = await _aWSService.UploadFile(fashionItemCreateReqModel.Thumbnail, "persfash-application", null);

            FashionItem fashionItem = new FashionItem
            {
                ItemName = fashionItemCreateReqModel.ItemName,
                Category = fashionItemCreateReqModel.Category,
                Brand = currPartner.PartnerName,
                Price = fashionItemCreateReqModel.Price,
                FitType = fashionItemCreateReqModel.FitType,
                GenderTarget = fashionItemCreateReqModel.GenderTarget,
                FashionTrend = string.Join(",", fashionItemCreateReqModel.FashionTrend),
                Size = string.Join(",", fashionItemCreateReqModel.Size),
                Color = string.Join(",", fashionItemCreateReqModel.Color),
                Material = string.Join(",", fashionItemCreateReqModel.Material),
                Occasion = string.Join(",", fashionItemCreateReqModel.Occasion),
                ThumbnailUrl = fashionItemCreateReqModel.Thumbnail,
                ProductUrl = fashionItemCreateReqModel.ProductUrl,
                PartnerId = currPartner.PartnerId,
                DateAdded = DateTime.Now,
                Status = StatusEnums.Available.ToString(),
            };

            var itemId = await _fashionItemRepository.AddFashionItem(fashionItem);

            foreach (var item in fashionItemCreateReqModel.ItemImages)
            {
                //var itemLink = await _aWSService.UploadFile(item, "persfash-application", null);

                FashionItemImage fashionItemImage = new FashionItemImage
                {
                    ItemId = itemId,
                    ImageUrl = item,
                };

                await _fashionItemImageRepository.Add(fashionItemImage);
            }
        }

        public async Task DeleteFashionItem(string token, int fashionItemId)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.Partner.ToString()))
            {
                throw new ApiException(System.Net.HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currPartner = await _partnerRepository.GetPartnerByUsername(decodedToken.username);

            if (currPartner == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "Partner does not exist");

            }

            var currItem = await _fashionItemRepository.Get(fashionItemId);
            if (currItem == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "Fashion item does not exist");
            }

            if (currPartner.PartnerId != currItem.PartnerId)
            {
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Can not delete other partners' fashion items");
            }

            currItem.Status = StatusEnums.Unavailable.ToString();

            await _fashionItemRepository.Update(currItem);
        }

        public Task<List<FashionItemViewListRes>> FashionItemRecommendationForCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FashionItemViewListRes>> SearchFashionItems(int? page, int? size, FashionItemFilterReqModel? fashionItemFilterReqModel, string? sortBy, string? searchValue)
        {
            var fashionItems = await _fashionItemRepository.GetFashionItems(page, size);


            if (!string.IsNullOrEmpty(searchValue))
            {
                fashionItems = fashionItems.Where(x => x.ItemName.ToLower().Contains(searchValue.Trim().ToLower())).ToList();
            }

            if (fashionItemFilterReqModel != null)
            {
                fashionItems = filterFashionItem(fashionItems, fashionItemFilterReqModel);
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                fashionItems = sortFashionItem(fashionItems, sortBy);
            }
            return _mapper.Map<List<FashionItemViewListRes>>(fashionItems);
        }

        public async Task UpdateFashionItem(string token, FashionItemUpdateReqModel fashionItemUpdateReqModel)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.Partner.ToString()))
            {
                throw new ApiException(System.Net.HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currPartner = await _partnerRepository.GetPartnerByUsername(decodedToken.username);

            if (currPartner == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "Partner does not exist");

            }

            var currItem = await _fashionItemRepository.Get(fashionItemUpdateReqModel.itemId);

            if (currItem == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "Current Item does not exist");

            }

            if (currPartner.PartnerId != currItem.PartnerId)
            {
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Can not modify other partners' fashion items");

            }

            //if (fashionItemUpdateReqModel.Thumbnail != null)
            //{
            //    _fileService.CheckImageFile(fashionItemUpdateReqModel.Thumbnail);
            //}


            //if (fashionItemUpdateReqModel.ItemImages != null && fashionItemUpdateReqModel.ItemImages.Count > 0)
            //{
            //    foreach (var item in fashionItemUpdateReqModel.ItemImages)
            //    {
            //        _fileService.CheckImageFile(item);
            //    }
            //}

            if (!string.IsNullOrEmpty(fashionItemUpdateReqModel.FitType))
            {
                if (!Enum.IsDefined(typeof(FitTypeEnums), fashionItemUpdateReqModel.FitType))
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "Invalid Fit type");
                }
            }

            if (!string.IsNullOrEmpty(fashionItemUpdateReqModel.GenderTarget))
            {
                if (!Enum.IsDefined(typeof(GenderTargertEnums), fashionItemUpdateReqModel.GenderTarget))
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "Invalid gender target type");
                }
            }

            //var thumbnailLink = fashionItemUpdateReqModel.Thumbnail != null ? await _aWSService.UploadFile(fashionItemUpdateReqModel.Thumbnail, "persfash-application", null) : null;

            currItem.ItemName = !string.IsNullOrEmpty(fashionItemUpdateReqModel.ItemName) ? fashionItemUpdateReqModel.ItemName : currItem.ItemName;
            currItem.Category = fashionItemUpdateReqModel.Category;
            currItem.Price = fashionItemUpdateReqModel.Price != null ? fashionItemUpdateReqModel.Price : currItem.Price;
            currItem.FitType = !string.IsNullOrEmpty(fashionItemUpdateReqModel.FitType) ? fashionItemUpdateReqModel.FitType : currItem.FitType;
            currItem.GenderTarget = !string.IsNullOrEmpty(fashionItemUpdateReqModel.GenderTarget) ? fashionItemUpdateReqModel.GenderTarget : currItem.GenderTarget;
            currItem.FashionTrend = (fashionItemUpdateReqModel.FashionTrend != null && fashionItemUpdateReqModel.FashionTrend.Count > 0) ? string.Join(", ", fashionItemUpdateReqModel.FashionTrend) : currItem.FashionTrend;
            currItem.Size = (fashionItemUpdateReqModel.Size != null && fashionItemUpdateReqModel.Size.Count > 0) ? string.Join(", ", fashionItemUpdateReqModel.Size) : currItem.Size;
            currItem.Color = (fashionItemUpdateReqModel.Color != null && fashionItemUpdateReqModel.Color.Count > 0) ? string.Join(", ", fashionItemUpdateReqModel.Color) : currItem.Color;
            currItem.Material = (fashionItemUpdateReqModel.Material != null && fashionItemUpdateReqModel.Material.Count > 0) ? string.Join(", ", fashionItemUpdateReqModel.Material) : currItem.Material;
            currItem.Occasion = (fashionItemUpdateReqModel.Occasion != null && fashionItemUpdateReqModel.Occasion.Count > 0) ? string.Join(", ", fashionItemUpdateReqModel.Occasion) : currItem.Occasion;
            
            if (!string.IsNullOrEmpty(fashionItemUpdateReqModel.Thumbnail))
            {
                var s3key = _aWSService.ExtractS3Key(currItem.ThumbnailUrl);
                if (!string.IsNullOrEmpty(s3key))
                {
                    await _aWSService.DeleteFile("persfash-application", s3key);
                }
            }
            
            currItem.ThumbnailUrl = !string.IsNullOrEmpty(fashionItemUpdateReqModel.Thumbnail) ? fashionItemUpdateReqModel.Thumbnail : currItem.ThumbnailUrl;
            currItem.ProductUrl = !string.IsNullOrEmpty(fashionItemUpdateReqModel.ProductUrl) ? fashionItemUpdateReqModel.ProductUrl : currItem.ProductUrl;

            await _fashionItemRepository.Update(currItem);

            if (fashionItemUpdateReqModel.ItemImages != null && fashionItemUpdateReqModel.ItemImages.Count > 0)
            {
                var currItemImages = await _fashionItemImageRepository.GetFashionItemImagesByFashionItemId(fashionItemUpdateReqModel.itemId);

                foreach (var item in currItemImages)
                {
                    var s3Key = _aWSService.ExtractS3Key(item.ImageUrl);

                    if (!string.IsNullOrEmpty(s3Key))
                    {
                        await _aWSService.DeleteFile("persfash-application", s3Key);
                    }
                    
                    await _fashionItemImageRepository.Remove(item);
                }

                foreach (var item in fashionItemUpdateReqModel.ItemImages)
                {
                    //var itemLink = await _aWSService.UploadFile(item, "persfash-application", null);

                    FashionItemImage fashionItemImage = new FashionItemImage
                    {
                        ItemId = currItem.ItemId,
                        ImageUrl = item,
                    };

                    await _fashionItemImageRepository.Add(fashionItemImage);
                }
            }
        }

        public async Task<FashionItemViewResModel> ViewDetailsFashionItem(int fashionItemId)
        {
            var fashionItem = await _fashionItemRepository.GetFashionItemsById(fashionItemId);

            if (fashionItem == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Fashion item does not exist");
            }

            var fashionItemImages = await _fashionItemImageRepository.GetFashionItemImagesByFashionItemId(fashionItem.ItemId);

            var images = fashionItemImages.Select(x => x.ImageUrl).ToList();

            var fashionItemDetails = _mapper.Map<FashionItemViewResModel>(fashionItem);

            fashionItemDetails.ItemImages = images;

            return fashionItemDetails;
        }

        public async Task<List<FashionItemViewListRes>> ViewFashionItems(int? page, int? size, FashionItemFilterReqModel? fashionItemFilterReqModel, string? sortBy)
        {
            var fashionItems = await _fashionItemRepository.GetFashionItems(page, size);

            if (fashionItemFilterReqModel != null)
            {
                fashionItems = filterFashionItem(fashionItems, fashionItemFilterReqModel);
            }

            if (!string.IsNullOrEmpty(sortBy)) {
                fashionItems = sortFashionItem(fashionItems, sortBy);
            }
            return _mapper.Map<List<FashionItemViewListRes>>(fashionItems);
        }

        public async Task<List<FashionItemViewListRes>> ViewFashionItemsByPartnerId(int partnerId, int? page, int? size)
        {
            var currPartner = await _partnerRepository.Get(partnerId);

            if (currPartner == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Partner does not exist");

            }

            var fashionItems = await _fashionItemRepository.GetFashionItemsByPartner(currPartner.PartnerId, page, size);

            return _mapper.Map<List<FashionItemViewListRes>>(fashionItems.Where(x => x.Status.Equals(StatusEnums.Available.ToString())).ToList());
        }

        public async Task<List<FashionItemViewListRes>> ViewFashionItemsOfCurrentPartner(string token, int? page, int? size)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.Partner.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currPartner = await _partnerRepository.GetPartnerByUsername(decodedToken.username);

            if (currPartner == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Partner does not exist");

            }

            var fashionItems = await _fashionItemRepository.GetFashionItemsByPartner(currPartner.PartnerId, page, size);

            return _mapper.Map<List<FashionItemViewListRes>>(fashionItems);
        }

        public List<FashionItem> filterFashionItem(List<FashionItem> fashionItems, FashionItemFilterReqModel fashionItemFilterReqModel)
        {
            if (fashionItemFilterReqModel.Category is not null && fashionItemFilterReqModel.Category.Any())
            {
                fashionItems = fashionItems
                    .Where(x => !string.IsNullOrEmpty(x.Category) && fashionItemFilterReqModel.Category
                    .Any(category => new List<string>(x.Category.Split(new[] { ", " }, StringSplitOptions.None))
                    .Contains(category))).ToList();
            }

            if (fashionItemFilterReqModel.Brand is not null && fashionItemFilterReqModel.Brand.Any())
            {
                fashionItems = fashionItems
                    .Where(x => fashionItemFilterReqModel.Brand
                    .Contains(x.Brand)).ToList();
            }

            if (fashionItemFilterReqModel.FitType is not null && fashionItemFilterReqModel.FitType.Any())
            {
                fashionItems = fashionItems
                    .Where(x => !string.IsNullOrEmpty(x.FitType) && fashionItemFilterReqModel.FitType
                    .Any(fitType => new List<string>(x.FitType.Split(new[] { ", " }, StringSplitOptions.None))
                    .Contains(fitType))).ToList();
            }

            if (fashionItemFilterReqModel.GenderTarget is not null && fashionItemFilterReqModel.GenderTarget.Any())
            {
                fashionItems = fashionItems
                    .Where(x => !string.IsNullOrEmpty(x.GenderTarget) && fashionItemFilterReqModel.GenderTarget
                    .Any(genderTarget => new List<string>(x.GenderTarget.Split(new[] { ", " }, StringSplitOptions.None))
                    .Contains(genderTarget))).ToList();
            }

            if (fashionItemFilterReqModel.FashionTrend is not null && fashionItemFilterReqModel.FashionTrend.Any())
            {
                fashionItems = fashionItems
                    .Where(x => !string.IsNullOrEmpty(x.FashionTrend) && fashionItemFilterReqModel.FashionTrend
                    .Any(fashionTrend => new List<string>(x.FashionTrend.Split(new[] { ", " }, StringSplitOptions.None))
                    .Contains(fashionTrend))).ToList();
            }

            if (fashionItemFilterReqModel.Size is not null && fashionItemFilterReqModel.Size.Any())
            {
                fashionItems = fashionItems
                    .Where(x => !string.IsNullOrEmpty(x.Size) && fashionItemFilterReqModel.Size
                    .Any(size => new List<string>(x.Size.Split(new[] { ", " }, StringSplitOptions.None))
                    .Contains(size))).ToList();
            }

            if (fashionItemFilterReqModel.Color is not null && fashionItemFilterReqModel.Color.Any())
            {
                fashionItems = fashionItems
                    .Where(x => !string.IsNullOrEmpty(x.Color) && fashionItemFilterReqModel.Color
                    .Any(color => new List<string>(x.Color.Split(new[] { ", " }, StringSplitOptions.None))
                    .Contains(color))).ToList();
            }

            if (fashionItemFilterReqModel.Material is not null && fashionItemFilterReqModel.Material.Any())
            {
                fashionItems = fashionItems
                    .Where(x => !string.IsNullOrEmpty(x.Material) && fashionItemFilterReqModel.Material
                    .Any(material => new List<string>(x.Material.Split(new[] { ", " }, StringSplitOptions.None))
                    .Contains(material))).ToList();
            }

            if (fashionItemFilterReqModel.Occasion is not null && fashionItemFilterReqModel.Occasion.Any())
            {
                fashionItems = fashionItems
                    .Where(x => !string.IsNullOrEmpty(x.Occasion) && fashionItemFilterReqModel.Occasion
                    .Any(occasion => new List<string>(x.Occasion.Split(new[] { ", " }, StringSplitOptions.None))
                    .Contains(occasion))).ToList();
            }

            if (fashionItemFilterReqModel.MinPrice.HasValue && fashionItemFilterReqModel.MaxPrice.HasValue)
            {
                fashionItems = fashionItems
                    .Where(x => x.Price >= fashionItemFilterReqModel.MinPrice && x.Price <= fashionItemFilterReqModel.MaxPrice)
                    .ToList();
            }
            return fashionItems;
        }

        public List<FashionItem> sortFashionItem(List<FashionItem> fashionItems, string? sortBy)
        {
            switch (sortBy)
            {
                case "name_asc":
                    fashionItems = fashionItems.OrderBy(x => x.ItemName).ToList();
                    break;

                case "name_desc":
                    fashionItems = fashionItems.OrderByDescending(x => x.ItemName).ToList();
                    break;

                case "price_asc":
                    fashionItems = fashionItems.OrderBy(x => x.Price).ToList();
                    break;

                case "price_desc":
                    fashionItems = fashionItems.OrderByDescending(x => x.Price).ToList();
                    break;

                default:
                    fashionItems = fashionItems.OrderBy(x => x.ItemName).ToList();
                    break;
            }

            return fashionItems;
        }
    }
}

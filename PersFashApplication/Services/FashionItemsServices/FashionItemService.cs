using AutoMapper;
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
                FashionTrend = string.Join(", ", fashionItemCreateReqModel.FashionTrend),
                Size = string.Join(", ", fashionItemCreateReqModel.Size),
                Color = string.Join(", ", fashionItemCreateReqModel.Color),
                Material = string.Join(", ", fashionItemCreateReqModel.Material),
                Occasion = string.Join(", ", fashionItemCreateReqModel.Occasion),
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
            currItem.ThumbnailUrl = !string.IsNullOrEmpty(fashionItemUpdateReqModel.Thumbnail) ? fashionItemUpdateReqModel.Thumbnail : currItem.ThumbnailUrl;
            currItem.ProductUrl = !string.IsNullOrEmpty(fashionItemUpdateReqModel.ProductUrl) ? fashionItemUpdateReqModel.ProductUrl : currItem.ProductUrl;

            await _fashionItemRepository.Update(currItem);

            if (fashionItemUpdateReqModel.ItemImages != null && fashionItemUpdateReqModel.ItemImages.Count > 0)
            {
                var currItemImages = await _fashionItemImageRepository.GetFashionItemImagesByFashionItemId(fashionItemUpdateReqModel.itemId);

                foreach (var item in currItemImages)
                {
                    var s3Key = _aWSService.ExtractS3Key(item.ImageUrl);
                    await _aWSService.DeleteFile("persfash-application", s3Key);
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

        public async Task<List<FashionItemViewListRes>> ViewFashionItems(int? page, int? size)
        {
            var fashionItems = await _fashionItemRepository.GetFashionItems(page, size);

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
    }
}

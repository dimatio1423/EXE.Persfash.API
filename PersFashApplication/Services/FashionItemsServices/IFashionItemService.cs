﻿using BusinessObject.Models.FashionItemsModel.Request;
using BusinessObject.Models.FashionItemsModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FashionItemsServices
{
    public interface IFashionItemService
    {
        Task<List<FashionItemViewListRes>> ViewFashionItems(int? page, int? size, FashionItemFilterReqModel? fashionItemFilterReqModel, string? sortBy);
        Task<List<FashionItemViewListRes>> ViewFashionItemsOfCurrentPartner(string token, int? page, int? size);
        Task<List<FashionItemViewListRes>> ViewFashionItemsByPartnerId(int partnerId, int? page, int? size);
        Task<FashionItemViewResModel> ViewDetailsFashionItem(int fashionItemId);
        Task CreateNewFashionItem(string token, FashionItemCreateReqModel fashionItemCreateReqModel);
        Task UpdateFashionItem(string token, FashionItemUpdateReqModel fashionItemUpdateReqModel);
        Task DeleteFashionItem(string token, int fashionItemId);
        Task<List<FashionItemViewListRes>> SearchFashionItems(int? page, int? size, FashionItemFilterReqModel? fashionItemFilterReqModel, string? sortBy, string? searchValue);
        Task<List<FashionItemViewListRes>> FashionItemRecommendationForCustomer(int customerId);
        Task<List<FashionItemViewListRes>> RecommendFashionItemForCustomer(int customerId);
    }
}

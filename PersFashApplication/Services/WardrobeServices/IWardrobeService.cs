﻿using BusinessObject.Models.FashionItemsModel.Response;
using BusinessObject.Models.Pagination;
using BusinessObject.Models.WardrobeModel.Request;
using BusinessObject.Models.WardrobeModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.WardrobeServices
{
    public interface IWardrobeService
    {
        Task<List<WardrobeViewListResModel>> ViewWardrobeOfCustomer(string token);
        Task<WardrobeViewDetailsResModel> ViewDetailsWardrobeOfCustomer(string token, int wardrobeId);
        Task<Pagination<FashionItemViewListRes>> ViewDetailsWardrobeOfCustomerFilter(string token, int wardrobeId, string filter, int? page, int? size);
        Task AddNewWardrobe(string token, WardrobeCreateReqModel wardrobeCreateReqModel);
        Task UpdateWardrobe(string token, WardrobeUpdateReqModel wardrobeUpdateReqModel);
        Task RemoveWardrobe(string token, int wardrobeId);
        Task AddNewItemToWardrobe(string token, WardrobeItemCreateReqModel wardrobeItemCreateReqModel);
        Task RemoveItemFromWardrobe(string token, int wardrobeItemId);
    }
}

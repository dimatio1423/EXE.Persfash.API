using BusinessObject.Models.OutfitModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OutfitServices
{
    public interface IOutfitService
    {
        Task GenerateOutfitForCustomer(string token);
        Task<List<OutfitViewListResModel>> ViewRecommendationOutfitForCustomer(string token);
        Task<OutfitViewDetailsResModel> ViewDetailsRecommendationOutfit(string token, int outfitId);
        Task<OutfitViewDetailsResModel> ViewDetailsFavoriteOutfit(string token, int outfitId);
        Task AddOutfitToFavoriteList(string token, int outfitId);
        Task RemoveOutfitFromFavoriteList(string token, int outfitId);
        Task<List<OutfitViewListResModel>> ViewFavoriteOutfits(string token);
    }
}

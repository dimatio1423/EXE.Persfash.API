using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OutfitCombinationRepos
{
    public interface IOutfitCombinationRepository : IGenericRepository<OutfitCombination>
    {
        Task<OutfitCombination> GetOutfitCombinationById(int outfitCombinationId);
        Task GenerateRecommendationOutfits(Customer customer, int numberOfOutfit, List<FashionItem> recommendedFashionItem);
        Task<List<OutfitCombination>> GetRecommendationOutfitForCustomer(Customer customer);

    }
}

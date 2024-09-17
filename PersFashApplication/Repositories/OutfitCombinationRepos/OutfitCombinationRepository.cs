using BusinessObject.Entities;
using BusinessObject.Enums;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OutfitCombinationRepos
{
    public class OutfitCombinationRepository : GenericRepository<OutfitCombination>, IOutfitCombinationRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public OutfitCombinationRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task GenerateRecommendationOutfits(Customer customer, int numberOfOutfit, List<FashionItem> recommendedFashionItem)
        {
            try
            {
                var customerOutfit = await _context.OutfitCombinations.Where(x => x.CustomerId == customer.CustomerId).ToListAsync();

                foreach (var item in customerOutfit)
                {
                    _context.OutfitCombinations.Remove(item);
                }

                List<OutfitCombination> outfitCombinations = new List<OutfitCombination>();

                for (int i = 0; i < numberOfOutfit; i++)
                {
                    var topItem = recommendedFashionItem
                        .Where(f => f.Category.Equals(CategoryEnums.Tops.ToString()))
                        .OrderBy(f => Guid.NewGuid())
                        .FirstOrDefault();

                    var bottomItem = recommendedFashionItem
                        .Where(f => f.Category.Equals(CategoryEnums.Bottoms.ToString()))
                        .OrderBy(f => Guid.NewGuid())
                        .FirstOrDefault();

                    var shoesItem = recommendedFashionItem
                        .Where(f => f.Category.Equals(CategoryEnums.Shoes.ToString()))
                        .OrderBy(f => Guid.NewGuid())
                        .FirstOrDefault();

                    var accessoriesItem = recommendedFashionItem
                        .Where(f => f.Category.Equals(CategoryEnums.Accessories.ToString()))
                        .OrderBy(f => Guid.NewGuid())
                        .FirstOrDefault();

                    var dressesItem = recommendedFashionItem
                        .Where(f => f.Category.Equals(CategoryEnums.Dresses.ToString()))
                        .OrderBy(f => Guid.NewGuid())
                        .FirstOrDefault();


                    if (dressesItem != null)
                    {
                        var outfitCombination = new OutfitCombination
                        {
                            CustomerId = customer.CustomerId,
                            DressItemId = dressesItem.ItemId,  // Only the dress
                            ShoesItemId = shoesItem != null ? shoesItem.ItemId : null,
                            AccessoriesItemId = accessoriesItem != null ? accessoriesItem.ItemId : null
                        };
                        if (!outfitCombinations.Contains(outfitCombination))
                        {
                            outfitCombinations.Add(outfitCombination);

                        }
                    }
                    else if (topItem != null && bottomItem != null)
                    {
                        var outfitCombination = new OutfitCombination
                        {
                            CustomerId = customer.CustomerId,
                            TopItemId = topItem.ItemId,
                            BottomItemId = bottomItem.ItemId,
                            ShoesItemId = shoesItem != null ? shoesItem.ItemId : null,
                            AccessoriesItemId = accessoriesItem != null ? accessoriesItem.ItemId : null
                        };
                        if (!outfitCombinations.Any(o =>
                         o.TopItemId == outfitCombination.TopItemId &&
                         o.BottomItemId == outfitCombination.BottomItemId &&
                         o.ShoesItemId == outfitCombination.ShoesItemId &&
                         o.AccessoriesItemId == outfitCombination.AccessoriesItemId))
                        {
                            outfitCombinations.Add(outfitCombination);
                        }
                    }
                }

                await _context.OutfitCombinations.AddRangeAsync(outfitCombinations);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OutfitCombination> GetOutfitCombinationById(int outfitCombinationId)
        {
            try
            {
                return await _context.OutfitCombinations
                    .Include(x => x.TopItem).ThenInclude(x => x.Partner)
                    .Include(x => x.BottomItem).ThenInclude(x => x.Partner)
                    .Include(x => x.AccessoriesItem).ThenInclude(x => x.Partner)
                    .Include(x => x.ShoesItem).ThenInclude(x => x.Partner)
                    .Include(x => x.DressItem).ThenInclude(x => x.Partner)
                    .Include(x => x.Customer)
                    .Where(x => x.OutfitId == outfitCombinationId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<OutfitCombination>> GetRecommendationOutfitForCustomer(Customer customer)
        {
            try
            {
                return await _context.OutfitCombinations
                    .Include(x => x.TopItem).ThenInclude(x => x.Partner)
                    .Include(x => x.BottomItem).ThenInclude(x => x.Partner)
                    .Include(x => x.AccessoriesItem).ThenInclude(x => x.Partner)
                    .Include(x => x.ShoesItem).ThenInclude(x => x.Partner)
                    .Include(x => x.DressItem).ThenInclude(x => x.Partner)
                    .Include(x => x.Customer)
                    .Where(x => x.CustomerId == customer.CustomerId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

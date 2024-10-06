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
                var isFemale = customer.Gender == GenderEnums.Female.ToString();

                var customerOutfit = await _context.OutfitCombinations.Where(x => x.CustomerId == customer.CustomerId).ToListAsync();

                //foreach (var item in customerOutfit)
                //{
                //    _context.OutfitCombinations.Remove(item);
                //}


                List<OutfitCombination> outfitCombinations = new List<OutfitCombination>();

                int updatedOutfitCount = 0;

                int dressOutfitCount = 0;

                for (int i = 0; i < numberOfOutfit; i++)
                {
                    var topItem = recommendedFashionItem
                        .Where(f => f.Category.Equals(CategoryEnums.Tops.ToString()) &&
                         (f.GenderTarget == GenderTargertEnums.Unisex.ToString() || f.GenderTarget == (isFemale ? GenderTargertEnums.Women.ToString() : GenderTargertEnums.Men.ToString())))
                        .OrderBy(f => Guid.NewGuid())
                        .FirstOrDefault();

                    var bottomItem = recommendedFashionItem
                        .Where(f => f.Category.Equals(CategoryEnums.Bottoms.ToString()) &&
                         (f.GenderTarget == GenderTargertEnums.Unisex.ToString() || f.GenderTarget == (isFemale ? GenderTargertEnums.Women.ToString() : GenderTargertEnums.Men.ToString())))
                        .OrderBy(f => Guid.NewGuid())
                        .FirstOrDefault();

                    var shoesItem = recommendedFashionItem
                        .Where(f => f.Category.Equals(CategoryEnums.Shoes.ToString()) &&
                        (f.GenderTarget == GenderTargertEnums.Unisex.ToString() || f.GenderTarget == (isFemale ? GenderTargertEnums.Women.ToString() : GenderTargertEnums.Men.ToString())))
                        .OrderBy(f => Guid.NewGuid())
                        .FirstOrDefault();

                    var accessoriesItem = recommendedFashionItem
                        .Where(f => f.Category.Equals(CategoryEnums.Accessories.ToString()) &&
                        (f.GenderTarget == GenderTargertEnums.Unisex.ToString() || f.GenderTarget == (isFemale ? GenderTargertEnums.Women.ToString() : GenderTargertEnums.Men.ToString())))
                        .OrderBy(f => Guid.NewGuid())
                        .FirstOrDefault();

                    var dressesItem = recommendedFashionItem
                        .Where(f => f.Category.Equals(CategoryEnums.Dresses.ToString()) &&
                         (f.GenderTarget == GenderTargertEnums.Unisex.ToString() || f.GenderTarget == (isFemale ? GenderTargertEnums.Women.ToString() : GenderTargertEnums.Men.ToString())))
                        .OrderBy(f => Guid.NewGuid())
                        .FirstOrDefault();


                    if (isFemale && dressesItem != null && dressOutfitCount < 2)
                    {

                        var existingDressOutfit = (i < customerOutfit.Count)
                                       ? customerOutfit.FirstOrDefault(x => x.DressItemId != null && x.OutfitId == customerOutfit[i].OutfitId)
                                       : null;
                        if (existingDressOutfit != null)
                        {
                            // Update existing outfit
                            existingDressOutfit.DressItemId = dressesItem.ItemId;
                            existingDressOutfit.ShoesItemId = shoesItem != null ? shoesItem.ItemId : null;
                            existingDressOutfit.AccessoriesItemId = accessoriesItem != null ? accessoriesItem.ItemId : null;
                            dressOutfitCount++;
                        }
                        else
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
                                dressOutfitCount++;
                            }
                        }
                        
                    }
                    else if (topItem != null && bottomItem != null)
                    {

                        var existingOutfit = (i < customerOutfit.Count) ? customerOutfit[i] : null;

                        if (existingOutfit != null)
                        {
                            // Update existing outfit
                            existingOutfit.TopItemId = topItem.ItemId;
                            existingOutfit.BottomItemId = bottomItem.ItemId;
                            existingOutfit.ShoesItemId = shoesItem != null ? shoesItem.ItemId : null;
                            existingOutfit.AccessoriesItemId = accessoriesItem != null ? accessoriesItem.ItemId : null;
                        }
                        else
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
                }

                if (outfitCombinations.Any())
                {
                    await _context.OutfitCombinations.AddRangeAsync(outfitCombinations);
                }
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
                    .Include(x => x.TopItem).ThenInclude(x => x.FashionItemImages)
                    .Include(x => x.BottomItem).ThenInclude(x => x.FashionItemImages)
                    .Include(x => x.AccessoriesItem).ThenInclude(x => x.FashionItemImages)
                    .Include(x => x.ShoesItem).ThenInclude(x => x.FashionItemImages)
                    .Include(x => x.DressItem).ThenInclude(x => x.FashionItemImages)
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
                    .Include(x => x.TopItem)
                    .Include(x => x.BottomItem)
                    .Include(x => x.AccessoriesItem)
                    .Include(x => x.ShoesItem)
                    .Include(x => x.DressItem)
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

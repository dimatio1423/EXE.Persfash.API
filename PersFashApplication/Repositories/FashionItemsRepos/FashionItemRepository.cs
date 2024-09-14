using Azure;
using BusinessObject.Entities;
using BusinessObject.Enums;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FashionItemsRepos
{
    public class FashionItemRepository : GenericRepository<FashionItem>, IFashionItemRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public FashionItemRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> AddFashionItem(FashionItem fashionItem)
        {
            try
            {
                await _context.FashionItems.AddAsync(fashionItem);
                await _context.SaveChangesAsync();

                return fashionItem.ItemId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<FashionItem>> GetFashionItems(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.FashionItems
                    .Include(x => x.Partner)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<FashionItem> GetFashionItemsById(int itemId)
        {
            try
            {
                return await _context.FashionItems.Include(x => x.Partner)
                .Where(x => x.ItemId == itemId)
                    .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<FashionItem>> GetFashionItemsByPartner(int partnerId, int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;
                
                return await _context.FashionItems
                    .Include(x => x.Partner)
                    .Where(x => x.PartnerId == partnerId)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<FashionItem>> GetRecommendationFashionItemForCustomer(int customerId, int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                var customerProfile = await _context.CustomerProfiles
                    .Include(x => x.Customer)
                    .Where(x => x.CustomerId == customerId)
                    .FirstOrDefaultAsync();

                var fashionItemQuery = await _context.FashionItems
                    .Include(x => x.Partner).AsQueryable().ToListAsync();

                var allCriteriaMatchItems = fashionItemQuery.AsEnumerable();  // For items matching all criteria
                var atLeastOneMatchItems = new List<FashionItem>();


                // fit type
                if (!string.IsNullOrEmpty(customerProfile.FitPreferences))
                {
                    var preferredFitType = customerProfile.FitPreferences.Split(new[] { ", " }, StringSplitOptions.None).ToList();

                    var fitMatches = fashionItemQuery.Where(item => !string.IsNullOrEmpty(item.FitType) &&
                    item.FitType.Split(new[] { ", " }, StringSplitOptions.None).Any(fit => preferredFitType.Contains(fit))).ToList();

                    allCriteriaMatchItems = allCriteriaMatchItems.Intersect(fitMatches).ToList();  // Narrow down to match all
                    atLeastOneMatchItems.AddRange(fitMatches);

                }

                // fashion type
                if (!string.IsNullOrEmpty(customerProfile.FashionStyle))
                {
                    var preferredFashion = customerProfile.FashionStyle.Split(new[] { ", " }, StringSplitOptions.None).ToList();

                    var fashionMatches = fashionItemQuery.Where(item => !string.IsNullOrEmpty(item.FashionTrend) &&
                    item.FashionTrend.Split(new[] { ", " }, StringSplitOptions.None).Any(fashion => preferredFashion.Contains(fashion))).ToList();

                    allCriteriaMatchItems = allCriteriaMatchItems.Intersect(fashionMatches).ToList();
                    atLeastOneMatchItems.AddRange(fashionMatches);

                }


                // size
                if (!string.IsNullOrEmpty(customerProfile.PreferredSize))
                {
                    var preferredSize = customerProfile.PreferredSize.Split(new[] { ", " }, StringSplitOptions.None).ToList();

                    var sizeMatches = fashionItemQuery.Where(item => !string.IsNullOrEmpty(item.Size) &&
                    item.Size.Split(new[] { ", " }, StringSplitOptions.None).Any(size => preferredSize.Contains(size))).ToList();

                    allCriteriaMatchItems = allCriteriaMatchItems.Intersect(sizeMatches).ToList();
                    atLeastOneMatchItems.AddRange(sizeMatches);

                }


                // color
                if (!string.IsNullOrEmpty(customerProfile.PreferredColors))
                {
                    var preferredColor = customerProfile.PreferredColors.Split(new[] { ", " }, StringSplitOptions.None).ToList();

                    var colorMatches = fashionItemQuery.Where(item => !string.IsNullOrEmpty(item.Color) &&
                    item.Color.Split(new[] { ", " }, StringSplitOptions.None).Any(color => preferredColor.Contains(color))).ToList();

                    allCriteriaMatchItems = allCriteriaMatchItems.Intersect(colorMatches).ToList();
                    atLeastOneMatchItems.AddRange(colorMatches);

                }


                // material
                if (!string.IsNullOrEmpty(customerProfile.PreferredMaterials))
                {
                    var preferredMaterial = customerProfile.PreferredMaterials.Split(new[] { ", " }, StringSplitOptions.None).ToList();

                    var materialMatches = fashionItemQuery.Where(item => !string.IsNullOrEmpty(item.Material) &&
                    item.Material.Split(new[] { ", " }, StringSplitOptions.None).Any(material => preferredMaterial.Contains(material))).ToList();

                    allCriteriaMatchItems = allCriteriaMatchItems.Intersect(materialMatches).ToList();
                    atLeastOneMatchItems.AddRange(materialMatches);

                }

                //occaion

                if (!string.IsNullOrEmpty(customerProfile.Occasion))
                {
                    var preferredOccasion = customerProfile.Occasion.Split(new[] { ", " }, StringSplitOptions.None).ToList();

                    var occasionMatches = fashionItemQuery.Where(item => !string.IsNullOrEmpty(item.Occasion) &&
                    item.Occasion.Split(new[] { ", " }, StringSplitOptions.None).Any(occaion => preferredOccasion.Contains(occaion))).ToList();

                    allCriteriaMatchItems = allCriteriaMatchItems.Intersect(occasionMatches).ToList();
                    atLeastOneMatchItems.AddRange(occasionMatches);

                }


                var finalItems = allCriteriaMatchItems.Union(atLeastOneMatchItems).Distinct().ToList();

                // gender
                if (customerProfile.Customer.Gender.Equals(GenderEnums.Male.ToString()))
                {
                    finalItems = finalItems.Where(x => x.GenderTarget.Equals(GenderTargertEnums.Men.ToString())
                    || x.GenderTarget.Equals(GenderTargertEnums.Unisex.ToString())).ToList();

                }
                else
                {
                    finalItems = finalItems.Where(x => x.GenderTarget.Equals(GenderTargertEnums.Women.ToString())
                    || x.GenderTarget.Equals(GenderTargertEnums.Unisex.ToString())).ToList();

                }

                return finalItems.Skip((pageIndex - 1) * sizeIndex).Take(sizeIndex).ToList();


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
    }
}

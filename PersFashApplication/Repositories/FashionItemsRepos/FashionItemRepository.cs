using Azure;
using BusinessObject.Entities;
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
    }
}

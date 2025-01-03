﻿using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.WardrobeItemRepos
{
    public class WardrobeItemRepository : GenericRepository<WardrobeItem>, IWardrobeItemRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public WardrobeItemRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<WardrobeItem>> GetWardrobeItemsByWardrobeId(int wardrobeId)
        {
            try
            {
                return await _context.WardrobeItems.Include(x => x.Item)
                    .Where(x => x.WardrobeId == wardrobeId).ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<WardrobeItem> GetWardrobeItemsByWardrobeIdAndItemId(int wardrobeId, int itemId)
        {
            try
            {
                return await _context.WardrobeItems.Include(x => x.Item)
                    .Where(x => x.WardrobeId == wardrobeId && x.ItemId == itemId).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

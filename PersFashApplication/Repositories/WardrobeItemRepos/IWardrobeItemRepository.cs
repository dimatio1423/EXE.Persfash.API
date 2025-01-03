﻿using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.WardrobeItemRepos
{
    public interface IWardrobeItemRepository : IGenericRepository<WardrobeItem>
    {
        Task<List<WardrobeItem>> GetWardrobeItemsByWardrobeId(int wardrobeId);
        Task<WardrobeItem> GetWardrobeItemsByWardrobeIdAndItemId(int wardrobeId, int itemId);
    }
}

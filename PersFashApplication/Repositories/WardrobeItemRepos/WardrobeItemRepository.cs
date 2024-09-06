﻿using BusinessObject.Entities;
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
    }
}

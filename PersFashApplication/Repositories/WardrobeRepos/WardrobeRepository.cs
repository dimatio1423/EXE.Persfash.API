using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.WardrobeRepos
{
    public class WardrobeRepository : GenericRepository<Wardrobe>, IWardrobeRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public WardrobeRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

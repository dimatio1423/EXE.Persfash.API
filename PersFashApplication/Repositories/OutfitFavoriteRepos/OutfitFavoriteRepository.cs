using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OutfitFavoriteRepos
{
    public class OutfitFavoriteRepository : GenericRepository<OutfitFavorite>, IOutfitFavoriteRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public OutfitFavoriteRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

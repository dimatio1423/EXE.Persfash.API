using BusinessObject.Entities;
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
    }
}

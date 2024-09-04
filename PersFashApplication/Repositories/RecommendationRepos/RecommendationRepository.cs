using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RecommendationRepos
{
    public class RecommendationRepository : GenericRepository<Recommendation>, IRecommnedationRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public RecommendationRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

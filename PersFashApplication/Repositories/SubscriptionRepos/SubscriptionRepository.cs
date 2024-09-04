using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SubscriptionRepos
{
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public SubscriptionRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

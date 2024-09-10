using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Subscription>> GetSubscriptionsByIds(List<int> subscriptionIds)
        {
            try
            {

                return await _context.Subscriptions.Where(x => subscriptionIds.Contains(x.SubscriptionId)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

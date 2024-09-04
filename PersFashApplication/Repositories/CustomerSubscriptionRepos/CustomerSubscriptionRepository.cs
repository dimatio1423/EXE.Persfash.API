using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UserSubscriptionRepos
{
    public class CustomerSubscriptionRepository : GenericRepository<CustomerSubscription>, ICustomerSubscriptionRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public CustomerSubscriptionRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

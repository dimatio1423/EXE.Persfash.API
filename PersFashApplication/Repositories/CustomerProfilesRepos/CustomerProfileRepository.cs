using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UserProfilesRepos
{
    public class CustomerProfileRepository : GenericRepository<CustomerProfile>, ICustomerProfileRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public CustomerProfileRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

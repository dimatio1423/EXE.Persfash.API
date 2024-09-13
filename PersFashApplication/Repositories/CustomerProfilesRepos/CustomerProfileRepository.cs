using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<CustomerProfile> GetCustomerProfileByCustomerId(int customerId)
        {
            try
            {

                return await _context.CustomerProfiles
                    .Include(x => x.Customer)
                    .Where(x => x.CustomerId == customerId).FirstOrDefaultAsync();

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

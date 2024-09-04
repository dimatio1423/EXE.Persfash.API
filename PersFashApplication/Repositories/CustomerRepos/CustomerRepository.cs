using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UserRepos
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public CustomerRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Customer> GetCustomerByEmail(string email)
        {
            try
            {
                return await _context.Customers.FirstOrDefaultAsync(x => x.Email.Equals(email));
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

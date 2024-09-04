using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PartnerRepos
{
    public class PartnerRepository : GenericRepository<Partner>, IPartnerRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public PartnerRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Partner> GetPartnerByEmail(string email)
        {
            try
            {
                return await _context.Partners.FirstOrDefaultAsync(x => x.Email.Equals(email));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

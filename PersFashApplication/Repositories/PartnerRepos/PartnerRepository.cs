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

        public async Task<Partner> GetPartnerByUsername(string username)
        {
            try
            {
                return await _context.Partners.FirstOrDefaultAsync(x => x.Username.Equals(username));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

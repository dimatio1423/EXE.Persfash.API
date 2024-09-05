using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FashionInfluencerRepos
{
    public class FashionInfluencerRepository : GenericRepository<FashionInfluencer>, IFashionInfluencerRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public FashionInfluencerRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FashionInfluencer> GetFashionInfluencerByUsername(string username)
        {
            try
            {
                return await _context.FashionInfluencers.FirstOrDefaultAsync(x => x.Username.Equals(username));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RefreshTokenRepos
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public RefreshTokenRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken> GetByRefreshToken(string refreshToken)
        {
            return await _context.RefreshTokens
                .Include(x => x.Customer)
                .Include(x => x.Influencer)
                .Include(x => x.Partner)
                .Include(x => x.Admin)
                .Where(x => x.Token.Equals(refreshToken)).FirstOrDefaultAsync();
        }
    }
}

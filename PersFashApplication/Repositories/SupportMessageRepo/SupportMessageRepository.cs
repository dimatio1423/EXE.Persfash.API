using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SupportMessageRepo
{
    public class SupportMessageRepository : GenericRepository<SupportMessage>, ISupportMessageRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public SupportMessageRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SupportMessage>> GetSupportMessagesBySupportQuestionId(int supportId)
        {
            return await _context.SupportMessages
                .Include(x => x.Support)
                .Where(x => x.SupportId == supportId).ToListAsync();
        }
    }
}

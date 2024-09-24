using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SupportQuestionRepo
{
    public class SupportQuestionRepository : GenericRepository<SupportQuestion>, ISupportQuestionRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public SupportQuestionRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<SupportQuestion>> GetSupportQuestions(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.SupportQuestions.Include(x => x.Customer)
                    .Include(x => x.SupportMessages).ThenInclude(x => x.Admin)
                    .Skip((pageIndex - 1) * sizeIndex).Take(sizeIndex)
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

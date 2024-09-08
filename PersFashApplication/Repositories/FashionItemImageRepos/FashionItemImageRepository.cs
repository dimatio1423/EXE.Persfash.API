using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FashionItemImageRepos
{
    public class FashionItemImageRepository : GenericRepository<FashionItemImage>, IFashionItemImageRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public FashionItemImageRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<FashionItemImage>> GetFashionItemImagesByFashionItemId(int fashionItemId)
        {
            try
            {
                return await _context.FashionItemImages.Where(x => x.ItemId == fashionItemId).ToListAsync();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FashionItemsRepos
{
    public class FashionItemRepository : GenericRepository<FashionItem>, IFashionItemRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public FashionItemRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

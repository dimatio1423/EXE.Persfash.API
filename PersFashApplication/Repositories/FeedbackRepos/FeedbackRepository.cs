using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FeedbackRepos
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public FeedbackRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

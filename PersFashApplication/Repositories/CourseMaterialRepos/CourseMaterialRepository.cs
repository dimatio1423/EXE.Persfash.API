using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CourseMaterialRepos
{
    public class CourseMaterialRepository : GenericRepository<CourseMaterial>, ICourseMaterialRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public CourseMaterialRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

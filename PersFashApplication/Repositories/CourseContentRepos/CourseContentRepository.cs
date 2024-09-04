using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CourseContentRepos
{
    public class CourseContentRepository : GenericRepository<CourseContent>, ICourseContentRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public CourseContentRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

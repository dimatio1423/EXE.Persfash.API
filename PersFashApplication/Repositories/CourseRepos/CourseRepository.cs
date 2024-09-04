using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CourseRepos
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public CourseRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

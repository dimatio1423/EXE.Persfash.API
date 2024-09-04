using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UserCourseRepos
{
    public class CustomerCourseRepository : GenericRepository<CustomerCourse>, ICustomerCourseRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public CustomerCourseRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

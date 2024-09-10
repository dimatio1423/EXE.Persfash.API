using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<CustomerCourse> CheckCustomerCourse(int customerId, int courseId)
        {
            try
            {
                return await _context.CustomerCourses.Where(x => x.CustomerId == customerId && x.CourseId == courseId).FirstOrDefaultAsync();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CustomerCourse>> GetCustomerCoursesByCustomerId(int customerId)
        {
            try
            {
                return await _context.CustomerCourses.Where(x => x.CustomerId == customerId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

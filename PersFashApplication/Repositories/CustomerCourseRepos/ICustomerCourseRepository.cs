using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UserCourseRepos
{
    public interface ICustomerCourseRepository : IGenericRepository<CustomerCourse>
    {
        Task<CustomerCourse> CheckCustomerCourse(int customerId, int courseId);
        Task<List<CustomerCourse>> GetCustomerCoursesByCustomerId(int customerId);
    }
}

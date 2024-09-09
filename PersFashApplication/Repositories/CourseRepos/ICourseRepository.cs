using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CourseRepos
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<List<Course>> GetCoursesByInfluencerId(int influencerId, int? page, int? size);
        Task<List<Course>> GetCoursesByInfluencerId(int influencerId);
        Task<List<Course>> GetCoursesByIds(List<int> courseId);
        Task<Course> GetCourseById(int courseId);
        Task<List<Course>> GetCourses(int? page, int? size);
        Task<int> AddCourse(Course course);
    }
}

using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CourseContentRepos
{
    public interface ICourseContentRepository : IGenericRepository<CourseContent>
    {
        Task<List<CourseContent>> GetCourseContentByCourseId(int courseId, int? page, int? size);
        Task<CourseContent> GetCourseContentById(int courseContentId);
        Task<int> AddCourseContent(CourseContent courseContent);
    }
}

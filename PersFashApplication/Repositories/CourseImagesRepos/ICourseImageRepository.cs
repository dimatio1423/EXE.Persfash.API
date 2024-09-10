using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CourseImagesRepos
{
    public interface ICourseImageRepository : IGenericRepository<CourseImage>
    {
        Task<List<CourseImage>> GetCourseImagesByCourseId(int courseId); 
    }
}

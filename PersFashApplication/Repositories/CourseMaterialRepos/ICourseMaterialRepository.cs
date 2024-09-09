using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CourseMaterialRepos
{
    public interface ICourseMaterialRepository : IGenericRepository<CourseMaterial>
    {
        Task<List<CourseMaterial>> GetCourseMaterialByCourseContentId(int courseContenId, int? page, int? size);
        Task<List<CourseMaterial>> GetCourseMaterialByCourseContentId(int courseContenId);
    }
}

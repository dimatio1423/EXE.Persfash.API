using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CourseImagesRepos
{
    public class CourseImageRepository : GenericRepository<CourseImage>, ICourseImageRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public CourseImageRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CourseImage>> GetCourseImagesByCourseId(int courseId)
        {
            try
            {
                return await _context.CourseImages.Where(x => x.CourseId == courseId).ToListAsync();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

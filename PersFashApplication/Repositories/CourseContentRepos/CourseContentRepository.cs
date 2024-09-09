using Azure;
using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public async Task<int> AddCourseContent(CourseContent courseContent)
        {
            try
            {
                await _context.CourseContents.AddAsync(courseContent);
                await _context.SaveChangesAsync();

                return courseContent.CourseContentId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CourseContent>> GetCourseContentByCourseId(int courseId, int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.CourseContents
                    .Include(x => x.Course)
                    .Where(x => x.CourseId == courseId)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CourseContent> GetCourseContentById(int courseContentId)
        {
            try
            {
                return await _context.CourseContents
                .Include(x => x.Course)
                    .Where(x => x.CourseContentId == courseContentId)
                    .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

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

namespace Repositories.CourseMaterialRepos
{
    public class CourseMaterialRepository : GenericRepository<CourseMaterial>, ICourseMaterialRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public CourseMaterialRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CourseMaterial>> GetCourseMaterialByCourseContentId(int courseContenId, int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.CourseMaterials
                    .Include(x => x.CourseContent)
                    .Where(x => x.CourseContentId == courseContenId)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CourseMaterial>> GetCourseMaterialByCourseContentId(int courseContenId)
        {
            try
            {
                return await _context.CourseMaterials
                .Include(x => x.CourseContent)
                    .Where(x => x.CourseContentId == courseContenId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

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

namespace Repositories.CourseRepos
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public CourseRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> AddCourse(Course course)
        {
            try
            {
                await _context.Courses.AddAsync(course);
                await _context.SaveChangesAsync();

                return course.CourseId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Course> GetCourseById(int courseId)
        {
            try
            {

                return await _context.Courses
                .Include(x => x.Instructor)
                .Where(x => x.CourseId== courseId)
                    .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Course>> GetCourses(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.Courses
                    .Include(x => x.Instructor)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Course>> GetCoursesByIds(List<int> courseIds)
        {
            try
            {

                return await _context.Courses
                .Include(x => x.Instructor)
                .Where(x => courseIds.Contains(x.CourseId))
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Course>> GetCoursesByInfluencerId(int influencerId, int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.Courses
                    .Include(x => x.Instructor)
                    .Where(x => x.InstructorId == influencerId)
                    .Skip((pageIndex - 1) * sizeIndex)
                    .Take(sizeIndex)
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Course>> GetCoursesByInfluencerId(int influencerId)
        {
            try
            {
                return await _context.Courses
                    .Include(x => x.Instructor)
                    .Where(x => x.InstructorId == influencerId)
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FeedbackRepos
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public FeedbackRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Feedback>> GetFeedbacksByCourseId(int courseId)
        {
            try
            {
                return await _context.Feedbacks
                    .Include(x => x.Customer)
                    .Include(x => x.Course)
                    .Where(x => x.CourseId == courseId).ToListAsync();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Feedback>> GetFeedbacksByInfluencerId(int influencerId)
        {
            try
            {
                return await _context.Feedbacks
                    .Include(x => x.Customer)
                    .Include(x => x.Influencer)
                    .Where(x => x.InfluencerId == influencerId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Feedback>> GetFeedbacksByItemId(int itemId)
        {
            try
            {
                return await _context.Feedbacks
                    .Include(x => x.Customer)
                    .Include(x => x.Item)
                    .Where(x => x.ItemId == itemId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

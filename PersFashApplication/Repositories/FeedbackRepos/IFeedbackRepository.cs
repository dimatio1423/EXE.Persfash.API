using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FeedbackRepos
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        Task<List<Feedback>> GetFeedbacksByCourseId(int courseId);
        Task<List<Feedback>> GetFeedbacksByItemId(int itemId);
        Task<List<Feedback>> GetFeedbacksByInfluencerId(int influencerId);
    }
}

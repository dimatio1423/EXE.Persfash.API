using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SupportMessageRepo
{
    public interface ISupportMessageRepository : IGenericRepository<SupportMessage>
    {
        Task<List<SupportMessage>> GetSupportMessagesBySupportQuestionId(int questionId);
    }
}

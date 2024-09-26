using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SupportQuestionRepo
{
    public interface ISupportQuestionRepository : IGenericRepository<SupportQuestion>
    {
        Task<List<SupportQuestion>> GetSupportQuestions(int? page, int? size);
        Task<List<SupportQuestion>> GetSupportQuestions();
    }
}

using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FashionInfluencerRepos
{
    public interface IFashionInfluencerRepository : IGenericRepository<FashionInfluencer>
    {
        Task<FashionInfluencer> GetFashionInfluencerByUsername(string username);

    }
}

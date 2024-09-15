using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.WardrobeRepos
{
    public interface IWardrobeRepository : IGenericRepository<Wardrobe>
    {
        Task<List<Wardrobe>> GetWardrobesByCustomerId(int customerId);
        Task<Wardrobe> GetWardrobeById(int wardrobeId);
    }
}

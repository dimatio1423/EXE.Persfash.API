using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OutfitFavoriteRepos
{
    public interface IOutfitFavoriteRepository : IGenericRepository<OutfitFavorite>
    {
        Task<List<OutfitFavorite>> GetOutfitFavoriteForCustomer(int customerId);
        Task<OutfitFavorite> GetOutfitFavoriteById(int outfitFavoriteId);
    }
}

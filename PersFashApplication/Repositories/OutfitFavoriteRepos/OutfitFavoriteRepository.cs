using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OutfitFavoriteRepos
{
    public class OutfitFavoriteRepository : GenericRepository<OutfitFavorite>, IOutfitFavoriteRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public OutfitFavoriteRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OutfitFavorite> GetOutfitFavoriteById(int outfitFavoriteId)
        {
            try
            {

                return await _context.OutfitFavorites
                    .Include(x => x.Customer)
                    .Include(x => x.TopItem).ThenInclude(x => x.Partner)
                    .Include(x => x.BottomItem).ThenInclude(x => x.Partner)
                    .Include(x => x.AccessoriesItem).ThenInclude(x => x.Partner)
                    .Include(x => x.ShoesItem).ThenInclude(x => x.Partner)
                    .Include(x => x.DressItem).ThenInclude(x => x.Partner)
                    .Where(x => x.OutfitFavoriteId == outfitFavoriteId).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<OutfitFavorite>> GetOutfitFavoriteForCustomer(int customerId)
        {
            try
            {

                return await _context.OutfitFavorites
                    .Include(x => x.Customer)
                    .Include(x => x.TopItem).ThenInclude(x => x.Partner)
                    .Include(x => x.BottomItem).ThenInclude(x => x.Partner)
                    .Include(x => x.AccessoriesItem).ThenInclude(x => x.Partner)
                    .Include(x => x.ShoesItem).ThenInclude(x => x.Partner)
                    .Include(x => x.DressItem).ThenInclude(x => x.Partner)
                    .Where(x => x.CustomerId == customerId).ToListAsync();

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

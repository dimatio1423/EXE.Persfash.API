using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.WardrobeRepos
{
    public class WardrobeRepository : GenericRepository<Wardrobe>, IWardrobeRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public WardrobeRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Wardrobe> GetWardrobeById(int wardrobeId)
        {
            try
            {
                return await _context.Wardrobes.Include(x => x.Customer).Where(x => x.WardrobeId == wardrobeId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Wardrobe>> GetWardrobesByCustomerId(int customerId)
        {
            try
            {
                return await _context.Wardrobes.Include(x => x.Customer).Where(x => x.CustomerId == customerId).ToListAsync();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

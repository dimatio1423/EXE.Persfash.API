using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SystemAdminRepos
{
    public class SystemAdminRepository : GenericRepository<SystemAdmin>, ISystemAdminRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public SystemAdminRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SystemAdmin> GetAdminByUsername(string username)
        {
            try
            {
                return await _context.SystemAdmins.FirstOrDefaultAsync(x => x.Username.Equals(username));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IsExistedByUsername(string username)
        {
            try
            {
                return await _context.SystemAdmins.FirstOrDefaultAsync(x => x.Username.Equals(username)) != null ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

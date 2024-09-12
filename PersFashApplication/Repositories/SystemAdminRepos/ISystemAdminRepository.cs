using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SystemAdminRepos
{
    public interface ISystemAdminRepository : IGenericRepository<SystemAdmin>
    {
        Task<SystemAdmin> GetAdminByUsername(string username);
        Task<bool> IsExistedByUsername(string username);
    }
}

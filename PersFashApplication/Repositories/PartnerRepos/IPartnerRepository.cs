using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PartnerRepos
{
    public interface IPartnerRepository : IGenericRepository<Partner>
    {
        Task<Partner> GetPartnerByUsername(string username);
        Task<Partner> GetPartnerByEmail(string email);
        Task<bool> IsExistedByEmail(string email);
        Task<bool> IsExistedByUsername(string username);

    }
}

using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UserProfilesRepos
{
    public interface ICustomerProfileRepository : IGenericRepository<CustomerProfile>
    {
        Task<CustomerProfile> GetCustomerProfileByCustomerId(int customerId);
    }
}

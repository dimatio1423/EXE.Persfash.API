using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SubscriptionRepos
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<List<Subscription>> GetSubscriptionsByIds(List<int> subscriptionIds);
        Task<Subscription> GetSubscriptionsByName(string name);
    }
}

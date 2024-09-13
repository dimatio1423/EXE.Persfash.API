using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.UserRepos
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetCustomerByUsername(string username);
        Task<Customer> GetCustomerByEmail(string email);
        Task<bool> IsExistedByEmail(string email);
        Task<bool> IsExistedByUsername(string username);
        Task<int> AddCustomer(Customer customer);

    }
}

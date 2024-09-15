using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PaymentTransactionRepos
{
    public class PaymentTransactionRepository : GenericRepository<PaymentTransaction>, IPaymentTransactionRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public PaymentTransactionRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PaymentRepos
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly PersfashApplicationDbContext _context;

        public PaymentRepository(PersfashApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

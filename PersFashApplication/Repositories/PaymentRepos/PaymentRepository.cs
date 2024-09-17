using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<int> AddPayment(Payment payment)
        {
            try
            {
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();

                return payment.PaymentId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Payment> GetPaymentById(int PaymentId)
        {
            try
            {
                return await _context.Payments
                    .Include(x => x.Customer)
                    .Include(x => x.Course)
                    .Include(x => x.Subscription)
                    .Where(x => x.PaymentId == PaymentId).FirstOrDefaultAsync();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

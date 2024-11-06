using Azure;
using BusinessObject.Entities;
using BusinessObject.Enums;
using Microsoft.EntityFrameworkCore;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<List<Payment>> GetPayments(int? page, int? size)
        {
            try
            {
                var pageIndex = (page.HasValue && page > 0) ? page.Value : 1;
                var sizeIndex = (size.HasValue && size > 0) ? size.Value : 10;

                return await _context.Payments
                    .Include(x => x.Customer)
                    .Include(x => x.Subscription)
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Payment>> GetPaymentsForAdmin()
        {
            try
            {
                return await _context.Payments
                    .Include(x => x.Customer)
                    .Include(x => x.Subscription)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<decimal> GetTotalRenenueForWeek(DateTime dateTime)
        {
            int diff = (7 + (dateTime.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime startOfWeek = dateTime.AddDays(-1 * diff).Date;

            DateOnly startDateOfWeek = DateOnly.FromDateTime(startOfWeek);

            // End of the week is Sunday
            DateTime endOfWeek = startOfWeek.AddDays(6);

            DateOnly endDateOfWeek = DateOnly.FromDateTime(endOfWeek);

            return await _context.Payments.Where(x => (DateOnly.FromDateTime(x.PaymentDate) >= startDateOfWeek && DateOnly.FromDateTime(x.PaymentDate) <= endDateOfWeek) && x.Status.Equals(PaymentStatusEnums.Paid.ToString())).SumAsync(x => x.Price);
        }

        public async Task<decimal> GetTotalRevenueForDayRange(DateOnly? startDate, DateOnly? endDate)
        {
            DateOnly start = (startDate.HasValue) ? startDate.Value : DateOnly.FromDateTime(DateTime.Now);
            DateOnly end = (endDate.HasValue) ? endDate.Value : start.AddDays(6);

            if (start > end)
            {
                DateOnly tmp = start;
                start = end;
                end = tmp;
            }
            
            return await _context.Payments.Where(x => (DateOnly.FromDateTime(x.PaymentDate) >= startDate && DateOnly.FromDateTime(x.PaymentDate) <= endDate) && x.Status.Equals(PaymentStatusEnums.Paid.ToString())).SumAsync(x => x.Price);
        }

        public async Task<decimal> GetTotalRevenueForMonth(DateTime dateTime)
        {
            DateTime startOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            DateOnly startDateOfMonth = DateOnly.FromDateTime(startOfMonth);
            DateOnly endDateOfMonth = DateOnly.FromDateTime(endOfMonth);

            return await _context.Payments.Where(x => (DateOnly.FromDateTime(x.PaymentDate) >= startDateOfMonth && DateOnly.FromDateTime(x.PaymentDate) <= endDateOfMonth) && x.Status.Equals(PaymentStatusEnums.Paid.ToString())).SumAsync(x => x.Price);

        }

        public async Task<decimal> GetTotalRevenueToday(DateOnly date)
        {
            return await _context.Payments.Where(x => (DateOnly.FromDateTime(x.PaymentDate) == date ) && x.Status.Equals(PaymentStatusEnums.Paid.ToString())).SumAsync(x => x.Price);
        }
    }
}

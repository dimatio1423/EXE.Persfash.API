using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PaymentRepos
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<int> AddPayment(Payment payment);
        Task<Payment> GetPaymentById(int PaymentId);
        Task<List<Payment>> GetPayments(int? page, int? size);
        Task<List<Payment>> GetPaymentsForAdmin();
        Task<decimal> GetTotalRevenueToday(DateOnly dateTime);
        Task<decimal> GetTotalRenenueForWeek(DateTime dateTime);
        Task<decimal> GetTotalRevenueForMonth(DateTime dateTime);
        Task<decimal> GetTotalRevenueForDayRange(DateOnly? startDate, DateOnly? endDate);
    }
}

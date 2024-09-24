using BusinessObject.Models.PaymentModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PaymentServices
{
    public interface IPaymentService
    {
        Task<List<PaymentViewListResModel>> ViewPaymentForAdmin(int? page, int? size);
    }
}

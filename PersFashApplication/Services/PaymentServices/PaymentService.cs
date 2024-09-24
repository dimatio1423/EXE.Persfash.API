using AutoMapper;
using BusinessObject.Models.PaymentModel.Response;
using Repositories.PaymentRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }
        public async Task<List<PaymentViewListResModel>> ViewPaymentForAdmin(int? page, int? size)
        {
            var payments = await _paymentRepository.GetPayments(page, size);

            return _mapper.Map<List<PaymentViewListResModel>>(payments);
        }
    }
}

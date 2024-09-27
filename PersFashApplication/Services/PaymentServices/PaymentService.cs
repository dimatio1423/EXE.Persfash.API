using AutoMapper;
using BusinessObject.Models.Pagination;
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
        public async Task<Pagination<PaymentViewListResModel>> ViewPaymentForAdmin(int? page, int? size)
        {
            var payments = await _paymentRepository.GetPayments(page, size);

            var totalPayment = await _paymentRepository.GetAll();

            return new Pagination<PaymentViewListResModel>
            {
                TotalItems = totalPayment.Count,
                PageSize = size ?? 10,
                CurrentPage = page ?? 1,
                Data = _mapper.Map<List<PaymentViewListResModel>>(payments)
            };
        }
    }
}

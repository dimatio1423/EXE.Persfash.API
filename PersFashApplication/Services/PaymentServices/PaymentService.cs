using AutoMapper;
using BusinessObject.Entities;
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
            var totalPayment = await _paymentRepository.GetPaymentsForAdmin();

            var payments = totalPayment;

            var pagedItems = payments.Skip(((page ?? 1) - 1) * (size ?? 10))
                    .Take(size ?? 10).ToList();

            return new Pagination<PaymentViewListResModel>
            {
                TotalItems = payments.Count,
                PageSize = size ?? 10,
                CurrentPage = page ?? 1,
                Data = _mapper.Map<List<PaymentViewListResModel>>(pagedItems)
            };
        }
    }
}

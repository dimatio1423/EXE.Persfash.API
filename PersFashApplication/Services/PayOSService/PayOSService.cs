using BusinessObject.Models.PayOSModel.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PayOSService
{
    public class PayOSService : IPayOSService
    {
        private readonly IConfiguration _config;

        public PayOSService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<CreatePaymentResult> createPaymentUrl(PayOSReqModel payOSReqModel)
        {
            PayOS payOS = new PayOS(_config["PayOS:ClientID"], _config["PayOS:ApiKey"], _config["PayOS:ChecksumKey"]);

            //int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));

            ItemData item = new ItemData(payOSReqModel.productName, 1, (int)payOSReqModel.Amount);
            List<ItemData> items = new List<ItemData>();
            items.Add(item);

            PaymentData paymentData = new PaymentData(payOSReqModel.OrderId, (int)payOSReqModel.Amount, "Thanh toán subscription", items, payOSReqModel.CancelUrl, payOSReqModel.RedirectUrl);

            CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);

            return createPayment;
        }
    }
}

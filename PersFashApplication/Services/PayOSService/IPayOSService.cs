using BusinessObject.Models.PayOSModel.Request;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PayOSService
{
    public interface IPayOSService
    {
        Task<CreatePaymentResult> createPaymentUrl(PayOSReqModel payOSReqModel);
    }
}

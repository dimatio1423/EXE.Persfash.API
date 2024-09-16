using BusinessObject.Models.VnPayModel.Request;
using BusinessObject.Models.VnPayModel.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.VnPayService
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPayReqModel model);

        VnPayResModel PaymentResponse(IQueryCollection colletions);
    }
}

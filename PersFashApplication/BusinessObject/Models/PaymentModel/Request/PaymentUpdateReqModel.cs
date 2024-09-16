using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.PaymentModel.Request
{
    public class PaymentUpdateReqModel
    {
        public int paymentId { get; set; }

        public string status { get; set; } = null!;
    }
}

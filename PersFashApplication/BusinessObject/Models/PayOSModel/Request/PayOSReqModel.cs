using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.PayOSModel.Request
{
    public class PayOSReqModel
    {
        public int OrderId { get; set; }
        public int PaymentId { get; set; }
        public string productName { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string RedirectUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}

using BusinessObject.Models.CustomerModels.Response;
using BusinessObject.Models.SubscriptionModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.PaymentModel.Response
{
    public class PaymentViewListResModel
    {
        public int PaymentId { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Price { get; set; }

        public CustomerViewModel Customer { get; set; }

        public SubscriptionViewDetailsResModel? Subscription { get; set; }

        public string? Status { get; set; }
    }
}

using BusinessObject.Models.SubscriptionModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CustomerSubscriptionModel.Response
{
    public class CustomerSubscriptionViewResModel
    {
        public int CustomerSubscriptionId { get; set; }

        public int CustomerId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsActive { get; set; }

        public SubscriptionViewDetailsResModel Subscription { get; set; }
    }
}

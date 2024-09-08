using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.SubscriptionModels.Response
{
    public class SubscriptionViewListResModel
    {
        public int SubscriptionId { get; set; }

        public string SubscriptionTitle { get; set; } = null!;

        public decimal? Price { get; set; }
    }
}

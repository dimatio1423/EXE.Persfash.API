using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.SubscriptionModels.Request
{
    public class SubscriptionUpdateReqModel
    {
        [Required(ErrorMessage ="SubscriptionId is required")]
        public int subscriptionId { get; set; }

        [Range(0.01, 10000000.00, ErrorMessage = "Price must be between 0.01 and 10,000,000.")]
        public decimal? Price { get; set; }

        public int? DurationInDays { get; set; }

        public List<string>? Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.SubscriptionModels.Request
{
    public class SubscriptionPaymentReqModel
    {
        [Required (ErrorMessage ="Subscription id is required")]
        public int subscriptionId { get; set; }
        [Required(ErrorMessage = "RedirectUrl id is required")]
        public string redirectUrl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.SubscriptionModels.Request
{
    public class SubscriptionCreateReqModel
    {
        [Required(ErrorMessage = "Subscription title is required.")]
        [StringLength(100, ErrorMessage = "Subscription title can't be longer than 100 characters.")]
        public string SubscriptionTitle { get; set; } = null!;

        [Range(0.01, 10000000.00, ErrorMessage = "Price must be between 0.01 and 10,000,000.")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Duration In Days is required.")]
        public int? DurationInDays { get; set; }

        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
        public string? Description { get; set; }
    }
}

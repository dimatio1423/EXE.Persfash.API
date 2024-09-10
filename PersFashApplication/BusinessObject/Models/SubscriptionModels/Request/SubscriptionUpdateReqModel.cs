﻿using System;
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
        public string? SubscriptionTitle { get; set; } = null!;

        [Range(0.01, 10000000.00, ErrorMessage = "Price must be between 0.01 and 10,000,000.")]
        public decimal? Price { get; set; }

        public int? DurationInDays { get; set; }

        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
        public string? Description { get; set; }
    }
}
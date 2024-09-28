using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.FeedbackModel.Request
{
    public class GiveFeedbackInfluencerReqModel
    {
        [Required(ErrorMessage = "InfluencerId is required")]
        public int InfluencerId { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Input rating from 1 to 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }
    }
}

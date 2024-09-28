using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.FeedbackModel.Request
{
    public class GiveFeedbackItemReqModel
    {
        [Required (ErrorMessage ="Item id is required")]
        public int ItemId { get; set; }

        [Required (ErrorMessage = "Rating is required")]
        [Range (1, 5, ErrorMessage = "Input rating from 1 to 5") ]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }
    }
}

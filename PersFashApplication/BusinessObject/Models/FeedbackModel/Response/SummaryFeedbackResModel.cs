using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.FeedbackModel.Response
{
    public class SummaryFeedbackResModel
    {
        public List<FeedbackViewResModel> Feedbacks { get; set; }
        public int totalFeedback { get; set; }
        public double averageRating { get; set; }
        public List<RatingResModel> Ratings { get; set; }

    }

    public class RatingResModel
    {
        public int rating { get; set; }

        public int countRating { get; set; }
    }
}

using BusinessObject.Models.CustomerModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.FeedbackModel.Response
{
    public class FeedbackViewResModel
    {
        public int FeedbackId { get; set; }
         
        public CustomerViewModel? Customer { get; set; }

        public int? Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? DateGiven { get; set; }
    }
}

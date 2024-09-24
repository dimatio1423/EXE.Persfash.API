using BusinessObject.Models.CustomerModels.Response;
using BusinessObject.Models.SupportMessage.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.SupportQuestion.Response
{
    public class SupportQuestionViewListResModel
    {
        public int SupportId { get; set; }

        public CustomerViewModel Customer { get; set; }

        public string? Question { get; set; }

        public string? Status { get; set; }

        public DateTime? DateCreated { get; set; }

        public List<SupportMessageViewListResModel> SupportMessages { get; set; }
    }
}

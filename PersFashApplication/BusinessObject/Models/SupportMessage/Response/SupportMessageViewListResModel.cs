using BusinessObject.Models.CustomerModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.SupportMessage.Response
{
    public class SupportMessageViewListResModel
    {
        public int MessageId { get; set; }
        public string? MessageText { get; set; }
        public AdminViewModel Admin { get; set; }
    }
}

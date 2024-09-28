using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.SupportMessage.Request
{
    public class SupportMessageUpdateReqModel
    {
        [Required(ErrorMessage = "MessageId is required")]

        public int MessageId { get; set; }

        public string? MessageText { get; set; }
    }
}

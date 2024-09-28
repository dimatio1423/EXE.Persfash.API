using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.SupportMessage.Request
{
    public class SupportMessageCreateReqModel
    {
        [Required(ErrorMessage = "SupportId is required")]

        public int SupportId { get; set; }

        [Required(ErrorMessage = "MessageText is required")]

        public string? MessageText { get; set; }
    }
}

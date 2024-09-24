using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.SupportQuestion.Request
{
    public class SupportQuestionUpdateReqModel
    {
        [Required (ErrorMessage ="Support ID is required")]
        public int SupportId { get; set; }
        public string? Question { get; set; }
    }
}

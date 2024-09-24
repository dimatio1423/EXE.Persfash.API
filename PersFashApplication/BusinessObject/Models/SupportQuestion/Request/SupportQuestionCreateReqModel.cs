using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.SupportQuestion.Request
{
    public class SupportQuestionCreateReqModel
    {
        [Required(ErrorMessage = "Question is required")]

        public string Question { get; set; }
    }
}

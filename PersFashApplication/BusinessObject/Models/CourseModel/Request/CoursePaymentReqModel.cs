using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CourseModel.Request
{
    public class CoursePaymentReqModel
    {
        [Required(ErrorMessage = "Course id is required")]
        public int courseId { get; set; }
        [Required(ErrorMessage = "RedirectUrl id is required")]
        public string redirectUrl { get; set; }
    }
}

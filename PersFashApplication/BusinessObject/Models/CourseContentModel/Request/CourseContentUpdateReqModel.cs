using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CourseContentModel.Request
{
    public class CourseContentUpdateReqModel
    {
        [Required (ErrorMessage ="Course content id is required")]
        public int CourseContentId { get; set; }

        public string? Content { get; set; }

        [Range(5, int.MaxValue, ErrorMessage = "Duration must be a positive integer and longer than 5 minutes")]
        public int? Duration { get; set; }
    }
}

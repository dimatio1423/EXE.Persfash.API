using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CourseModel.Request
{
    public class CourseUpdateReqModel
    {
        [Required (ErrorMessage ="CourseId is required")]
        public int CourseId { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 100 characters")]
        public string? CourseName { get; set; }

        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal? Price { get; set; }

        public string? Thumbnail { get; set; }

        public List<string>? CourseImages { get; set; }
    }
}

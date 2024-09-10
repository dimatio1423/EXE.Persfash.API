using BusinessObject.Models.CourseContentModel.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CourseModel.Request
{
    public class CourseCreateReqModel
    {
        [Required(ErrorMessage = "Course name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 100 characters")]
        public string? CourseName { get; set; }

        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal? Price { get; set; }

        [MinLength(1, ErrorMessage = "There must be at least one course content")]
        public List<CourseContentCreateReqModel> CourseContents { get; set; }

        [Required(ErrorMessage = "Thumbnail URL is required.")]
        public string Thumbnail { get; set; }

        [Required(ErrorMessage = "At least one image is required.")]
        [MinLength(1, ErrorMessage = "At least one image is required.")]
        public List<string> CourseImages { get; set; }
    }
}

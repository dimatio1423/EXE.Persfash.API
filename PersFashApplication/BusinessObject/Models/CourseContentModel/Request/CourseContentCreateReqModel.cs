using BusinessObject.Models.CourseMaterialModel.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CourseContentModel.Request
{
    public class CourseContentCreateReqModel
    {
        [Required(ErrorMessage = "Content is required")]
        public string? Content { get; set; }

        [Range(5, int.MaxValue, ErrorMessage = "Duration must be a positive integer and longer than 5 minutes")]
        public int? Duration { get; set; }

        [MinLength(1, ErrorMessage = "There must be at least one course material")]

        public List<CourseMaterialCreateReqModel> CourseMaterials { get; set; }

    }

    public class ContentCreateReqModel
    {
        [Required(ErrorMessage = "CourseId is required")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string? Content { get; set; }

        [Range(5, int.MaxValue, ErrorMessage = "Duration must be a positive integer and longer than 5 minutes")]
        public int? Duration { get; set; }

        [MinLength(1, ErrorMessage = "There must be at least one course material")]

        public List<CourseMaterialCreateReqModel> CourseMaterials { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CourseMaterialModel.Request
{
    public class CourseMaterialCreateReqModel
    {
        [Required (ErrorMessage ="Material Name is required")]
        public string? MaterialName { get; set; }

        [Required (ErrorMessage ="Material Link is required")]
        public string? MaterialLink { get; set; }
    }

    public class MaterialCreateReqModel
    {
        [Required (ErrorMessage ="Course content Id is required")]
        public int CourseContentId { get; set; }

        [Required(ErrorMessage = "Material Name is required")]
        public string? MaterialName { get; set; }

        [Required(ErrorMessage = "Material Link is required")]
        public string? MaterialLink { get; set; }
    }
}

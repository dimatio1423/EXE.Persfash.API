using BusinessObject.Models.CourseMaterialModel.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CourseContentModel.Request
{
    public class CourseContentCreateReqModel
    {
        public string? Content { get; set; }

        public int? Duration { get; set; }

        public List<CourseMaterialCreateReqModel> CourseMaterials { get; set; }

    }
}

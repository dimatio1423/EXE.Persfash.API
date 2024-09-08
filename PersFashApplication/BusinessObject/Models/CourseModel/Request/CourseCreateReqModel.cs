using BusinessObject.Models.CourseContentModel.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CourseModel.Request
{
    public class CourseCreateReqModel
    {
        public string? CourseName { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public List<CourseContentCreateReqModel> CourseContents { get; set; }
    }
}

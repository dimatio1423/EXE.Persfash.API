using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CourseModel.Response
{
    public class CourseViewDetailsModel
    {
        public int CourseId { get; set; }

        public string? CourseName { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? InstructorId { get; set; }

        public string? Status { get; set; }
    }
}

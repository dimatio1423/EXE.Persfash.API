using BusinessObject.Models.CourseModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CustomerCourseModel.Response
{
    public class CustomerCourseViewResModel
    {
        public int CustomerCourseId { get; set; }

        public int? CustomerId { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        public CourseViewListResModel Course { get; set; }
    }
}

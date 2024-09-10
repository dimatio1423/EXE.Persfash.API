using BusinessObject.Models.InfluencerModel.Response;
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

        public FashionInfluencerViewResModel Instructor { get; set; }

        public string? Status { get; set; }
    }
}

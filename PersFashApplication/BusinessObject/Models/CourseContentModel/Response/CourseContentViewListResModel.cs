using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CourseContentModel.Response
{
    public class CourseContentViewListResModel
    {
        public int CourseContentId { get; set; }

        public string? Content { get; set; }

        public int? Duration { get; set; }

        public int? CourseId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CourseMaterialModel.Response
{
    public class CourseMaterialViewListResModel
    {
        public int CourseMaterialId { get; set; }

        public string? MaterialName { get; set; }

        public string? MaterialLink { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CourseContentId { get; set; }
    }
}

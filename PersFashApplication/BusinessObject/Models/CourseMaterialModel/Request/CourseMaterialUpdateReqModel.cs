using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CourseMaterialModel.Request
{
    public class CourseMaterialUpdateReqModel
    {
        [Required (ErrorMessage = "CourseMaterialId is required")]
        public int CourseMaterialId { get; set; }

        public string? MaterialName { get; set; }

        public string? MaterialLink { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.WardrobeModel.Request
{
    public class WardrobeCreateReqModel
    {
        [Required (ErrorMessage = "Wardrobe title is required")]
        public string? Title { get; set; }
    }
}

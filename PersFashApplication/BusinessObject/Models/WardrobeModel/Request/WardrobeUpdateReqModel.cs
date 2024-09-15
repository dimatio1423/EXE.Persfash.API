using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.WardrobeModel.Request
{
    public class WardrobeUpdateReqModel
    {
        [Required(ErrorMessage = "Wardrobe id is required")]
        public int WardrobeId { get; set; }
        public string? Title { get; set; }
    }
}

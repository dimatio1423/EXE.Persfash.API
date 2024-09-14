using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.WardrobeModel.Request
{
    public class WardrobeItemCreateReqModel
    {
        [Required(ErrorMessage ="Wardrobe id is required")]
        public int? WardrobeId { get; set; }

        [Required(ErrorMessage = "Item id is required")]

        public int? ItemId { get; set; }
    }
}

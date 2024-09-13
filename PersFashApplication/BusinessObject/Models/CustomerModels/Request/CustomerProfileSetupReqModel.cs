using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CustomerModels.Request
{
    public class CustomerProfileSetupReqModel
    {
        [Required(ErrorMessage = "Body Type is required.")]
        public string BodyType { get; set; }

        [Required(ErrorMessage = "At least one Fashion Style is required.")]
        [MinLength(1, ErrorMessage = "You must specify at least one fashion style.")]
        public List<string> FashionStyle { get; set; }

        [Required(ErrorMessage = "At least one Fit Preference is required.")]
        [MinLength(1, ErrorMessage = "You must specify at least one fit preference.")]
        public List<string> FitPreferences { get; set; }

        [Required(ErrorMessage = "At least one Preferred Size is required.")]
        [MinLength(1, ErrorMessage = "You must specify at least one preferred size.")]
        public List<string> PreferredSize { get; set; }

        [Required(ErrorMessage = "At least one Preferred Color is required.")]
        [MinLength(1, ErrorMessage = "You must specify at least one preferred color.")]
        public List<string> PreferredColors { get; set; }

        [Required(ErrorMessage = "At least one Preferred Material is required.")]
        [MinLength(1, ErrorMessage = "You must specify at least one preferred material.")]
        public List<string> PreferredMaterials { get; set; }

        [Required(ErrorMessage = "At least one Occasion is required.")]
        [MinLength(1, ErrorMessage = "You must specify at least one occasion.")]
        public List<string> Occasion { get; set; }

        [Required(ErrorMessage = "At least one Lifestyle is required.")]
        [MinLength(1, ErrorMessage = "You must specify at least one lifestyle.")]
        public List<string> Lifestyle { get; set; }
    }
}

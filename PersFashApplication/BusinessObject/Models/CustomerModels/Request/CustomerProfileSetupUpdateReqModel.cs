using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CustomerModels.Request
{
    public class CustomerProfileSetupUpdateReqModel
    {
        [Required(ErrorMessage ="Profile ID is required")]
        public int ProfileId { get; set; }

        public string? BodyType { get; set; }

        public List<string>? FashionStyle { get; set; }

        public List<string>? FitPreferences { get; set; }

        public List<string>? PreferredSize { get; set; }

        public List<string>? PreferredColors { get; set; }

        public List<string>? PreferredMaterials { get; set; }

        public List<string>? Occasion { get; set; }

        public List<string>? Lifestyle { get; set; }

        public string? FacebookLink { get; set; }

        public string? InstagramLink { get; set; }

        public string? TikTokLink { get; set; }

    }
}

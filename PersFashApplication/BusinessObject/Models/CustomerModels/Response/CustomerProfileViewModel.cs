using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CustomerModels.Response
{
    public class CustomerProfileViewModel
    {
        public int ProfileId { get; set; }

        public CustomerViewModel? CustomerId { get; set; }

        public string? BodyType { get; set; }

        public string? FashionStyle { get; set; }

        public string? FitPreferences { get; set; }

        public string? PreferredSize { get; set; }

        public string? PreferredColors { get; set; }

        public string? PreferredMaterials { get; set; }

        public string? Occasion { get; set; }

        public string? Lifestyle { get; set; }

        public string? FacebookLink { get; set; }

        public string? InstagramLink { get; set; }

        public string? TikTokLink { get; set; }

        public bool? ProfileSetupComplete { get; set; }
    }

    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfilePicture { get; set; }
    }
}

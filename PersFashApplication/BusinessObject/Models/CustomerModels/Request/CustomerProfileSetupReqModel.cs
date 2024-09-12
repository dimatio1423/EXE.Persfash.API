using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CustomerModels.Request
{
    public class CustomerProfileSetupReqModel
    {
        public string? BodyType { get; set; }

        public List<string>? FashionStyle { get; set; }

        public List<string>? FitPreferences { get; set; }

        public List<string>? PreferredSize { get; set; }

        public List<string>? PreferredColors { get; set; }

        public List<string>? PreferredMaterials { get; set; }

        public List<string>? Occasion { get; set; }

        public List<string>? Lifestyle { get; set; }
    }
}

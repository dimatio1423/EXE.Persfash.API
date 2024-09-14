using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.PartnerModel.Response
{
    public class PartnerViewModel
    {
        public int PartnerId { get; set; }

        public string? PartnerName { get; set; }

        public string? PartnerProfilePicture { get; set; }
    }
}

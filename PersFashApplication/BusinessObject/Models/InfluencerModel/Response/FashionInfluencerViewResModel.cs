using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.InfluencerModel.Response
{
    public class FashionInfluencerViewResModel
    {
        public int InfluencerId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? ProfilePicture { get; set; }
    }
}

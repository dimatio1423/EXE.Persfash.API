using BusinessObject.Models.CustomerModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.WardrobeModel.Response
{
    public class WardrobeViewListResModel
    {
        public int WardrobeId { get; set; }

        public CustomerViewModel Customer { get; set; }

        public string? Notes { get; set; }
    }
}

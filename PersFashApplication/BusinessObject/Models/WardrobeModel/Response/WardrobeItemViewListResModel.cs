using BusinessObject.Models.FashionItemsModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.WardrobeModel.Response
{
    public class WardrobeItemViewListResModel
    {
        public int WardrobeItemId { get; set; }
        public int? WardrobeId { get; set; }
        public FashionItemViewListRes? Item { get; set; }
    }
}

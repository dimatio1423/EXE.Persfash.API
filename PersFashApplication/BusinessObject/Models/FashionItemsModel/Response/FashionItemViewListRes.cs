using BusinessObject.Models.PartnerModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.FashionItemsModel.Response
{
    public class FashionItemViewListRes
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string ThumbnailURL { get; set; }
        public string ProductURL { get; set; }
        //public PartnerViewModel Partner { get; set; }
    }
}

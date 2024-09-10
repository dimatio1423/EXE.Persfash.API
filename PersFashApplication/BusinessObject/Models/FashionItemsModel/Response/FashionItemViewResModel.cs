using BusinessObject.Models.PartnerModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.FashionItemsModel.Response
{
    public class FashionItemViewResModel
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; } = null!;

        public string Category { get; set; }

        public string Brand { get; set; }

        public decimal Price { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public string Material { get; set; }

        public string Occasion { get; set; }

        public string ProductUrl { get; set; }

        public string ThumbnailURL { get; set; }

        public List<string> ItemImages { get; set; }

        public PartnerViewModel Partner { get; set; }
    }
}

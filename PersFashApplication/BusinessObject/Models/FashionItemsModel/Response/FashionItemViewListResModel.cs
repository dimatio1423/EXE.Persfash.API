using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.FashionItemsModel.Response
{
    public class FashionItemViewListResModel
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; } = null!;

        public string Category { get; set; }

        public string Brand { get; set; }

        public decimal Price { get; set; }

        public string? FitType { get; set; }

        public string? GenderTarget { get; set; }

        public List<string>? FashionTrend { get; set; }

        public List<string> Size { get; set; }

        public List<string> Color { get; set; }

        public List<string> Material { get; set; }

        public List<string> Occasion { get; set; }

        public string ProductUrl { get; set; }

        public string ThumbnailURL { get; set; }

        public List<string> ItemImages { get; set; }

        public string Status { get; set; }
    }
}

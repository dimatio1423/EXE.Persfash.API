using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.FashionItemsModel.Request
{
    public class FashionItemFilterReqModel
    {
        public List<string>? Category { get; set; }

        public List<string>? Brand { get; set; }

        public List<string>? FitType { get; set; }

        public List<string>? GenderTarget { get; set; }

        public List<string>? FashionTrend { get; set; }

        public List<string>? Size { get; set; }

        public List<string>? Color { get; set; }

        public List<string>? Material { get; set; }

        public List<string>? Occasion { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }
    }
}

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
        public string ThumnailURL { get; set; }

    }
}

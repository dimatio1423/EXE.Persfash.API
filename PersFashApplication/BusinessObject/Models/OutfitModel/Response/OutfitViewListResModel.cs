using BusinessObject.Models.CustomerModels.Response;
using BusinessObject.Models.FashionItemsModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.OutfitModel.Response
{
    public class OutfitViewListResModel
    {
        public int OutfitId { get; set; }

        public CustomerViewModel? Customer { get; set; }

        public FashionItemViewListRes? TopItem { get; set; }

        public FashionItemViewListRes? BottomItem { get; set; }

        public FashionItemViewListRes? ShoesItem { get; set; }

        public FashionItemViewListRes? AccessoriesItem { get; set; }

        public FashionItemViewListRes? DressItem { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}

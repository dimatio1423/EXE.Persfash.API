using BusinessObject.Models.CustomerModels.Response;
using BusinessObject.Models.FashionItemsModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.OutfitModel.Response
{
    public class OutfitViewDetailsResModel
    {
        public int OutfitId { get; set; }

        public CustomerViewModel? Customer { get; set; }

        public FashionItemViewResModel? TopItem { get; set; }

        public FashionItemViewResModel? BottomItem { get; set; }

        public FashionItemViewResModel? ShoesItem { get; set; }

        public FashionItemViewResModel? AccessoriesItem { get; set; }

        public FashionItemViewResModel? DressItem { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}

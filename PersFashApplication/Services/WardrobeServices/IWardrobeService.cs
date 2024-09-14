using BusinessObject.Models.WardrobeModel.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.WardrobeServices
{
    public interface IWardrobeService
    {
        Task<Dictionary<Object, string>> ViewWardrobeOfCustomer(string token);
        Task AddNewItemToWardrobe(string token, WardrobeItemCreateReqModel wardrobeItemCreateReqModel);
        Task RemoveItemFromWardrobe(string token, int wardrobeItemId);
    }
}

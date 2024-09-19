using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FashionItemsRepos
{
    public interface IFashionItemRepository : IGenericRepository<FashionItem>
    {
        Task<int> AddFashionItem(FashionItem fashionItem);
        //Task<List<FashionItem>> GetFashionItemsByPartner(int partnerId, int? page, int? size);
        Task<FashionItem> GetFashionItemsById(int itemId);
        Task<List<FashionItem>> GetFashionItems(int? page, int? size);
        Task<List<FashionItem>> GetRecommendationFashionItemForCustomer(int customerId, int? page, int? size);
        Task<List<FashionItem>> GetRecommendationFashionItemForCustomer(int customerId);
        Task<List<FashionItem>> GetRecommendationFashionItemForCustomerFilter(int customerId, int? page, int? size, string filter);
    }
}

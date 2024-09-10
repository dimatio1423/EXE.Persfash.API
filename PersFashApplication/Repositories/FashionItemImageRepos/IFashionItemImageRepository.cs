using BusinessObject.Entities;
using Repositories.GenericRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FashionItemImageRepos
{
    public interface IFashionItemImageRepository : IGenericRepository<FashionItemImage>
    {
        Task<List<FashionItemImage>> GetFashionItemImagesByFashionItemId(int fashionItemId);
    }
}

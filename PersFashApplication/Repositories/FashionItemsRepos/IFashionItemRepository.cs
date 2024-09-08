﻿using BusinessObject.Entities;
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

        Task<List<FashionItem>> GetFashionItemsByPartner(int partnerId, int? page, int? size);
    }
}

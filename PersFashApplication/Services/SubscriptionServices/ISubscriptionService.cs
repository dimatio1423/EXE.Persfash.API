﻿using BusinessObject.Models.CustomerSubscriptionModel.Response;
using BusinessObject.Models.SubscriptionModels.Request;
using BusinessObject.Models.SubscriptionModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SubscriptionServices
{
    public interface ISubscriptionService
    {
        Task<List<SubscriptionViewDetailsResModel>> ViewSubscriptions(int? page, int? size);
        Task<SubscriptionViewDetailsResModel> ViewDetailsSubsription(int subscriptionId);
        Task CreateNewSubscription(string token, SubscriptionCreateReqModel subscriptionCreateReqModel);
        Task UpdateSubscription(string token, SubscriptionUpdateReqModel subscriptionUpdateReqModel);
        Task RemoveSubscription(string token, int subscriptionId);
        Task<CustomerSubscriptionViewResModel> ViewDetailsSubscriptionOfCustomer(string token, int customerSubscriptionId);
        Task<List<CustomerSubscriptionViewResModel>> ViewCurrentSubscriptionsOfCustomer(string token);
        Task<List<CustomerSubscriptionViewResModel>> ViewSubscriptionHistoryOfCustomer(string token);
    }
}

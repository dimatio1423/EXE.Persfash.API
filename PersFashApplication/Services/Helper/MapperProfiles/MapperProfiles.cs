using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Models.CustomerSubscriptionModel.Response;
using BusinessObject.Models.FashionItemsModel.Response;
using BusinessObject.Models.SubscriptionModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helper.MapperProfiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {

            //FashionItem
            CreateMap<FashionItem, FashionItemViewResModel>().ReverseMap();

            CreateMap<FashionItem, FashionItemViewListRes>().ReverseMap();

            //Subscription
            CreateMap<Subscription, SubscriptionViewDetailsResModel>().ReverseMap();

            CreateMap<Subscription, SubscriptionViewListResModel>().ReverseMap();

            //CustomerSubscription
            CreateMap<CustomerSubscription, CustomerSubscriptionViewResModel>().ReverseMap();

        }
    }
}

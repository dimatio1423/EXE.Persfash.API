using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Models.CourseContentModel.Response;
using BusinessObject.Models.CourseMaterialModel.Response;
using BusinessObject.Models.CourseModel.Response;
using BusinessObject.Models.CustomerSubscriptionModel.Response;
using BusinessObject.Models.FashionItemsModel.Response;
using BusinessObject.Models.InfluencerModel.Response;
using BusinessObject.Models.PartnerModel.Response;
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

            //Course
            CreateMap<Course, CourseViewListResModel>().ReverseMap();
            CreateMap<Course, CourseViewDetailsModel>().ReverseMap();

            //CourseContent
            CreateMap<CourseContent, CourseContentViewListResModel>().ReverseMap();

            //CourseMaterial
            CreateMap<CourseMaterial, CourseMaterialViewListResModel>().ReverseMap();

            //FashionInfluencer
            CreateMap<FashionInfluencer, FashionInfluencerViewResModel>().ReverseMap();

            //Partner
            CreateMap<Partner, PartnerViewModel>().ReverseMap();


        }
    }
}

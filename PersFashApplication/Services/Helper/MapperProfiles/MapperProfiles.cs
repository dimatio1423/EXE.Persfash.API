using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Models.CourseContentModel.Response;
using BusinessObject.Models.CourseMaterialModel.Response;
using BusinessObject.Models.CourseModel.Response;
using BusinessObject.Models.CustomerModels.Response;
using BusinessObject.Models.CustomerSubscriptionModel.Response;
using BusinessObject.Models.FashionItemsModel.Response;
using BusinessObject.Models.InfluencerModel.Response;
using BusinessObject.Models.PartnerModel.Response;
using BusinessObject.Models.SubscriptionModels.Response;
using BusinessObject.Models.WardrobeModel.Response;
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

            //CustomerProfile
            CreateMap<CustomerProfile, CustomerProfileViewModel>()
                .ForMember(dest => dest.FashionStyle, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.FashionStyle) ? new List<string>(src.FashionStyle.Split(new[] { ", " }, StringSplitOptions.None)) : null))
                .ForMember(dest => dest.FitPreferences, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.FitPreferences) ? new List<string>(src.FitPreferences.Split(new[] { ", " }, StringSplitOptions.None)) : null))
                .ForMember(dest => dest.PreferredSize, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.PreferredSize) ? new List<string>(src.PreferredSize.Split(new[] { ", " }, StringSplitOptions.None)) : null))
                .ForMember(dest => dest.PreferredMaterials, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.PreferredMaterials) ? new List<string>(src.PreferredMaterials.Split(new[] { ", " }, StringSplitOptions.None)) : null))
                .ForMember(dest => dest.PreferredColors, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.PreferredColors) ? new List<string>(src.PreferredColors.Split(new[] { ", " }, StringSplitOptions.None)) : null))
                .ForMember(dest => dest.Occasion, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Occasion) ? new List<string>(src.Occasion.Split(new[] { ", " }, StringSplitOptions.None)) : null))
                .ForMember(dest => dest.Lifestyle, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Lifestyle) ? new List<string>(src.Lifestyle.Split(new[] { ", " }, StringSplitOptions.None)) : null))
                .ReverseMap();

            //Customer
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
            CreateMap<Customer, CustomerInformationViewModel>().ReverseMap();


            //Wardrobe
            CreateMap<Wardrobe, WardrobeViewListResModel>().ReverseMap();
            CreateMap<Wardrobe, WardrobeViewDetailsResModel>().ReverseMap();

            //WardrobeItem
            CreateMap<WardrobeItem, WardrobeItemViewListResModel>().ReverseMap();



        }
    }
}

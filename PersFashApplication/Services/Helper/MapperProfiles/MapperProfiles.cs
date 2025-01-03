﻿using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Models.CourseContentModel.Response;
using BusinessObject.Models.CourseMaterialModel.Response;
using BusinessObject.Models.CourseModel.Response;
using BusinessObject.Models.CustomerModels.Response;
using BusinessObject.Models.CustomerSubscriptionModel.Response;
using BusinessObject.Models.FashionItemsModel.Response;
using BusinessObject.Models.FeedbackModel.Response;
using BusinessObject.Models.InfluencerModel.Response;
using BusinessObject.Models.OutfitModel.Response;
using BusinessObject.Models.PartnerModel.Response;
using BusinessObject.Models.PaymentModel.Response;
using BusinessObject.Models.SubscriptionModels.Response;
using BusinessObject.Models.SupportMessage.Response;
using BusinessObject.Models.SupportQuestion.Response;
using BusinessObject.Models.WardrobeModel.Response;
using Services.Helper.Resolver.CustomerResolver;
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

            CreateMap<FashionItem, FashionItemViewListResModel>()
                .ForMember(dest => dest.FashionTrend, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.FashionTrend) ? new List<string>(src.FashionTrend.Split(new[] { ", " }, StringSplitOptions.None)) : null))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Size) ? new List<string>(src.Size.Split(new[] { ", " }, StringSplitOptions.None)) : null))
                .ForMember(dest => dest.Material, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Material) ? new List<string>(src.Material.Split(new[] { ", " }, StringSplitOptions.None)) : null))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Color) ? new List<string>(src.Color.Split(new[] { ", " }, StringSplitOptions.None)) : null))
                .ForMember(dest => dest.Occasion, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Occasion) ? new List<string>(src.Occasion.Split(new[] { ", " }, StringSplitOptions.None)) : null))
                .ForMember(dest => dest.ItemImages, opt => opt.MapFrom(src => src.FashionItemImages != null ? src.FashionItemImages.Select(x => x.ImageUrl).ToList() : null))
                .ReverseMap();

            //Subscription
            CreateMap<Subscription, SubscriptionViewDetailsResModel>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Description) ? new List<string>(src.Description.Split(new[] {", "}, StringSplitOptions.None)) : null))
                .ReverseMap();

            CreateMap<Subscription, SubscriptionViewListResModel>().ReverseMap();

            //CustomerSubscription
            CreateMap<CustomerSubscription, CustomerSubscriptionViewResModel>()
                .ReverseMap();

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
            //CreateMap<Partner, PartnerViewModel>().ReverseMap();

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
            CreateMap<Customer, CustomerInformationViewModel>()
                .ForMember(dest => dest.Subscription, opt => opt.MapFrom<GetCustomerSubscriptionResolver>())
                .ForMember(dest => dest.IsDoneProfileSetup, opt => opt.MapFrom<GetCustomerProfileSetup>())
                .ReverseMap();


            //Wardrobe
            CreateMap<Wardrobe, WardrobeViewListResModel>().ReverseMap();
            CreateMap<Wardrobe, WardrobeViewDetailsResModel>().ReverseMap();

            //WardrobeItem
            CreateMap<WardrobeItem, WardrobeItemViewListResModel>().ReverseMap();

            //Outfit
            CreateMap<OutfitCombination, OutfitViewListResModel>().ReverseMap();
            CreateMap<OutfitCombination, OutfitViewDetailsResModel>().ReverseMap();

            CreateMap<OutfitFavorite, OutfitViewListResModel>()
                .ForMember(dest => dest.OutfitId, opt => opt.MapFrom(src => src.OutfitFavoriteId))
                .ReverseMap();

            CreateMap<OutfitFavorite, OutfitViewDetailsResModel>()
                .ForMember(dest => dest.OutfitId, opt => opt.MapFrom(src => src.OutfitFavoriteId))
                .ReverseMap();


            //Feedback
            CreateMap<Feedback, FeedbackViewResModel>().ReverseMap();

            //SupportQuestion

            CreateMap<SupportQuestion, SupportQuestionViewListResModel>().ReverseMap();

            //Support Message
            CreateMap<SupportMessage, SupportMessageViewListResModel>().ReverseMap();

            // Admin
            CreateMap<SystemAdmin, AdminViewModel>().ReverseMap();

            //Payment
            CreateMap<Payment, PaymentViewListResModel>().ReverseMap();
        }
    }
}

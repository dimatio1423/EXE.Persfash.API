using Amazon.S3;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MusicStreamingAPI.MiddleWare;
using Repositories;
using Repositories.CourseContentRepos;
using Repositories.CourseImagesRepos;
using Repositories.CourseMaterialRepos;
using Repositories.CourseRepos;
using Repositories.FashionInfluencerRepos;
using Repositories.FashionItemImageRepos;
using Repositories.FashionItemsRepos;
using Repositories.FeedbackRepos;
using Repositories.GenericRepos;
using Repositories.InfluencerPaymentInformationRepos;
using Repositories.OutfitCombinationRepos;
using Repositories.OutfitFavoriteRepos;
using Repositories.PartnerRepos;
using Repositories.PaymentRepos;
using Repositories.PaymentTransactionRepos;
using Repositories.RefreshTokenRepos;
using Repositories.SubscriptionRepos;
using Repositories.SupportMessageRepo;
using Repositories.SupportQuestionRepo;
using Repositories.SystemAdminRepos;
using Repositories.UserCourseRepos;
using Repositories.UserProfilesRepos;
using Repositories.UserRepos;
using Repositories.UserSubscriptionRepos;
using Repositories.WardrobeItemRepos;
using Repositories.WardrobeRepos;
using Services.AuthenticationServices;
using Services.AWSService;
using Services.AWSServices;
using Services.BackgroundServices;
using Services.CloudinaryService;
using Services.CourseContentServices;
using Services.CourseMaterialServices;
using Services.CourseServices;
using Services.EmailService;
using Services.FashionInfluencerServices;
using Services.FashionItemsServices;
using Services.FeedbackServices;
using Services.FileServices;
using Services.Helper.MapperProfiles;
using Services.Helper.VerifyCode;
using Services.Helpers.Handler.DecodeTokenHandler;
using Services.JWTService;
using Services.OutfitServices;
using Services.PartnerServices;
using Services.PaymentServices;
using Services.PayOSService;
using Services.RefreshTokenServices;
using Services.SubscriptionServices;
using Services.SupportMessageServices;
using Services.SupportQuestionServices;
using Services.UserCourseServices;
using Services.UserProfilesServices;
using Services.UserServices;
using Services.UserSubscriptionServices;
using Services.VnPayService;
using Services.WardrobeServices;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//-----------------------------------------AWS-----------------------------------------
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

//-----------------------------------------AUTOMAPPER-----------------------------------------

builder.Services.AddAutoMapper(typeof(MapperProfiles).Assembly);

//-----------------------------------------MIDDLEWARE-----------------------------------------

builder.Services.AddSingleton<GlobalExceptionMiddleware>();

//-----------------------------------------REPOSITORIES-----------------------------------------
builder.Services.AddScoped<ICourseContentRepository, CourseContentRepository>();
builder.Services.AddScoped<ICourseMaterialRepository, CourseMaterialRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IFashionInfluencerRepository, FashionInfluencerRepository>();
builder.Services.AddScoped<IFashionItemRepository, FashionItemRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
//builder.Services.AddScoped<IPartnerRepository, PartnerRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ICustomerProfileRepository, CustomerProfileRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerSubscriptionRepository, CustomerSubscriptionRepository>();
builder.Services.AddScoped<ICustomerCourseRepository, CustomerCourseRepository>();
builder.Services.AddScoped<ISystemAdminRepository, SystemAdminRepository>();
builder.Services.AddScoped<IWardrobeRepository, WardrobeRepository>();
builder.Services.AddScoped<IWardrobeItemRepository, WardrobeItemRepository>();
builder.Services.AddScoped<IFashionItemImageRepository, FashionItemImageRepository>();
builder.Services.AddScoped<ICourseImageRepository, CourseImageRepository>();
builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
builder.Services.AddScoped<IInfluencerPaymentInformationRepository, InfluencerPaymentInformationRepository>();
builder.Services.AddScoped<IOutfitCombinationRepository, OutfitCombinationRepository>();
builder.Services.AddScoped<IOutfitFavoriteRepository, OutfitFavoriteRepository>();
builder.Services.AddScoped<ISupportQuestionRepository, SupportQuestionRepository>();
builder.Services.AddScoped<ISupportMessageRepository, SupportMessageRepository>();

//-----------------------------------------SERVICES-----------------------------------------

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAWSService, AWSService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<ICourseContentService, CourseContentService>();
builder.Services.AddScoped<ICourseMaterialService, CourseMaterialService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFashionInfluencerService, FashionInfluencerService>();
builder.Services.AddScoped<IFashionItemService, FashionItemService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IFileService, FileService>();
//builder.Services.AddScoped<IPartnerService, PartnerService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ICustomerCourseService, CustomerCourseService>();
builder.Services.AddScoped<ICustomerProfileService, CustomerProfileService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerSubscriptionService, CustomerSubscriptionService>();
builder.Services.AddScoped<IDecodeTokenHandler, DecodeTokenHandler>();
builder.Services.AddScoped<IWardrobeService, WardrobeService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddScoped<IOutfitService, OutfitService>();
builder.Services.AddScoped<ISupportQuestionService, SupportQuestionService>();
builder.Services.AddScoped<ISupportMessageService, SupportMessageService>();
builder.Services.AddScoped<IPayOSService, PayOSService>();

//-----------------------------------------VerificationCodeCache-----------------------------------------

builder.Services.AddSingleton<VerificationCodeCache>();

//-----------------------------------------DB-----------------------------------------

builder.Services.AddDbContext<PersfashApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));


//-----------------------------------------AWS-----------------------------------------
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

//-----------------------------------------BGSERVICE-----------------------------------------
builder.Services.AddHostedService<BGService>();

//-----------------------------------------CORS-----------------------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

//-----------------------------------------AUTHENTICATION-----------------------------------------

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:JwtKey"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
        };
    });

//-----------------------------------------AUTHORIZATION-----------------------------------------
builder.Services.AddAuthorization();


//----------------------------------------------------------------------------------

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
            new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
            new string[] {}
         }
     });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

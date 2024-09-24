using BusinessObject.Entities;
using BusinessObject.Models.CourseModel.Request;
using BusinessObject.Models.CourseModel.Response;
using BusinessObject.Models.CustomerCourseModel.Response;
using BusinessObject.Models.PaymentModel.Request;
using Microsoft.AspNetCore.Http;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CourseServices
{
    public interface ICourseService
    {
        Task<List<CourseViewListResModel>> GetCourses(string? token, int? page, int? size, string? sortBy);
        Task<List<CourseViewListResModel>> GetCoursesByInfluencerId(int influencerId, int? page, int? size);
        Task<List<CourseViewListResModel>> GetCoursesOfCurrentInfluencerId(string token, int? page, int? size);
        Task CreateNewCourse(string token, CourseCreateReqModel courseCreateReqModel);
        Task UpdateCourse(string token, CourseUpdateReqModel courseCreateReqModel);
        Task<bool> ActivateDeactivateCourse(string token, int courseId);
        Task<bool> CheckCustomerCourse(string token, int courseId);
        Task<CourseViewDetailsModel> GetCourseDetails(int courseId);
        Task<List<CourseViewListResModel>> GetCourseOfCustomer(string token);
        Task<List<CourseViewListResModel>> SearchCourses(string? token, int? page, int? size, string? searchValue, string? sortBy);

        // Payment
        Task<int> CreateCustomerCourseTransaction(string token, int courseId);
        Task<string> GetPaymentUrl(HttpContext context, int paymentId, string redirectUrl);
        Task<Payment> UpdateCustomerCourseTransaction(PaymentUpdateReqModel paymentUpdateReqModel);
        Task AddCustomerCourse(string token, int courseId);
    }
}

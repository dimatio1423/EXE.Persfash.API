using BusinessObject.Models.CourseModel.Request;
using BusinessObject.Models.CourseModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CourseServices
{
    public interface ICourseService
    {
        Task<CourseViewListResModel> GetCourses(int? page, int? size);
        Task CreateNewCourse(string token, CourseCreateReqModel courseCreateReqModel);
    }
}

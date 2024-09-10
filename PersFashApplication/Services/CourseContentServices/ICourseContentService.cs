using BusinessObject.Models.CourseContentModel.Request;
using BusinessObject.Models.CourseContentModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CourseContentServices
{
    public interface ICourseContentService
    {
        Task<List<CourseContentViewListResModel>> GetCourseContentByCourseId(int courseId, int? page, int? size);
        Task CreateNewCourseContent(string token, ContentCreateReqModel contentCreateReqModel);
        Task UpdateCourseContent(string token, CourseContentUpdateReqModel courseContentUpdateReq);
        Task RemoveCourseContent(string token, int courseContentId);
    }
}

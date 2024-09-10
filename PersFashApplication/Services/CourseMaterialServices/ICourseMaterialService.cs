using BusinessObject.Models.CourseMaterialModel.Request;
using BusinessObject.Models.CourseMaterialModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CourseMaterialServices
{
    public interface ICourseMaterialService
    {
        Task<List<CourseMaterialViewListResModel>> GetCourseMaterialByCourseContentId(int courseContentId, int? page, int? size);

        Task CreateNewCourseMaterial(string token, MaterialCreateReqModel materialCreateReqModel);

        Task UpdateCourseMaterial(string token, CourseMaterialUpdateReqModel courseMaterialUpdateReqModel);

        Task RemoveCourseMaterial(string token, int materialId);
    }
}

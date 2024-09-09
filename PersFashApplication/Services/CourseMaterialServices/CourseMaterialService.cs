using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.CourseContentModel.Request;
using BusinessObject.Models.CourseContentModel.Response;
using BusinessObject.Models.CourseMaterialModel.Request;
using BusinessObject.Models.CourseMaterialModel.Response;
using Repositories.CourseContentRepos;
using Repositories.CourseMaterialRepos;
using Repositories.CourseRepos;
using Repositories.FashionInfluencerRepos;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.CourseMaterialServices
{
    public class CourseMaterialService : ICourseMaterialService
    {
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly ICourseMaterialRepository _courseMaterialRepository;
        private readonly IFashionInfluencerRepository _fashionInfluencerRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly IDecodeTokenHandler _decodeToken;

        public CourseMaterialService(ICourseContentRepository courseContentRepository, 
            ICourseMaterialRepository courseMaterialRepository,
            IFashionInfluencerRepository fashionInfluencerRepository,
            ICourseRepository courseRepository,
             IMapper mapper, IDecodeTokenHandler decodeToken)
        {
            _courseContentRepository = courseContentRepository;
            _courseMaterialRepository = courseMaterialRepository;
            _fashionInfluencerRepository = fashionInfluencerRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
            _decodeToken = decodeToken;
        }
        public async Task CreateNewCourseMaterial(string token, MaterialCreateReqModel materialCreateReqModel)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.FashionInfluencer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currFashionInfluencer = await _fashionInfluencerRepository.GetFashionInfluencerByUsername(decodedToken.username);

            if (currFashionInfluencer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Fashion influencer does not exist");
            }

            var currCourseContent = await _courseContentRepository.GetCourseContentById(materialCreateReqModel.CourseContentId);

            if (currCourseContent == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course content does not exist");
            }

            var influencerCourses = await _courseRepository.GetCoursesByInfluencerId(currFashionInfluencer.InfluencerId);

            if (!influencerCourses.Select(x => x.CourseId).ToList().Contains((int)currCourseContent.CourseId))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify another fashion influencer's material");
            }

            CourseMaterial courseMaterial = new CourseMaterial
            {
                MaterialName = materialCreateReqModel.MaterialName,
                MaterialLink = materialCreateReqModel.MaterialLink,
                CourseContentId = currCourseContent.CourseContentId
            };

            await _courseMaterialRepository.Add(courseMaterial);

        }

        public async Task<List<CourseMaterialViewListResModel>> GetCourseMaterialByCourseContentId(int courseContentId, int? page, int? size)
        {
            var currCourseContent = await _courseContentRepository.Get(courseContentId);
            if (currCourseContent == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course content does not exist");
            }

            var courseMaterials = await _courseMaterialRepository.GetCourseMaterialByCourseContentId(currCourseContent.CourseContentId, page, size);

            return _mapper.Map<List<CourseMaterialViewListResModel>>(courseMaterials);
        }

        public async Task RemoveCourseMaterial(string token, int materialId)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.FashionInfluencer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currFashionInfluencer = await _fashionInfluencerRepository.GetFashionInfluencerByUsername(decodedToken.username);

            if (currFashionInfluencer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Fashion influencer does not exist");
            }

            var currCourseMaterial = await _courseMaterialRepository.Get(materialId);

            if (currCourseMaterial == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course material does not exist");
            }

            var courseContent = await _courseContentRepository.GetCourseContentById((int)currCourseMaterial.CourseContentId);

            if (courseContent == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course content does not exist");
            }

            var influencerCourses = await _courseRepository.GetCoursesByInfluencerId(currFashionInfluencer.InfluencerId);

            if (!influencerCourses.Select(x => x.CourseId).ToList().Contains((int)courseContent.CourseId))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify another fashion influencer's material");
            }

            await _courseMaterialRepository.Remove(currCourseMaterial);

        }

        public async Task UpdateCourseMaterial(string token, CourseMaterialUpdateReqModel courseMaterialUpdateReqModel)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.FashionInfluencer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currFashionInfluencer = await _fashionInfluencerRepository.GetFashionInfluencerByUsername(decodedToken.username);

            if (currFashionInfluencer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Fashion influencer does not exist");
            }

            var currCourseMaterial = await _courseMaterialRepository.Get(courseMaterialUpdateReqModel.CourseMaterialId);

            if (currCourseMaterial == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course material does not exist");
            }

            var courseContent = await _courseContentRepository.GetCourseContentById((int)currCourseMaterial.CourseContentId);

            if (courseContent == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course content does not exist");
            }

            var influencerCourses = await _courseRepository.GetCoursesByInfluencerId(currFashionInfluencer.InfluencerId);

            if (!influencerCourses.Select(x => x.CourseId).ToList().Contains((int)courseContent.CourseId))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify another fashion influencer's material");
            }

            currCourseMaterial.MaterialName = courseMaterialUpdateReqModel.MaterialName;
            currCourseMaterial.MaterialLink = courseMaterialUpdateReqModel.MaterialLink;

            await _courseMaterialRepository.Update(currCourseMaterial);
        }
    }
}

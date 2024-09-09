using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.CourseContentModel.Request;
using BusinessObject.Models.CourseContentModel.Response;
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

namespace Services.CourseContentServices
{
    public class CourseContentService : ICourseContentService
    {
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseMaterialRepository _courseMaterialRepository;
        private readonly IFashionInfluencerRepository _fashionInfluencerRepository;
        private readonly IDecodeTokenHandler _decodeToken;
        private readonly IMapper _mapper;

        public CourseContentService(ICourseContentRepository courseContentRepository, 
            ICourseRepository courseRepository, 
            ICourseMaterialRepository courseMaterialRepository,
            IFashionInfluencerRepository fashionInfluencerRepository,
            IDecodeTokenHandler decodeToken, IMapper mapper)
        {
            _courseContentRepository = courseContentRepository;
            _courseRepository = courseRepository;
            _courseMaterialRepository = courseMaterialRepository;
            _fashionInfluencerRepository = fashionInfluencerRepository;
            _decodeToken = decodeToken;
            _mapper = mapper;
        }
        public async Task CreateNewCourseContent(string token, ContentCreateReqModel contentCreateReqModel)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.FashionInfluencer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currFashionInfluencer = await _fashionInfluencerRepository.GetFashionInfluencerByUsername(decodedToken.username);

            if (currFashionInfluencer == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "Fashion influencer does not exist");
            }

            var currCourse = await _courseRepository.GetCourseById(contentCreateReqModel.CourseId);

            if (currCourse == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course does not exist");
            }

            if (currFashionInfluencer.InfluencerId != currCourse.InstructorId)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify another fashion influencer's content");
            }

            CourseContent courseContent = new CourseContent
            {
                Content = contentCreateReqModel.Content,
                Duration = contentCreateReqModel.Duration,
                CourseId = currCourse.CourseId
            };

            var courseContentId = await _courseContentRepository.AddCourseContent(courseContent);

            List<CourseMaterial> courseMaterials = new List<CourseMaterial>();

            foreach (var courseMaterial in contentCreateReqModel.CourseMaterials)
            {
                CourseMaterial newMaterial = new CourseMaterial
                {
                    MaterialName = courseMaterial.MaterialName,
                    MaterialLink = courseMaterial.MaterialLink,
                    CreatedDate = DateTime.Now,
                    CourseContentId = courseContentId
                };

                courseMaterials.Add(newMaterial);
            }

            await _courseMaterialRepository.AddRange(courseMaterials);
        }

        public async Task<List<CourseContentViewListResModel>> GetCourseContentByCourseId(int courseId, int? page, int? size)
        {
            var currCourse = await _courseRepository.GetCourseById(courseId);
            if (currCourse == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course does not exist");
            }

            var courseContent = await _courseContentRepository.GetCourseContentByCourseId(currCourse.CourseId, page, size);

            return _mapper.Map<List<CourseContentViewListResModel>>(courseContent);
        }

        public async Task RemoveCourseContent(string token, int courseContentId)
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

            var currCourseContent = await _courseContentRepository.Get(courseContentId);

            if (currCourseContent == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course content does not exist");
            }

            var influencerCourses = await _courseRepository.GetCoursesByInfluencerId(currFashionInfluencer.InfluencerId);

            if (!influencerCourses.Select(x => x.CourseId).ToList().Contains((int) currCourseContent.CourseId))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not remove another fashion influencer's content");
            }

            var courseContentMaterial = await _courseMaterialRepository.GetCourseMaterialByCourseContentId(currCourseContent.CourseContentId);

            foreach (var material in courseContentMaterial)
            {
                await _courseMaterialRepository.Remove(material);
            }

            await _courseContentRepository.Remove(currCourseContent);
        }

        public async Task UpdateCourseContent(string token, CourseContentUpdateReqModel courseContentUpdateReqModel)
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

            var currCourseContent = await _courseContentRepository.GetCourseContentById(courseContentUpdateReqModel.CourseContentId);

            if (currCourseContent == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course content does not exist");
            }

            var influencerCourses = await _courseRepository.GetCoursesByInfluencerId(currFashionInfluencer.InfluencerId);

            if (!influencerCourses.Select(x => x.CourseId).ToList().Contains((int)currCourseContent.CourseId))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify another fashion influencer's content");
            }

            currCourseContent.Content = !string.IsNullOrEmpty(courseContentUpdateReqModel.Content) ? courseContentUpdateReqModel.Content : currCourseContent.Content;
            currCourseContent.Duration = courseContentUpdateReqModel.Duration != null ? courseContentUpdateReqModel.Duration : currCourseContent.Duration;

            await _courseContentRepository.Update(currCourseContent);
        }
    }
}

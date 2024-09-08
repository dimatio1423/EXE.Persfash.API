using AutoMapper;
using BusinessObject.Models.CourseModel.Request;
using BusinessObject.Models.CourseModel.Response;
using Repositories.CourseContentRepos;
using Repositories.CourseMaterialRepos;
using Repositories.CourseRepos;
using Repositories.FashionInfluencerRepos;
using Services.Helpers.Handler.DecodeTokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CourseServices
{
    public class CourseService : ICourseService
    {
        private readonly IFashionInfluencerRepository _fashionInfluencerRepository;
        private readonly IMapper _mapper;
        private readonly IDecodeTokenHandler _decodeToken;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly ICourseMaterialRepository _courseMaterialRepository;

        public CourseService(IFashionInfluencerRepository fashionInfluencerRepository, 
            IMapper mapper, 
            IDecodeTokenHandler decodeToken, 
            ICourseRepository courseRepository,
            ICourseContentRepository courseContentRepository, 
            ICourseMaterialRepository courseMaterialRepository)
        {
            _courseRepository = courseRepository;
            _courseContentRepository = courseContentRepository;
            _courseMaterialRepository = courseMaterialRepository;
            _fashionInfluencerRepository = fashionInfluencerRepository;
            _mapper = mapper;
            _decodeToken = decodeToken;

        }
        public Task CreateNewCourse(string token, CourseCreateReqModel courseCreateReqModel)
        {
            throw new NotImplementedException();
        }

        public Task<CourseViewListResModel> GetCourses(int? page, int? size)
        {
            throw new NotImplementedException();
        }
    }
}

using AutoMapper;
using Azure.Core;
using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.CourseModel.Request;
using BusinessObject.Models.CourseModel.Response;
using BusinessObject.Models.FashionItemsModel.Request;
using BusinessObject.Models.FashionItemsModel.Response;
using Newtonsoft.Json.Linq;
using Repositories.CourseContentRepos;
using Repositories.CourseImagesRepos;
using Repositories.CourseMaterialRepos;
using Repositories.CourseRepos;
using Repositories.FashionInfluencerRepos;
using Repositories.SystemAdminRepos;
using Repositories.UserCourseRepos;
using Repositories.UserRepos;
using Services.AWSService;
using Services.AWSServices;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.CourseServices
{
    public class CourseService : ICourseService
    {
        private readonly IFashionInfluencerRepository _fashionInfluencerRepository;
        private readonly ICustomerCourseRepository _customerCourseRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICourseImageRepository _courseImageRepository;
        private readonly IAWSService _aWSService;
        private readonly ISystemAdminRepository _systemAdminRepository;
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
            ICourseMaterialRepository courseMaterialRepository,
            ICustomerCourseRepository customerCourseRepository,
            ICourseImageRepository courseImageRepository, 
            IAWSService aWSService,
            ICustomerRepository customerRepository,
            ISystemAdminRepository systemAdminRepository)
        {
            _courseRepository = courseRepository;
            _courseContentRepository = courseContentRepository;
            _courseMaterialRepository = courseMaterialRepository;
            _fashionInfluencerRepository = fashionInfluencerRepository;
            _customerCourseRepository = customerCourseRepository;
            _customerRepository = customerRepository;
            _courseImageRepository = courseImageRepository;
            _aWSService = aWSService;
            _systemAdminRepository = systemAdminRepository;
            _mapper = mapper;
            _decodeToken = decodeToken;

        }

        public async Task<bool> ActivateDeactivateCourse(string token, int courseId)
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

            var currCourse = await _courseRepository.Get(courseId);

            if (currCourse == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course does not exist");
            }

            if (currCourse.InstructorId != currFashionInfluencer.InfluencerId)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify other influencer's course");
            }

            currCourse.Status = currCourse.Status.Equals(StatusEnums.Available.ToString()) ? StatusEnums.Unavailable.ToString() : StatusEnums.Available.ToString();

            await _courseRepository.Update(currCourse);

            return currCourse.Status.Equals(StatusEnums.Available.ToString()) ? true : false;
        }

        public async Task<bool> CheckCustomerCourse(string token, int courseId)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodedToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currCourse = await _courseRepository.Get(courseId);

            if (currCourse == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course does not exist");
            }

            var isCustomerOwnCourse = await _customerCourseRepository.CheckCustomerCourse(currCustomer.CustomerId, currCourse.CourseId);

            return isCustomerOwnCourse != null ? true : false;
        }

        public async Task CreateNewCourse(string token, CourseCreateReqModel courseCreateReqModel)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.FashionInfluencer.ToString()))
            {
                throw new ApiException(System.Net.HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currFashionInfluencer = await _fashionInfluencerRepository.GetFashionInfluencerByUsername(decodedToken.username);

            if (currFashionInfluencer == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "Fashion influencer does not exist");
            }

            Course course = new Course
            {
                CourseName = courseCreateReqModel.CourseName,
                Description = courseCreateReqModel.Description,
                Price = courseCreateReqModel.Price,
                InstructorId = currFashionInfluencer.InfluencerId,
                Status = StatusEnums.Unavailable.ToString(),
                ThumbnailUrl = courseCreateReqModel.Thumbnail
            };

            var courseId = await _courseRepository.AddCourse(course);

            List<CourseImage> courseImages = new List<CourseImage>();

            foreach (var item in courseCreateReqModel.CourseImages)
            {
                CourseImage courseImage = new CourseImage
                {
                    CourseId = courseId,
                    ImageUrl = item,
                };

                courseImages.Add(courseImage);
            }

            await _courseImageRepository.AddRange(courseImages);

            foreach (var content in courseCreateReqModel.CourseContents)
            {
                CourseContent newCourseContent = new CourseContent
                {
                    Content = content.Content,
                    Duration = content.Duration,
                    CourseId = courseId,
                };

                var coureseContenId = await _courseContentRepository.AddCourseContent(newCourseContent);

                foreach (var material in content.CourseMaterials)
                {
                    CourseMaterial newCourseMaterial = new CourseMaterial
                    {
                        MaterialName = material.MaterialName,
                        MaterialLink = material.MaterialLink,
                        CourseContentId = coureseContenId,
                        CreatedDate = DateTime.Now
                    };

                    await _courseMaterialRepository.Add(newCourseMaterial);
                }
            }
        }

        public async Task<CourseViewDetailsModel> GetCourseDetails(int courseId)
        {
            var currCourse = await _courseRepository.GetCourseById(courseId);

            if (currCourse == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course does not exist");
            }

            return _mapper.Map<CourseViewDetailsModel>(currCourse);

        }

        public async Task<List<CourseViewListResModel>> GetCourseOfCustomer(string token)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.Customer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodedToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var customerCourses = await _customerCourseRepository.GetCustomerCoursesByCustomerId(currCustomer.CustomerId);

            var courses = await _courseRepository.GetCoursesByIds(customerCourses.Select(x => (int)x.CourseId).ToList());

            return _mapper.Map<List<CourseViewListResModel>>(courses);
        }
    

        public async Task<List<CourseViewListResModel>> GetCourses(int? page, int? size)
        {
            var courses = await _courseRepository.GetCourses( page, size);
          
            return _mapper.Map<List<CourseViewListResModel>>(courses.Where(x => x.Status.Equals(StatusEnums.Available.ToString())).ToList());
        }

        public async Task<List<CourseViewListResModel>> GetCoursesByInfluencerId(int influencerId, int? page, int? size)
        {
            var currInfluencer = await _fashionInfluencerRepository.Get(influencerId);

            if (currInfluencer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "The influencer does not exist");
            }

            var courses = await _courseRepository.GetCoursesByInfluencerId(influencerId, page, size);

            return _mapper.Map<List<CourseViewListResModel>>(courses.Where(x => x.Status.Equals(StatusEnums.Available.ToString())).ToList());
        }

        public async Task<List<CourseViewListResModel>> GetCoursesOfCurrentInfluencerId(string token, int? page, int? size)
        {
            var decodedToken = _decodeToken.decode(token);

            if (!decodedToken.roleName.Equals(RoleEnums.FashionInfluencer.ToString()))
            {
                throw new ApiException(HttpStatusCode.Forbidden, "You do not have permission to perform this function");
            }

            var currInfluencer = await _fashionInfluencerRepository.GetFashionInfluencerByUsername(decodedToken.username);

            if (currInfluencer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Fashion Influencer does not exist");

            }

            var courses = await _courseRepository.GetCoursesByInfluencerId(currInfluencer.InfluencerId, page, size);

            return _mapper.Map<List<CourseViewListResModel>>(courses);
        }

        public async Task UpdateCourse(string token, CourseUpdateReqModel courseUpdateReqModel)
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

            var currCourse = await _courseRepository.Get(courseUpdateReqModel.CourseId);

            if (currCourse == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course does not exist");
            }

            if (currCourse.InstructorId != currFashionInfluencer.InfluencerId)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can not modify other influencer's course");
            }

            currCourse.CourseName = !string.IsNullOrEmpty(courseUpdateReqModel.CourseName) ? courseUpdateReqModel.CourseName : currCourse.CourseName;
            currCourse.Price = courseUpdateReqModel.Price != null ? courseUpdateReqModel.Price : currCourse.Price;
            currCourse.Description = !string.IsNullOrEmpty(courseUpdateReqModel.Description) ? courseUpdateReqModel.Description : currCourse.Description;
            currCourse.ThumbnailUrl = !string.IsNullOrEmpty(courseUpdateReqModel.Thumbnail) ? courseUpdateReqModel.Thumbnail : currCourse.ThumbnailUrl;

            if (courseUpdateReqModel.CourseImages != null && courseUpdateReqModel.CourseImages.Count > 0)
            {
                var currCoursesImages = await _courseImageRepository.GetCourseImagesByCourseId(currCourse.CourseId);

                foreach (var item in currCoursesImages)
                {
                    var s3Key = _aWSService.ExtractS3Key(item.ImageUrl);
                    
                    if (!string.IsNullOrEmpty(s3Key))
                    {
                        await _aWSService.DeleteFile("persfash-application", s3Key);
                    }
                   
                    await _courseImageRepository.Remove(item);
                }

                List<CourseImage> courseImages = new List<CourseImage>();

                foreach (var item in courseUpdateReqModel.CourseImages)
                {
                    //var itemLink = await _aWSService.UploadFile(item, "persfash-application", null);

                    CourseImage courseImage = new CourseImage
                    {
                        CourseId = currCourse.CourseId,
                        ImageUrl = item,
                    };

                    courseImages.Add(courseImage);
                }

                await _courseImageRepository.AddRange(courseImages);
            }

            await _courseRepository.Update(currCourse);
        }
    }
}

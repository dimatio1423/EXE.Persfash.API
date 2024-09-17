using AutoMapper;
using Azure.Core;
using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.CourseModel.Request;
using BusinessObject.Models.CourseModel.Response;
using BusinessObject.Models.FashionItemsModel.Request;
using BusinessObject.Models.FashionItemsModel.Response;
using BusinessObject.Models.PaymentModel.Request;
using BusinessObject.Models.VnPayModel.Request;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Repositories.CourseContentRepos;
using Repositories.CourseImagesRepos;
using Repositories.CourseMaterialRepos;
using Repositories.CourseRepos;
using Repositories.FashionInfluencerRepos;
using Repositories.PartnerRepos;
using Repositories.PaymentRepos;
using Repositories.PaymentTransactionRepos;
using Repositories.SystemAdminRepos;
using Repositories.UserCourseRepos;
using Repositories.UserRepos;
using Services.AWSService;
using Services.AWSServices;
using Services.EmailService;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using Services.VnPayService;
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
        private readonly IPartnerRepository _partnerRepository;
        private readonly IAWSService _aWSService;
        private readonly ISystemAdminRepository _systemAdminRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentTransactionRepository _paymentTransactionRepository;
        private readonly IMapper _mapper;
        private readonly IVnPayService _vnPayService;
        private readonly IDecodeTokenHandler _decodeToken;
        private readonly IEmailService _emailService;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly ICourseMaterialRepository _courseMaterialRepository;

        public CourseService(
            IFashionInfluencerRepository fashionInfluencerRepository, 
            IMapper mapper, 
            IDecodeTokenHandler decodeToken, 
            ICourseRepository courseRepository,
            ICourseContentRepository courseContentRepository, 
            ICourseMaterialRepository courseMaterialRepository,
            ICustomerCourseRepository customerCourseRepository,
            ICourseImageRepository courseImageRepository, 
            IPartnerRepository partnerRepository,
            IAWSService aWSService,
            ICustomerRepository customerRepository,
            ISystemAdminRepository systemAdminRepository,
            IPaymentRepository paymentRepository,
            IPaymentTransactionRepository paymentTransactionRepository,
            IEmailService emailService, 
            IVnPayService vnPayService)
        {
            _courseRepository = courseRepository;
            _courseContentRepository = courseContentRepository;
            _courseMaterialRepository = courseMaterialRepository;
            _fashionInfluencerRepository = fashionInfluencerRepository;
            _customerCourseRepository = customerCourseRepository;
            _customerRepository = customerRepository;
            _courseImageRepository = courseImageRepository;
            _partnerRepository = partnerRepository;
            _aWSService = aWSService;
            _systemAdminRepository = systemAdminRepository;
            _paymentRepository = paymentRepository;
            _paymentTransactionRepository = paymentTransactionRepository;
            _mapper = mapper;
            _vnPayService = vnPayService;
            _decodeToken = decodeToken;
            _emailService = emailService;

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
    

        public async Task<List<CourseViewListResModel>> GetCourses(string? token, int? page, int? size, string? sortBy)
        {

            List<Course> courses = new List<Course>();

            if (!string.IsNullOrEmpty(token))
            {
                var decode = _decodeToken.decode(token);

                var currentCustomer = await _customerRepository.GetCustomerByUsername(decode.username);

                if (currentCustomer != null)
                {
                    var customerCourse = await _customerCourseRepository.GetCustomerCoursesByCustomerId(currentCustomer.CustomerId);

                    courses = await _courseRepository.GetCourses(page, size);

                    courses = sortCourse(courses, sortBy);

                    return _mapper.Map<List<CourseViewListResModel>>(courses.Where(x => x.Status.Equals(StatusEnums.Available.ToString())
                    && !customerCourse.Select(uc => uc.CourseId).ToList().Contains(x .CourseId)).ToList());
                } else
                {
                    courses = await _courseRepository.GetCourses(page, size);

                    courses = sortCourse(courses, sortBy);

                    return _mapper.Map<List<CourseViewListResModel>>(courses.Where(x => x.Status.Equals(StatusEnums.Available.ToString())).ToList());
                }
            }else
            {
                courses = await _courseRepository.GetCourses(page, size);

                courses = sortCourse(courses, sortBy);

                return _mapper.Map<List<CourseViewListResModel>>(courses.Where(x => x.Status.Equals(StatusEnums.Available.ToString())).ToList());
            }
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

        public async Task<List<CourseViewListResModel>> SearchCourses(string? token, int? page, int? size, string? searchValue, string? sortBy)
        {
            var courses = await _courseRepository.GetCourses(page, size);

            if (!string.IsNullOrEmpty(searchValue))
            {
                courses = courses.Where(x => x.CourseName.ToLower().Contains(searchValue.Trim().ToLower())).ToList();
            }

            courses = sortCourse(courses, sortBy);

            if (!string.IsNullOrEmpty(token))
            {
                var decode = _decodeToken.decode(token);

                var currentCustomer = await _customerRepository.GetCustomerByUsername(decode.username);

                if (currentCustomer != null)
                {
                    var customerCourse = await _customerCourseRepository.GetCustomerCoursesByCustomerId(currentCustomer.CustomerId);


                    return _mapper.Map<List<CourseViewListResModel>>(courses.Where(x => x.Status.Equals(StatusEnums.Available.ToString())
                    && !customerCourse.Select(uc => uc.CourseId).ToList().Contains(x.CourseId)).ToList());
                }
                else
                {
                    return _mapper.Map<List<CourseViewListResModel>>(courses.Where(x => x.Status.Equals(StatusEnums.Available.ToString())).ToList());
                }
            }
            else
            {
                return _mapper.Map<List<CourseViewListResModel>>(courses.Where(x => x.Status.Equals(StatusEnums.Available.ToString())).ToList());
            }
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

            if (!string.IsNullOrEmpty(courseUpdateReqModel.Thumbnail))
            {
                var s3key = _aWSService.ExtractS3Key(currCourse.ThumbnailUrl);
                if (!string.IsNullOrEmpty(s3key))
                {
                    await _aWSService.DeleteFile("persfash-application", s3key);
                }
            }

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

        public List<Course> sortCourse(List<Course> courses, string? sortBy)
        {
            switch(sortBy)
            {
                case "name_asc":
                    courses = courses.OrderBy(x => x.CourseName).ToList();
                    break;

                case "name_desc":
                    courses = courses.OrderByDescending(x => x.CourseName).ToList();
                    break;

                case "price_asc":
                    courses = courses.OrderBy(x => x.Price).ToList();
                    break;

                case "price_desc":
                    courses = courses.OrderByDescending(x => x.Price).ToList();
                    break;

                default:
                    courses = courses.OrderBy(x => x.CourseName).ToList();
                    break;
            }

            return courses;
        }

        public async Task<int> CreateCustomerCourseTransaction(string token, int courseId)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currCourse = await _courseRepository.GetCourseById(courseId);

            if (currCourse == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course does not exist");
            }

            var currCustomerCourse = await _customerCourseRepository.GetCustomerCoursesByCustomerId(currCustomer.CustomerId);

            var currCoursesOfCustomer = await _courseRepository.GetCoursesByIds(currCustomerCourse.Select(x => (int)x.CourseId).ToList());

            if (currCoursesOfCustomer.Contains(currCourse))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Customer has already owned the course");
            }

            Payment newPayment = new Payment
            {
                PaymentDate = DateTime.Now,
                Price = (decimal)currCourse.Price,
                CustomerId = currCustomer.CustomerId,
                CourseId = currCourse.CourseId,
                Status = PaymentStatusEnums.Unpaid.ToString()
            };

            var paymentId = await _paymentRepository.AddPayment(newPayment);

            return paymentId;
        }

        public async Task<string> GetPaymentUrl(HttpContext context, int paymentId, string redirectUrl)
        {
            var currPayment = await _paymentRepository.Get(paymentId);

            if (currPayment == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Payment does not exist");
            }

            if (currPayment.Status.Equals(PaymentStatusEnums.Paid.ToString()))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "The payment has already been paid");
            }

            VnPayReqModel vnPayReqModel = new VnPayReqModel
            {
                OrderId = (int)currPayment.CourseId,
                PaymentId = currPayment.PaymentId,
                Amount = currPayment.Price,
                CreatedDate = currPayment.PaymentDate,
                RedirectUrl = redirectUrl,
            };

            return _vnPayService.CreatePaymentUrl(context, vnPayReqModel);
        }

        public async Task<Payment> UpdateCustomerCourseTransaction(PaymentUpdateReqModel paymentUpdateReqModel)
        {
            var currPayment = await _paymentRepository.GetPaymentById(paymentUpdateReqModel.paymentId);

            if (currPayment == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Payment does not exist");
            }

            if (!currPayment.Status.Equals(PaymentStatusEnums.Unpaid.ToString()))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "The payment is not in unpaid status");
            }

            if (!Enum.IsDefined(typeof(PaymentStatusEnums), paymentUpdateReqModel.status))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Please choose valid status");
            }

            currPayment.Status = paymentUpdateReqModel.status;
            currPayment.PaymentDate = DateTime.Now;
            await _paymentRepository.Update(currPayment);

            // sau khi thanh toán thành công đổi status bên Payment rồi thêm bên PaymentTransaction để chuyển tiền cho Influencer

            var currCourse = await _courseRepository.GetCourseById((int)currPayment.CourseId);

            if (currCourse == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course payment does not exist");
            }

            PaymentTransaction paymentTransaction = new PaymentTransaction
            {
                PaymentId = currPayment.PaymentId,
                InfluencerId = (int)currCourse.InstructorId,
                OriginalAmount = (decimal)currCourse.Price,
                ComissionRate = 5,
                CommissionAmount = (decimal)currCourse.Price * 5 / 100,
                TransferredAmount = (decimal)currCourse.Price - ((decimal)currCourse.Price * 5 / 100),
                TransferDate = null,
                Status = PaymentStatusEnums.Unpaid.ToString(),

            };

            await _paymentTransactionRepository.Add(paymentTransaction);

            return currPayment;
        }

        public async Task AddCustomerCourse(string token, int courseId)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currCourse = await _courseRepository.GetCourseById(courseId);

            if (currCourse == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course does not exist");
            }

            CustomerCourse customerCourse = new CustomerCourse
            {
                CourseId = currCourse.CourseId,
                CustomerId = currCustomer.CustomerId,
                EnrollmentDate = DateTime.Now
            };

            await _customerCourseRepository.Add(customerCourse);

            await _emailService.SendCoursePaymentSuccessEmail(currCustomer.FullName, currCourse.CourseName, currCustomer.Email);
        }
    }
}


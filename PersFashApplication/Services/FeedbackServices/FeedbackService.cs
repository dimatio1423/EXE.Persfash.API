using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Models.FeedbackModel.Request;
using BusinessObject.Models.FeedbackModel.Response;
using Repositories.CourseRepos;
using Repositories.FashionInfluencerRepos;
using Repositories.FashionItemsRepos;
using Repositories.FeedbackRepos;
using Repositories.SystemAdminRepos;
using Repositories.UserRepos;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TagLib.Ape;

namespace Services.FeedbackServices
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IFashionItemRepository _fashionItemRepository;
        private readonly IFashionInfluencerRepository _fashionInfluencerRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISystemAdminRepository _systemAdminRepository;
        private readonly IDecodeTokenHandler _decodeToken;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, ICourseRepository courseRepository, IFashionItemRepository fashionItemRepository,
            IFashionInfluencerRepository fashionInfluencerRepository, IDecodeTokenHandler decodeToken, IMapper mapper, ICustomerRepository customerRepository,
            ISystemAdminRepository systemAdminRepository)
        {
            _feedbackRepository = feedbackRepository;
            _courseRepository = courseRepository;
            _fashionItemRepository = fashionItemRepository;
            _fashionInfluencerRepository = fashionInfluencerRepository;
            _customerRepository = customerRepository;
            _systemAdminRepository = systemAdminRepository;
            _decodeToken = decodeToken;
            _mapper = mapper;

        }
        public async Task<SummaryFeedbackResModel> GetFeedbackByCourseId(int courseId)
        {
            var currCourse = await _courseRepository.GetCourseById(courseId);

            if (currCourse == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course does not exist");
            }

            var feedbackList = await _feedbackRepository.GetFeedbacksByCourseId(currCourse.CourseId);

            var ratingResModel = GetRating(feedbackList);

            var averageRating = GetAverateRating(ratingResModel);

            return new SummaryFeedbackResModel
            {
                Feedbacks = _mapper.Map<List<FeedbackViewResModel>>(feedbackList),
                totalFeedback = feedbackList.Count,
                Ratings = ratingResModel,
                averageRating = averageRating,
            };
        }

        public async Task<SummaryFeedbackResModel> GetFeedbackByFashionInfluenerId(int fashionInfluencerId)
        {
            var currInfluencer = await _fashionInfluencerRepository.Get(fashionInfluencerId);

            if (currInfluencer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Fashion influencer does not exist");
            }

            var feedbackList = await _feedbackRepository.GetFeedbacksByInfluencerId(currInfluencer.InfluencerId);

            var ratingResModel = GetRating(feedbackList);

            var averageRating = GetAverateRating(ratingResModel);

            return new SummaryFeedbackResModel
            {
                Feedbacks = _mapper.Map<List<FeedbackViewResModel>>(feedbackList),
                totalFeedback = feedbackList.Count,
                Ratings = ratingResModel,
                averageRating = averageRating,
            };
        }

        public async Task<SummaryFeedbackResModel> GetFeedbackByItemId(int itemId)
        {
            var currItem = await _fashionItemRepository.GetFashionItemsById(itemId);

            if (currItem == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Fashion Item does not exist");
            }

            var feedbackList = await _feedbackRepository.GetFeedbacksByItemId(currItem.ItemId);

            var ratingResModel = GetRating(feedbackList);

            var averageRating = GetAverateRating(ratingResModel);

            return new SummaryFeedbackResModel
            {
                Feedbacks = _mapper.Map<List<FeedbackViewResModel>>(feedbackList),
                totalFeedback = feedbackList.Count,
                Ratings = ratingResModel,
                averageRating = averageRating,
            };
        }

        public async Task GiveFeedbackForCourse(string token, GiveFeedbackCourseReqModel giveFeedbackCourseReqModel)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currCourse = await _courseRepository.GetCourseById(giveFeedbackCourseReqModel.CourseId);

            if (currCourse == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Course does not exist");
            }

            Feedback feedback = new Feedback
            {
                CourseId = currCourse.CourseId,
                CustomerId = currCustomer.CustomerId,
                Rating = giveFeedbackCourseReqModel.Rating,
                Comment = giveFeedbackCourseReqModel.Comment,
                DateGiven = DateTime.Now
            };

            await _feedbackRepository.Add(feedback);
        }

        public async Task GiveFeedbackForFashionInfluencer(string token, GiveFeedbackInfluencerReqModel giveFeedbackInfluencerReqModel)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currInfluencer = await _fashionInfluencerRepository.Get(giveFeedbackInfluencerReqModel.InfluencerId);

            if (currInfluencer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Fashion influencer does not exist");
            }

            Feedback feedback = new Feedback
            {
                CourseId = currInfluencer.InfluencerId,
                CustomerId = currCustomer.CustomerId,
                Rating = giveFeedbackInfluencerReqModel.Rating,
                Comment = giveFeedbackInfluencerReqModel.Comment,
                DateGiven = DateTime.Now
            };

            await _feedbackRepository.Add(feedback);
        }

        public async Task GiveFeedbackForItem(string token, GiveFeedbackItemReqModel giveFeedbackItemReqModel)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            if (currCustomer == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Customer does not exist");
            }

            var currItem = await _fashionItemRepository.GetFashionItemsById(giveFeedbackItemReqModel.ItemId);

            if (currItem == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Fashion item does not exist");
            }

            Feedback feedback = new Feedback
            {
                ItemId = currItem.ItemId,
                CustomerId = currCustomer.CustomerId,
                Rating = giveFeedbackItemReqModel.Rating,
                Comment = giveFeedbackItemReqModel.Comment,
                DateGiven = DateTime.Now
            };

            await _feedbackRepository.Add(feedback);
        }

        public async Task RemoveFeedback(string token, int feedbackId)
        {
            var decodeToken = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decodeToken.username);

            var admin = await _systemAdminRepository.GetAdminByUsername(decodeToken.username);

            var feedback = await _feedbackRepository.Get(feedbackId);

            if (feedback == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Feedback does not exist");
            }

            if (currCustomer != null)
            {
                if (currCustomer.CustomerId != feedback.CustomerId)
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "Can not remove other customer feedback");
                }

                await _feedbackRepository.Remove(feedback);

            } else if (admin != null)
            {
                await _feedbackRepository.Remove(feedback);
            }else
            {
                throw new ApiException(HttpStatusCode.NotFound, "User does not exist");
            }
        }

        public List<RatingResModel> GetRating(List<Feedback> feedbacks)
        {
            List<RatingResModel> ratingResModels = new List<RatingResModel>();

            ratingResModels = feedbacks.GroupBy(x => x.Rating).Select(x => new RatingResModel
            {
                rating = (int) x.Key,
                countRating = x.Count(),
            }).ToList();

            for (int i = 1; i <= 5; i++)
            {
                if (!ratingResModels.Any(r => r.rating == i))
                {
                    ratingResModels.Add(new RatingResModel { rating = i, countRating = 0 });
                }
            }

            return ratingResModels.OrderBy(x => x.rating).ToList();
        }

        public double GetAverateRating(List<RatingResModel> ratingResModels)
        {
            double totalWeightedRating = ratingResModels.Sum(x => x.rating * x.countRating);
            int totalNumberOfRatings = ratingResModels.Sum(x => x.countRating);

            if (totalNumberOfRatings == 0) return 0;

            return totalWeightedRating / totalNumberOfRatings;
        }

        public async Task<SummaryFeedbackResModel> GetAllFeedbackForAdmin(int? page, int? size)
        {
            var feedbackList = await _feedbackRepository.GetAll(page, size);

            var ratingResModel = GetRating(feedbackList);

            var averageRating = GetAverateRating(ratingResModel);

            return new SummaryFeedbackResModel
            {
                Feedbacks = _mapper.Map<List<FeedbackViewResModel>>(feedbackList),
                totalFeedback = feedbackList.Count,
                Ratings = ratingResModel,
                averageRating = averageRating,
            };
        }
    }
}

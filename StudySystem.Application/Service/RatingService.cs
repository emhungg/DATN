using Microsoft.Extensions.Logging;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.EF;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service
{
    public class RatingService : BaseService, IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRatingProductRepository _ratingRepository;
        private readonly string _currentUser;
        private readonly ILogger<RatingService> _logger;
        public RatingService(IUnitOfWork unitOfWork, UserResolverSerive user, ILogger<RatingService> logger) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _ratingRepository = unitOfWork.RatingProductRepository;
            _currentUser = user.GetUser();
            _logger = logger;
        }

        /// <summary>
        /// AddCommentRating
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> AddCommentRating(RatingRequestModel request)
        {
            try
            {
                return await _ratingRepository.AddRatingProduct(_currentUser, request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        /// <summary>
        /// GetRatingProductById
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<RatingResponseModel> GetRatingProductById(string productId)
        {
            try
            {
                return await _ratingRepository.GetRatingByProductId(productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}

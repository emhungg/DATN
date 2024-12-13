using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service.Interfaces
{
    public interface IRatingService : IBaseService
    {
        Task<bool> AddCommentRating(RatingRequestModel request);

        Task<RatingResponseModel> GetRatingProductById(string productId);

    }
}

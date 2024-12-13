using StudySystem.Data.Entites;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories.Interfaces
{
    public interface IRatingProductRepository : IRepository<RatingProduct>
    {
        /// <summary>
        /// AddRatingProduct
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> AddRatingProduct(string userId, RatingRequestModel request);
        /// <summary>
        /// GetRatingByProductId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<RatingResponseModel> GetRatingByProductId(string productId);
    }
}

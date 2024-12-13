using LinqToDB;
using NetTopologySuite.Triangulate.QuadEdge;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories
{
    public class RatingRepository : Repository<RatingProduct>, IRatingProductRepository
    {
        private readonly AppDbContext _context;
        public RatingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        /// AddRatingProduct
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> AddRatingProduct(string userId, RatingRequestModel request)
        {
            var query = await _context.RatingProduct.FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId.Equals
            (request.ProductId)).ConfigureAwait(false);
            if (query != null)
            {
                query.Rating = request.Rating;
                query.Comment = request.Comment;
                query.UserName = request.UserName;
                query.Phone = request.PhoneNumber;
                query.Email = request.Email;
                query.CreateDateAt = DateTime.UtcNow;
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            else
            {
                RatingProduct data = new RatingProduct
                {
                    UserId = userId,
                    Comment = request.Comment,
                    Email = request.Email,
                    ProductId = request.ProductId,
                    Rating = request.Rating,
                    Phone = request.PhoneNumber,
                    UserName = request.UserName
                };
                await _context.AddAsync(data).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
        }

        /// <summary>
        /// GetRatingByProductId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<RatingResponseModel> GetRatingByProductId(string productId)
        {
            var query = await _context.RatingProduct.Where(x => x.ProductId.Equals(productId)).Select(x =>
            new
            {
                UserName = x.UserName,
                Rating = x.Rating,
                Comment = x.Comment,
                CommentAt = x.CreateDateAt.ToString("dd/MM/yyyy")
            }).ToListAsync();

            int totalRatings = query.Count();
            int totalRatingPoints = query.Sum(x => x.Rating);

            List<RatingDataModel> ratingDataList = query.Select(x => new RatingDataModel
            {
                UserName = x.UserName,
                Rating = x.Rating,
                Comment = x.Comment,
                CommentAt = x.CommentAt,

            }).ToList();

            RatingResponseModel rs = new RatingResponseModel
            {
                RatioRating = totalRatings > 0 ? (float)totalRatingPoints / totalRatings : 0,
                RatingData = ratingDataList
            };
            return rs;
        }
    }
}

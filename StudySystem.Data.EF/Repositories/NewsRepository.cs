using LinqToDB;
using NetTopologySuite.Triangulate.QuadEdge;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        private readonly AppDbContext _context;
        public NewsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        /// CreateNews
        /// </summary>
        /// <param name="request"></param>
        /// <param name="createUser"></param>
        /// <returns></returns>
        public async Task<bool> CreateNews(NewsRequestModel request, string createUser)
        {
            News news = new News
            {
                TitleHeader = request.Title,
                ImageNew = ImageConverter.ConvertToBase64(request.Image),
                Content = request.Content,
                CreateUser = createUser
            };
            await _context.AddAsync(news).ConfigureAwait(false);
            var rs = await _context.SaveChangesAsync().ConfigureAwait(false);
            if (rs != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// DeteletNews
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeteletNews(int id)
        {
            var query = await _context.News.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            if (query != null)
            {
                _context.Remove(query);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }
        /// <summary>
        /// GetNewsById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<NewsDataModel> GetNewsById(int id)
        {
            var query = await _context.News.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            if (query != null)
            {
                return new NewsDataModel
                {
                    IdNews = query.Id,
                    Content = query.Content,
                    Title = query.TitleHeader,
                    CreateAt = StringUtils.TimeZoneUTC(query.CreateDateAt),
                    CreateUser = _context.UserDetails.FirstOrDefault(u => u.UserID.Equals(query.CreateUser)).UserFullName,
                    Image = query.ImageNew
                };
            }
            return null;
        }

        /// <summary>
        /// GetNewsDataList
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<NewResponseModel> GetNewsDataList()
        {
            var query = await _context.News.Select(x => new NewsDataModel
            {
                IdNews = x.Id,
                Title = x.TitleHeader,
                Image = x.ImageNew,
                Content = StringUtils.ShortenString(x.TitleHeader, 50),
                CreateAt = StringUtils.TimeZoneUTC(x.CreateDateAt),
                CreateUser = _context.UserDetails.FirstOrDefault(u => u.UserID.Equals(x.CreateUser)).UserFullName,
            }).ToListAsync();
            NewResponseModel rs = new NewResponseModel
            {
                NewsData = query
            };
            return rs;
        }

        public async Task<bool> UpdateNews(NewsRequestModel request, int id)
        {
            var query = await _context.News.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            if (query != null)
            {
                query.TitleHeader = request.Title;
                query.Content = request.Content;
                query.ImageNew = ImageConverter.ConvertToBase64(request.Image);
                query.CreateDateAt = DateTime.UtcNow;
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }
    }
}

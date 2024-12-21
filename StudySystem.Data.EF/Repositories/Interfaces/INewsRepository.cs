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
    public interface INewsRepository : IRepository<News>
    {
        /// <summary>
        /// CreateNews
        /// </summary>
        /// <param name="request"></param>
        /// <param name="createUser"></param>
        /// <returns></returns>
        Task<bool> CreateNews(NewsRequestModel request, string createUser);
        /// <summary>
        /// GetNewsDataList
        /// </summary>
        /// <returns></returns>
        Task<NewResponseModel> GetNewsDataList();

        /// <summary>
        /// DeteletNews
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeteletNews(int id);
        /// <summary>
        /// GetNewsById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<NewsDataModel> GetNewsById(int id);
        /// <summary>
        /// UpdateNews
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> UpdateNews(NewsRequestModel request, int id);
    }
}

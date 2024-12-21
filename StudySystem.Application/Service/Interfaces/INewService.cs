using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service.Interfaces
{
    public interface INewService : IBaseService
    {
        /// <summary>
        /// CreateNews
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> CreateNews(NewsRequestModel request);
        /// <summary>
        /// GetNewsDataList
        /// </summary>
        /// <returns></returns>
        Task<NewResponseModel> GetNewsDataList();
        /// <summary>
        /// DeteleNews
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeteleNews(int id);
        /// <summary>
        /// GetNewsById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<NewsDataModel> GetNewsById(int id);
        /// <summary>
        /// UpdateNew
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> UpdateNew(NewsRequestModel request, int id);
    }
}

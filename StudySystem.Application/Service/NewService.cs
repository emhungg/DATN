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
    public class NewService : BaseService, INewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NewService> _logger;
        private readonly INewsRepository _newsRepository;
        private readonly string _currentUser;
        public NewService(IUnitOfWork unitOfWork, ILogger<NewService> logger, UserResolverSerive currentUser) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _newsRepository = unitOfWork.NewsRepository;
            _currentUser = currentUser.GetUser();
        }
        /// <summary>
        /// CreateNews
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> CreateNews(NewsRequestModel request)
        {
            try
            {
                return await _newsRepository.CreateNews(request, _currentUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// DeteleNews
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeteleNews(int id)
        {
            try
            {
                return await _newsRepository.DeteletNews(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// GetNewsById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<NewsDataModel> GetNewsById(int id)
        {
            try
            {
                return await _newsRepository.GetNewsById(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// GetNewsDataList
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<NewResponseModel> GetNewsDataList()
        {
            try
            {
                return await _newsRepository.GetNewsDataList().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
        /// <summary>
        /// UpdateNew
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateNew(NewsRequestModel request, int id)
        {
            try
            {
                return await _newsRepository.UpdateNews(request, id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message, ex);
                return false;
            }
        }
    }
}

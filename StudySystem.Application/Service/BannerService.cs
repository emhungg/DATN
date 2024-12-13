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
    public class BannerService : BaseService, IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BannerService> _logger;
        public BannerService(IUnitOfWork unitOfWork, ILogger<BannerService> logger) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _bannerRepository = unitOfWork.BannerRepository;
        }
        /// <summary>
        /// CreateBanner
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> CreateBanner(BannerDataRequestModel request)
        {
            try
            {
                return await _bannerRepository.AddBannerAsync(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                return await _bannerRepository.DeletebyId(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// GetBanner
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<BannerResponseModel> GetBanner()
        {
            try
            {
                return await _bannerRepository.GetBanner().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateBanner(int id, bool active)
        {
            try
            {
                return await _bannerRepository.UpdateBanner(id, active).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}

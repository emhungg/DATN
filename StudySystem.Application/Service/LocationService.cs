using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Math.EC.Rfc7748;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.EF;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Data;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service
{
    public class LocationService : BaseService, ILocationService
    {
        private readonly ILocationRepository<Province> _provincesRepository;
        private readonly ILocationRepository<District> _districtsRepository;
        private readonly ILocationRepository<Ward> _wardsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LocationService> _logger;
        public LocationService(IUnitOfWork unitOfWork, ILogger<LocationService> logger) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _provincesRepository = unitOfWork.ProvioncesRepository;
            _districtsRepository = unitOfWork.DistrictsRepository;
            _wardsRepository = unitOfWork.WardsRepository;
        }
        /// <summary>
        /// GetDistricts
        /// </summary>
        /// <param name="province_code"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DistrictsResponseModel> GetDistricts(string province_code)
        {
            DistrictsResponseModel result = new DistrictsResponseModel();
            try
            {
                var districtList = await _districtsRepository.FindAllAsync(x => x.ProvinceCode.Equals(province_code)).ConfigureAwait(false);
                result.Districts = districtList.OrderBy(x => x.Name).Select(x => new LocationDataModel { Code = x.Code, Name = x.FullName }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }
        /// <summary>
        /// GetProvinces
        /// </summary>
        /// <returns></returns>
        public async Task<ProvincesResponseModel> GetProvinces()
        {
            ProvincesResponseModel result = new ProvincesResponseModel();
            try
            {
                var proinces = await _provincesRepository.GetAllLocation().ConfigureAwait(false);
                result.Provinces = proinces.OrderBy(x => x.AdministrativeRegionId).Select(x => new LocationDataModel { Code = x.Code, Name = x.FullName }).ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
            return result;
        }
        /// <summary>
        /// GetProvincesById
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ProvincesResponseModel> GetProvincesById(string code)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// GetWards
        /// </summary>
        /// <param name="district_code"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WardsResponseModel> GetWards(string district_code)
        {
            WardsResponseModel result = new WardsResponseModel();
            try
            {
                var wardList = await _wardsRepository.FindAllAsync(x => x.DistrictCode.Equals(district_code)).ConfigureAwait(false);
                result.Wards = wardList.OrderBy(x => x.Name).Select(x => new LocationDataModel { Code = x.Code, Name = x.FullName }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }
    }
}

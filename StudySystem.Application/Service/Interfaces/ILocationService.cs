using StudySystem.Data.Models.Data;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service.Interfaces
{
    public interface ILocationService : IBaseService
    {
        /// <summary>
        /// GetProvinces
        /// </summary>
        /// <returns></returns>
        public Task<ProvincesResponseModel> GetProvinces();
        /// <summary>
        /// GetProvincesById
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task<ProvincesResponseModel> GetProvincesById(string code);
        /// <summary>
        /// GetDistricts
        /// </summary>
        /// <param name="province_code"></param>
        /// <returns></returns>
        public Task<DistrictsResponseModel> GetDistricts(string province_code);
        /// <summary>
        /// GetWards
        /// </summary>
        /// <param name="district_code"></param>
        /// <returns></returns>
        public Task<WardsResponseModel> GetWards(string district_code);

    }
}

// <copyright file="LocationController.cs" ownedby="Xuan Truong">
//  Copyright (c) XuanTruong. All rights reserved.
//  FileType: Visual CSharp source file
//  Created On: 18/10/2023
//  Last Modified On: 18/10/2023
//  Description: LocationController.cs
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.CommonConstant;

namespace StudySystem.Controllers
{
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> _logger;
        private readonly ILocationService _locationService;
        public LocationController(ILogger<LocationController> logger, ILocationService locationService)
        {
            _logger = logger;
            _locationService = locationService;
        }
        /// <summary>
        /// api: ~/api/get-provinces
        /// GetProvinces
        /// </summary>
        /// <returns><list type="number">List Provicnes</list></returns>
        [HttpGet(Router.GetProvinces)]
        public async Task<ActionResult<StudySystemAPIResponse<ProvincesResponseModel>>> GetProvinces()
        {
            var result = await _locationService.GetProvinces();
            return new StudySystemAPIResponse<ProvincesResponseModel>(StatusCodes.Status200OK, new Response<ProvincesResponseModel>(true, result));
        }
        /// <summary>
        /// GetDistrictByProvinceCode
        /// </summary>
        /// <param name="province_code"></param>
        /// <returns></returns>
        [HttpGet(Router.GetDistricts)]
        public async Task<ActionResult<StudySystemAPIResponse<DistrictsResponseModel>>> GetDistrictByProvinceCode(string province_code)
        {
            var result = await _locationService.GetDistricts(province_code).ConfigureAwait(false);
            return new StudySystemAPIResponse<DistrictsResponseModel>(StatusCodes.Status200OK, new Response<DistrictsResponseModel>(true, result));
        }
        /// <summary>
        /// GetWardsByDistrictCode
        /// </summary>
        /// <param name="district_code"></param>
        /// <returns></returns>
        [HttpGet(Router.GetWards)]
        public async Task<ActionResult<StudySystemAPIResponse<WardsResponseModel>>> GetWardsByDistrictCode(string district_code)
        {
            var result = await _locationService.GetWards(district_code).ConfigureAwait(false);
            return new StudySystemAPIResponse<WardsResponseModel>(StatusCodes.Status200OK, new Response<WardsResponseModel>(true, result));
        }
    }
}

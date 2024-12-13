// <copyright file="ChartsController.cs" ownedby="Xuan Truong">
//  Copyright (c) XuanTruong. All rights reserved.
//  FileType: Visual CSharp source file
//  Created On: 20/12/2023
//  Last Modified On: 20/12/2023
//  Description: ChartsController.cs
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.Models.Response;
using StudySystem.Middlewares;

namespace StudySystem.Controllers
{
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly IChartService _chartService;
        public ChartsController(IChartService chartService)
        {
            _chartService = chartService;
        }

        [HttpGet("~/api/get-statistic")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<StatisticResponseModel>>> GetStatictic(int month)
        {
            var rs1 = await _chartService.GetStatisticResponse(month);

            return new StudySystemAPIResponse<StatisticResponseModel>(StatusCodes.Status200OK, new Response<StatisticResponseModel>(true, rs1));
        }
    }
}

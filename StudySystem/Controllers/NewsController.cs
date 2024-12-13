// <copyright file="NewsController.cs" ownedby="Xuan Truong">
//  Copyright (c) XuanTruong. All rights reserved.
//  FileType: Visual CSharp source file
//  Created On: 29/09/2023
//  Last Modified On: 29/09/2023
//  Description: NewsController.cs
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using StudySystem.Middlewares;

namespace StudySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly INewService _newsService;
        public NewsController(ILogger<NewsController> logger, INewService newService)
        {
            _logger = logger;
            _newsService = newService;
        }
        /// <summary>
        /// CreateNews
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpPost("~/api/create-news")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> CreateNews([FromForm] NewsRequestModel model)
        {
            var rs = await _newsService.CreateNews(model).ConfigureAwait(false);
            if (!rs)
            {
                throw new BadHttpRequestException(Infrastructure.Resources.Message.ErrorCreateProduct);
            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }
        /// <summary>
        /// GetNews
        /// </summary>
        /// <returns></returns>
        [HttpGet("~/api/get-news")]
        public async Task<ActionResult<StudySystemAPIResponse<NewResponseModel>>> GetNews()
        {
            var rs = await _newsService.GetNewsDataList().ConfigureAwait(false);
            return new StudySystemAPIResponse<NewResponseModel>(StatusCodes.Status200OK, new Response<NewResponseModel>(true, rs));
        }

        [HttpDelete("~/api/detele-news")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> DeleteNews(int id)
        {
            var rs = await _newsService.DeteleNews(id).ConfigureAwait(false);
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }

        /// <summary>
        /// GetDetail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("~/api/get-news-detail")]
        public async Task<ActionResult<StudySystemAPIResponse<NewsDataModel>>> GetDetail(int id)
        {
            NewsDataModel rs = await _newsService.GetNewsById(id).ConfigureAwait(false);
            if (rs == null)
            {
                return NotFound();
            }
            return new StudySystemAPIResponse<NewsDataModel>(StatusCodes.Status200OK,
                new Response<NewsDataModel>(true, rs));
        }

        [HttpPut("~/api/update-news")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> UpdateNews([FromForm] NewsRequestModel request, int id)
        {
            var rs = await _newsService.UpdateNew(request, id).ConfigureAwait(false);
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }


    }
}

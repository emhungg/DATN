// <copyright file="EmailVerificationController.cs" ownedby="Xuan Truong">
//  Copyright (c) XuanTruong. All rights reserved.
//  FileType: Visual CSharp source file
//  Created On: 29/09/2023
//  Last Modified On: 29/09/2023
//  Description: EmailVerificationController.cs
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySystem.Application.Service;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.CommonConstant;
using System.ComponentModel.DataAnnotations;

namespace StudySystem.Controllers
{
    [ApiController]
    public class EmailVerificationController : ControllerBase
    {
        private readonly ISendMailService _sendMailService;
        public EmailVerificationController(ISendMailService sendMailService)
        {
            _sendMailService = sendMailService;

        }
        /// <summary>
        /// SendMail
        /// </summary>
        /// <returns></returns>
        [HttpGet(Router.SendMail)]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> SendMail()
        {
            var result = await _sendMailService.SendMailAsync();
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(result, new object()));
        }
        /// <summary>
        /// VerificationEmail
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost(Router.VerificationEmail)]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> VerificationEmail(string code)
        {
            var result = await _sendMailService.VerificationCode(code);
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(result, new object()));
        }
        /// <summary>
        /// RegisterMail Ads
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost(Router.UserRegisterAds)]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> RegisterMail(string email)
        {
            var result = await _sendMailService.RegisterMail(email);
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(result, new object()));
        }

        [HttpPost("~/api/reset-pwd")]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> ResetPwdMail(string userId)
        {
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(true, new object()));
        }

    }
}

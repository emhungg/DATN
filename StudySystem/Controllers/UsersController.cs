
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.EF;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.CommonConstant;
using StudySystem.Infrastructure.Resources;
using StudySystem.Middlewares;
using System.Net;

namespace StudySystem.Controllers
{
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        /// <summary>
        /// <para>api: ~/api/list-user-detail</para>
        /// <para>GetUserDetail</para>
        /// </summary>
        /// <returns></returns>
        [HttpGet(Router.GetListUserDetail)]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<UsersInforManageResponseModel>>> GetListUserDetail()
        {
            var rs = await _userService.GetUsersInforManage();
            if (rs == null)
            {
                throw new BadHttpRequestException(Message._500);
            }
            return new StudySystemAPIResponse<UsersInforManageResponseModel>(StatusCodes.Status200OK, new Response<UsersInforManageResponseModel>(true, rs));
        }
        /// <summary>
        /// <para>api: ~/api/get-user-by-id</para>
        /// <para>GetUserById</para>
        /// </summary>
        /// <returns></returns>
        [HttpGet(Router.GetUserById)]
        public async Task<ActionResult<StudySystemAPIResponse<UserInformationResponseModel>>> GetUserById()
        {
            var rs = await _userService.GetUserById();
            return new StudySystemAPIResponse<UserInformationResponseModel>(StatusCodes.Status200OK, new Response<UserInformationResponseModel>(true, rs));
        }

        /// <summary>
        /// admin
        /// UpdateUser
        /// </summary>
        /// <returns></returns>
        [HttpPost("~/api/update-user")]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> UpdateUser(string userId, int role, bool active)
        {
            var rs = await _userService.UpdateUser(userId, role, active);
            if (!rs)
            {
                throw new BadHttpRequestException(Message._500);
            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }
        /// <summary>
        /// Admin
        /// DeleteUser
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpDelete("~/api/delete-user")]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> DeleteUser(string userId)
        {
            var rs = await _userService.DeleteUser(userId);
            if (!rs)
            {
                throw new BadHttpRequestException(Message._500);
            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }
        /// <summary>
        /// ChangeNameGender
        /// </summary>
        /// <param name="reques"></param>
        /// <returns></returns>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpPost("~/api/change-name-gender")]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> ChangeNameGender(ChangeNameGenderRequestModel reques)
        {
            var rs = await _userService.ChangeNameGender(reques);
            if (!rs)
            {
                throw new BadHttpRequestException(Message.UpdateError);
            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }

        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="reques"></param>
        /// <returns></returns>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpPost("~/api/change-password")]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> ChangePassword(PassswordChangeRequestModel reques)
        {
            var rs = await _userService.ChangePassword(reques);
            if (!rs)
            {
                throw new BadHttpRequestException(Message.UpdateError);
            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }

        /// <summary>
        /// ChangeAddress
        /// </summary>
        /// <param name="reques"></param>
        /// <returns></returns>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpPost("~/api/change-address")]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> ChangeAddress(ChangeAddressRequestModel reques)
        {
            var rs = await _userService.ChangeAddress(reques);
            if (!rs)
            {
                throw new BadHttpRequestException(Message.UpdateError);
            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }
    }
}

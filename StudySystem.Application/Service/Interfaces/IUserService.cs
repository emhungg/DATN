using StudySystem.Data.Entites;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service.Interfaces
{
    public interface IUserService : IBaseService
    {
        /// <summary>
        /// RegisterUserDetail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> RegisterUserDetail(UserRegisterRequestModel request);
        /// <summary>
        /// DoLogin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserDetail> DoLogin(LoginRequestModel request);
        /// <summary>
        /// GetUserById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserInformationResponseModel> GetUserById();
        /// <summary>
        /// UserPermissionRolesAuth
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> UserPermissionRolesAuth(string userId);
        /// <summary>
        /// GetUsersInforManage
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UsersInforManageResponseModel> GetUsersInforManage();
        /// <summary>
        /// Admin
        /// UpdateUser
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        Task<bool> UpdateUser(string userId, int role, bool active);
        /// <summary>
        /// admin
        /// DeleteUser
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(string userId);
        /// <summary>
        /// user
        /// ChangeNameGender
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> ChangeNameGender(ChangeNameGenderRequestModel request);
        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> ChangePassword(PassswordChangeRequestModel request);
        /// <summary>
        /// ChangeAddress
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> ChangeAddress(ChangeAddressRequestModel request);
    }
}

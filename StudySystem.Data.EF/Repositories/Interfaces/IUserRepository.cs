using StudySystem.Data.Entites;
using StudySystem.Data.Models.Data;
using StudySystem.Data.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<UserDetail>
    {
        /// <summary>
        /// IsUserExists
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsUserExists(string userId);
        /// <summary>
        /// InsertUserDetails
        /// </summary>
        /// <param name="userDetail"></param>
        /// <returns></returns>
        Task<bool> InsertUserDetails(UserDetail userDetail);
        /// <summary>
        /// UpdateStatusActiveUser
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task UpdateStatusActiveUser(string userID);
        /// <summary>
        /// GetUserDetail
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserDetailDataModel> GetUserDetail(string userId);
        /// <summary>
        /// GetAllUsersInfo
        /// </summary>
        /// <returns></returns>
        IQueryable<UsersInforDataModel> GetAllUsersInfo();
        /// <summary>
        /// Admin
        /// UpdateUserInfor
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleNew"></param>
        /// <param name="activeNew"></param>
        /// <returns></returns>
        Task<bool> UpdateUserInfor(string userId, int roleNew, bool activeNew);

        /// <summary>
        /// Admin
        /// DeleteUserInfor
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteUserInfor(string userId);
        /// <summary>
        /// user
        /// ChangeNameGender
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> ChangeNameGender(string userId, ChangeNameGenderRequestModel request);
        /// <summary>
        /// user
        /// ChangePassword
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordNew"></param>
        /// <returns></returns>
        Task<bool> ChangePassword(string userId,string oldPassword, string passwordNew);
        /// <summary>
        /// ChangeAddress
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> ChangeAddress(string userId, ChangeAddressRequestModel request);
    }
}

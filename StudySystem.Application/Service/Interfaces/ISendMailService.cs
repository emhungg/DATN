using StudySystem.Data.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service.Interfaces
{
    public interface ISendMailService
    {
        Task<bool> SendMailAsync();
        /// <summary>
        /// VerificationCode follow by token header and get user id form here
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        Task<bool> VerificationCode(string verificationCode);
        /// <summary>
        /// VerificationCodeByEmail used for verifivation when user forgot password
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        Task<string> VerificationCodeByEmail(string verificationCode);
        Task<bool> RegisterMail(string emailRegister);
        Task<bool> SendCodeEmail(string userID, string emailName);
        Task<bool> NotiEmailOrderDone(string content, NotiOrderDoneRequestModel data);
    }
}

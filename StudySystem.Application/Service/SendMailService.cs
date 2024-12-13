using Microsoft.Extensions.Logging;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.EF;
using StudySystem.Data.EF.Repositories;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Request;
using StudySystem.Infrastructure.Configuration;
using StudySystem.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service
{
    /// <summary>
    /// SendMailService
    /// </summary>
    public class SendMailService : ISendMailService
    {
        private readonly ILogger<SendMailService> logger;
        private readonly string _currentUser;
        private readonly IUserRepository _userRepository;
        private readonly IUserVerificationOTPsRepository _userVerificationOTPsRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SendMailService(ILogger<SendMailService> _logger, UserResolverSerive currentUser, IUnitOfWork unitOfWork)
        {
            logger = _logger;
            _currentUser = currentUser.GetUser();
            _unitOfWork = unitOfWork;
            _userRepository = unitOfWork.UserRepository;
            _userVerificationOTPsRepository = unitOfWork.UserVerificationOTPsRepository;
            _userTokenRepository = unitOfWork.UserTokenRepository;
            logger.LogInformation("Create SendMailService");
        }

        /// <summary>
        /// SendMailAsync
        /// </summary>
        /// <returns>Code</returns>
        public async Task<bool> SendMailAsync()
        {
            if (string.IsNullOrEmpty(this._currentUser))
            {
                return false;
            }
            try
            {
                Random random = new Random();
                var userEmail = _userRepository.Find(x => x.UserID.Equals(_currentUser));

                await _userVerificationOTPsRepository.DeleteCode(userEmail.UserID).ConfigureAwait(false);
                string verificationCode = random.Next(100000, 1000000).ToString();
                var expireTimeCode = DateTime.UtcNow.AddMinutes(5);
                await _userVerificationOTPsRepository.InsertCode(new VerificationOTP { UserID = userEmail.UserID, Code = verificationCode, ExpireTime = expireTimeCode });

                var email = new MimeMessage();
                email.Sender = new MailboxAddress(AppSetting.MailName, AppSetting.Mail);
                email.From.Add(new MailboxAddress(AppSetting.MailName, AppSetting.Mail));

                email.To.Add(new MailboxAddress(userEmail.Email, userEmail.Email));
                email.Subject = "Verify code";

                var builder = new BodyBuilder();
                builder.HtmlBody = Body(verificationCode);
                email.Body = builder.ToMessageBody();

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    await smtp.ConnectAsync(AppSetting.MailHost, AppSetting.MailPort, MailKit.Security.SecureSocketOptions.StartTls).ConfigureAwait(false);
                    await smtp.AuthenticateAsync(AppSetting.Mail, AppSetting.MailPassword).ConfigureAwait(false);
                    await smtp.SendAsync(email).ConfigureAwait(false);
                    smtp.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        private string Body(string code)
        {
            return $"<h1>Xác minh tài khoản</h1><h3>Mã bảo mật tồn tại trong 5 phút</h3>Vui lòng sử dụng mã bảo mật sau cho tài khoản.<br/><br/>Mã bảo mật: {code}<br/><br/>Xin cám ơn.";
        }
        /// <summary>
        /// VerificationCode
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <returns>bool</returns>
        public async Task<bool> VerificationCode(string verificationCode)
        {
            if (string.IsNullOrEmpty(this._currentUser))
            {
                return false;
            }
            try
            {
                var user = _userVerificationOTPsRepository.Find(x => x.UserID.Equals(_currentUser));
                if (user.Code.Equals(verificationCode) && user.ExpireTime > DateTime.UtcNow)
                {
                    await _userVerificationOTPsRepository.DeleteCode(user.UserID).ConfigureAwait(false);
                    await _userRepository.UpdateStatusActiveUser(user.UserID).ConfigureAwait(false);
                    await _userTokenRepository.UpdateStatusActiveToken(user.UserID).ConfigureAwait(false);
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return false;
        }

        public async Task<bool> RegisterMail(string emailRegister)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(AppSetting.MailName, AppSetting.Mail);
            email.From.Add(new MailboxAddress(AppSetting.MailName, AppSetting.Mail));

            email.To.Add(new MailboxAddress(emailRegister, emailRegister));
            email.Subject = "HoangHaMoBile";

            var builder = new BodyBuilder();
            builder.HtmlBody = StringUtils.NewUserRegisterAds();
            email.Body = builder.ToMessageBody();

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                await smtp.ConnectAsync(AppSetting.MailHost, AppSetting.MailPort, MailKit.Security.SecureSocketOptions.StartTls).ConfigureAwait(false);
                await smtp.AuthenticateAsync(AppSetting.Mail, AppSetting.MailPassword).ConfigureAwait(false);
                await smtp.SendAsync(email).ConfigureAwait(false);
                smtp.Disconnect(true);
            }
            return true;
        }

        public async Task<bool> SendCodeEmail(string userID, string emailName)
        {
            try
            {
                Random random = new Random();

                await _userVerificationOTPsRepository.DeleteCode(userID).ConfigureAwait(false);
                string verificationCode = random.Next(100000, 1000000).ToString();
                var expireTimeCode = DateTime.UtcNow.AddMinutes(5);
                await _userVerificationOTPsRepository.InsertCode(new VerificationOTP { UserID = userID, Code = verificationCode, ExpireTime = expireTimeCode });

                var email = new MimeMessage();
                email.Sender = new MailboxAddress(AppSetting.MailName, AppSetting.Mail);
                email.From.Add(new MailboxAddress(AppSetting.MailName, AppSetting.Mail));

                email.To.Add(new MailboxAddress(emailName, emailName));
                email.Subject = "Verify code";

                var builder = new BodyBuilder();
                builder.HtmlBody = Body(verificationCode);
                email.Body = builder.ToMessageBody();

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    await smtp.ConnectAsync(AppSetting.MailHost, AppSetting.MailPort, MailKit.Security.SecureSocketOptions.StartTls).ConfigureAwait(false);
                    await smtp.AuthenticateAsync(AppSetting.Mail, AppSetting.MailPassword).ConfigureAwait(false);
                    await smtp.SendAsync(email).ConfigureAwait(false);
                    smtp.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<string> VerificationCodeByEmail(string verificationCode)
        {
            try
            {
                var user = _userVerificationOTPsRepository.Find(x => x.Code.Equals(verificationCode));
                if (user != null && user.ExpireTime > DateTime.UtcNow)
                {
                    await _userVerificationOTPsRepository.DeleteCode(user.UserID).ConfigureAwait(false);
                    return user.UserID;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return "";
        }

        public async Task<bool> NotiEmailOrderDone(string content, NotiOrderDoneRequestModel data)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = new MailboxAddress(AppSetting.MailName, AppSetting.Mail);
                email.From.Add(new MailboxAddress(AppSetting.MailName, AppSetting.Mail));

                email.To.Add(new MailboxAddress(data.Email, data.Email));
                email.Subject = "Đặt hàng thành công";

                var builder = new BodyBuilder();
                builder.HtmlBody = SendVerificationLinkEmail(content, data);
                email.Body = builder.ToMessageBody();

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    await smtp.ConnectAsync(AppSetting.MailHost, AppSetting.MailPort, MailKit.Security.SecureSocketOptions.StartTls).ConfigureAwait(false);
                    await smtp.AuthenticateAsync(AppSetting.Mail, AppSetting.MailPassword).ConfigureAwait(false);
                    await smtp.SendAsync(email).ConfigureAwait(false);
                    smtp.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        //Gửi mail khi đặt hàng thành công
        private string SendVerificationLinkEmail(string content, NotiOrderDoneRequestModel data)
        {
            var body = content;
            var PaymentMethod = data.MethodPayment == "0" ? "Thanh toán trả sau" : "Chuyển khoản";
            body = body.Replace("{{BodyContent}}", "Yêu cầu đặt hàng cho đơn hàng <span style='color: rgb(1, 181, 187); font-weight: 500;'>#" + "" + "</span> của bạn đã được tiếp nhận và đang chờ Nhà bán hàng xử lý, với hình thức thanh toán là <span>" + PaymentMethod + "</span> Chúng tôi sẽ tiếp tục cập nhật với bạn về trạng thái tiếp theo của đơn hàng.");
            body = body.Replace("{{OrderStatus}}", "");
            body = body.Replace("{{ButtonConfirm}}", "Xem đơn đặt hàng");
            body = body.Replace("{{ButtonConfirmLink}}", "http://localhost:4200/");
            body = body.Replace("{{UserEmail}}", data.Email);
            body = body.Replace("{{DiscountPrice}}", data.TotalAmount.ToString());
            body = body.Replace("{{UserName}}", data.UserName);
            body = body.Replace("{{UserAddress}}", data.AddressReceive);
            body = body.Replace("{{UserPhoneNumber}}", data.PhoneNumber);
            body = body.Replace("{{SubOrderTotal}}", "");
            body = body.Replace("{{OrderTotal}}", "");

            string listOrder = "";
            for (int i = 0; i < data.ProductNoTiRequest.Count(); i++)
            {
                listOrder += "<tr>";
                listOrder += "<td>" + data.ProductNoTiRequest[i].ProductName + "</td>";
                listOrder += "<td>" + data.ProductNoTiRequest[i].Quantity + "</td>";
                listOrder += "<td>" + data.ProductNoTiRequest[i].Price + "</td>";
                listOrder += "</tr>";
            }

            body = body.Replace("{{ProductOrder}}", listOrder);
            body = body.Replace("{{Payment}}", PaymentMethod);
            body = body.Replace("{{Delivery}}", data.Note);
            body = body.Replace("{{DeliveryId}}", "");
            return body;
        }
    }
}

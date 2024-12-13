using DemoVNPay.Others;
using Microsoft.Extensions.Logging;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.EF;
using StudySystem.Data.Models.Request;
using StudySystem.Infrastructure.Configuration;
using StudySystem.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly string _ipAddress;
        public PaymentService(ILogger<PaymentService> logger, UserResolverSerive user)
        {
            _logger = logger;
            _ipAddress = user.GetIpAdressUser();
        }
        public string GetPaymentUrl(OrderRequestModel paymentRequest, string orderId)
        {
            string url = AppSetting.VnpUrl;
            string returnUrl = AppSetting.VnpReturnUrl;
            string tmnCode = AppSetting.VnpTmnCode;
            string hashSecret = AppSetting.VnpHashSecret;
            string version = AppSetting.VnpVersion;

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", version);
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", tmnCode);
            pay.AddRequestData("vnp_Amount", (paymentRequest.TotalAmount * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", "");
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", _ipAddress);
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang " + orderId);
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", returnUrl);
            pay.AddRequestData("vnp_TxnRef", orderId);

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
            return paymentUrl;
        }
    }
}

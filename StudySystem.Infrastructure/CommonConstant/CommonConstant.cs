using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Infrastructure.CommonConstant
{
    public class CommonConstant
    {
        public const string PathFolderCsv = @"..\StudySystem.Data.EF\Seed Data\FileCSV\";
        public const string TypeFileCsv = @".csv";
        public const string CsvFileUserDetails = "_UserDetails_";
        public const string CsvAdministrativeRegions = "administrative_regions";
        public const string CsvAdministrativeUnits = "administrative_units";
        public const string CsvDistricts = "districts";
        public const string CsvProvinces = "provinces";
        public const string CsvWards = "wards";
        public const string CsvCategories = "_Categories_";
        public const string UserIdSession = "User_Session_";
        public const string OrderStatusNotYetPayment = "1";
        public const string OrderStatusPaymented = "0";
        public const string OrderStatusCancelPayment = "2";
        public const string ReceiveTypeAtHome = "0";
        public const string ReceiveTypeAtShop = "1";


    }
    public static class Router
    {
        public const string RegisterUser = "~/api/register-user";
        public const string LoginUser = "~/api/login";
        public const string SendMail = "~/api/send-mail";
        public const string LogOut = "~/api/logout";
        public const string VerificationEmail = "~/api/verify-email";
        public const string IsUserOnl = "~/api/user-onl";
        public const string GetProvinces = "~/api/get-provinces";
        public const string GetDistricts = "~/api/get-districts/{province_code}";
        public const string GetWards = "~/api/get-wards/{district_code}";
        public const string GetListUserDetail = "~/api/list-user-detail";
        public const string GetUserById = "~/api/get-user-by-id";
        public const string UserRegisterAds = "~/api/register-mail";
        public const string SupplierImg = "~/api/supplier-img";
        public const string CreateProduct = "~/api/create-product";
        public const string GetAllProduct = "~/api/get-product";
        public const string UpdateProduct = "~/api/update-product";
        public const string DeleteProduct = "~/api/delete-product";
        public const string GetProductDetail = "~/api/product-details";
        public const string UpdateCart = "~/api/update-cart";
        public const string GetCart = "~/api/get-cart";
        public const string CartCalculate = "~/api/calculate-total";
        public const string CartDelete = "~/api/cart-delete";
        public const string CreatedOrder = "~/api/created-order";
        public const string VerifyIPN = "~/api/vnpay_return/IPN";

    }

    public static class StatusOrdetItem
    {
        public const string Paid = "0"; // đã thanh toán
        public const string Waiting = "1";
        public const string Cancel = "2";
    }

    public static class DateTimeConstant
    {
        public const int ExpireOnl = 2;
    }
    public static class Roles
    {
        public const string RolesAdmin = "1";
        public const string RolesUser = "0";

    }

    public static class OrderStatusReceive
    {
        // 0: đang vận chuyển, 1 đã giao hàng, 2 đã hủy, 3 chờ xác nhận
        public const int IsShipping = 0;
        public const int IsShipped = 1;
        public const int IsCanceled = 2;
        public const int IsWatting = 3;
    }
}

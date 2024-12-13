using DemoVNPay.Others;
using MailKit.Search;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.EF;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.CommonConstant;
using StudySystem.Infrastructure.Configuration;
using StudySystem.Infrastructure.Extensions;
using StudySystem.Infrastructure.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly string _currentUser;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        public OrderService(IUnitOfWork unitOfWork, IPaymentService paymentService, UserResolverSerive user, ILogger<OrderService> logger, ICartService cartService) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _currentUser = user.GetUser();
            _userRepository = unitOfWork.UserRepository;
            _orderRepository = unitOfWork.OrderRepository;
            _logger = logger;
            _cartService = cartService;
            _productRepository = unitOfWork.ProductRepository;
            _orderItemRepository = unitOfWork.OrderItemRepository;
        }

        /// <summary>
        /// CreatedOrder
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        public async Task<string> CreatedOrder(OrderRequestModel orderRequest)
        {
            string orderId = StringUtils.NewGuid();
            string userId = _currentUser != "" ? _currentUser : CommonConstant.UserIdSession + orderId;
            string paymentRedirect = string.Empty;
            try
            {

                if (orderRequest.MethodPayment == "0") // thanh toán sau nhận hàng
                {
                    paymentRedirect = "/";
                }
                else if (orderRequest.MethodPayment == "1") // thanh toán ngân hàng vnpay
                {
                    paymentRedirect = _paymentService.GetPaymentUrl(orderRequest, orderId);
                }
                if (!string.IsNullOrEmpty(paymentRedirect))
                {
                    await CheckOutUser(orderRequest, userId).ConfigureAwait(false);

                    await CreateOrderUser(orderRequest, orderId, userId).ConfigureAwait(false);

                    await CreateOrderItem(orderRequest, orderId, userId).ConfigureAwait(false);

                    List<AddressBook> addresses = new List<AddressBook>();
                    addresses.Add(new AddressBook { OrderId = orderId, District = orderRequest.District, AddressReceive = orderRequest.AddressReceive, Province = orderRequest.Province });
                    await _unitOfWork.BulkInserAsync(addresses).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return paymentRedirect;
        }
        /// <summary>
        /// CheckOutUser
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task CheckOutUser(OrderRequestModel orderRequest, string userId)
        {
            if (_currentUser == "")
            {
                UserDetail userOrder = new UserDetail { UserID = userId, UserFullName = orderRequest.UserName, Email = orderRequest.Email, Password = PasswordHasher.HashPassword("12345"), PhoneNumber = orderRequest.PhoneNumber };
                await _userRepository.InsertUserDetails(userOrder);
            }
            else
            {
                CartItemsRequestModel cartItemsRequestModel = new CartItemsRequestModel();
                cartItemsRequestModel.CartInsertData = orderRequest.OrderItemInsertData;
                await _cartService.DeleteCart(cartItemsRequestModel);
            }
        }
        /// <summary>
        /// CreateOrderUser
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task CreateOrderUser(OrderRequestModel orderRequest, string orderId, string userId)
        {
            Order orderNew = new Order { OrderId = orderId, CreateUser = userId, UserId = userId, Status = CommonConstant.OrderStatusNotYetPayment, Payment = orderRequest.MethodPayment, ReceiveType = orderRequest.ReceiveType.ToString(), TotalAmount = orderRequest.TotalAmount.ToString(), Note = orderRequest.Note };
            await _orderRepository.CreatedOrder(orderNew);
        }
        /// <summary>
        /// CreateOrderItem
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task CreateOrderItem(OrderRequestModel orderRequest, string orderId, string userId)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in orderRequest.OrderItemInsertData)
            {
                orderItems.Add(new OrderItem { OrderId = orderId, ProductId = item.ProductId, Price = item.Price, Quantity = item.Quantity, CreateUser = userId, UpdateUser = userId });
            }

            await _unitOfWork.BulkInserAsync(orderItems).ConfigureAwait(false);
        }
        /// <summary>
        /// UpdatedOrder
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<OrderCompletedResponse> UpdatedOrder(VNPayIPNRequest request)
        {
            OrderCompletedResponse response = new OrderCompletedResponse();
            try
            {
                string vnp_HashSecret = AppSetting.VnpHashSecret;
                PayLib payLib = new PayLib();

                foreach (var propertyInfo in typeof(VNPayIPNRequest).GetProperties())
                {
                    string propertyName = StringUtils.ConvertToLowerCaseStart(propertyInfo.Name);
                    string propertyValue = propertyInfo.GetValue(request)?.ToString();

                    // Kiểm tra và thêm vào dữ liệu của vnpay nếu tên thuộc tính bắt đầu bằng "Vnp_"
                    if (!string.IsNullOrEmpty(propertyName) && propertyName.StartsWith("vnp_"))
                    {
                        payLib.AddResponseData(propertyName, propertyValue);
                    }
                }
                //Lay danh sach tham so tra ve tu VNPAY
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

                string orderId = request.Vnp_TxnRef;
                long vnp_Amount = Convert.ToInt64(request.Vnp_Amount) / 100;
                string vnp_ResponseCode = request.Vnp_ResponseCode;
                string vnp_TransactionStatus = request.Vnp_TransactionStatus;
                string vnp_SecureHash = request.Vnp_SecureHash;
                bool checkSignature = payLib.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    //Cap nhat ket qua GD
                    //Yeu cau: Truy van vao CSDL cua  Merchant => lay ra duoc OrderInfo
                    //Giả sử OrderInfo lấy ra được như giả lập bên dưới
                    var order = await _orderRepository.FindAsync(x => x.OrderId.Equals(request.Vnp_TxnRef)).ConfigureAwait(false);

                    if (order != null)
                    {
                        if (Int64.Parse(order.TotalAmount) == vnp_Amount)
                        {
                            if (order.Status == "1") // chưa thanh toán
                            {
                                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                                {

                                    order.Status = "0"; // set thanh toán thành công
                                    try
                                    {
                                        var paymentedProduct = await _orderItemRepository.ReturnProductChanged(order.OrderId);
                                        await _productRepository.UpdateProductQuantity(paymentedProduct);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex.Message);
                                    }

                                    response.ResponseMessage = "Thanh toán thành công";
                                    response.ResponseCode = "0";
                                }
                                else
                                {
                                    //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                                    order.Status = "2";
                                    response.ResponseMessage = "Thanh toán không thành công";
                                    response.ResponseCode = "2";
                                }
                                await _unitOfWork.CommitAsync().ConfigureAwait(false);
                            }
                            else
                            {
                                response.ResponseMessage = "Đơn hàng đã xác nhận";
                                response.ResponseCode = "2";
                            }
                        }
                        else
                        {
                            response.ResponseMessage = "invalid amount";
                            response.ResponseCode = "4";
                        }
                    }
                    else
                    {
                        response.ResponseMessage = "Order not found";
                        response.ResponseCode = "01";
                    }
                }
                else
                {
                    response.ResponseMessage = "Invalid signature";
                    response.ResponseCode = "97";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return response;
        }
        /// <summary>
        /// AllOrders
        /// </summary>
        /// <returns></returns>
        public async Task<OrdersAllResponseModel> AllOrders()
        {
            try
            {
                return await _orderRepository.GetOrders();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }

        }

        public async Task<bool> DeleteOrder(string orderId)
        {
            try
            {
                return await _orderRepository.DeleteOrder(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// admin
        /// UpdateStatus
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <param name="statusReceive"></param>
        /// <returns></returns>
        public async Task<bool> UpdateStatus(string orderId, string status, int statusReceive)
        {
            try
            {
                var orderChange = await _orderRepository.UpdateStatusOrder(orderId, status, statusReceive);
                if (orderChange && status == CommonConstant.OrderStatusPaymented)
                {
                    var paymentedProduct = await _orderItemRepository.ReturnProductChanged(orderId);
                    await _productRepository.UpdateProductQuantity(paymentedProduct);
                    return true;
                }
                return orderChange;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return false;
            }
        }
        /// <summary>
        /// GetOrderCustomser
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OrderInformationByUserResponseModel> GetOrderCustomser()
        {
            try
            {
                return await _orderRepository.GetOrderCustomser(_currentUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
        /// <summary>
        /// GetOrderDetailByOderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OrderDetailByOderIdResponseModel> GetOrderDetailByOderId(string orderId)
        {
            try
            {
                return await _orderRepository.GetOrderDetails(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<InvoiceResponseModel> GetInvoice(string orderId)
        {
            var rs = await _orderRepository.GetInvoice(orderId);
            return rs;
        }
    }
}

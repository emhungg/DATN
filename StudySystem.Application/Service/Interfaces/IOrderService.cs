using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service.Interfaces
{
    public interface IOrderService : IBaseService
    {
        Task<string> CreatedOrder(OrderRequestModel orderRequest);
        Task<OrderCompletedResponse> UpdatedOrder(VNPayIPNRequest request);
        Task<OrdersAllResponseModel> AllOrders();
        Task<bool> DeleteOrder(string orderId);
        Task<bool> UpdateStatus(string orderId, string status, int statusReceive);
        /// <summary>
        /// GetOrderCustomser
        /// </summary>
        /// <returns></returns>
        Task<OrderInformationByUserResponseModel> GetOrderCustomser();
        /// <summary>
        /// GetOrderDetailByOderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<OrderDetailByOderIdResponseModel> GetOrderDetailByOderId(string orderId);
        /// <summary>
        /// GetInvoice
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<InvoiceResponseModel> GetInvoice(string orderId);
    }
}

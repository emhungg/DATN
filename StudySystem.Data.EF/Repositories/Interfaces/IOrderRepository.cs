using StudySystem.Data.Entites;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// CreatedOrder
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<bool> CreatedOrder(Order order);
        /// <summary>
        /// GetOrders
        /// </summary>
        /// <returns></returns>
        Task<OrdersAllResponseModel> GetOrders();
        /// <summary>
        /// admin
        /// UpdateStatusOrder
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <param name="statusReceive"></param>
        /// <returns></returns>
        Task<bool> UpdateStatusOrder(string orderId, string orderStatus, int statusReceive);
        /// <summary>
        /// DeleteOrder
        /// admin
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<bool> DeleteOrder(string orderId);
        /// <summary>
        /// GetOrderCustomser
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<OrderInformationByUserResponseModel> GetOrderCustomser(string userId);

        /// <summary>
        /// GetOrderDetails
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<OrderDetailByOderIdResponseModel> GetOrderDetails(string orderId);
        /// <summary>
        /// GetInvoice
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<InvoiceResponseModel> GetInvoice(string orderId);
    }
}

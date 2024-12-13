// <copyright file="OrdersController.cs" ownedby="Xuan Truong">
//  Copyright (c) XuanTruong. All rights reserved.
//  FileType: Visual CSharp source file
//  Created On: 29/09/2023
//  Last Modified On: 29/09/2023
//  Description: OrdersController.cs
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.CommonConstant;
using StudySystem.Infrastructure.Resources;
using StudySystem.Middlewares;

namespace StudySystem.Controllers
{
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IOrderService _orderService;
        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;

        }

        /// <summary>
        /// CreatesOrder
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost(Router.CreatedOrder)]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> CreatesOrder(OrderRequestModel requestModel)
        {
            string rs = await _orderService.CreatedOrder(requestModel);
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(true, rs));
        }

        /// <summary>
        /// VerifyIPN
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Router.VerifyIPN)]
        public async Task<ActionResult<StudySystemAPIResponse<OrderCompletedResponse>>> VerifyIPN(VNPayIPNRequest request)
        {
            var rs = await _orderService.UpdatedOrder(request);
            return new StudySystemAPIResponse<OrderCompletedResponse>(StatusCodes.Status200OK, new Response<OrderCompletedResponse>(true, rs));
        }

        [HttpGet("~/api/order-list")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> GetOrder()
        {
            var rs = await _orderService.AllOrders();
            if (rs == null)
            {
                throw new BadHttpRequestException(Message._500);
            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(true, rs));
        }

        [HttpDelete("~/api/detele-order")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> DeleteOrder(string orderId)
        {
            var rs = await _orderService.DeleteOrder(orderId);
            if (!rs)
            {
                throw new BadHttpRequestException(Message._500);
            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, "Xóa thành công"));
        }

        [HttpPost("~/api/update-status-order")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> UpdateStatusOrder(string orderId, string statusNew, int statusReceive)
        {
            var rs = await _orderService.UpdateStatus(orderId, statusNew, statusReceive);
            if (!rs)
            {
                throw new BadHttpRequestException(Message.UpdateError);
            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }

        [HttpGet("~/api/customer-get-order")]
        //[Authorize]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> CustomerGetOrder()
        {
            var rs = await _orderService.GetOrderCustomser();
            if (rs != null)
            {
                string hosturl = $"{this.Request.Scheme}:/{this.Request.Host}{this.Request.PathBase}/Product/";
                foreach (var image in rs.GroupOrderItems)
                {
                    image.ImageOrder = hosturl + $"{image.ProductId}/" + image.ImageOrder;
                }
            }
            
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(true, rs));
        }


        [HttpGet("~/api/get-order-details")]
        //[Authorize]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> GetOrderDetails(string orderId)
        {
            var rs = await _orderService.GetOrderDetailByOderId(orderId);
            if(rs != null)
            {
                string hosturl = $"{this.Request.Scheme}:/{this.Request.Host}{this.Request.PathBase}/Product/";
                foreach (var image in rs.Data)
                {
                    image.Image = hosturl + $"{image.ProductId}/" + image.Image;
                }
            }
            
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(true, rs));
        }

        [NonAction]
        private string GetFilepath(string productId)
        {
            return _environment.WebRootPath + "\\product\\" + productId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class Order : BaseEntity
    {
        [Key]
        public string OrderId { get; set; }
        public string? UserId { get; set; }
        // 0: Đã thanh toán, 1 chưa thanh toán, Status payment
        public string? Status { get; set; }
        // 0: đang vận chuyển, 1 đã giao hàng, 2 đã hủy, 3 chờ xác nhận
        public int? StatusReceive { get; set; } = 3;
        // method payment 0: trả sau, 1 thanh toán vnpay
        public string? Payment { get; set; }
        public string? Note { get; set; }
        public string? ReceiveType { get; set; } // 0: tại của hàng; 1: tại nhà
        public string TotalAmount { get; set; }
        public UserDetail? UserDetail { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Request
{
    public class NotiOrderDoneRequestModel
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ReceiveType { get; set; }
        public string Province { get; set; }
        public string AddressReceive { get; set; }
        public string Note { get; set; }
        public string District { get; set; }
        public string MethodPayment { get; set; }
        public List<OrderItemInsertData> OrderItemInsertData { get; set; }
        public List<ProductNoTiRequest> ProductNoTiRequest { get; set; }
        public long TotalAmount { get; set; }
    }

    public class OrderItemInsertData
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
    }

    public class ProductNoTiRequest
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int PriceSell { get; set; }
        public int Price { get; set; }
        public string ImagePath { get; set; }
        public int TotalQuantity { get; set; }
        public bool IsSelected { get; set; }
        public int Quantity { get; set; }
    }

}

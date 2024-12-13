using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class OrdersAllResponseModel
    {
        public List<OrdersResponseDataModel> Orders { get; set; }
    }

    public class OrdersResponseDataModel
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string ReciveType { get; set; }
        public string AddressReceive { get; set; }
        public string Note { get; set; }
        public string StatusOrder { get; set; }
        public int StatusReceive { get; set; }
        public string OrderDateAt { get; set; }
        public string MethodPayment { get; set; }
        public string TotalAmount { get; set; }
        public List<ProductOrderListDataModel> ProductOrderListDataModels { get; set; }
    }
    public class ProductOrderListDataModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

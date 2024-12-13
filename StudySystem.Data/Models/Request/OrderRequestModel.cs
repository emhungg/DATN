using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Request
{
    public class OrderRequestModel
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int ReceiveType { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string AddressReceive { get; set; }
        public List<CartUpdateDataModel> OrderItemInsertData { get; set; }
        public string Note { get; set; }
        public string MethodPayment { get; set; }
        public double TotalAmount { get; set; }

    }
}

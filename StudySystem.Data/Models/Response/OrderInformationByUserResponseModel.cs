using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class OrderInformationByUserResponseModel
    {
        public int QuantityOrder { get; set; }
        public double TotalAmount { get; set; }
        public List<GroupOrderItems>? GroupOrderItems { get; set; }
    }

    public class GroupOrderItems
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string? ImageOrder { get; set; }
        public string? NameOrder { get; set; } 
        public int QuantityOtherItems { get; set; }
        public int? StatusReceiveOrder { get; set; }
        public double TotalAmountOrder { get; set; }
        public string OrderAt { get; set; }
    }

}

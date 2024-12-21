using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class OrderDetailByOderIdResponseModel
    {
        public List<ProductOrderDetails> Data { get; set;}
    }

    public class ProductOrderDetails
    {
        public string? ProductId { get; set; }
        public string? Image { get; set; }
        public string? NameProduct { get; set; }
        public string? TotalPriceByProduct { get; set; }
        public string? TotalQuantityByProduct { get; set; }
    }
}

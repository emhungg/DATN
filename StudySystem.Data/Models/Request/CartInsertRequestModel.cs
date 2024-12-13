using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Request
{
    public class CartItemsRequestModel
    {
        public List<CartUpdateDataModel> CartInsertData { get; set; }
    }

    public class CartUpdateDataModel
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

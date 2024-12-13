using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class CartResponseModel
    {
        public List<CartDataModel> CartData { get; set; }
    }

    public class CartDataModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double PriceSell { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public int TotalQuantity { get; set; }
        public int Quantity { get; set; }
    }
}

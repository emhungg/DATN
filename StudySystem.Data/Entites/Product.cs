using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class Product : BaseEntity
    {
        [Key]
        public string ProductId { get; set; } = null!;
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public string? ProductionDate { get; set; }
        public double PriceSell { get; set; }
        // 0 new, 1 old
        public int ProductStatus { get; set; }
        public string? BrandName { get; set; }

        public List<ProductCategory>? ProductCategories { get; set; }
        public List<CartItem>? CartItems { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public List<WishList>? WishLists { get; set; }
    }
}

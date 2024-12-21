using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class OrderItem : BaseEntity
    {
        [Key]
        public string OrderId { get; set; }
        [Key]
        public string? ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class CartItem : BaseEntity
    {
        [Key]
        public string CartId { get; set; } = null!;
        [Key]
        public string? ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Cart? Cart { get; set; }
        public Product? Product { get; set; }
    }
}

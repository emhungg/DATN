using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class ProductCategory
    {
        [Key]
        public string ProductId { get; set; } = null!;
        [Key]
        public string CategoryId { get; set; } = null!;
        public Product? Product { get; set; }
        public Category? Category { get; set; }
    }
}

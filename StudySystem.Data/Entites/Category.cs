using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class Category : BaseEntity
    {
        [Key]
        public string CategoryId { get; set; } = null!;
        public string? CategoryName { get; set; }
        public List<ProductCategory>? ProductCategories { get; set; }

    }
}

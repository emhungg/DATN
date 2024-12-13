using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class ProductConfiguration
    {
        [Key]
        public string ProductId { get; set; } = null!;
        public string? Chip { get; set; }
        public int Ram { get; set; }
        public int Rom { get; set; }
        public string? Screen { get; set;}
    }
}

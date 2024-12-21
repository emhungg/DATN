using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Request
{
    public class UpdateProductRequestModel
    {
        public string ProductId { get; set; }
        public string? ProductName { get; set; }
        public double Price { get; set; }
        public double PriceSell { get; set; }
        public string? Description { get; set; }
        public string? ProductionDate { get; set; }
        public int ProductQuantity { get; set; }
        public int ProductStatus { get; set; }
        public string? ProductCategoryId { get; set; }
        public string? ProductBrand { get; set; }
        public string? ChipProduct { get; set; }
        public int RamProduct { get; set; }
        public int RomProduct { get; set; }
        public string? ScreenProduct { get; set; }
        public IFormFileCollection ImageProducts { get; set; }
    }
}

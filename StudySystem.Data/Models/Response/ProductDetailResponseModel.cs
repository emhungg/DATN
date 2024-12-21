using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class ListProductDetailResponseModel
    {
        public List<ProductDetailResponseModel> listProductDeatails { get; set; }
    }
    public class ProductDetailResponseModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
        public double ProductPrice { get; set; }
        public double ProductSell { get; set; }
        public int ProductQuantity { get; set; }
        public string ProductBrand { get; set; }
        public string ProductionDate { get; set; }
        public int ProductStatus { get; set; }
        public string CategoryId { get; set; }
        public bool IsLike { get; set; }
        public List<ImageProductData> Images { get; set; }
        public ProductConfigData ProductConfig { get; set; }
    }

    public class ProductConfigData
    {
        public string Chip { get; set; }
        public int Ram { get; set; }
        public int Rom { get; set; }
        public string Screen { get; set; }
    }

    public class ImageProductData
    {
        public string ImagePath { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Data
{
    public class UpdateProductQuantityDataModel
    {
        public List<ProductChangedDataModel> ProductChangedData { get; set; }
    }
    public class ProductChangedDataModel
    {
        public string ProductId { get; set;}
        public int Quantity { get; set;}
    }
}

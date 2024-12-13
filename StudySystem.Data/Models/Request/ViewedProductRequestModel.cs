using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Request
{
    public class ViewedProductRequestModel
    {
        public List<ViewedProductDataModel>? ProductIdData { get; set; }
    }
    public class ViewedProductDataModel
    {
        public string ProductId { get; set;}
    }
}

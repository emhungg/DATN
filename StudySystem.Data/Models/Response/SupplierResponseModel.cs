using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class SupplierResponseModel
    {
        public List<SupplierDataModel>? Imgs { get; set; }
        public SupplierResponseModel()
        {
            
        }
    }
    public class SupplierDataModel
    {
        public string? Image { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Request
{
    public class ChangeAddressRequestModel
    {
        public string? Descriptions { get; set; }
        public string? WardCode { get; set; }
        public string? DistrictCode { get; set; }
        public string? ProvinceCode { get; set; }
    }
}

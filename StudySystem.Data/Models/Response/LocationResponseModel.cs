using StudySystem.Data.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class ProvincesResponseModel
    {
        public List<LocationDataModel>? Provinces { get; set; }
        public ProvincesResponseModel()
        {
            
        }
    }

    public class DistrictsResponseModel
    {
        public List<LocationDataModel>? Districts { get; set; }
        public DistrictsResponseModel() { }
    }

    public class WardsResponseModel
    {
        public List<LocationDataModel>? Wards { get; set; }
        public WardsResponseModel() { }
    }
}

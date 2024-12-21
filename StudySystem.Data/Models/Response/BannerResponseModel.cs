using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class BannerResponseModel
    {
        public List<BannerDataModel> Data { get; set; }
    }

    public class BannerDataModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public bool isActive { get; set; }
        public string CreateAt { get; set; }
    }
}

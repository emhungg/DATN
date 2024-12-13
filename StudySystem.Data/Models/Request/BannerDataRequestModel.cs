using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Request
{
    public class BannerDataRequestModel
    {
        public string Title { get; set; }
        public IFormFile Image { get; set; }
        public bool IsActive { get; set; }
    }
}

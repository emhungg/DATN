using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class NewResponseModel
    {
        public List<NewsDataModel> NewsData { get; set; }
    }

    public class NewsDataModel
    {
        public int IdNews { get; set; }
        public string Title { get; set; }
        public string CreateUser { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string CreateAt { get; set; }
    }
}

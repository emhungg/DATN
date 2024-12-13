using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class RatingResponseModel
    {
        public float RatioRating { get; set; }
        public List<RatingDataModel> RatingData { get; set; }
    }

    public class RatingDataModel
    {
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string CommentAt { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Request
{
    public class RatingRequestModel
    {
        public string ProductId { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public int Rating { get; set; }
    }
}

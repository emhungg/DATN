using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class RatingProduct : BaseEntity
    {
        [Key]
        public string UserId { get; set; } = null!;
        [Key]
        public string ProductId { get; set; } = null!;
        public string UserName { get; set; }
        
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}

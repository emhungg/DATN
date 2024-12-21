using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class UserToken
    {
        [Key]
        public string UserID { get; set; } = null!;
        [Required]
        public string Token { get; set; } = null!;
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime ExpireTime { get; set; }
        public DateTime ExpireTimeOnline { get; set; }
    }
}

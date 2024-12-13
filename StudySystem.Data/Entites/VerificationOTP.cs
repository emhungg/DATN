using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class VerificationOTP
    {
        [Key]
        public string UserID { get; set; } = null!;
        [Required]
        public string Code { get; set; } = null!;
        [Required]
        public DateTime ExpireTime { get; set; }
    }
}

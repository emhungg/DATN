

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Request
{
    public class LoginRequestModel
    {
        [Required]
        public string UserID { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}

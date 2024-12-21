using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class LoginResponseModel
    {
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string Token { get; set; } = null!;
        [Required]
        public int Role { get; set; }
        public LoginResponseModel(bool isActive, string token, int role)
        {
            IsActive = isActive;
            Token = token;
            Role = role;
        }
    }
}

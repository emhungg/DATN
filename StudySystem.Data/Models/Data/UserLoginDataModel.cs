using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Data
{
    public class UserLoginDataModel
    {
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
        public UserLoginDataModel(string userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }
    }
}

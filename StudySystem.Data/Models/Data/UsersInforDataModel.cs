using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Data
{
    public class UsersInforDataModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        // 0 male, 1 female
        [Range(0, 1)]
        public int Gender { get; set; }
        // 0 user, 1 admin
        [Range(0, 1)]
        public int Role { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public string? CreateDateAt { get; set; }
        public string RankUser { get; set; }
    }
}

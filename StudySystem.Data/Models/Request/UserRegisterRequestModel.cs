using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Request
{
    public class UserRegisterRequestModel
    {
        [MaxLength(12)]
        public string UserID { get; set; } = null!;
        [Required]
        public string UserFullName { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        public AddressUserDataModel? Address { get; set; }
        // 0 male, 1 female
        [Range(0, 1)]
        public int Gender { get; set; }
    }
    public class AddressUserDataModel
    {
        public string Descriptions { get; set; } = string.Empty;
        public string WardCode { get; set; } = string.Empty;
        public string DistrictCode { get; set; } = string.Empty;
        public string ProvinceCode { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Data
{
    public class UserDetailDataModel
    {
        public string? UserFullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? RankUser { get; set; }
        public string? JoinDateAt { get; set; }
        public decimal PriceBought { get; set; }
        public int CountOrderItem { get; set; }
        public string? AddressUserDes { get; set; }
        public string? AddressUserWard { get; set; }
        public string? AddressUserDistrict { get; set; }
        public string? AddressUserProvince { get; set; }
        public string? WardCode { get; set; }
        public string? DistrictCode { get; set; }
        public string? ProvinceCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class AddressUser : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? Descriptions { get; set; } 
        public string? WardCode { get; set; } 
        public string? DistrictCode { get; set; } 
        public string? ProvinceCode { get; set; } 
        [MaxLength(12)]
        public string UserID { get; set; } = null!;
        public UserDetail? UserDetail { get; set; }
        public Ward? Ward { get; set; }
        public District? District { get; set; }
        public Province? Province { get; set; }
    }
}

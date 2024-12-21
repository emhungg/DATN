using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    [Table("wards")]
    public class Ward
    {
        [Key]
        public string Code { get; set; } = null!;
        public string? Name { get; set; }
        public string? NameEn { get; set; } 
        public string? FullName { get; set; }
        public string? FullNameEn { get; set; }
        public string? CodeName { get; set; } 
        public string? DistrictCode { get; set; }
        public int? AdministrativeUnitId { get; set; }
        public District District { get; set; } = null!;
        public AdministrativeUnit AdministrativeUnit { get; set; } = null!;
        public ICollection<AddressUser> AddressUsers { get; set; } = new List<AddressUser>();
    }
}

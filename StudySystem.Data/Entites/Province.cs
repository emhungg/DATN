using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    [Table("provinces")]
    public class Province
    {
        [Key]
        public string Code { get; set; } = null!;
        public string? Name { get; set; }
        public string? NameEn { get; set; }
        public string? FullName { get; set; }
        public string? FullNameEn { get; set; }
        public string? CodeName { get; set; } 
        public int? AdministrativeUnitId { get; set; }
        public int? AdministrativeRegionId { get; set; }
        public AdministrativeRegion? AdministrativeRegion { get; set; }
        public AdministrativeUnit? AdministrativeUnit { get; set; }
        public ICollection<District> Districts { get; set; } = new List<District>();
        public ICollection<AddressUser> AddressUsers { get; set; } = new List<AddressUser>();

    }
}

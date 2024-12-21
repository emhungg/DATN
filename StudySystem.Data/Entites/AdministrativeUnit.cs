using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    [Table("administrative_units")]
    public class AdministrativeUnit
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string FullNameEn { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string ShortNameEn { get; set; } = string.Empty;
        public string CodeName { get; set; } = string.Empty;
        public string CodeNameEn { get; set; } = string.Empty;
        public ICollection<Province> Provinces { get; set; } = new List<Province>();
        public ICollection<District> Districts { get; set; } = new List<District>();
        public ICollection<Ward> Wards { get; set; } = new List<Ward>();
    }
}

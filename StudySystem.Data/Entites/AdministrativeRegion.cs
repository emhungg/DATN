using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    [Table("administrative_regions")]
    public class AdministrativeRegion
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string CodeName { get; set; } = string.Empty;
        public string CodeNameEn { get; set; } = string.Empty;
        public ICollection<Province> Provinces { get; set; } = new List<Province>();

    }
}

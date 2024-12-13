using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class Image : BaseEntity
    {
        [Key]
        public string Id { get; set; } = null!;
        public string? ImageDes { get; set; }
        public string? ProductId { get; set; }
    }
}

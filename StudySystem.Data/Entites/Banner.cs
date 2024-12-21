using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class Banner : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
    }
}

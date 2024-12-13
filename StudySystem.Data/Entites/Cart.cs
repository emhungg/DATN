using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class Cart : BaseEntity
    {
        [Key]
        public string CartId { get; set; } = null!;
        public string? UserId { get; set; }
        public UserDetail? UserDetail { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}

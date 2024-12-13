using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class WishList : BaseEntity
    {
        [Key]
        public int WishListId { get; set; }
        public string? UserId { get; set; }
        public string? ProductId { get; set; }
        public UserDetail? UserDetail { get; set; }
        public Product? Product { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Entites
{
    public class AddressBook
    {
        [Key]
        public string OrderId { get; set; } = null!;
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? AddressReceive { get; set; }
    }
}

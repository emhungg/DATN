using StudySystem.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Request
{
    public class PassswordChangeRequestModel
    {
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
        [CompareWith("PasswordNew")]
        public string ConfirmPasswordNew { get; set; }
    }
}

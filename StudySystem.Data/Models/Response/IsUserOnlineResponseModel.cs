using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class IsUserOnlineResponseModel
    {
        public bool IsUserOnline { get; set; }
        public DateTime ExpireDateTimeOnle { get; set; }
        public IsUserOnlineResponseModel(bool isUserOnl, DateTime expireTime)
        {
            IsUserOnline = isUserOnl;
            ExpireDateTimeOnle = expireTime;
        }
    }
}

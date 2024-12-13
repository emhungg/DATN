using StudySystem.Data.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class UserInformationResponseModel
    {
        public UserDetailDataModel? User { get; set; }
        public UserInformationResponseModel()
        {
            
        }
    }
}

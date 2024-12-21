using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class StudySystemErrorResponseModel
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public StudySystemErrorResponseModel(string msg)
        {
            ErrorMessage = msg;
        }
        public StudySystemErrorResponseModel(int code, string msg)
        {
            ErrorCode = code;
            ErrorMessage = msg;
        }
    }
}

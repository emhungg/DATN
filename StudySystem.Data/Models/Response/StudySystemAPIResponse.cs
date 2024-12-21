using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class StudySystemAPIResponse<T> where T : class
    {
        [Required]
        public int Code { get; set; }
        [Required]
        public Response<T> Response { get; set; }
        public StudySystemAPIResponse(int statusCode = 200, Response<T> response = null)
        {
            Response = response;
            Code = statusCode;
        }
    }
    public class Response<T>
    {
        [Required]
        public bool Success { get; set; }
        [Required]
        public T Data { get; set; }
        public Response(bool success, T data)
        {
            Success = success;
            Data = data;
        }
    }
}

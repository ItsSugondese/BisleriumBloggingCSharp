using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view
{
    public class ApiError
    {
        public int Status { get; set; }
        public int HttpCode { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public ApiError(int status, int httpCode, string message, List<string> errors)
        {
            Status = status;
            HttpCode = httpCode;
            Message = message;
            Errors = errors;
        }

        public ApiError(int status, int httpCode, string message, string error)
        {
            Status = status;
            HttpCode = httpCode;
            Message = message;
            Errors = new List<string> { error };
        }

        public ApiError()
        {

        }

        // Add a constructor for the additional parameters if needed
    }
}

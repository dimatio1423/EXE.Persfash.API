using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helper.CustomExceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode Status { get; }
        public new string Message { get; }

        public ApiException(HttpStatusCode status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}

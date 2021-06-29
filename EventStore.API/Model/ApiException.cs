using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EventStore.API.Model
{
    public class ApiException : Exception
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public ApiException(string errorCode, string errorMessage, HttpStatusCode statusCode)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }
    }
}

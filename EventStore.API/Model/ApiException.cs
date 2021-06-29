using System;
using System.Net;

namespace EventStore.API.Model
{
    public class ApiException : Exception
    {
        #region Public Constructors

        public ApiException(string errorCode, string errorMessage, HttpStatusCode statusCode)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        #endregion Public Constructors

        #region Public Properties

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        #endregion Public Properties
    }
}
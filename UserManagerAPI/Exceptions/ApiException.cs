using System;

namespace UserManagerAPI.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public ApiException() : base( "Something went wrong!" ) { StatusCode = 500; }
        public ApiException(string message, int statusCode = 404 ) : base( message ) { StatusCode = statusCode; }
    }
}

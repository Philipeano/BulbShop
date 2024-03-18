using System.Net;

namespace BulbShop.Api.Models
{
    public class ErrorResponse
    {
        public ErrorResponse(string message, HttpStatusCode code)
        {
            Message = message;
            Code = code;    
        }

        public ErrorResponse(string message)
        {
            Message = message;
        }

        public ErrorResponse(HttpStatusCode code)
        {
            Code = code;
        }

        public string Message { get; set; }

        public HttpStatusCode Code { get; set; }
    }
}

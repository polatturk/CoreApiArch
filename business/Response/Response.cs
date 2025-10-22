using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Response
{
    public class Response
    {
        public bool IsSuccess { get; protected set;}

        public string Message { get; protected set;}

        public Response(bool isSuccess, string message) {

            IsSuccess = isSuccess;
            Message = message;
        }

        public static Response Success(string message = "")
        {
            return new Response(true, message);
        }
        public static Response Error(string message = "")
        {
            return new Response(false, message);
        }
    }
}
    
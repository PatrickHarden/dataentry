using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace dataentry.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate Request;

        public ExceptionMiddleware(RequestDelegate requestDelegate)
        {
            Request = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await Request(httpContext);
            }
            catch (BadHttpRequestException ex)
            {
                if (string.Equals(ex.Message,"Request body too large.",StringComparison.OrdinalIgnoreCase))
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.RequestEntityTooLarge;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}

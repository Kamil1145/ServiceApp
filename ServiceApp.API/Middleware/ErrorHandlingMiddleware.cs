using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceApp.API.Exceptions;
using System.Net;

namespace ServiceApp.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (JiraException e)
            {
                httpContext.Response.StatusCode = 401;
                ProblemDetails details = new ProblemDetails()
                {
                    Title = e.Message,
                    Status = (int)HttpStatusCode.Unauthorized
                };

                var result = JsonConvert.SerializeObject(new { message = e?.Message });
                await httpContext.Response.WriteAsync(result);
            }
            catch (Exception e)
            {
                httpContext.Response.StatusCode = 400;
                ProblemDetails details = new ProblemDetails()
                {
                    Title = e.Message,
                    Status = (int)HttpStatusCode.BadRequest
                };

                var result = JsonConvert.SerializeObject(new { message = e?.Message });
                await httpContext.Response.WriteAsync(result);
            }
        }
    }
}

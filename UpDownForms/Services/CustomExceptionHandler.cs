
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System;

namespace UpDownForms.Services
{

    public class EntityNotFoundException : Exception {
        public EntityNotFoundException(string message) : base(message) { }
    }
    public class CustomExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                BadHttpRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                EntityNotFoundException => StatusCodes.Status404NotFound,
                
                
                
                _ => StatusCodes.Status500InternalServerError
            };

            var problemDetails = new ProblemDetails
            {
                Title = statusCode == StatusCodes.Status500InternalServerError
                    ? "(500) Internal Server Error"
                    : "Exception handled successfully",
                Status = statusCode,
                Type = exception?.GetType().Name,
                Detail = exception?.Message,
                Instance = httpContext.Request.Path
            };

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}

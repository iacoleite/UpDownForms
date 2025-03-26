
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System;
using System.Text.Json;

namespace UpDownForms.Services
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                BadHttpRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                UnauthorizedException => StatusCodes.Status403Forbidden,
                EntityNotFoundException => StatusCodes.Status404NotFound,
                                                                
                _ => StatusCodes.Status500InternalServerError
            };

            httpContext.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Title = GetExceptionTitle(statusCode),
                Status = statusCode,
                Type = exception?.GetType().Name,
                Detail = exception?.Message,
                Instance = httpContext.Request.Path,
            };

            httpContext.Response.ContentType = "application/problem+json";
            
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problemDetails), cancellationToken);

            return true;
        }

        public string GetExceptionTitle(int httpErrorCode)
        {
            switch (httpErrorCode)
            {
                case 400:
                    return "400 - Bad Request";
                    break;
                case 401:
                    return "401 - Unauthorized";
                    break;
                case 403:
                    return "403 - Forbidden";
                    break;
                case 404:
                    return "404 - Not Found";
                    break;
                default:
                    return "500 - Internal Server Error";
                    break;
            }
        }
    }
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message) { }
        public EntityNotFoundException() { }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
        public UnauthorizedException() { }
    }

    
}

using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace Animal_Adoption_Management_System_Backend.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) 
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (IException exc)
            {
                _logger.LogError(exc, $"Something went wrong while processing {context.Request.Path}");
                await HandleExceptionAsync(context, exc);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"Something went wrong while processing {context.Request.Path}");
                await HandleExceptionAsync(context, exc);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exc)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            ErrorDetails errorDetails = new()
            {
                ErrorType = "Failure",
                ErrorMessage = exc.Message,
            };

            string response = JsonConvert.SerializeObject(errorDetails);
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(response);
        }
        private Task HandleExceptionAsync(HttpContext context, IException exc)
        {
            context.Response.ContentType = "application/json";
            ErrorDetails errorDetails = new()
            {
                ErrorType = exc.ErrorType,
                ErrorMessage = exc.Message,
            };

            string response = JsonConvert.SerializeObject(errorDetails);
            context.Response.StatusCode = (int)exc.StatusCode;
            return context.Response.WriteAsync(response);
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Net;
using DebtusTest.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace StaffService.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ExceptionResponseDto
            {
                Message = ex.Message,
                StatusCode = (int)statusCode
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
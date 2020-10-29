using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace booksapi.Middleware
{
    public class HandlerExceptions
    {
        private readonly RequestDelegate _next;

        public HandlerExceptions(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                Log.Logger.Information($"New request to: ${context.Request.Path.ToString()}");
                await _next(context);
            } catch (Exception ex) {
                Log.Logger.Error($"Error: {ex.Message}. InnerException: {ex.InnerException}. StackTrace: {ex.StackTrace}");
            }
        }
    }

    public static class HandlerExceptionsExtensions
    {
        public static IApplicationBuilder UseHandlerExceptions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HandlerExceptions>();
        }
    }
}
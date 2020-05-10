using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using post_ang_webapi_sql.Models;
using post_ang_webapi_sql.Services;

namespace post_ang_webapi_sql.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        private readonly ILoggerManager _nlogger;
        public ExceptionMiddleware(RequestDelegate next, ILogger logger, ILoggerManager nlogger)
        {
            _logger = logger;
            _next = next;
            _nlogger = nlogger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong custom middleware: {ex}");
                _nlogger.LogError($"Something went wrong NLOG custom middleware-------------: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error from the custom middleware."
            }.ToString());
        }
    }
}
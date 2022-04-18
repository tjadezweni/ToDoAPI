using Microsoft.AspNetCore.Diagnostics;
using ToDo.Contracts.Exceptions;
using ToDoAPI.Models;

namespace ToDoAPI.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature is not null)
                    {
                        var exception = contextFeature.Error;
                        var statusCode = StatusCodes.Status500InternalServerError;
                        var message = "Internal Server Error";
                        switch (exception)
                        {
                            case NotFoundException:
                                statusCode = StatusCodes.Status404NotFound;
                                message = exception.Message;
                                break;
                            default:
                                break;
                        }
                        context.Response.StatusCode = statusCode;
                        var responseText = new ErrorDetails()
                        {
                            Message = message,
                        }.ToString();
                        await context.Response.WriteAsync(responseText);
                    }
                });
            });
        }
    }
}

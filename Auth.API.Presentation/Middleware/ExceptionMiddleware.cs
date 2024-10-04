using Auth.API.Core.Exceptions;
using Azure;

namespace Auth.API.Presentation.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            switch (exception)
            {
                case NotFoundException _:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                case UnauthorizedException _:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    break;
                case AlreadyUsedException _:
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    break;
                case BadRequestException _:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                case ExternalServiceException _:
                    context.Response.StatusCode = StatusCodes.Status502BadGateway;
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = exception.Message
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}

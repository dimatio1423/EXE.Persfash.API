
using Services.Helper.CustomExceptions;

namespace MusicStreamingAPI.MiddleWare
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred");

                context.Response.ContentType = "application/json";

                if (ex is ApiException apiEx)
                {
                    context.Response.StatusCode = (int)apiEx.Status;
                    
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex is ApiException apiException ? apiException.Message : ex.Message,
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}

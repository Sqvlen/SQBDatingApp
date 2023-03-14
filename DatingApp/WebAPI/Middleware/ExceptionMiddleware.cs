using System.Net;
using System.Text.Json;
using WebAPI.Errors;

namespace WebAPI.Middleware
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate _requestDelegate;
        public readonly ILogger<ExceptionMiddleware> _logger;
        public readonly IHostEnvironment _hostEnvironment;



        public ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger,
            IHostEnvironment hostEnvironment)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _requestDelegate(httpContext);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ApiException response;
                
                if (_hostEnvironment.IsDevelopment())
                {
                    response = new ApiException(httpContext.Response.StatusCode, exception.Message, exception.StackTrace?.ToString());
                }
                else
                {
                    response = new ApiException(httpContext.Response.StatusCode, exception.Message, "Internal Server Error");
                }

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}

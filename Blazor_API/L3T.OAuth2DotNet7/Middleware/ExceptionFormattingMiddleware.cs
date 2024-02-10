using System.Net;
using L3T.OAuth2DotNet7.CommonModel;
using Newtonsoft.Json;

namespace L3T.OAuth2DotNet7.Middleware
{
    public class ExceptionFormattingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ExceptionFormattingMiddleware> _logger;

        public ExceptionFormattingMiddleware(RequestDelegate next, IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _env = env;
            _logger = loggerFactory
                .CreateLogger<ExceptionFormattingMiddleware>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex, _env);
            }
        }

        private Task HandleException(HttpContext context, Exception exception, IWebHostEnvironment env)
        {
            var code = HttpStatusCode.InternalServerError;
            var error = new ApiResponse();
            error.StatusCode = (int)code;
            error.Status = "Error";

            if (_env.IsDevelopment())
            {
                error.Message = exception.Message;
                error.Data = exception.StackTrace;
            }
            else
            {
                //  ...in production 
                error.Message = "The service is unavailable";
                error.Data = "The server is under maintenance";
            }

            if (exception is GlobalApplicationException)
            {
                error.Message = exception.Message;
                error.Data = exception.StackTrace;

                error.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                code = HttpStatusCode.UnprocessableEntity;
            }

            else if (exception is UnauthorizedAccessException)
            {
                error.StatusCode = 401;
                error.Message = exception.Message;
                error.Data = "Unauthorized Access";
                code = HttpStatusCode.Unauthorized;
            }


            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

        private static string DummyMessage()
        {
            return "Oops, Something wrong on our side, Please try again";
        }
    }
}

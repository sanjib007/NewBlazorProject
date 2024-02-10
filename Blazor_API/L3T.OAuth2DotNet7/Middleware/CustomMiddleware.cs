using L3T.OAuth2DotNet7.DataAccess.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace L3T.OAuth2DotNet7.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomMiddleware> _logger;
        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                var path = httpContext.Request.Path.ToString();
                string controllerName = httpContext.Request.RouteValues["controller"].ToString();
                string actionName = httpContext.Request.RouteValues["action"].ToString();

                //dbContext.Add(new MenuSetupModel()
                //{
                //    ControllerName = controllerName,
                //    MethodName = actionName,
                //    ApplicationBaseUrl = path,
                //    CreatedAt = DateTime.Now
                //});
                //await dbContext.SaveChangesAsync();

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Something went wrong: {ex.Message}");
            }
        }
    }
}

using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.RequestModel.Permission;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.MenuAndPermission;
using L3T.Utility.Helper;
using Newtonsoft.Json;

namespace L3T.ChangeRequest.API.Middleware
{
    public class PermissionValidationMiddleware : IMiddleware
    {
        private readonly IBaseControllerCommonService _baseControllerCommonService;
        private readonly IMenuAndPermissionSetupService _menuAndPermissionService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PermissionValidationMiddleware> _logger;
        public PermissionValidationMiddleware(IBaseControllerCommonService baseControllerCommonService, IConfiguration configuration, IMenuAndPermissionSetupService menuAndPermissionService, ILogger<PermissionValidationMiddleware> logger)
        {
            _baseControllerCommonService = baseControllerCommonService;
            _configuration = configuration;
            _menuAndPermissionService = menuAndPermissionService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if ((!context.Request.Path.Value.Contains("/uploads/") && !context.Request.Path.Value.Contains("/favicon.ico")))
            {
                try
                {

                    await _baseControllerCommonService.InsertMenuSetupTable(context);
                    var projectName = _configuration.GetValue<string>("DefaultApproverDepartment:ProjectName");
                    var getUserid = context.User.GetClaimUserId();
                    string controllerName = context.Request.RouteValues["controller"].ToString();
                    string actionName = context.Request.RouteValues["action"].ToString();
                    var request = new GetAllMenuSetupAndPermissionRequestModel()
                    {
                        projectName = projectName,
                        roleName = context.User.GetClaimUserRoles()
                    };
                    if (!context.User.Identity.IsAuthenticated)
                    {
                        throw new Exception("Unauthorized");
                    }

                    var allUserOrRoleWisePermission = await _menuAndPermissionService.PermissionCheckFromMiddleware(request, controllerName, actionName, "Middleware", getUserid);
                    if (allUserOrRoleWisePermission.Count > 0)
                    {
                        await next(context);
                    }
                    else
                    {
                        throw new Exception("Permission Denied");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Error Message: {ex.Message}");
                    _logger.LogInformation($"Error Details: {JsonConvert.SerializeObject(ex)}");

                    var response = new ApiResponse();
                    if (ex.Message == "Unauthorized")
                    {
                        response = new ApiResponse()
                        {
                            Status = "Error",
                            StatusCode = 401,
                            Message = "Unauthorized",
                            Data = null
                        };
                    }
                    else
                    {
                        response = new ApiResponse()
                        {
                            Status = "Error",
                            StatusCode = 400,
                            Message = ex.Message,
                            Data = ex
                        };
                    }
                    
                    var result = JsonConvert.SerializeObject(response);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = response.StatusCode;
                    await context.Response.WriteAsync(result);
                }
            }
            else
            {
                await next(context);
            }
            

        }
    }
}

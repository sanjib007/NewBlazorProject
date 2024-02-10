using L3T.Utility.Helper;
using L3TIdentityOAuth2Server.DataAccess.RequestModel;
using L3TIdentityTokenServer.CommonModel;
using L3TIdentityTokenServer.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

namespace L3TIdentityTokenServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public SeedController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }



        [AllowAnonymous]
        [HttpPost("ClientInsert")]
        public async Task<IActionResult> ClientInsert([FromBody] ClientInsertRequestModel model)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<IdentityTokenServerDBContext>();
            await context.Database.EnsureCreatedAsync();

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync(model.ClientId) is null)
            {
                var newClient = new OpenIddictApplicationDescriptor();

                newClient.ClientId = model.ClientId;
                newClient.ClientSecret = model.ClientSecret;
                newClient.DisplayName = model.DisplayName;

                foreach (var permission in model.Permissions)
                {
                    if (permission == CommonHelper.Permissions.Token.ToString())
                    {
                        newClient.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                    }
                    else if (permission == CommonHelper.Permissions.ClientCredentials.ToString())
                    {
                        newClient.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
                    }
                    else if (permission == CommonHelper.Permissions.Password.ToString())
                    {
                        newClient.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
                    }
                    else if (permission == CommonHelper.Permissions.RefreshToken.ToString())
                    {
                        newClient.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
                    }
                    else if (permission == CommonHelper.Permissions.Email.ToString())
                    {
                        newClient.Permissions.Add(OpenIddictConstants.Permissions.Scopes.Email);
                    }
                    else if (permission == CommonHelper.Permissions.Profile.ToString())
                    {
                        newClient.Permissions.Add(OpenIddictConstants.Permissions.Scopes.Profile);
                    }
                    else if (permission == CommonHelper.Permissions.Roles.ToString())
                    {
                        newClient.Permissions.Add(OpenIddictConstants.Permissions.Scopes.Roles);
                    }
                    else if (permission == CommonHelper.Permissions.Introspection.ToString())
                    {
                        newClient.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
                    }
                }


                await manager.CreateAsync(newClient);
                var response = new ApiSuccess()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Client insert successful."
                };
                return Ok(response);
            }
            throw new GlobalApplicationException("Client is already exist");
        }
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "admin")]
        [HttpGet("ClientAdmin")]
        public async Task<IActionResult> ClientAdmin()
        {
            return Ok("this is admin access");
        }
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Roles = "customer")]
        [HttpGet("ClientCustomer")]
        public async Task<IActionResult> ClientCustomer()
        {
            return Ok("this is Customer access");
        }
    }
}
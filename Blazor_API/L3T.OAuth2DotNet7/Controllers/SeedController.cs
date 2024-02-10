using System.Text;
using L3T.OAuth2DotNet7.CommonModel;
using L3T.OAuth2DotNet7.DataAccess;
using L3T.OAuth2DotNet7.DataAccess.RequestModel;
using L3T.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

namespace L3T.OAuth2DotNet7.Controllers
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

        [HttpGet("parseXml")]
        public async Task<object> parseXml()
        {
            var xmlData = "<?xml version=\"1.0\" encoding=\"utf-8\"?><xml><Successful_Message>Success !</Successful_Message><Ticket_ID>L3-23Aug2022-24300907</Ticket_ID>\r\n</xml>";

            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(xmlData);

            //var node = xmlDoc.GetElementsByTagName("Successful_Message")[0].InnerText;

            if (xmlData.Contains("Successful_Message"))
            {
                var lenghtofString = xmlData.Length;
                var newString = xmlData.Substring(104, lenghtofString - 104 - 20);
            }


            return null;
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("test")]
        public async Task<IActionResult> testHttp()
        {
            return Ok("Client credential");
        }
    }
}

public class httpTest
{
    public string RouterIp { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string CustomerIp { get; set; }
}
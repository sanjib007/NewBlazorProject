using System.Collections.Immutable;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;
using Azure.Core;
using L3T.OAuth2DotNet7.CommonModel;
using L3T.OAuth2DotNet7.DataAccess;
using L3T.OAuth2DotNet7.DataAccess.IdentityModels;
using L3T.OAuth2DotNet7.DataAccess.Model;
using L3T.OAuth2DotNet7.DataAccess.RequestModel;
using L3T.OAuth2DotNet7.Services.Implementation;
using L3T.OAuth2DotNet7.Services.Interface;
using L3T.Utility.Helper;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;

namespace L3T.OAuth2DotNet7.Controllers
{
    [ApiController]
    [Route(CommonHelper.ControllerRoute)]
    public class AuthorizationController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IdentityTokenServerDBContext _applicationDbContext;
        private static bool _databaseChecked;
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IThirdPartyHttpRequestService _thirdPartyHttpRequestService;
        private readonly ILogger<AuthorizationController> _logger;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRSAEncryptionSerivce _encryptionSerivce;
        private readonly IIdentityReauestResponseService _reqResService;


        public AuthorizationController(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IdentityTokenServerDBContext applicationDbContext,
            IOpenIddictApplicationManager applicationManager,
            IThirdPartyHttpRequestService thirdPartyHttpRequestService,
            ILogger<AuthorizationController> logger,
            IConfiguration config,
            IServiceProvider serviceProvider,
            IRSAEncryptionSerivce encryptionSerivce,
            IIdentityReauestResponseService reqResService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
            _applicationManager = applicationManager;
            _thirdPartyHttpRequestService = thirdPartyHttpRequestService;
            _logger = logger;
            _config = config;
            _serviceProvider = serviceProvider;
            _encryptionSerivce = encryptionSerivce;
            _reqResService = reqResService;
        }
        /*
            https://localhost:7023/api/connect/token
            content-type:  x-www-form-urlencoded
            username:admin@example.com
            password:L3T123..
            grant_type:password
            client_id:Test
            client_secret:test123
            scope:offline_access  for access_token + refresh_token 
            if not submit scope as a parametter then its gives only access_token
         */

        [HttpGet("rsaEncryp/{data}")]
        public async Task<string> RsaEncrypData(string data)
        {
            var changeData = await _encryptionSerivce.EncryptUsingCertificate(data);
            return changeData;
        }


        protected async Task<string> GetClientIPAddress()
        {
            var httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();
            var localIpAddress = httpConnectionFeature?.LocalIpAddress;
            var remoteIpAddress = httpConnectionFeature?.RemoteIpAddress;
            var ip = localIpAddress.ToString() + "/" + remoteIpAddress.ToString();
            return ip;
        }

        [HttpPost("~/api/connect/token"), Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var methodName = "AuthorizationController/Exchange";
            var ip = await GetClientIPAddress();
            var request = HttpContext.GetOpenIddictServerRequest();
            //_logger.LogInformation("Method Name: " + methodName + "Request: " + JsonConvert.SerializeObject(request));
            try
            {
                if (request.IsPasswordGrantType())
                {
                    var encryptCheck = new RSAEncryptDataDuplocationCheckModel()
                    {
                        UserName = request.Username.Replace(" ", "+"),
                        Password = request.Password.Replace(" ", "+"),
                        CreatedAt = DateTime.Now,
                    };

                    var checkResult = await _reqResService.AddRSAEncryptDataForDuplicationCheck(encryptCheck);
                    if (checkResult.StatusCode != 200 && checkResult.Status != "Success")
                    {
                        throw new Exception("Encrypted value is invalid.");
                    }

                    request.Username = await _encryptionSerivce.DecryptUsingCertificate(encryptCheck.UserName);
                    request.Password = await _encryptionSerivce.DecryptUsingCertificate(encryptCheck.Password);


                    AppUser user = new AppUser();
                    bool isAdLoginValid = false;
                    if (request.Username.Contains("@"))
                    {
                        user = await _userManager.FindByEmailAsync(request.Username);
                    }
                    else
                    {
                        user = await _userManager.FindByNameAsync(request.Username);
                    }

                    if (user == null)
                    {
                        user = await _thirdPartyHttpRequestService.GetUserInformation(request.Username, request.Password);
                        if (user == null)
                        {
                            var properties = new AuthenticationProperties(new Dictionary<string, string>
                            {
                                [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                                    OpenIddictConstants.Errors.InvalidGrant,
                                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                    "The username/password couple is invalid."
                            });
                            Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                            throw new Exception("The username/password couple is invalid.");
                        }
                    }

                    //if (user.IsLoginWithAD)
                    //{
                    //    var adip = _config.GetValue<string>("Server:AD");
                    //    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, adip))
                    //    {
                    //        isAdLoginValid = pc.ValidateCredentials(user.Email, request.Password);
                    //    }

                    //    if (!isAdLoginValid)
                    //    {
                    //        var properties = new AuthenticationProperties(new Dictionary<string, string>
                    //        {
                    //            [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                    //                OpenIddictConstants.Errors.InvalidGrant,
                    //            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                    //                "The username/password couple is invalid."
                    //        });

                    //        Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    //        throw new Exception("The username/password couple is invalid.");
                    //    }
                    //}
                    //else
                    //{
                    //    // Validate the username/password parameters and ensure the account is not locked out.
                    //    var result =
                    //        await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
                    //    if (!result.Succeeded)
                    //    {
                    //        var properties = new AuthenticationProperties(new Dictionary<string, string>
                    //        {
                    //            [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                    //                OpenIddictConstants.Errors.InvalidGrant,
                    //            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                    //                "The username/password couple is invalid."
                    //        });

                    //        Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                    //        throw new Exception("The username/password couple is invalid.");
                    //    }
                    //}

                    var userRoleList = await _userManager.GetRolesAsync(user);
                    var roleListString = string.Join(", ", userRoleList.ToArray());


                    // Create the claims-based identity that will be used by OpenIddict to generate tokens.
                    var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)
                        .AddClaim(OpenIddictConstants.Claims.Subject, await _userManager.GetUserIdAsync(user))
                        .AddClaim(OpenIddictConstants.Claims.Email, await _userManager.GetEmailAsync(user))
                        .AddClaim(OpenIddictConstants.Claims.Name, await _userManager.GetUserNameAsync(user))
                        .AddClaim("FullName", user.FullName)
                        .AddClaim("Designation", user.User_designation)
                        .AddClaim("Department", user.Department)
                        .AddClaim("PhoneNo", user.PhoneNumber)
                        .AddClaim("L3Id", user.Userid)
                        .AddClaims(OpenIddictConstants.Claims.Role, (await _userManager.GetRolesAsync(user)).ToImmutableArray());

                    // Set the list of scopes granted to the client application.
                    identity.SetScopes(new[]
                    {
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                    OpenIddictConstants.Scopes.OfflineAccess
                }.Intersect(request.GetScopes()));

                    var principal = new ClaimsPrincipal(identity);
                    //principal.SetScopes(tt.GetScopes());
                    //principal.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
                    principal.SetResources("Client1");
                    identity.SetDestinations(GetDestinations);

                    var newProperties = new AuthenticationProperties(
                        items: new Dictionary<string, string>(),
                        parameters: new Dictionary<string, object?>
                        {
                            ["roles"] = roleListString,
                            ["Email"] = user.Email,
                            ["FullName"] = user.FullName,
                            ["PhoneNo"] = user.PhoneNumber,
                            ["Subject"] = user.UserName,
                            ["Designation"] = user.User_designation,
                            ["Department"] = user.Department
                        });

                    return SignIn(new ClaimsPrincipal(identity), newProperties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }
                else if (request.IsRefreshTokenGrantType())
                {
                    // Retrieve the claims principal stored in the authorization code/device code/refresh token.
                    var principal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;

                    // Retrieve the user profile corresponding to the refresh token.
                    var test = principal.GetClaim(OpenIddictConstants.Claims.Subject);
                    var user = await _userManager.FindByIdAsync(test);
                    if (user == null)
                    {
                        var properties = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The refresh token is no longer valid."
                        });

                        Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                        throw new Exception("The refresh token is no longer valid.");
                    }

                    // Ensure the user is still allowed to sign in.
                    if (!await _signInManager.CanSignInAsync(user))
                    {
                        var properties = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                        });

                        Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                        throw new Exception("The user is no longer allowed to sign in.");
                    }
                    principal.SetResources("Client1");
                    principal.SetDestinations(GetDestinations);

                    var userRoleList = await _userManager.GetRolesAsync(user);
                    var roleListString = string.Join(", ", userRoleList.ToArray());

                    var newProperties = new AuthenticationProperties(
                        items: new Dictionary<string, string>(),
                        parameters: new Dictionary<string, object>
                        {
                            ["roles"] = roleListString,
                            ["Email"] = user.Email,
                            ["FullName"] = user.FullName,
                            ["PhoneNo"] = user.PhoneNumber,
                            ["Subject"] = user.UserName,
                            ["Designation"] = user.User_designation,
                            ["Department"] = user.Department
                        });

                    return SignIn(principal, newProperties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }
                else if (request.IsClientCredentialsGrantType())
                {
                    // Note: the client credentials are automatically validated by OpenIddict:
                    // if client_id or client_secret are invalid, this action won't be invoked.

                    var application = await _applicationManager.FindByClientIdAsync(request.ClientId);
                    if (application == null)
                    {
                        throw new Exception("The application details cannot be found in the database.");
                    }

                    // Create the claims-based identity that will be used by OpenIddict to generate tokens.
                    var identity = new ClaimsIdentity(
                        authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                        nameType: OpenIddictConstants.Claims.Name,
                        roleType: OpenIddictConstants.Claims.Role);

                    // Add the claims that will be persisted in the tokens (use the client_id as the subject identifier).
                    identity.AddClaim(OpenIddictConstants.Claims.Subject, await _applicationManager.GetClientIdAsync(application));
                    identity.AddClaim(OpenIddictConstants.Claims.Name, await _applicationManager.GetDisplayNameAsync(application));

                    // Note: In the original OAuth 2.0 specification, the client credentials grant
                    // doesn't return an identity token, which is an OpenID Connect concept.
                    //
                    // As a non-standardized extension, OpenIddict allows returning an id_token
                    // to convey information about the client application when the "openid" scope
                    // is granted (i.e specified when calling principal.SetScopes()). When the "openid"
                    // scope is not explicitly set, no identity token is returned to the client application.

                    // Set the list of scopes granted to the client application in access_token.
                    identity.SetScopes(request.GetScopes());
                    //identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
                    identity.SetResources("Client1");
                    identity.SetDestinations(GetDestinations);

                    return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }
                throw new Exception("The specified grant type is not implemented.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Method Name: " + methodName + "Error: " + JsonConvert.SerializeObject(ex) + "Request: " + JsonConvert.SerializeObject(request));
                return Ok(await _reqResService.CreateResponseRequest(request, ex, ip, methodName, request.Username, null, ex.Message));                
            }

        }


        private async Task testEencryp(string data)
        {
            var encryptdat = await _encryptionSerivce.EncryptUsingCertificate(data);
            var orginaldata = await _encryptionSerivce.DecryptUsingCertificate(encryptdat);
        }

        private static IEnumerable<string> GetDestinations(Claim claim)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            switch (claim.Type)
            {
                case OpenIddictConstants.Claims.Name:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (claim.Subject.HasScope(OpenIddictConstants.Permissions.Scopes.Profile))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                case OpenIddictConstants.Claims.Email:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (claim.Subject.HasScope(OpenIddictConstants.Permissions.Scopes.Email))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                case OpenIddictConstants.Claims.Role:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (claim.Subject.HasScope(OpenIddictConstants.Permissions.Scopes.Roles))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return OpenIddictConstants.Destinations.AccessToken;
                    yield break;
            }
        }

        [HttpPost("~/api/connect/authorize")]
        public async Task<IActionResult> Authorize()
        {
            return null;
        }

        #region Helpers

        // The following code creates the database and schema if they don't exist.
        // This is a temporary workaround since deploying database through EF migrations is
        // not yet supported in this release.
        // Please see this http://go.microsoft.com/fwlink/?LinkID=615859 for more information on how to do deploy the database
        // when publishing your application.
        private static void EnsureDatabaseCreated(IdentityTokenServerDBContext context)
        {
            if (!_databaseChecked)
            {
                _databaseChecked = true;
                context.Database.EnsureCreated();
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion

        [HttpPost("Registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            EnsureDatabaseCreated(_applicationDbContext);
            if (ModelState.IsValid)
            {
                var oldUser = await _userManager.FindByNameAsync(model.UserName);
                if (oldUser != null)
                {
                    return StatusCode(StatusCodes.Status409Conflict);
                }
                var user = await _thirdPartyHttpRequestService.GetUserInformation(model.UserName, model.Password);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status409Conflict);
                }
                var response = new ApiSuccess()
                {
                    Status = "Success.",
                    StatusCode = 200,
                    Message = "User Insert Successful.",
                    Data = user
                };
                return Ok(response);

                //    var user = new AppUser()
                //{
                //    UserName = model.UserName,
                //    Email = model.Email,
                //    SecurityStamp = Guid.NewGuid().ToString(),
                //    PhoneNumber = model.PhoneNumber
                //};
                //var result = await _userManager.CreateAsync(user, model.Password);
                //var result1 = await _userManager.AddToRoleAsync(user, model.Role);
                //if (result.Succeeded && result1.Succeeded)
                //{
                //var response = new ApiSuccess()
                //{
                //    Status = "Success.",
                //    StatusCode = 200,
                //    Message = "User Insert Successful."
                //};
                //return Ok(response);
                //}
                //AddErrors(result);
            }
            // If we got this far, something failed.
            throw new ApplicationException("Something is wrong");
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("~/api/connect/logout")]
        public async Task<IActionResult> Logout()
        {
            var userSub = User.GetClaimUserId();
            var oi_au_id = User.Claims.Where(c => c.Type == "oi_au_id").Select(c => c.Value).FirstOrDefault();
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityTokenServerDBContext>();
            var sql = "Update OpenIddictAuthorizations SET Status = 'invalid' Where Subject= '" + userSub + "' And id = '" + oi_au_id + "';";
            var res = await context.Database.ExecuteSqlRawAsync(sql);
            var sql1 = "Update OpenIddictTokens SET Status = 'invalid' Where Subject= '" + userSub + "' And AuthorizationId = '" + oi_au_id + "';";
            var res1 = await context.Database.ExecuteSqlRawAsync(sql1);

            if (res > 0 || res1 > 0)
            {
                return Ok(new ApiSuccess
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Logout Success"
                });
            }
            return BadRequest("Logout is not success");

        }
    }

}

using L3TIdentityServer.Models;
using L3TIdentityServer.Models.Account;
using L3TIdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace L3TIdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAccountService _accountService;
        public AccountController(UserManager<IdentityUser> userManager
            , SignInManager<IdentityUser> signInManager
            , RoleManager<IdentityRole> roleManager
            ,IAccountService accountService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _accountService = accountService;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin([FromBody] Register registerModel)
        {
            var userExists = await _userManager.FindByNameAsync(registerModel.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new IdentityUser()
            {
                Email = registerModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerModel.Username,
                PhoneNumber = registerModel.PhoneNumber
            };

            // Create user
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            // Checking roles in database and creating if not exists
            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            if (!await _roleManager.RoleExistsAsync(Roles.User))
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));

            // Add role to user
            if (!string.IsNullOrEmpty(registerModel.Role) && registerModel.Role == Roles.Admin)
            {
                await _userManager.AddToRoleAsync(user, Roles.Admin);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, Roles.User);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Login")]
        //public async Task<IActionResult> Login(Login user)
        //{
        //    var result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);

        //    if (result.Succeeded)
        //    {
        //        return Ok(new Response { Status = "Success", Message = "Login successfully!" });
        //    }

        //    //ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
        //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Invalid Login Attempt" });
        //}

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Login
        ///     {        
        ///       "username": "user1",
        ///       "password": "Abc$123456",
        ///       "RememberLogin" : false
        ///     }
        /// </remarks>
        /// <param name="LoginRequest" example="false">login request</param>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _accountService.LoginAsync(loginRequest);

            if (!response.Succeeded && response.PhoneNumberNotConfirmed)
            {
                return StatusCode((int)response.HttpStatusCode, new { PhoneNumberNotConfirmed = true, PhoneNumber = response.PhoneNumber });
            }
            else if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            
            return Ok(new Response { Status = "Success", Message = "Login successfully!" });
            //return Ok(response.AccessToken);
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok(new Response { Status = "Success", Message = "Logout successfully!" });
        }

        [HttpPost]
        //[Authorize(Policy = "IsAdmin")]
        [Route("AddRole")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var response = new Response()
            {
                Status = "Error",
                Message = "Role creation failed!"
            };

            if (string.IsNullOrEmpty(roleName))
            {
                //return response;
                return StatusCode(StatusCodes.Status204NoContent, response);
            }

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                response.Message = "Already exist";
                //return response;
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            await _roleManager.CreateAsync(new IdentityRole { Name = roleName });

            return Ok(new Response { Status = "Success", Message = "Role created successfully!" });
        }

        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/ChangePassword
        ///     { 
        ///     //user1
        ///         "currentPassword": "Abc$123456",
        ///         "newPassword": "string"
        ///     }
        /// </remarks>
        [HttpPost]
        //[Authorize]
        [Route("ChangePassword")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var response = await _accountService.ChangePasswordAsync(request, User.Identity.Name);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }

        /// <summary>
        /// ForgetPassword
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/ForgetPassword
        ///     {
        ///       "email": "string",
        ///       "username": "string"
        ///     }
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("ForgetPassword")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var response = await _accountService.GenerateForgetPasswordTokenAsync(request);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);

            var token = HttpUtility.UrlEncode(response.Message);

            var link = $@"
                    <a href='https://daralber.app/auth/reset-password-v2?token={token}&email={request.Email}'>
                        Click here to reset password
                    </a>";
            return Ok(new { link });
            //var res = await emailSendClient.GetResponse<MB_EmailSend>(new MB_EmailSend
            //{
            //    Body = link,
            //    EmailReceiver = request.Email,
            //    Subject = "Reset Password Daralber",
            //});

            //return Ok(new { IsSend = res.Message.IsSend });
        }

        /// <summary>
        /// ResetPassword
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/ResetPassword
        ///     {        
        ///       "password": "string",
        ///       "confirmPassword": "string",
        ///       "email": "string",
        ///       "token": "string"
        ///     }
        /// </remarks>
        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel request)
        {
            var response = await _accountService.ResetPasswordAsync(request);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }

        [HttpPost("ChangePhoneNumber")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePhoneNumber([FromBody] ChangePhoneNumberRequest request)
        {
            var response = await _accountService.ChangePhoneNumberAsync(request);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);

            return Ok(response);
        }

        [HttpPost("ConfirmPhoneNumber")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPhoneNumber(ConfirmPhoneNumberRequest request)
        {
            var response = await _accountService.ConfirmPhoneNumberAsync(request);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);

            return Ok(response);
        }

        /// <summary>
        /// ConfirmPhoneNumberToken
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/ConfirmPhoneNumberToken
        ///     {        
        ///       "PhoneNumber": "string",
        ///       "Username": "string"
        ///     }
        /// </remarks>
        [HttpPost("ConfirmPhoneNumberToken")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPhoneNumberToken(PhoneNumberTokenRequest request)
        {
            var response = await _accountService.GeneratePhoneNumberTokenAsync(request);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);

            return Ok(response);
        }

        /// <summary>
        /// ConfirmEmail
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/ConfirmEmail
        ///     {        
        ///       "token": "string",
        ///       "email": "string"
        ///     }
        /// </remarks>
        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var response = await _accountService.ConfirmEmailAsync(token, email);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves the user profile
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Account/Profile
        ///     {        
        ///       "userId": "1be1ca92-74a9-4ca7-9bb1-9130a007eb86"
        ///     }
        /// </remarks>
        /// <returns>The user profile</returns>
        [HttpGet("Profile")]
        //[Authorize(Policy = "NotCustomer")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var response = await _accountService.GetUserProfile(userId);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }

        [HttpGet("MyProfile")]
        [Authorize()]
        public async Task<IActionResult> GetMyProfile()
        {
            //var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
            var userId = _userManager.GetUserId(User);
            var response = await _accountService.GetUserProfile(userId);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }

        ///// <summary>
        ///// Updates the user's profile
        ///// </summary>
        ///// <remarks>
        ///// Sample request:
        ///// 
        /////     PATCH api/Account/Profile
        /////     {        
        /////       "firstName": "Davood",
        /////       "middleName": "string",
        /////       "lastName": "Pournabi",
        /////       "email": "Davood@gmail.com",
        /////       "gender": "Male",
        /////       "phoneNumber": "555444333",
        /////       "profileImageUrl": "NULL"
        /////     }
        ///// </remarks>
        //[HttpPost("Profile")]
        //[Authorize()]
        ////[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileRequest request)
        //{

        //    var response = await _accountService.UpdateUserProfileAsync(request, User.Identity.Name);
        //    if (!response.Succeeded)
        //        return StatusCode((int)response.HttpStatusCode, response.Message);
        //    return Ok(response);
        //}

        /// <summary>
        /// Retrieves the user roles
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Account/Profile
        ///     {
        ///       "userId": "1be1ca92-74a9-4ca7-9bb1-9130a007eb86"
        ///     }
        /// </remarks>
        /// <returns>The user roles list</returns>
        [HttpGet("Roles")]
        [Authorize]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var response = await _accountService.GetRolesByUserIdAsync(userId);
            if (!response.Succeeded)
                return StatusCode((int)response.HttpStatusCode, response.Message);
            return Ok(response);
        }
    }
}

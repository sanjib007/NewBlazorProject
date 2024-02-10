using L3TIdentityServer.Models.Account;
using L3TIdentityServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace L3TIdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AdministratorController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// AddUserToRole
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/AddUserToRole
        ///     {     
        ///       "Counter Staff"
        ///     }
        /// </remarks>
        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole([FromBody] string role)
        {
            try
            {
                var response = await accountService.AddUserToRoleAsync(role, User.Identity.Name);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An error occurred in AddUserToRole");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// RemoveUserFromRole
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/RemoveUserFromRole
        ///     { 
        ///       "Counter Staff"
        ///     }
        /// </remarks>
        [HttpPost("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] string role)
        {
            try
            {
                var response = await accountService.RemoveUserFromRoleAsync(role, User.Identity.Name);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An error occurred in RemoveUserFromRole");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// AddUserToRoles
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/AddUserToRoles
        ///     {        
        ///          "roles": ["Zaka Manager","Board Member"]
        ///     }
        /// </remarks>
        [HttpPost("AddUserToRoles")]
        public async Task<IActionResult> AddUserToRoles([FromBody] AddUserToRolesRequest request)
        {
            try
            {
                var response = await accountService.AddUserToRolesAsync(request, request.UserName);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An error occurred in AddUserToRoles");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// RemoveUserFromRoles
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/RemoveUserFromRoles
        ///     {        
        ///          "roles": ["Zaka Manager","Board Member"]
        ///     }
        /// </remarks>
        [HttpPost("RemoveUserFromRoles")]
        public async Task<IActionResult> RemoveUserFromRoles([FromBody] AddUserToRolesRequest request)
        {
            try
            {
                var response = await accountService.RemoveUserFromRolesAsync(request, request.UserName);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An error occurred in RemoveUserFromRoles");
                return StatusCode(500);
            }
        }
                

        /// <summary>
        /// GetUserClaims
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/GetUserClaims
        ///     {        
        ///       "pageIndex": 0,
        ///       "pageSize": 0,
        ///       "userName": "string",
        ///       "claimName": "",
        ///       "claimType": "",
        ///       "pagingMode": false
        ///     }
        /// </remarks>
        [HttpPost("GetUserClaims")]
        public async Task<IActionResult> GetUserClaims(GetUserClaimsRequest request)
        {
            try
            {
                var response = await accountService.GetUserClaimsAsync(request);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An error occurred in GetUserClaims");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GetUserRoles
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/GetUserRoles"
        ///     {        
        ///       "pageIndex": 0,
        ///       "pageSize": 0,
        ///       "userName": "user1",
        ///       "roleName": "",
        ///       "pagingMode": false
        ///     }
        /// </remarks>
        [HttpPost("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(GetUserRolesRequest request)
        {
            try
            {
                var response = await accountService.GetUserRolesAsync(request);
                if (!response.Succeeded)
                    return StatusCode((int)response.HttpStatusCode, response.Message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An error occurred in GetUserRoles");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// GetRoleUsers
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Account/Administrator/GetRoleUsers
        ///     {        
        ///       "pageIndex": 0,
        ///       "pageSize": 0,
        ///       "roleName": "admin",
        ///       "userName": "",
        ///       "pagingMode": false
        ///     }
        /// </remarks>
        [HttpPost("GetRoleUsers")]
        public async Task<IActionResult> GetRoleUsers(GetRoleUsersRequest request)
        {
            try
            {
                var response = await accountService.GetRoleUsersAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An error occurred in GetRoleUsers");
                return StatusCode(500);
            }
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                return Ok(await accountService.GetAllUsersAsync());
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An error occurred in GetAllUsers");
                return StatusCode(500);
            }
        }

        //[HttpPost("GetPagingUsers")]
        //public async Task<IActionResult> GetPagingUsers(GetUserRequest request) => Ok(await accountService.GetAllUsersAsync(request));

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                return Ok(await accountService.GetAllRolesAsync());
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An error occurred in GetAllRoles");
                return StatusCode(500);
            }
        }

        [HttpGet("GetAllUserClaims")]
        public async Task<IActionResult> GetAllUserClaims()
        {
            try
            {
                return Ok(await accountService.GetAllUserClaimsAsync());
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An error occurred in GetAllUserClaims");
                return StatusCode(500);
            }
        }

        [HttpGet("GetAllRoleClaims")]
        public async Task<IActionResult> GetAllRoleClaims()
        {
            try
            {
                return Ok(await accountService.GetAllRoleClaimsAsync());
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "An error occurred in GetAllRoleClaims");
                return StatusCode(500);
            }
        }

        [Route("GetPagingRoles")]
        [HttpPost]
        public async Task<IActionResult> GetPagingRoles(GetRoleRequest request) => Ok(await accountService.GetAllRolesAsync(request));


        [HttpPost("AddNewRole")]
        public async Task<IActionResult> AddNewRole(string roleName)
        {
            return Ok(await accountService.AddRole(roleName));
        }

        [HttpPost("EditRole")]
        public async Task<IActionResult> AddNewRole(EditRoleNameRequest req)
        {
            return Ok(await accountService.EditRole(req));
        }

        [HttpPost("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            return Ok(await accountService.DeleteRole(id));
        }

        [HttpPost("CreateUserByAdmin")]
        public async Task<ActionResult> CreateUser(CreateUserByAdminRequest request)
        {
            var res = await accountService.CreateUserAsync(request);
            if (res.Succeeded)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
    }
}

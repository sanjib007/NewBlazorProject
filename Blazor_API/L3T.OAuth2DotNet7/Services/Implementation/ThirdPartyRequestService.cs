using Azure.Core;
using L3T.OAuth2DotNet7.DataAccess;
using L3T.OAuth2DotNet7.DataAccess.IdentityModels;
using L3T.OAuth2DotNet7.DataAccess.ViewModel;
using L3T.OAuth2DotNet7.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Polly;
using System.Security.Cryptography.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace L3T.OAuth2DotNet7.Services.Implementation
{
    public class ThirdPartyRequestService : IThirdPartyHttpRequestService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ThirdPartyRequestService> _logger;
        private readonly MisDBContext _misDBContext;
        private readonly IdentityTokenServerDBContext _identityDbContext;
        private readonly RoleManager<AppRoles> _roleManager;
        private readonly IIdentityReauestResponseService _reqResService;


        public ThirdPartyRequestService(

            UserManager<AppUser> userManager,
            ILogger<ThirdPartyRequestService> logger,
            MisDBContext misDBContext,
            IdentityTokenServerDBContext identityDbContext,
            RoleManager<AppRoles> roleManager,
            IIdentityReauestResponseService reqResService
            )
        {

            _userManager = userManager;
            _logger = logger;
            _misDBContext = misDBContext;
            _identityDbContext = identityDbContext;
            _roleManager = roleManager;
            _reqResService = reqResService;
        }

        public async Task<AppUser> GetUserInformation(string id, string pass)
        {
            var methodName = "ThirdPartyRequestService/GetUserInformation";
            try
            {
                var appUser = new AppUser();
                var userInfoResult = await _misDBContext.UserProfileInformation.FromSqlRaw("exec UserProfileInformation {0}", id).ToListAsync();// .IgnoreQueryFilters().FirstOrDefaultAsync();
                var userInfo = userInfoResult.FirstOrDefault();


                if (userInfo != null)
                {
                    var setRole = await IsValidRole(userInfo.userid);

                    appUser.FullName = userInfo.Emp_Mas_First_Name + " " + userInfo.Emp_Mas_Last_Name;
                    appUser.Email = userInfo.user_email;
                    appUser.PhoneNumber = string.IsNullOrEmpty(userInfo.Emp_Mas_HandSet) ? userInfo.Emp_Mas_Homephone : userInfo.Emp_Mas_HandSet;
                    appUser.UserName = userInfo.userid;
                    appUser.SecurityStamp = Guid.NewGuid().ToString();
                    appUser.IsLoginWithAD = true;
                    appUser.CreatedAt = DateTime.Now;
                    appUser.CreatedBy = "System";
                    appUser.IsActive = true;

                    appUser.Userid = userInfo.userid;
                    appUser.User_name = userInfo.user_name;
                    appUser.User_email = userInfo.user_email;
                    appUser.User_designation = userInfo.user_designation;
                    appUser.Department = userInfo.department;
                    appUser.Status = userInfo.status.ToString();
                    appUser.Resign_date = userInfo.resign_date.ToString();
                    appUser.First_Name = userInfo.Emp_Mas_First_Name;
                    appUser.Last_Name = userInfo.Emp_Mas_Last_Name;
                    appUser.Father_Name = userInfo.Emp_Mas_Father_Name;
                    appUser.Mother_Name = userInfo.Emp_Mas_Mother;
                    appUser.Address = userInfo.Emp_Mas_Address;
                    appUser.City = userInfo.Emp_Mas_City;
                    appUser.State = userInfo.Emp_Mas_State;
                    appUser.Post_Code = userInfo.Emp_Mas_Post_Code;
                    appUser.Country = userInfo.Emp_Mas_Country;
                    appUser.Permanent_Address = userInfo.emp_mas_Perm_Addr;
                    appUser.Permanent_City = userInfo.emp_mas_Perm_City;
                    appUser.Permanent_state = userInfo.emp_mas_Perm_state;
                    appUser.Permanent_Post = userInfo.emp_mas_Perm_Post;
                    appUser.Permanent_Country = userInfo.Emp_Mas_Perm_Country;
                    appUser.Religion = userInfo.Emp_Mas_Religion;
                    appUser.DOB = userInfo.Emp_Mas_DOB.ToString();
                    appUser.MaritalStatus = userInfo.Emp_Mas_MaritalStatus;
                    appUser.Children = userInfo.Emp_Mas_No_Children.ToString();
                    appUser.Gender = userInfo.Emp_Mas_Gender;
                    appUser.Blood_Group = userInfo.Emp_Mas_Bloodgrp;
                    appUser.Homephone = userInfo.Emp_Mas_Homephone;
                    appUser.Workphone = userInfo.Emp_Mas_Workphone;
                    appUser.HandSet = userInfo.Emp_Mas_HandSet;
                    appUser.Join_Date = userInfo.Emp_Mas_Join_Date.ToString();
                    appUser.Confrim_Date = userInfo.Emp_Mas_Confrim_Date.ToString();
                    appUser.TIN = userInfo.Emp_Mas_TIN;
                    appUser.Section = userInfo.Sect;
                    appUser.Office = userInfo.Office;
                    appUser.WorkLocation = userInfo.WorkLocation;


                    var success = await _userManager.CreateAsync(appUser, pass);
                    if (!success.Succeeded)
                    {
                        throw new Exception("user is not inserted. " + success.Errors.Select(x => x.Description).FirstOrDefault());
                    }                    

                    var roleAdded = await _userManager.AddToRolesAsync(appUser, setRole);
                    if (!roleAdded.Succeeded)
                    {
                        throw new Exception("Role is not inserted. " + roleAdded.Errors.Select(x => x.Description).FirstOrDefault());
                    }

                    return appUser;
                }

                throw new Exception("User information is not found from MIS.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Method Name: " + methodName + "Error: " + JsonConvert.SerializeObject(ex) + "Request: " + JsonConvert.SerializeObject(id));
                await _reqResService.CreateResponseRequest("id : "+ id, ex, null, methodName, id, null, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<string>> IsValidRole(string l3Id)
        {
            var strRoleList = new List<string>();

            var getPreRole = await _identityDbContext.EmployeePreAssignRoles.FirstOrDefaultAsync(x => x.EmployeeId == l3Id);

            if (getPreRole != null)
            {
                strRoleList.Add(getPreRole.AssignRole);
            }
            else
            {
                //strRoleList.Add("User");
                throw new Exception("You can't login. Please contact your Administrator.");
            }

            var isHave = await _roleManager.RoleExistsAsync(strRoleList[0]);
            if (!isHave)
            {
                throw new ApplicationException("This role is not exist.");
            }
            return strRoleList;
        }

    }
}

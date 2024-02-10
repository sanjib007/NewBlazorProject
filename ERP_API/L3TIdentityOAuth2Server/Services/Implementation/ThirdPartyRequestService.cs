using L3TIdentityOAuth2Server.CommonModel;
using L3TIdentityOAuth2Server.DataAccess;
using L3TIdentityOAuth2Server.DataAccess.IdentityModels;
using L3TIdentityOAuth2Server.DataAccess.ViewModel;
using L3TIdentityOAuth2Server.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Polly;
using System.Security.Cryptography.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace L3TIdentityOAuth2Server.Services.Implementation
{
    public class ThirdPartyRequestService : IThirdPartyHttpRequestService
    {
        private readonly IdentityTokenServerDBContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRoles> _roleManager;
        private readonly ILogger<ThirdPartyRequestService> _logger;
        private readonly MisDBContext _misDBContext;
        public ThirdPartyRequestService(
            IHttpClientFactory httpClientFactory,
            IdentityTokenServerDBContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRoles> roleManager,
            ILogger<ThirdPartyRequestService> logger,
            MisDBContext misDBContext
            )
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _misDBContext = misDBContext;
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
                    var strRoleList = new List<string>() { "Admin" };
                    var roleAdded = await _userManager.AddToRolesAsync(appUser, strRoleList);
                    if (!roleAdded.Succeeded)
                    {
                        throw new Exception("Role is not inserted. "+ roleAdded.Errors.Select(x => x.Description).FirstOrDefault());
                    }

                    return appUser;
                }

                throw new Exception("User information is not found from.");
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                return null;
            }
        }

        public async Task storeAppInformation(string info, string userId)
        {
            var methodName = "ThirdPartyRequestService/storeAppInformation";
            try
            {
                AppInfoModel data = JsonConvert.DeserializeObject<AppInfoModel>(info);
                if (data != null)
                {
                    data.UserId = userId;
                    data.CreatedAt = DateTime.Now;
                    await _context.AppInfos.AddAsync(data);
                    await _context.SaveChangesAsync();                    
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
            }
        }


        private async Task<string> errorMethord(Exception ex, string info)
        {
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Method Name : " + info + ", Exception " + errormessage);
            //await _systemService.ErrorLogEntry(info, info, errormessage);
            //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
            return errormessage;
        }
    }
}

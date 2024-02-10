using System.Linq.Expressions;
using AutoMapper;
using L3T.OAuth2DotNet7.CommonModel;
using L3T.OAuth2DotNet7.DataAccess;
using L3T.OAuth2DotNet7.DataAccess.IdentityModels;
using L3T.OAuth2DotNet7.DataAccess.Model;
using L3T.OAuth2DotNet7.DataAccess.RequestModel;
using L3T.OAuth2DotNet7.DataAccess.ViewModel;
using L3T.OAuth2DotNet7.Helper;
using L3T.OAuth2DotNet7.Pagination;
using L3T.OAuth2DotNet7.Pagination.Helper;
using L3T.OAuth2DotNet7.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Polly;

public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<AppRoles> _roleManager;
    private readonly IdentityTokenServerDBContext _context;
    private readonly IUriService _uriService;
    private readonly IMapper _mapper;
    private readonly LNKDBContext _lnkDbContext;
    public AccountService(IdentityTokenServerDBContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRoles> roleManager, IUriService uriService, IMapper mapper, LNKDBContext lnkDbContext)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _uriService = uriService;
        _mapper = mapper;
        _lnkDbContext = lnkDbContext;
    }

    public async Task<ApiResponse> GetAllDepartment()
    {
        string strSql = "select Department from [L3TIdentityDB].[dbo].[AspNetUsers] where Department <> 'System' group by Department ";
        var allDepartment = await _context.GetAllDepartment.FromSqlRaw(strSql).Select(x => x.Department.Trim()).ToListAsync();
        var response = new ApiResponse()
        {
            Status = "success",
            StatusCode = 200,
            Data = allDepartment,
            Message = "Ok"
        };
        return response;
    }

    public async Task<ApiResponse> GetAllDepartmentWiseEmployee(string department)
    {
        string strSql = $@"select CONCAT(UserName, ' - ', FullName, ' - ', Email) AS Employee from [L3TIdentityDB].[dbo].[AspNetUsers] where Department = '{department}'";
        var allEmployee = await _context.GetAllEmployee.FromSqlRaw(strSql).Select(x => x.Employee.Trim()).ToListAsync();
		var response = new ApiResponse()
		{
			Status = "success",
			StatusCode = 200,
			Data = allEmployee,
			Message = "Ok"
		};
		return response;
	}

    public async Task<List<AppUserModel>> GetAllEmployee()
    {
        var sql = $@"select u.Id, u.FullName, u.UserName, u.Department, u.User_designation, u.Email, u.PhoneNumber, r.Name as RoleName from AspNetUsers u 
                    inner join AspNetUserRoles ur ON ur.UserId = u.Id
                    inner join AspNetRoles r ON r.Id = ur.RoleId";
        return await _context.AppUserView.FromSqlRaw(sql).ToListAsync();
    }

    public async Task<ApiSuccess> ChangePasswordAsync(ChangePasswordRequestModel request, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new GlobalApplicationException("User not found.");
        }
        var isPasswordValid =
            await _signInManager.CheckPasswordSignInAsync(user, request.CurrentPassword, lockoutOnFailure: true);
        if (!isPasswordValid.Succeeded)
        {
            throw new GlobalApplicationException("Password is not match.");
        }
        var changePassword = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (changePassword.Succeeded)
        {
            var response = new ApiSuccess()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Password is changed."
            };
            return response;
        }
        throw new GlobalApplicationException("Somthing is worng.");
    }
    
    public async Task<ApiSuccess> GenerateForgetPasswordTokenAsync(ForgetPasswordRequestModel request)
    {
        var response = new ApiSuccess();
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email.Trim());
            if (user == null)
            {
                throw new GlobalApplicationException("User is not found");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
            response.Status = "Success";
            response.StatusCode = 200;
            response.Data = new
            {
                Token = token,
                Email = request.Email
            };
            return response;
            
        }
        catch (Exception ex)
        {
            throw new GlobalApplicationException(ex.Message);
        }

        return response;
    }

    public async Task<ApiSuccess> ResetPasswordAsync(ResetPasswordModel requestModel)
    {
        var response = new ApiSuccess();
        var user = await _userManager.FindByEmailAsync(requestModel.Email.Trim());
        if (user != null)
        {
            var result = await _userManager.ResetPasswordAsync(user, requestModel.Token, requestModel.NewPassword);
            if (result.Succeeded)
            {
                response.Status = "Success";
                response.StatusCode = 200;
                response.Message = "Password change successful";
                return response;
            }
            
            throw new GlobalApplicationException(result.Errors.Select(x=> x.Description).FirstOrDefault());
        }
        throw new GlobalApplicationException("User infromation is not found.");
    }

    public async Task<ApiSuccess> ChangePhoneNumberAsync(ChangePhoneNumberRequestModel requestModel, string userId)
    {
        var response = new ApiSuccess();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new GlobalApplicationException("User information is not found.");
        }
        
        //Generate phone token
        await _userManager.SetPhoneNumberAsync(user, user.PhoneNumber);    
        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
        //logger.Debug("PhoneNumber is:" + user.PhoneNumber + "-" + token);
        ////Send token via SMS
        //if (!string.IsNullOrEmpty(token) && false)
        //{
        //    var res = await messageSendClient.GetResponse<MB_MessageSend>(new MB_MessageSend
        //    {
        //        Body = $"Daralber Verification Code: {token}",
        //        Receiver = user.PhoneNumber,
        //        UniCode = "UTF"
        //    });
        //    //logger.Debug("res:" + request.Password);
        //    response.Succeeded = res?.Message?.IsSent ?? false;

        //}
        
        response.Status = "Success";
        response.StatusCode = 200;
        response.Message = "Send token";
        response.Data = new
        {
            Token = token,
            PhoneNumber = requestModel.PhoneNumber
        };
        return response;
    }

    public async Task<ApiSuccess> ConfirmPhoneNumberAsync(ConfirmPhoneNumberRequestModel requestModel, string userId)
    {
        var response = new ApiSuccess();


        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new GlobalApplicationException("User information is not found.");
        }
        
        var res = await _userManager.ChangePhoneNumberAsync(user, requestModel.PhoneNumber, requestModel.Token);
        if (!res.Succeeded)
        {
            throw new GlobalApplicationException(res.Errors.Select(x=> x.Description).FirstOrDefault());
        }

        response.Status = "Success";
        response.StatusCode = 200;
        response.Message = "Phone number change is successful.";
        return response;
    }

    public async Task<ApiSuccess> GeneratePhoneNumberTokenAsync(ChangePhoneNumberRequestModel requestModel, string userId)
    {
        var response = new ApiSuccess();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new GlobalApplicationException("User information is not found.");
        }
        
        //Generate phone token
        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
        //logger.Debug("PhoneNumber is:" + user.PhoneNumber + "-" + token);
        ////Send token via SMS
        //if (!string.IsNullOrEmpty(token) && false)
        //{
        //    var res = await messageSendClient.GetResponse<MB_MessageSend>(new MB_MessageSend
        //    {
        //        Body = $"Daralber Verification Code: {token}",
        //        Receiver = user.PhoneNumber,
        //        UniCode = "UTF"
        //    });
        //    //logger.Debug("res:" + request.Password);
        //    response.Succeeded = res?.Message?.IsSent ?? false;

        //}
        
        response.Status = "Success";
        response.StatusCode = 200;
        response.Message = "Send token";
        response.Data = new
        {
            Token = token,
            PhoneNumber = requestModel.PhoneNumber
        };
        return response;
    }

    public async Task<ApiSuccess> ChangeEmailAsync(ChangeEmailRequestModel requestModel, string userId)
    {
        var response = new ApiSuccess();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new GlobalApplicationException("User information is not found.");
        }
        
        //Generate Email token
        var token = await _userManager.GenerateChangeEmailTokenAsync(user, user.PhoneNumber);
        
        response.Status = "Success";
        response.StatusCode = 200;
        response.Message = "Send token";
        response.Data = new
        {
            Token = token,
            Email = requestModel.Email
        };
        return response;
    }

    public async Task<ApiSuccess> ChangeEmailConfirmWithToken(ConfirmEmailRequestModel requestModel, string userId)
    {
        var response = new ApiSuccess();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new GlobalApplicationException("User information is not found.");
        }
        
        var res = await _userManager.ChangeEmailAsync(user, requestModel.Email, requestModel.Token);
        if (!res.Succeeded)
        {
            throw new GlobalApplicationException(res.Errors.Select(x=> x.Description).FirstOrDefault());
        }

        response.Status = "Success";
        response.StatusCode = 200;
        response.Message = "Phone number change is successful.";
        return response;
    }

    public async Task<ApiSuccess> GetProfile(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new GlobalApplicationException("User id can't be empty");
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new GlobalApplicationException("User information is not found");
        }
        user.PasswordHash = null;
        return new ApiSuccess()
        {
            Status = "Success",
            StatusCode = 200,
            Message = "User Information.",
            Data = user
        };
    }

    public async Task<ApiSuccess> GetRolesByUserIdAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new GlobalApplicationException("User id can't be empty");
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new GlobalApplicationException("User infromation is not found.");
        }
        var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        var response = new ApiSuccess()
        {
            Status = "Success",
            StatusCode = 200,
            Message = "Role information",
            Data = roles
        };
        return response;
    }

    public async Task<ApiSuccess> AddUserToRoleAsync(string role, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new GlobalApplicationException("User information is not found.");
        }

        var assignRole = await _userManager.GetRolesAsync(user);
        var getRole = assignRole.FirstOrDefault(x => x == role);
        if (!string.IsNullOrEmpty(getRole))
        {
            throw new GlobalApplicationException("This role already assigned.");
        }
        var result = await _userManager.AddToRoleAsync(user, role);
        if (result.Succeeded)
        {
            return new ApiSuccess()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Role added."
            };
        }

        throw new GlobalApplicationException(result.Errors.Select(x=> x.Description).FirstOrDefault());
    }

    public async Task<ApiSuccess?> RemoveUserFromRoleAsync(string role, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new GlobalApplicationException("User infromation is not found.");
        }
        var result = await _userManager.RemoveFromRoleAsync(user, role);
        if (result.Succeeded)
        {
            return new ApiSuccess()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Role removed."
            };
        }

        throw new GlobalApplicationException(result.Errors.Select(x=> x.Description).FirstOrDefault());
    }

    public async Task<ApiSuccess> AddUserToRolesAsync(AddUserToRolesRequest request, string requestUserName)
    {
        var user = await _userManager.FindByNameAsync(requestUserName);
        if (user == null)
        {
            throw new GlobalApplicationException("User infromation is not found.");
        }

        var errorList = new List<string>();
        var newRoleList = new List<string>();
        var assignRole = await _userManager.GetRolesAsync(user);
        foreach (var aRole in request.roles)
        {
            var getRole = assignRole.FirstOrDefault(x => x == aRole);
            if (!string.IsNullOrEmpty(getRole))
            {
                errorList.Add(aRole+ " role already assigned.");
            }
            else
            {
                newRoleList.Add(aRole);
            }
        }

        if (errorList.Count > 0)
        {
            throw new GlobalApplicationException(string.Join(',', errorList.Select(a => a).ToList()));
        }
        
        
        var result = await _userManager.AddToRolesAsync(user, newRoleList);
        if (result.Succeeded)
        {
            return new ApiSuccess()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Role added."
            };
        }

        throw new GlobalApplicationException(result.Errors.Select(x=> x.Description).FirstOrDefault());
    }

    public async Task<ApiSuccess> RemoveUserFromRolesAsync(AddUserToRolesRequest request, string requestUserName)
    {
        var user = await _userManager.FindByNameAsync(requestUserName);
        if (user == null)
        {
            throw new GlobalApplicationException("User infromation is not found.");
        };

        var errorList = new List<string>();
        var getAllRoles = await _roleManager.Roles.ToListAsync();
        foreach (var aRole in request.roles)
        {
            var getARole = getAllRoles.FirstOrDefault(x => x.Name == aRole);
            if (getARole == null)
            {
                errorList.Add("Role "+ aRole + " does not exist");
            }
        }

        if (errorList.Count > 0)
        {
            throw new GlobalApplicationException(string.Join(",", errorList.Select(x => x).ToList()));
        }

        var result = await _userManager.RemoveFromRolesAsync(user, request.roles);
        if (result.Succeeded)
        {
            return new ApiSuccess()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Role removed."
            };
        }

        throw new GlobalApplicationException(result.Errors.Select(x => x.Description).FirstOrDefault());
    }

    public async Task<ApiSuccess> GetUserClaimsAsync(GetUserClaimsRequestModel request)
    {
       
        var user = await _userManager.FindByNameAsync(request.UserName).ConfigureAwait(false);
        if (user != null)
        {
            var claims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(request.ClaimName))
            {
                claims = claims.Where(x => x.Value == request.ClaimName).ToList();
            }

            if (!string.IsNullOrEmpty(request.ClaimType))
            {
                claims = claims.Where(x => x.Type == request.ClaimType).ToList();
            }

            return new ApiSuccess()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Claim",
                Data = claims
            };
        }

        throw new GlobalApplicationException("User infromation is not found.");
    }

    public async Task<ApiSuccess> GetUserRolesAsync(GetUserRolesRequestModel request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName).ConfigureAwait(false);
        if (user != null)
        {
            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(request.RoleName))
            {
                roles = roles.Where(x => x.Contains(request.RoleName)).ToList();
            }

            return new ApiSuccess()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Roles",
                Data = roles
            };
        }

        throw new GlobalApplicationException("User information not found.");
    }

    public async Task<ApiSuccess> GetAllUsersAsync(GetUserWithFilter model, string route)
    {
        var expr = PredicateBuilder.True<AppUser>();
        var original = expr;
        if (!string.IsNullOrEmpty(model.Email))
        {
            expr = expr.And(x => x.Email == model.Email);
        }
        if (!string.IsNullOrEmpty(model.UserName))
        {
            expr = expr.And(x => x.UserName == model.UserName); //model.UserName);
        }
        if (!string.IsNullOrEmpty(model.FullName))
        {
            expr = expr.And(x => x.FullName.Contains(model.FullName));
        }
        if (!string.IsNullOrEmpty(model.PhoneNumber))
        {
            expr = expr.And(x => x.PhoneNumber == model.PhoneNumber);
        }
        if (model.PhoneNumberConfirm != null)
        {
            expr = expr.And(x => x.PhoneNumberConfirmed == model.PhoneNumberConfirm);
        }
        if (model.EmailConfirm != null)
        {
            expr = expr.And(x => x.EmailConfirmed == model.EmailConfirm);
        }
        if (model.IsActive != null)
        {
            expr = expr.And(x => x.IsActive == model.IsActive);
        }
        if (expr == original)
        {
            expr = x => true;
        }
        
        //var totalRecords = await _context.Users.Where(x=> x.UserName == model.UserName).CountAsync();
        var totalRecords = await _context.Users.Where(expr).CountAsync();

        var pagedData = await _context.Users.Where(expr)
            .Skip((model.PageNumber - 1) * model.PageSize)
            .Take(model.PageSize)
            .ToListAsync();
        
        var newList = _mapper.Map<List<AppUserViewModel>>(pagedData);
        foreach (var aData in newList)
        {
            var appUserModel = _mapper.Map<AppUser>(aData);
            aData.PasswordHash = null;
            var roleList = await _userManager.GetRolesAsync(appUserModel);
            aData.RoleName = roleList.ToList();
        }
        var pagedReponse = PaginationHelper.CreatePagedReponse<AppUserViewModel>(newList, model, totalRecords, _uriService, route);
        return new ApiSuccess()
        {
            Status = "Success",
            StatusCode = 200,
            Message = "All user list",
            Data = pagedReponse
        };
    }
    public async Task<List<string>> GetAllUsersForSearchStrListAsync(string searchText)
    {
        var sql = $@"select u.Id, u.FullName, u.UserName, u.Department, u.User_designation, u.Email, u.PhoneNumber, r.Name as RoleName from AspNetUsers u 
                    inner join AspNetUserRoles ur ON ur.UserId = u.Id
                    inner join AspNetRoles r ON r.Id = ur.RoleId
                    where (u.UserName LIKE '%{searchText}%' OR u.FullName LIKE '%{searchText}%' OR u.Email LIKE '%{searchText}%')";
        var totalRecords = await _context.AppUserView.FromSqlRaw(sql).ToListAsync();
        return totalRecords.Select(x=> x.fullName +"/"+x.userName+"/"+x.email+"/"+x.RoleName).ToList();
    }
    public async Task<ApiResponse> GetAllUsersForSearchAsync(string searchText)
	{
        var sql = $@"select TOP (1) u.Id, u.FullName, u.UserName, u.Department, u.User_designation, u.Email, u.PhoneNumber, r.Name as RoleName from AspNetUsers u 
                    inner join AspNetUserRoles ur ON ur.UserId = u.Id
                    inner join AspNetRoles r ON r.Id = ur.RoleId
                    where (u.UserName LIKE '%{searchText}%' OR u.FullName LIKE '%{searchText}%' OR u.Email LIKE '%{searchText}%')";

		var totalRecords = await _context.AppUserView.FromSqlRaw(sql).ToListAsync();
        var response = new ApiResponse()
        {
            Status = "Success",
            StatusCode = 200,
            Message = "OK",
            Data = totalRecords
        };
        return response;
	}

	public async Task<ApiSuccess> GetAllRolesAsync()
    {
        var getAllRoles = await _roleManager.Roles.ToListAsync();
        if (getAllRoles.Count <= 0)
        {
            throw new GlobalApplicationException("Role list is empty");
        }
        return new ApiSuccess()
        {
            Status = "Success",
            StatusCode = 200,
            Message = "All roles",
            Data = getAllRoles
        };
    }

    public Task<ApiSuccess> GetAllUserClaimsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ApiSuccess> GetAllRoleClaimsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ApiSuccess> EditRole(EditRoleNameRequestModel req)
    {
        if (await _roleManager.RoleExistsAsync(req.RoleName))
        {
            throw new GlobalApplicationException("This role already exist");
        }

        var role = await _roleManager.FindByIdAsync(req.Id);
        role.Name = req.RoleName;
        var result = await _roleManager.UpdateAsync(role);
        if (result.Succeeded)
        {
            return new ApiSuccess()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Role update successfully."
            };
        }

        throw new GlobalApplicationException(result.Errors.Select(x => x.Description).FirstOrDefault());

    }

    public async Task<ApiSuccess> DeleteRole(string id)
    {
        var getRole = await _roleManager.FindByIdAsync(id);
        if (getRole == null)
        {
            throw new GlobalApplicationException("Role dose not exist");
        }
        var result = await _roleManager.DeleteAsync(getRole);
        if (result.Succeeded)
        {
            return new ApiSuccess()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "Role Delete Successfully."
            };
        }
        throw new GlobalApplicationException(result.Errors.Select(x => x.Description).FirstOrDefault());
    }

    public async Task<ApiSuccess> UpdateUserInfo(string id, UpdateUserRequestModel model, string userId)
    {
        var oldUser = await _userManager.FindByIdAsync(id);
        if (oldUser == null)
        {
            throw new GlobalApplicationException("User is not found.");
        }

        if (!string.IsNullOrEmpty(model.Email) && oldUser.Email != model.Email)
        {
            var isEmailExist = await _userManager.FindByEmailAsync(model.Email);
            if (isEmailExist != null)
            {
                throw new GlobalApplicationException("User Email already exist!.");
            }
        }
        
        if (!string.IsNullOrEmpty(model.PhoneNumber) && oldUser.PhoneNumber != model.PhoneNumber)
        {
            var isPhoneNumberExist = await _context.Users.FirstOrDefaultAsync(x=> x.Email == model.Email);
            if (isPhoneNumberExist != null)
            {
                throw new GlobalApplicationException("User phone number already exist!.");
            }
        }

        

        oldUser.FullName = model.FullName;
        oldUser.Email = model.Email;
        oldUser.EmailConfirmed = model.EmailConfirmed;
        oldUser.PhoneNumber = model.PhoneNumber;
        oldUser.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
        oldUser.IsActive = model.IsActive;
        oldUser.LastUpdatedAt = DateTime.Now;
        oldUser.LastUpdatedBy = userId;
        
        var result = await _userManager.UpdateAsync(oldUser);

        if (result.Succeeded)
        {
            var roleList = await _userManager.GetRolesAsync(oldUser);
            foreach (var aRole in model.RoleName)
            {
                var IsExistRole = roleList.FirstOrDefault(x => x == aRole);
                if (string.IsNullOrEmpty(IsExistRole))
                {
                    await _userManager.AddToRoleAsync(oldUser, aRole);
                }
            }
            
            var response = new ApiSuccess()
            {
                Status = "Success.",
                StatusCode = 200,
                Message = "User update successful."
            };
            return response;
        }
        throw new GlobalApplicationException(result.Errors.Select(x => x.Description).FirstOrDefault());
    }

    public async Task<ApiSuccess> DeleteUserInfo(string id)
    {
        var oldUser = await _userManager.FindByIdAsync(id);
        if (oldUser == null)
        {
            throw new GlobalApplicationException("User is not found.");
        }

        var result = await _userManager.DeleteAsync(oldUser);

        if (result.Succeeded)
        {
            var response = new ApiSuccess()
            {
                Status = "Success.",
                StatusCode = 200,
                Message = "User delete successful."
            };
            return response;
        }
        throw new GlobalApplicationException(result.Errors.Select(x => x.Description).FirstOrDefault());

    }
    public async Task<ApiSuccess> AddUserInfo(UserRequestModel model, string userId)
    {
        var oldUser = await _userManager.FindByNameAsync(model.UserName);
        if (oldUser != null)
        {
            throw new GlobalApplicationException("User already exist!.");
        }

        var isEmailExist = await _userManager.FindByEmailAsync(model.Email);
        if (isEmailExist != null)
        {
            throw new GlobalApplicationException("User Email already exist!.");
        }
        
        var isPhoneNumberExist = await _context.Users.FirstOrDefaultAsync(x=> x.Email == model.Email);
        if (isPhoneNumberExist != null)
        {
            throw new GlobalApplicationException("User phone number already exist!.");
        }

        var user = new AppUser()
        {
            UserName = model.UserName,
            Email = model.Email,
            EmailConfirmed = model.EmailConfirm,
            PhoneNumber = model.PhoneNumber,
            PhoneNumberConfirmed = model.PhoneNumberConfirm,
            FullName = model.FullName,
            IsActive = model.IsActive,
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.Now,
            CreatedBy = userId,
            LastUpdatedAt = Convert.ToDateTime("1970-01-01 00:00:00.000"),
            LastUpdatedBy = "0"
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            throw new GlobalApplicationException(result.Errors.Select(x => x.Description).FirstOrDefault());
        }
        var roleAdded = await _userManager.AddToRolesAsync(user, model.RoleName);
        if (!roleAdded.Succeeded)
        {
            throw new GlobalApplicationException(roleAdded.Errors.Select(x => x.Description).FirstOrDefault());
        }
        var response = new ApiSuccess()
        {
            Status = "Success.",
            StatusCode = 200,
            Message = "User added successful."
        };
        return response;
        
    }

    public async Task<ApiResponse> PreAssignForCR(PreAssignForCRRequestModel model, string l3Id)
    {
        var str = $@"exec [dbo].[SetCRUserWithIdOrEmail] '{model.L3Id}', '{model.RoleName}', {l3Id}";

        var result = await _context.Database.ExecuteSqlRawAsync(str);

        if (result > 0)
        {
            var response = new ApiResponse()
            {
                Status = "Success.",
                StatusCode = 200,
                Message = "User Inserted Successful."
            };
            return response;
        }
        throw new Exception("Something is wrong.");
    }

    public async Task<List<string>> SearchEmployee(string searchText)
    {
        var strSql = $@"SELECT CONCAT(EmpName, ' - ', EmpID) as Employee from [LNK].dbo.Emp_Details WHERE 1=1 AND
                            EmpStatus='N' and (EmpID like '%{searchText}%' OR EmpName like '%{searchText}%' OR Email like '%{searchText}%')";
        var allEmployee = await _lnkDbContext.SearchLNKEmployees.FromSqlRaw(strSql).Select(x => x.Employee).ToListAsync();
        return allEmployee;
    }
}
using L3T.OAuth2DotNet7.DataAccess;
using L3T.OAuth2DotNet7.DataAccess.Model.Parmission;
using L3T.OAuth2DotNet7.DataAccess.Model.Parmission.RequestModel;
using L3T.OAuth2DotNet7.Repositories.Interface.MenuSetupAndPermission;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace L3T.OAuth2DotNet7.Repositories.Implementation.MenuSetupAndPermission
{
    public class MenuSetupRepository : IMenuSetupRepository
    {
        private readonly ApplicationMenuAndRoleWiseMenuPermissionDBContext _context;
        private readonly ILogger<MenuSetupRepository> _logger;
        public MenuSetupRepository(ApplicationMenuAndRoleWiseMenuPermissionDBContext context, ILogger<MenuSetupRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InsertMenuSetupTable(string controllerName, string actionName, string url, string projectName, string type)
        {
            try
            {
                var duplicateCheck = await _context.MenuSetup.Where(x => x.ControllerName == controllerName && x.MethodName == actionName && x.ApplicationName == projectName).FirstOrDefaultAsync();
                if (duplicateCheck is null)
                {
                    await _context.MenuSetup.AddAsync(
                        new MenuSetupModel
                        {
                            ControllerName = controllerName,
                            MethodName = actionName,
                            FeatureName = actionName,
                            ApplicationBaseUrl = url,
                            ApplicationName = projectName,
                            IsActive = true,
                            IsVisible = true,
                            ShowInMenuItem = false,
                            MenuSequence = 0,
                            CreatedAt = DateTime.Now,
                            AllowAnonymous = true,
                            MethodType = type,
                        });
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
            }
        }

        public async Task<List<MenuSetupAndPermissionViewModel>> getAllMenuSetupAndRoleWiseMenuPermissionProjectWise(string projectName)
        {
            try
            {
                string strQuery = $@"SELECT m.*, p.MenuSetupId, p.RoleName, p.UserId FROM [dbo].[MenuSetup] m 
                                    LEFT JOIN dbo.RoleWiseMenuPermission p on p.MenuSetupId = m.Id 
                                    where m.ApplicationName = '{projectName}' and m.IsActive = 1";

                var allMenu = await _context.MenuSetupAndPermissionView.FromSqlRaw(strQuery).ToListAsync();
                if (allMenu is null)
                {
                    throw new Exception("Please at first add menu and permission for this Project");
                }
                return allMenu;

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MenuSetupAndPermissionViewModel>> getAllMenuSetupAndRoleWiseMenuPermissionForUserOrRoleWise(GetAllMenuSetupAndPermissionRequestModel model)
        {
            try
            {
                List<MenuSetupAndPermissionViewModel> allMenu = new List<MenuSetupAndPermissionViewModel>();
                string strQuery = $@"SELECT m.*, p.MenuSetupId, p.RoleName, p.UserId FROM [dbo].[MenuSetup] m 
                                    LEFT JOIN dbo.RoleWiseMenuPermission p on p.MenuSetupId = m.Id 
                                    where m.ApplicationName = '{model.projectName}' and m.IsActive = 1";

                if (!string.IsNullOrEmpty(model.UserId) && !string.IsNullOrEmpty(model.roleName))
                {
                    strQuery = $@"{strQuery} and (p.RoleName = '{model.roleName}' and p.UserId = '{model.UserId}') OR m.AllowAnonymous = 1";
                }
                else if (string.IsNullOrEmpty(model.UserId) && !string.IsNullOrEmpty(model.roleName))
                {
                    strQuery = $@"{strQuery} and (p.RoleName = '{model.roleName}' and p.UserId IS NULL) OR m.AllowAnonymous = 1";
                }
                else
                {
                    throw new Exception("Invalid input data.");
                }

                allMenu = await _context.MenuSetupAndPermissionView.FromSqlRaw(strQuery).ToListAsync();
                if (allMenu is null)
                {
                    throw new Exception("Please at first add menu and permission for this Project");
                }
                return allMenu;

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MenuSetupAndPermissionViewModel>> PermissionCheckFromMiddleware(GetAllMenuSetupAndPermissionRequestModel model, string controllerName, string methodName)
        {
            try
            {
                List<MenuSetupAndPermissionViewModel> allMenu = new List<MenuSetupAndPermissionViewModel>();
                string strQuery = $@"SELECT m.*, p.MenuSetupId, p.RoleName, p.UserId FROM [dbo].[MenuSetup] m 
                                    LEFT JOIN dbo.RoleWiseMenuPermission p on p.MenuSetupId = m.Id 
                                    where m.ApplicationName = '{model.projectName}' and m.IsActive = 1 
                                    and ControllerName = '{controllerName}' and MethodName = '{methodName}'";

                if (!string.IsNullOrEmpty(model.UserId) && !string.IsNullOrEmpty(model.roleName))
                {
                    strQuery = $@"{strQuery} and ((p.RoleName = '{model.roleName}' and p.UserId = '{model.UserId}') OR m.AllowAnonymous = 1)";
                }
                else if (string.IsNullOrEmpty(model.UserId) && !string.IsNullOrEmpty(model.roleName))
                {
                    strQuery = $@"{strQuery} and ((p.RoleName = '{model.roleName}' and p.UserId IS NULL) OR m.AllowAnonymous = 1)";
                }
                else
                {
                    throw new Exception("Invalid input data.");
                }

                allMenu = await _context.MenuSetupAndPermissionView.FromSqlRaw(strQuery).ToListAsync();
                if (allMenu is null)
                {
                    throw new Exception("Please at first add menu and permission for this Project");
                }
                return allMenu;

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MenuSetupModel>> getMenuAndPermissionSetup(string projectName)
        {
            try
            {
                var allMenu = await _context.MenuSetup.Where(x => x.ApplicationName == projectName && x.IsActive == true).ToListAsync();
                if (allMenu is null)
                {
                    throw new Exception("Please at first setup your menu");
                }
                return allMenu;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SingleMenuUpdate(MenuSetupRequestModel model)
        {
            try
            {
                var oldMenu = await _context.MenuSetup.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (oldMenu is null)
                {
                    throw new Exception("Item is not found.");
                }
                oldMenu.MenuName = model.MenuName;
                oldMenu.FeatureName = model.FeatureName;
                oldMenu.MenuIcon = model.MenuIcon;
                oldMenu.AllowAnonymous = model.AllowAnonymous;
                oldMenu.ParentId = model.ParentId;
                oldMenu.MenuSequence = model.MenuSequence;
                oldMenu.ShowInMenuItem = model.ShowInMenuItem;
                oldMenu.IsVisible = model.IsVisible;

                _context.MenuSetup.Update(oldMenu);
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> PermissionSetupRoleAndUserWise(SetPermissionForRoleOrUserRequestModel model)
        {
            try
            {
                var newPermissionInsert = new List<RoleWiseMenuPermissionModel>();
                var permissionData = new List<RoleWiseMenuPermissionModel>();
                if (!string.IsNullOrEmpty(model.UserId) && !string.IsNullOrEmpty(model.RoleName))
                {
                    permissionData = await _context.RoleWiseMenuPermission.Where(x => x.UserId == model.UserId && x.RoleName == model.RoleName).ToListAsync();

                }
                else if (string.IsNullOrEmpty(model.UserId) && !string.IsNullOrEmpty(model.RoleName))
                {
                    permissionData = await _context.RoleWiseMenuPermission.Where(x => x.UserId == null && x.RoleName == model.RoleName).ToListAsync();
                }

                if (permissionData.Count > 0)
                {
                    _context.RoleWiseMenuPermission.RemoveRange(permissionData);
                }

                foreach (var aId in model.MenuId)
                {
                    newPermissionInsert.Add(new RoleWiseMenuPermissionModel()
                    {
                        MenuSetupId = aId,
                        RoleName = model.RoleName,
                        UserId = string.IsNullOrWhiteSpace(model.UserId) ? null : model.UserId,
                    });
                }
                await _context.AddRangeAsync(newPermissionInsert);
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> DeletePermissionSetupRoleAndUserWise(SetPermissionForRoleOrUserRequestModel model)
        {
            try
            {
                var newPermissionInsert = new List<RoleWiseMenuPermissionModel>();
                var permissionData = new List<RoleWiseMenuPermissionModel>();
                if (!string.IsNullOrEmpty(model.UserId) && !string.IsNullOrEmpty(model.RoleName))
                {
                    permissionData = await _context.RoleWiseMenuPermission.Where(x => x.UserId == model.UserId && x.RoleName == model.RoleName).ToListAsync();

                }
                else if (string.IsNullOrEmpty(model.UserId) && !string.IsNullOrEmpty(model.RoleName))
                {
                    permissionData = await _context.RoleWiseMenuPermission.Where(x => x.UserId == null && x.RoleName == model.RoleName).ToListAsync();
                }

                if (permissionData.Count is not 0)
                {
                    _context.RoleWiseMenuPermission.RemoveRange(permissionData);
                    return _context.SaveChanges() > 0;
                }
                return false;

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                throw new Exception(ex.Message);
            }
        }




    }
}

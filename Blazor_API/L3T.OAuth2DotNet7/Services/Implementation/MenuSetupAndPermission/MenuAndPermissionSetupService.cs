using L3T.OAuth2DotNet7.CommonModel;
using L3T.OAuth2DotNet7.DataAccess.Model.Parmission;
using L3T.OAuth2DotNet7.DataAccess.Model.Parmission.RequestModel;
using L3T.OAuth2DotNet7.Repositories.Interface.MenuSetupAndPermission;
using L3T.OAuth2DotNet7.Services.Interface;
using L3T.OAuth2DotNet7.Services.Interface.MenuAndPermission;
using Newtonsoft.Json;

namespace L3T.OAuth2DotNet7.Services.Implementation.MenuSetupAndPermission
{
    public class MenuAndPermissionSetupService : IMenuAndPermissionSetupService
    {
        private readonly IMenuSetupRepository _menuSetupRepository;
        private readonly IIdentityReauestResponseService _cRRequestResponseService;
        private readonly ILogger<MenuAndPermissionSetupService> _logger;
        public MenuAndPermissionSetupService(
            IMenuSetupRepository menuSetupRepository,
            IIdentityReauestResponseService cRRequestResponseService,
            ILogger<MenuAndPermissionSetupService> logger)
        {
            _menuSetupRepository = menuSetupRepository;
            _cRRequestResponseService = cRRequestResponseService;
            _logger = logger;
        }

        public async Task<ApiResponse> SpecificUserAndRoleWiseGetAllMenuAndPermission(GetAllMenuSetupAndPermissionRequestModel model, string ip, string userId)
        {
            var methodName = "MenuAndPermissionSetupService/SpecificUserAndRoleWiseGetAllMenuAndPermission";
            try
            {
                List<MenuSetupAndPermissionViewModel> newListData = new List<MenuSetupAndPermissionViewModel>();
                List<MenuSetupAndPermissionViewModel> allAnonymousData;
                List<MenuSetupAndPermissionViewModel> userSpcific;

                var allData = await _menuSetupRepository.getAllMenuSetupAndRoleWiseMenuPermissionProjectWise(model.projectName);

                allAnonymousData = allData.FindAll(x => x.AllowAnonymous == true && x.IsVisible == true);

                userSpcific = allData.FindAll(x => x.UserId == userId && x.RoleName == model.roleName && x.IsVisible == true).ToList();

                if (userSpcific.Count == 0)
                {
                    userSpcific = allData.FindAll(x => x.UserId is null && x.RoleName == model.roleName && x.IsVisible == true).ToList();
                }

                newListData.AddRange(userSpcific);
                newListData.AddRange(allAnonymousData); 
                return await _cRRequestResponseService.CreateResponseRequest(model, newListData, ip, methodName, userId, "Ok");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(model, ex, ip, methodName, userId, "Error", ex.Message.ToString());
            }
        }

        public async Task<ApiResponse> getAllMenuSetupAndRoleWiseMenuPermissionForUserOrRoleWise(GetAllMenuSetupAndPermissionRequestModel model, string ip, string userId)
        {
            var methodName = "MenuAndPermissionSetupService/getAllMenuSetupAndRoleWiseMenuPermissionForUserOrRoleWise";
            try
            {
                var allData = await _menuSetupRepository.getAllMenuSetupAndRoleWiseMenuPermissionForUserOrRoleWise(model);

                return await _cRRequestResponseService.CreateResponseRequest(model, allData, ip, methodName, userId, "OK");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(model, ex, ip, methodName, userId, "Error", ex.Message.ToString());

            }
        }


        public async Task<ApiResponse> getMenuAndPermissionSetup(string projectName, string ip, string userId)
        {
            var methodName = "MenuAndPermissionSetupService/getMenuAndPermissionSetup";
            try
            {
                var allData = await _menuSetupRepository.getMenuAndPermissionSetup(projectName);
                return await _cRRequestResponseService.CreateResponseRequest(projectName, allData, ip, methodName, userId, "OK");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(projectName, ex, ip, methodName, userId, "Error", ex.Message.ToString());

            }
        }

        public async Task<ApiResponse> SingleMenuUpdate(MenuSetupRequestModel model, string ip, string userId)
        {
            var methodName = "MenuAndPermissionSetupService/SingleMenuUpdate";

            try
            {
                var allData = await _menuSetupRepository.SingleMenuUpdate(model);
                return await _cRRequestResponseService.CreateResponseRequest(model, allData, ip, methodName, userId, "OK");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(model, ex, ip, methodName, userId, "Error", ex.Message.ToString());
            }
        }

        public async Task<ApiResponse> PermissionSetupRoleAndUserWise(SetPermissionForRoleOrUserRequestModel model, string ip, string userId)
        {
            var methodName = "MenuAndPermissionSetupService/PermissionSetupRoleAndUserWise";

            try
            {
                var allData = await _menuSetupRepository.PermissionSetupRoleAndUserWise(model);
                return await _cRRequestResponseService.CreateResponseRequest(model, allData, ip, methodName, userId, "Permission is successfully reset.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(model, ex, ip, methodName, userId, "Error", ex.Message.ToString());

            }
        }

        public async Task<ApiResponse> DeletePermissionSetupRoleAndUserWise(SetPermissionForRoleOrUserRequestModel model, string ip, string userId)
        {
            var methodName = "MenuAndPermissionSetupService/DeletePermissionSetupRoleAndUserWise";

            try
            {
                var allData = await _menuSetupRepository.DeletePermissionSetupRoleAndUserWise(model);
                return await _cRRequestResponseService.CreateResponseRequest(model, allData, ip, methodName, userId, "Permission is successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(model, ex, ip, methodName, userId, "Error", ex.Message.ToString());

            }
        }

        public async Task<List<MenuSetupAndPermissionViewModel>> PermissionCheckFromMiddleware(GetAllMenuSetupAndPermissionRequestModel model, string controllerName, string methodName, string ip, string userId)
        {
            var methodName1 = "MenuAndPermissionSetupService/getAllMenuSetupAndRoleWiseMenuPermissionForUserOrRoleWise";
            try
            {
                var allData = await _menuSetupRepository.PermissionCheckFromMiddleware(model, controllerName, methodName);
                await _cRRequestResponseService.CreateResponseRequest(model, allData, ip, methodName, userId, "OK");

                return allData;

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                await _cRRequestResponseService.CreateResponseRequest(model, ex, ip, methodName1, userId, "Error", ex.Message.ToString());
                return new List<MenuSetupAndPermissionViewModel>();

            }
        }


    }
}

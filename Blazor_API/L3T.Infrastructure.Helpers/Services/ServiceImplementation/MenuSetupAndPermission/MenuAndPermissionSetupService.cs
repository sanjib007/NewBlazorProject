using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.Parmission;
using L3T.Infrastructure.Helpers.Models.RequestModel.Permission;
using L3T.Infrastructure.Helpers.Repositories.Interface.MenuSetupAndPermission;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.MenuAndPermission;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.MenuSetupAndPermission
{
    public class MenuAndPermissionSetupService : IMenuAndPermissionSetupService
    {
        private readonly IMenuSetupRepository _menuSetupRepository;
        private readonly ICRRequestResponseService _cRRequestResponseService;
        private readonly ILogger<MenuAndPermissionSetupService> _logger;
        private readonly IConfiguration _configuration;
		public MenuAndPermissionSetupService(
			IMenuSetupRepository menuSetupRepository,
			ICRRequestResponseService cRRequestResponseService,
			ILogger<MenuAndPermissionSetupService> logger,
			IConfiguration configuration)
		{
			_menuSetupRepository = menuSetupRepository;
			_cRRequestResponseService = cRRequestResponseService;
			_logger = logger;
			_configuration = configuration;
		}

		public async Task<ApiResponse> SpecificUserAndRoleWiseGetAllMenuAndPermission(GetAllMenuSetupAndPermissionRequestModel model, string ip, string userId)
        {
            var methodName = "MenuAndPermissionSetupService/SpecificUserAndRoleWiseGetAllMenuAndPermission";
            try
            {
                List<MenuSetupAndPermissionViewModel> newListData = new List<MenuSetupAndPermissionViewModel>();
                List<MenuSetupAndPermissionViewModel> allAnonymousData;
                List<MenuSetupAndPermissionViewModel> userSpcific;
                List<MenuSetupAndPermissionViewModel> roleSpcific;

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
                return await _cRRequestResponseService.CreateResponseRequest(projectName, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
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
				return await _cRRequestResponseService.CreateResponseRequest(model, ex, ip, methodName, userId,"Error", ex.Message.ToString());
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
                return await _cRRequestResponseService.CreateResponseRequest(model, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
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

                if (_configuration.GetValue<bool>("DefaultApproverDepartment:AutoInProgress"))
                {
					await _menuSetupRepository.AutoUpdateCrApprovedToInProgress();
				}
                
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

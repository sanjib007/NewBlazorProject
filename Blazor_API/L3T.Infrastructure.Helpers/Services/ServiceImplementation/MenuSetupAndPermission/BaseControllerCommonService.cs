﻿using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.Parmission;
using L3T.Infrastructure.Helpers.Models.RequestModel.Permission;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Repositories.Interface.MenuSetupAndPermission;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.MenuAndPermission;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class BaseControllerCommonService : IBaseControllerCommonService
    {
        private readonly IMenuSetupRepository _menuSetupRepository;
        private readonly ICRRequestResponseService _cRRequestResponseService;
        private readonly ILogger<BaseControllerCommonService> _logger;
        private readonly IConfiguration _config;
        //private readonly ApplicationMenuAndRoleWiseMenuPermissionDBContext _context;
        public BaseControllerCommonService(
            IMenuSetupRepository menuSetupRepository,
            ICRRequestResponseService cRRequestResponseService,
            ILogger<BaseControllerCommonService> logger,
            IConfiguration config
            //ApplicationMenuAndRoleWiseMenuPermissionDBContext context
            )
        {
            _menuSetupRepository = menuSetupRepository;
            _cRRequestResponseService = cRRequestResponseService;
            _logger = logger;
            _config = config;
            //_context = context;
        }

        public async Task InsertMenuSetupTable(string controllerName, string actionName, string url, string projectName, string type)
        {
            var methodName = "BaseControllerCommonService/InsertMenuSetupTable";
            try
            {
                await _menuSetupRepository.InsertMenuSetupTable(controllerName, actionName, url, projectName, type);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                await _cRRequestResponseService.CreateResponseRequest($@"Controller : { controllerName}, actionName : {actionName}, url: {url}, projectName: {projectName}, type: {type}", ex, null, methodName, null, ex.Message.ToString());
            }
        }

        






    }
}

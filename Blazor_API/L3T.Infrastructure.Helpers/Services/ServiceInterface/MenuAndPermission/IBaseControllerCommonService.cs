using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.RequestModel.Permission;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.MenuAndPermission
{
    public interface IBaseControllerCommonService
    {
        Task InsertMenuSetupTable(string controllerName, string actionName, string url, string projectName, string type);
    }
}

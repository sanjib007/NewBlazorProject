using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.ThirdPartyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.ThirdParty
{
	public interface IThirdPartyService
	{
		Task<ApiResponse<List<AppUserModel>>> GetAllDefaultApprovalFlow(string l3Id);
	}
}

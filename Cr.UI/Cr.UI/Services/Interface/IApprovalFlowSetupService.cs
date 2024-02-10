using Cr.UI.Data;
using Cr.UI.Data.ApprovalFlow;

namespace Cr.UI.Services.Interface
{
	public interface IApprovalFlowSetupService
	{
		Task<ApiResponse<List<string>>> GetAllStringList(string requestUri);
		Task<ApiResponse<List<CrDefaultApprovalFlow>>> GetAllDefaultApprovalFlow(string requestUri);
		Task<ApiResponse> InsertDefaultApprovalFlow(string requestUri, AddCrDefaultApprovalFlowReq obj);
		Task<ApiResponse> DefaultApproverActiveInActive(string requestUri, CrDefaultApprovalFlow obj);
		Task<ApiResponse> CRAddEmployee(string requestUri, AddUser obj);
        Task<ApiResponse<List<AppUserModel>>> GetEmployeeList(string requestUri);
    }
}

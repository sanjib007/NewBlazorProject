using Cr.UI.Data;
using Cr.UI.Data.ChangeRequirementModel;

namespace Cr.UI.Services.Interface
{
    public interface ICrReportService
    {
        Task<ApiResponse<PaginationModel<List<AssignEmployeeListResponse>>>> GetAssignEmployeeList(string requestUri, AssignEmployeeListReqModel obj);
    }
}

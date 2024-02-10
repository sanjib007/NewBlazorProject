using Cr.UI.Data;
using Cr.UI.Data.ChangeRequirementModel;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Cr.UI.Services.Interface
{
    public interface IChangeRequirementService
    {
        Task<ApiResponse<List<ViewChangeReq>>> GetViewChangeReqAllAsync(string requestUri);
        Task<ApiResponse> GetChangeReqByIdAsync(string requestUri, int Id);
        Task<ApiResponse<TempChangeRequestedInfo>> StepOneRequest(StepOneRequestModel reqestModel);
        Task<ApiResponse<TempChangeRequestedInfo>> StepTwoRequest(StepTwoRequestModel reqestModel);
        Task<ApiResponse<TempChangeRequestedInfo>> StepThreeRequest(StepThreeRequestModel reqestModel, IBrowserFile files);
        Task<ApiResponse<TempChangeRequestedInfo>> StepFourRequest(StepFourRequestModel reqestModel);
        Task<ApiResponse> StepFiveRequest();
        Task<ApiResponse<TempChangeRequestedInfo>> SaveAsync(string requestUri, AddChangeReq obj, IBrowserFile files);
        Task<ApiResponse> DeleteAsync(string requestUri);
        Task<ApiResponse<PaginationModel<List<ChangeRequestModel>>>> GetAllCrList(string requestUri, ChangeRequestListRequestModel obj);
        Task<ApiResponse> AddAssignEmployeeSaveAsync(string requestUri, AddAssignEmployeeReq obj);
        Task<ApiResponse> AddRemark(string requestUri, AddRemarkRequestModel obj);
        Task<ApiResponse> ChangeCrStaurs(string requestUri);
        Task<ApiResponse<List<CrAttatchedFile>>> RemovedFile(string requestUri, RemovedFileRequestModel obj);
        Task<ApiResponse<List<CrAttatchedFile>>> GetAllFiles(string requestUri);
        Task<ApiResponse> ChangeTaskAsync(string requestUri);
    }
}

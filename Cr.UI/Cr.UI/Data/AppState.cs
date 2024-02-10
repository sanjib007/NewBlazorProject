using Cr.UI.Data.ChangeRequirementModel;
using Microsoft.AspNetCore.Components;


namespace Cr.UI.Data
{
    public class AppState
    {
        public event Action OnChange;

        public string LoginUserName { get; set; }

        public Action<int> OnLoadChanged { get; set; }

        public List<CrCreateStepsModel> SetSteps { get; set; }

        public AssignEmployeeListResponse EmployeeAssignResponse { get; set; }

        public ApiResponse<PaginationModel<List<ChangeRequestModel>>> PendingCrResponse { get; set; }
        public List<ChangeRequestModel> PendingCr { get; set; }
        public int TotalPending { get; set; }

        public string PathName { get; set; }
        public string AddClass { get; set; }

        public async Task SetDataFunction(List<CrCreateStepsModel> steps)
        {
            if(steps != null && steps.Count > 0)
            {
                SetSteps = steps;
                await NotifyStateChanged();
            }
        }

        public async Task SetCrAssainEmployee(AssignEmployeeListResponse steps)
        {
            if(steps != null)
            {
                EmployeeAssignResponse = steps;
                await NotifyStateChanged();
            }
        }

        public async Task SetPendingCrList(ApiResponse<PaginationModel<List<ChangeRequestModel>>> response)
        {
            if(response != null)
            {
                PendingCrResponse = response;
                PendingCr = response.Data.item;
                TotalPending = response.Data.totalRecords;
                await NotifyStateChanged();
            }
        }

        public async Task SetLoginUserName(string loginUserName)
        {
            if(!string.IsNullOrEmpty(loginUserName))
            {
                LoginUserName = loginUserName;
                await NotifyStateChanged();
            }
        }

        public async Task SetPathNameCss(string path, string className)
        { 
            if(!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(className))
            {
                PathName = path;
                AddClass = className;
                await NotifyStateChanged();
            }
        }

        private async Task NotifyStateChanged() => OnChange?.Invoke();
    }
}

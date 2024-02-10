using Cr.UI.Data;
using Cr.UI.Data.ChangeRequirementModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cr.UI.Services.Interface
{
    public interface IUserService
    {
        Task<string> Encryption(string Data);
        public Task<UserModel> LoginAsync(UserModel user);
        public Task<ApiResponse> RegisterUserAsync(RegistrationModel user);
        public Task<ApiResponse> RefreshTokenAsync();
        public Task Logout();
        public Task<ApiResponse<PaginationModel<List<UserListModel>>>> GetAllUserList(GetUserFilterModel model);
        public Task setLocalStorage(UserModel user);
        public Task removedLocalStorage();
        public Task<ApiResponse> ErrorMethod(string statusCode, string responseBody);
        public Task<List<CrCreateStepsModel>> ChangeSteps(List<CrCreateStepsModel> steps, int currentStep);



    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using L3T.Infrastructure.Helpers.Models.Test;

namespace L3T.Infrastructure.Helpers.Interface.Cams;

public interface ICamsService
{
    Task<ApiResponse> GetUserInfoFromMikrotikRouter(GetUserInfoFromMikrotikRequestModel requestModel);
    ApiResponse GetAllUsersInfoFromMikrotikRouter(MikrotikRouterFilterParams requestModel);
    Task<ApiResponse> AddUserInMikrotikRouter(AddUserInfoInMikrotikRouterRequestModel requestModel);
    ApiResponse AddUserInMikrotikRouterSyncMethod(AddUserInfoInMikrotikRouterRequestModel requestModel);
    Task<ApiResponse> BlockUserInfoFromMikrotikRouter(BlockUserFromMikrotikRouterRequestModel requestModel);
    Task<ApiResponse> UnblockUserInfoFromMikrotikRouter(UnblockUserFromMikrotikRouterRequestModel requestModel);
    Task<ApiResponse> DeleteUserInfoFromMikrotikRouter(DeleteUserFromMikrotikRouerRequestModel requestModel);
    ApiResponse BlockUserInfoFromMikrotikRouterSyncMethod(BlockUserFromMikrotikRouterRequestModel requestModel);
    ApiResponse UnblockUserInfoFromMikrotikRouterSyncMethod(UnblockUserFromMikrotikRouterRequestModel requestModel);
    ApiResponse BlockListsUserInfoFromMikrotikRouterSyncMethod(BlockUserListsFromMikrotikRouterRequestModel requestModel);
    ApiResponse GetAllQueueFromMikrotikRouter(GetAllQueueRequestModel requestModel);
    ApiResponse SetQueueInfoIntoMikrotikRouter(SetQueueIntoMikrotikRouterRequestModel model);
    Task<ApiResponse> MikrotikRouterInterfaceData(MikrotikRouterFilterParams model);
}
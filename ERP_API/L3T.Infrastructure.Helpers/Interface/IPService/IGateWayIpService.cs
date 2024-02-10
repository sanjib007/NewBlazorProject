using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.RequestModel;
using L3T.Infrastructure.Helpers.Models.Ipservice.Entities;
using L3T.Infrastructure.Helpers.Models.Test;

namespace L3T.Infrastructure.Helpers.Interface.IPService;

public interface IGateWayIpService
{
    // Task<ApiResponse> GetUserInfoFromMikrotikRouter(GetUserInfoFromMikrotikRequestModel requestModel);

    Task<ApiResponse> AddGateWayIpAddress(AddGateWayIpReq requestModel);
   

    Task<ApiResponse> UpdateGateWayIpAddress( UpdateGatewayIpReq reqModel);

    Task<ApiResponse> GetAllGateWayIpAddressQuery();

    Task<ApiResponse> GetGateWayIpAddressById(long Id);
    Task<ApiResponse> GateWayIpAddressStatusChange(long Id,bool Status);

}
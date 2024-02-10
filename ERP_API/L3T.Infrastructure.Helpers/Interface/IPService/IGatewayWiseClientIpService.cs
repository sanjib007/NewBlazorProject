using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.RequestModel;
using L3T.Infrastructure.Helpers.Models.Ipservice.Entities;
using L3T.Infrastructure.Helpers.Models.Test;

namespace L3T.Infrastructure.Helpers.Interface.IPService;

public interface IGatewayWiseClientIpService
{
  

    Task<ApiResponse> AddGatewayWiseClientIpAddress(AddGatewayWiseClientIpReq requestModel);
   

    Task<ApiResponse> UpdateGatewayWiseClientIpAddress(UpdateGatewayWiseClientIpReq reqModel);

    Task<ApiResponse> GetAllGatewayWiseClientIpAddressQuery();

    Task<ApiResponse> GetGatewayWiseClientIpAddressById(long Id);
    Task<ApiResponse> GatewayWiseClientIpAddressStatusChange(long Id,bool Status,string LastModifiedby);

}
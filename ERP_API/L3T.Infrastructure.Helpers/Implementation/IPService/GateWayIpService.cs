using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.CamsDBContext;
using L3T.Infrastructure.Helpers.Helper;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Interface.Common;
using L3T.Infrastructure.Helpers.Models.Mikrotik;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using L3T.Infrastructure.Helpers.Models.Mikrotik.ResponseModel;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using tik4net;
using tik4net.Objects.User;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using L3T.Infrastructure.Helpers.Interface.IPService;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.RequestModel;
using L3T.Infrastructure.Helpers.DataContext.IpServiceDBContext;
using L3T.Infrastructure.Helpers.Models.Ipservice.Entities;
using L3T.Infrastructure.Helpers.Models.BTS;
using System.Security.Principal;
using L3T.Infrastructure.Helpers.Models.Ipservice.DTOs.ResponseModel;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace L3T.Infrastructure.Helpers.Implementation.Cams;

public class GateWayIpService : IGateWayIpService
{
    private readonly IpServiceDataWriteContext _IpServiceContext;
    private readonly ILogger<GateWayIpService> _logger;
    //private readonly ISystemService _systemService;
   

    public GateWayIpService(
        IpServiceDataWriteContext ipServiceContext,
        ILogger<GateWayIpService> logger
       // ISystemService systemService
    )
    {
        _IpServiceContext = ipServiceContext;
        _logger = logger;
        //_systemService = systemService;
        
    }
    
    public async Task<ApiResponse> AddGateWayIpAddress(AddGateWayIpReq requestModel)
    {
        try
        {
            var response = new ApiResponse();
            var newData = new GatewayIpAddress()
            {
                BtsId            = requestModel.BtsId,
                DistributorId    = requestModel.DistributorId,
                RouterType       = requestModel.RouterType,
                RouterName       = requestModel.RouterName,
                RouterPort       = requestModel.RouterPort,
                GateWay          = requestModel.GateWay,
                RouterHostName   = requestModel.RouterHostName,
                RouterModelName  = requestModel.RouterModelName,
                RouterSwitchIp   = requestModel.RouterSwitchIp,
                Vlan             = requestModel.Vlan,
                HostName         = requestModel.HostName,
                Remarks          = requestModel.Remarks,
                CreatedBy        = requestModel.CreatedBy,
                CreatedAt        = requestModel.CreatedAt
            };
            await _IpServiceContext.GatewayIpAddresses.AddAsync(newData);
            if(await _IpServiceContext.SaveChangesAsync()> 0)
            {
                response = new ApiResponse()
                {
                    Message = "create data",
                    Status = "success",
                    StatusCode = 200
                };
                return response;
            };
            response = new ApiResponse()
            {
                Message = "data is not inserted",
                Status = "Error",
                StatusCode = 400
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Error saving Ip service SubmitAsync-", ex);
            var response = new ApiResponse()
            {
                Message = ex.Message,
                Status = "Error",
                StatusCode = 400
            };
            return response;
        }
    }

    public async Task<ApiResponse> GateWayIpAddressStatusChange(long Id, bool Status)
    {
        _logger.LogInformation("Status Change  GateWay IP by id : " + Id);

        var Output = new ApiResponse();

        if (!String.IsNullOrEmpty(Status.ToString()))
        {
            Output.StatusCode = 400;
            Output.Status = "Error";
            Output.Message = "Please enter required fields.";
        }

        try
        {
            var item = await _IpServiceContext.GatewayIpAddresses.SingleOrDefaultAsync(f => f.Id == Id);
            if (item != null)
            {
                item.Status = Status;
                item.LastModifiedBy = "l3t2125";
                item.LastModifiedAt = DateTime.UtcNow;

                //var newChangeData = new GatewayIpAddress()
                //{
                //   Status = Status,
                //   LastModifiedAt = DateTime.UtcNow
                //};

                _IpServiceContext.GatewayIpAddresses.Update(item);
                await _IpServiceContext.SaveChangesAsync();


                Output.Message = "Status Change success,";
                Output.Status = "success";
                Output.StatusCode = 200;
               

                return Output;

            }
            else
            {
                Output.Message = " Your requested Data not found.";
                Output.Status = "False";
                Output.StatusCode = 400;
                return Output;
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation("An error occurred when Status Change the GateWay IP by id:" + Id + " Exception: ", ex);

            Output.Message = " Status Not Change Status";
            Output.Status = "False";
            Output.StatusCode = 400;
            
            return Output;
        }
    }

    public async Task<ApiResponse> GetAllGateWayIpAddressQuery()
    {

        try
        {
            var output = new ApiResponse();

            var gatewayIpAddressList = await _IpServiceContext.GatewayIpAddresses.ToListAsync();
            
            var gatewayIpAddresses = new List<GetAllGatewayIpAddressResponse>();

            if (gatewayIpAddressList.Count <= 0)
            {
                output.Status = "Fail";
                output.StatusCode = 400;
                output.Message = "Data Not found";
            }
            foreach (var gatewayIpAddress in gatewayIpAddressList)
            {
                var aData = new GetAllGatewayIpAddressResponse();
                aData.Id = gatewayIpAddress.Id;
                aData.BtsId = gatewayIpAddress.BtsId;
                aData.DistributorId = gatewayIpAddress.DistributorId;
                aData.RouterType = gatewayIpAddress.RouterType;
                aData.RouterName = gatewayIpAddress.RouterName;
                aData.RouterPort = gatewayIpAddress.RouterPort;
                aData.GateWay = gatewayIpAddress.GateWay;
                aData.RouterHostName = gatewayIpAddress.RouterHostName;
                aData.RouterModelName = gatewayIpAddress.RouterModelName;
                aData.RouterSwitchIp = gatewayIpAddress.RouterSwitchIp;
                aData.Vlan = gatewayIpAddress.Vlan;
                aData.HostName = gatewayIpAddress.HostName;
                aData.Remarks = gatewayIpAddress.Remarks;
                aData.Status = gatewayIpAddress.Status;
                aData.CreatedBy = gatewayIpAddress.CreatedBy;
                aData.CreatedAt = gatewayIpAddress.CreatedAt;
                gatewayIpAddresses.Add(aData);

            }
            output.Message = "Successfully Fetching Data";
            output.Status = "success";
            output.StatusCode = 200;
            output.Data = gatewayIpAddresses;

            return output;
        }
        catch (Exception ex)
        {
            var output = new ApiResponse()
            {
                Message = ex.Message,
                Status = "Error",
                StatusCode = 400
            };
        
            _logger.LogInformation("An error occurred when fetching the GetAllIpQuery method  Exception: " + ex.Message.ToString());
            return output;
        }
       
    }

    public async Task<ApiResponse> GetGateWayIpAddressById(long Id)
    {
        _logger.LogInformation("Fetching GetTicketById by Id" + Id);
        var output = new ApiResponse();
        try
        {
            var aData = new GetAllGatewayIpAddressResponse();

            var giaobj = await _IpServiceContext.GatewayIpAddresses.FirstOrDefaultAsync(f => f.Id == Id).ConfigureAwait(false);
            aData.Id = giaobj.Id;
            aData.BtsId = giaobj.BtsId;
            aData.DistributorId = giaobj.DistributorId;
            aData.RouterType = giaobj.RouterType;
            aData.RouterName = giaobj.RouterName;
            aData.RouterPort = giaobj.RouterPort;
            aData.GateWay = giaobj.GateWay;
            aData.RouterHostName = giaobj.RouterHostName;
            aData.RouterModelName = giaobj.RouterModelName;
            aData.RouterSwitchIp = giaobj.RouterSwitchIp;
            aData.Vlan = giaobj.Vlan;
            aData.HostName = giaobj.HostName;
            aData.Remarks = giaobj.Remarks;
            aData.Status = giaobj.Status;
            aData.CreatedBy = giaobj.CreatedBy;
            aData.CreatedAt = giaobj.CreatedAt;

            output.Message = "Successfully Fetching Data";
            output.Status = "success";
            output.StatusCode = 200;
            output.Data = aData;

            return output;
        }
        catch (Exception ex)
        {
            output.Message = ex.Message;
            output.Status = "Error";
            output.StatusCode = 400;
            _logger.LogInformation("An error occurred when updating  the TicketEntry  id is :" + Id + " Exception: ", ex);
            return output;
        }
       
    }

    public async Task<ApiResponse> UpdateGateWayIpAddress( UpdateGatewayIpReq reqModel)
    {
        _logger.LogInformation("updating GateWayIpAddress id is : " + reqModel.Id);

       var  Output = new ApiResponse();

        if(reqModel == null)
        {
            Output.StatusCode = 400;
            Output.Status = "Error";
            Output.Message = "Please enter required fields.";
        }

        try
        {
            var OldEntity = await _IpServiceContext.GatewayIpAddresses.FirstOrDefaultAsync(x => x.Id == reqModel.Id);
            if (OldEntity != null)
            {


                OldEntity.BtsId = reqModel.BtsId;
                OldEntity.DistributorId = reqModel.DistributorId;
                OldEntity.RouterType = reqModel.RouterType;
                OldEntity.RouterName = reqModel.RouterName;
                OldEntity.RouterPort = reqModel.RouterPort;
                OldEntity.GateWay = reqModel.GateWay;
                OldEntity.RouterHostName = reqModel.RouterHostName;
                OldEntity.RouterModelName = reqModel.RouterModelName;
                OldEntity.RouterSwitchIp = reqModel.RouterSwitchIp;
                OldEntity.Vlan = reqModel.Vlan;
                OldEntity.HostName = reqModel.HostName;
                OldEntity.Remarks = reqModel.Remarks;
                OldEntity.LastModifiedBy = reqModel.LastModifiedBy;
                OldEntity.LastModifiedAt = DateTime.UtcNow;


                _IpServiceContext.GatewayIpAddresses.Update(OldEntity);
                await _IpServiceContext.SaveChangesAsync();

                Output.Message = "Gateway Ip Address Update Success.";
                Output.Status = "success";
                Output.StatusCode = 200;
               
                return Output;
            }
            else
            {

                Output.Message = "Your requested data not found";
                Output.Status = "Error";
                Output.StatusCode = 400;
               
                return Output;
            }

        }
        catch (Exception ex)
        {
            _logger.LogInformation("An error occurred when updating  the Gateway Ip Address  id is :" + reqModel.Id + " Exception: ", ex);
            var response = new ApiResponse()
            {
                Message = ex.Message,
                Status = "Error",
                StatusCode = 400
            };
            return response;
        }
    }

}
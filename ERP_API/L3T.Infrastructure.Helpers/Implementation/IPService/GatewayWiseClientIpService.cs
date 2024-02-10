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

public class GatewayWiseClientIpService : IGatewayWiseClientIpService
{
    private readonly IpServiceDataWriteContext _IpServiceContext;
    private readonly ILogger<GateWayIpService> _logger;
    //private readonly ISystemService _systemService;
   

    public GatewayWiseClientIpService(
        IpServiceDataWriteContext ipServiceContext,
        ILogger<GateWayIpService> logger
       // ISystemService systemService
    )
    {
        _IpServiceContext = ipServiceContext;
        _logger = logger;
        //_systemService = systemService;
        
    }
    
    public async Task<ApiResponse> AddGatewayWiseClientIpAddress(AddGatewayWiseClientIpReq requestModel)
    {
        try
        {
            var response = new ApiResponse();
            var newData = new GatewayWiseClientIpAddress()
            {
               
                GatewayIpAddressId = requestModel.GetewayIpId,
                PackageId = requestModel.PackageId,
                IpAddress = requestModel.IpAddress,
                PoolName = requestModel.PoolName,               
                SubNetMask = requestModel.SubNetMask,
                LookBackAddress = requestModel.LookBackAddress,
                SubscriberId = requestModel.SubscriberId,
                SubscriberSlNo = requestModel.SubscriberSlNo,
                UsedStatus = requestModel.UsedStatus,
                Remarks = requestModel.Remarks,
                Status = requestModel.Status,
                CreatedBy = requestModel.CreatedBy,
                CreatedAt = DateTime.UtcNow,
            };
            await _IpServiceContext.GatewayWiseClientIpAddresses.AddAsync(newData);
            if(await _IpServiceContext.SaveChangesAsync()> 0)
            {
                response = new ApiResponse()
                {
                    Message = "Gateway wise client ip address createed successfully!",
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

    public async Task<ApiResponse> GatewayWiseClientIpAddressStatusChange(long Id, bool Status,string LastModifiedby)
    {
        _logger.LogInformation("Status Change  GateWay Wise IP by id : " + Id);

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
                item.LastModifiedBy = LastModifiedby;
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
            _logger.LogInformation("An error occurred when Status Change the GateWay Wise Client IP by id:" + Id + " Exception: ", ex);

            Output.Message = " Status Not Change";
            Output.Status = "False";
            Output.StatusCode = 400;
            
            return Output;
        }
    }

    public async Task<ApiResponse> GetAllGatewayWiseClientIpAddressQuery()
    {

        try
        {
            var output = new ApiResponse();

            var gatewayWiseIpAddressList = await _IpServiceContext.GatewayWiseClientIpAddresses.ToListAsync();
            
            var gatewayIpAddresses = new List<GetAllGatewayWiseClientIpResponse>();

            if (gatewayWiseIpAddressList.Count <= 0)
            {
                output.Status = "Fail";
                output.StatusCode = 400;
                output.Message = "Data Not found";
            }
            foreach (var gatewayIpAddress in gatewayWiseIpAddressList)
            {
                var aData = new GetAllGatewayWiseClientIpResponse();

                aData.Id = gatewayIpAddress.Id;
                aData.GetewayIpId = gatewayIpAddress.GatewayIpAddressId;
                aData.PackageId = gatewayIpAddress.PackageId;
                aData.IpAddress = gatewayIpAddress.IpAddress;
                aData.PoolName = gatewayIpAddress.PoolName;
                aData.SubNetMask = gatewayIpAddress.SubNetMask;
                aData.LookBackAddress = gatewayIpAddress.LookBackAddress;
                aData.SubscriberId = gatewayIpAddress.SubscriberId;
                aData.SubscriberSlNo = gatewayIpAddress.SubscriberSlNo;
                aData.UsedStatus = gatewayIpAddress.UsedStatus;
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

    public async Task<ApiResponse> GetGatewayWiseClientIpAddressById(long Id)
    {
        _logger.LogInformation("Fetching GetGateway Wise Client Ip Address by Id" + Id);
        var output = new ApiResponse();
        try
        {
            var aData = new GetGatewayWiseClientIpByIdResponse();

            var giaobj = await _IpServiceContext.GatewayWiseClientIpAddresses.FirstOrDefaultAsync(f => f.Id == Id).ConfigureAwait(false);
            aData.Id = giaobj.Id;
            aData.GetewayIpId = giaobj.GatewayIpAddressId;
            aData.PackageId = giaobj.PackageId;
            aData.IpAddress = giaobj.IpAddress;
            aData.PoolName = giaobj.PoolName;
            aData.SubNetMask = giaobj.SubNetMask;
            aData.LookBackAddress = giaobj.LookBackAddress;
            aData.SubscriberId = giaobj.SubscriberId;
            aData.SubscriberSlNo = giaobj.SubscriberSlNo;
            aData.UsedStatus = giaobj.UsedStatus;
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

    public async Task<ApiResponse> UpdateGatewayWiseClientIpAddress(UpdateGatewayWiseClientIpReq reqModel)
    {
        _logger.LogInformation("updating GatewayWiseClientIpAddress id is : " + reqModel.Id);

       var  Output = new ApiResponse();

        if(reqModel == null)
        {
            Output.StatusCode = 400;
            Output.Status = "Error";
            Output.Message = "Please enter required fields.";
        }

        try
        {
            var OldEntity = await _IpServiceContext.GatewayWiseClientIpAddresses.FirstOrDefaultAsync(x => x.Id == reqModel.Id);
            if (OldEntity != null)
            {
                OldEntity.GatewayIpAddressId = reqModel.GetewayIpId;
                OldEntity.PackageId = reqModel.PackageId;
                OldEntity.IpAddress = reqModel.IpAddress;
                OldEntity.PoolName = reqModel.PoolName;
                OldEntity.SubNetMask = reqModel.SubNetMask;
                OldEntity.LookBackAddress = reqModel.LookBackAddress;
                OldEntity.SubscriberId = reqModel.SubscriberId;
                OldEntity.SubscriberSlNo = reqModel.SubscriberSlNo;
                OldEntity.UsedStatus = reqModel.UsedStatus;
                OldEntity.Remarks = reqModel.Remarks;
                OldEntity.LastModifiedBy = reqModel.LastModifiedBy;
                OldEntity.LastModifiedAt = DateTime.UtcNow;
               

                _IpServiceContext.GatewayWiseClientIpAddresses.Update(OldEntity);
                await _IpServiceContext.SaveChangesAsync();

                Output.Message = "Gateway Wise Client Ip Address Updated Successfully.";
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
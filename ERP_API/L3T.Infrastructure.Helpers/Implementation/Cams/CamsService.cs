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
using L3T.Infrastructure.Helpers.Interface.Email;

namespace L3T.Infrastructure.Helpers.Implementation.Cams;

public class CamsService : ICamsService
{
    private readonly CamsDataWriteContext _camsContext;
    private readonly ILogger<CamsService> _logger;
    private readonly ISystemService _systemService;
    private readonly IMailSenderService _mailrepo;

    public CamsService(
        CamsDataWriteContext camsContext,
        ILogger<CamsService> logger,
        ISystemService systemService,
        IMailSenderService mailrepo
    )
    {
        _camsContext = camsContext;
        _logger = logger;
        _systemService = systemService;
        _mailrepo = mailrepo;
    }

    public async Task<ApiResponse> GetUserInfoFromMikrotikRouter(GetUserInfoFromMikrotikRequestModel requestModel)
    {
        var methordName = "CamsService/GetUserInfoFromMikrotikRouter";
        try
        {
            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);
                var newList = new List<GetMikrotikRouterUserInfoResponseModel>();
                var userInfoInsideRouter = new GetMikrotikRouterUserInfoResponseModel();

                //var blockResponse = await BlockCheckMethod(connection, requestModel.CustomerIp);

                //if (blockResponse.Count() == 0)
                //{
                //    var response = await getUserByIpMethod(connection, requestModel.CustomerIp);
                //    if (response.Count() == 0)
                //    {
                //        throw new Exception("User not found.");
                //    }else if (response.Count() == 1)
                //    {
                //        var itemDtails = response.Single();
                //        userInfoInsideRouter = await ParseModel(itemDtails.Words);

                //    }
                //    else
                //    {

                //        foreach( var item in response)
                //        {
                //            var singleInfo = new GetMikrotikRouterUserInfoResponseModel();
                //            singleInfo = await ParseModel(item.Words);
                //            newList.Add(singleInfo);
                //        }
                //        userInfoInsideRouter = newList;
                //    }
                //}
                //else
                //{
                //    if(blockResponse.Count() == 1)
                //    {
                //        var blockItemDtails = blockResponse.Single();
                //        userInfoInsideRouter = await ParseModel(blockItemDtails.Words);
                //    }
                //    else
                //    {
                //        foreach (var item in blockResponse)
                //        {
                //            var singleInfo = new GetMikrotikRouterUserInfoResponseModel();
                //            singleInfo = await ParseModel(item.Words);
                //            newList.Add(singleInfo);
                //        }
                //        userInfoInsideRouter = newList;
                //    }

                //}
                var response = await getUserByIpMethod(connection, requestModel.CustomerIp);
                if (response.Count() == 0)
                {
                    throw new Exception("User not found.");
                }
                else 
                {
                    foreach (var item in response)
                    {
                        var singleInfo = new GetMikrotikRouterUserInfoResponseModel();
                        singleInfo = await ParseModel(item.Words, requestModel.RouterIp);
                        newList.Add(singleInfo);
                    }
                }
                
                var apiResponse = new ApiResponse()
                {
                    Message = "get data",
                    Status = "success",
                    StatusCode = 200,
                    Data = newList
                };
                await InsertRequestResponse(requestModel, apiResponse, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
                return apiResponse;
            }
        }
        catch (Exception ex)
        {
            await errorMethord(ex, methordName);
            var response = new ApiResponse()
            {
                Message = ex.Message,
                Status = "Error",
                StatusCode = 400
            };
            await InsertRequestResponse(requestModel, response, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
            return response;
        }
    }

    public ApiResponse GetAllUsersInfoFromMikrotikRouter(MikrotikRouterFilterParams requestModel)
    {
        var methordName = "CamsService/GetAllUsersInfoFromMikrotikRouter";
        try
        {
            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);
                var newList = new List<GetMikrotikRouterUserInfoResponseModel>();
                var userInfoInsideRouter = new GetMikrotikRouterUserInfoResponseModel();

                var response = getAllUsersMethod(connection, requestModel);
                if (response.Count() == 0)
                {
                    throw new Exception("User not found.");
                }
                else
                {
                    foreach (var item in response)
                    {
                        var singleInfo = new GetMikrotikRouterUserInfoResponseModel();
                        singleInfo = ParseModel(item.Words, requestModel.RouterIp).Result;
                        singleInfo.UniqueId = requestModel.UniqueId;
                        singleInfo.UserId = requestModel.UserId;
                        newList.Add(singleInfo);
                    }
                }
                _camsContext.MikrotikRouterUserInfos.AddRange(newList);
                _camsContext.SaveChanges();
                var apiResponse = new ApiResponse()
                {
                    Message = "get data",
                    Status = "success",
                    StatusCode = 200,
                    Data = newList
                };
                InsertRequestResponseSync(requestModel, apiResponse, requestModel.CustomerIP, requestModel.RouterIp, methordName, requestModel.CallerId);
                return apiResponse;
            }
        }
        catch (Exception ex)
        {
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Exception " + errormessage);
            var response = new ApiResponse()
            {
                Message = ex.Message,
                Status = "Error",
                StatusCode = 400
            };
            InsertRequestResponseSync(requestModel, response, requestModel.CustomerIP, requestModel.RouterIp, methordName, requestModel.CallerId);
            return response;
        }
    }

    public async Task<ApiResponse> AddUserInMikrotikRouter(AddUserInfoInMikrotikRouterRequestModel requestModel)
    {
        var methordName = "CamsService/AddUserInMikrotikRouter";
        try
        {
            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);

                await addUserMethod(connection, requestModel.LBLpackageplan, requestModel.TXTRealIP,
                    requestModel.MySubId);

                var response = new ApiResponse()
                {
                    Message = "create data",
                    Status = "success",
                    StatusCode = 200
                };
                await InsertRequestResponse(requestModel, response, requestModel.TXTRealIP, requestModel.RouterIp, methordName, requestModel.CallerId, requestModel.MySubId);
                return response;
            }
        }
        catch (Exception ex)
        {
            string errormessage = await errorMethord(ex, methordName);
            var response = new ApiResponse()
            {
                Message = errormessage,
                Status = "Error",
                StatusCode = 400
            };
            await InsertRequestResponse(requestModel, response, requestModel.TXTRealIP, requestModel.RouterIp, methordName, requestModel.CallerId, requestModel.MySubId);
            return response;
        }
    }

    public ApiResponse AddUserInMikrotikRouterSyncMethod(AddUserInfoInMikrotikRouterRequestModel requestModel)
    {
        var methordName = "CamsService/AddUserInMikrotikRouterSyncMethod";
        try
        {
            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);

                //addUserMethod(connection, requestModel.LBLpackageplan, requestModel.TXTRealIP, requestModel.MySubId);
                var command = connection.CreateCommandAndParameters("/ip/firewall/address-list/add",
                               "list", requestModel.LBLpackageplan, "address", requestModel.TXTRealIP, "comment", requestModel.MySubId);
                command.ExecuteNonQuery();
                var response = new ApiResponse()
                {
                    Message = "create data",
                    Status = "success",
                    StatusCode = 200
                };
                InsertRequestResponseSync(requestModel, response, requestModel.TXTRealIP, requestModel.RouterIp, methordName, requestModel.CallerId, requestModel.MySubId);
                return response;
            }
        }
        catch (Exception ex)
        {
            string errormessage = errorMethord(ex, methordName).Result;
            var response = new ApiResponse()
            {
                Message = errormessage,
                Status = "Error",
                StatusCode = 400
            };
            InsertRequestResponseSync(requestModel, response, requestModel.TXTRealIP, requestModel.RouterIp, methordName, requestModel.CallerId, requestModel.MySubId);
            return response;
        }
    }

    public async Task<ApiResponse> BlockUserInfoFromMikrotikRouter(BlockUserFromMikrotikRouterRequestModel requestModel)
    {
        var methordName = "CamsService/BlockUserInfoFromMikrotikRouter";
        try
        {
            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);
                var newList = new List<GetMikrotikRouterUserInfoResponseModel>();
                var blockPlan = "Block";
                var realInfo = new GetMikrotikRouterUserInfoResponseModel();

                var getCustomer = await getUserByIpMethod(connection, requestModel.CustomerIp);
                if (getCustomer.Count() == 0)
                {
                    throw new Exception("User not found.");
                }
                else
                {
                    foreach (var item in getCustomer)
                    {
                        var singleInfo = new GetMikrotikRouterUserInfoResponseModel();
                        singleInfo = await ParseModel(item.Words, requestModel.RouterIp);
                        if(singleInfo.comment != null)
                        {
                            newList.Add(singleInfo);
                        }                        
                    }
                }

                realInfo = newList.FirstOrDefault(x=> !string.IsNullOrEmpty(x.list) && x.list != "Block" && !string.IsNullOrEmpty(x.comment));
                if(realInfo != null)
                {
                    var delData = newList.Where(x => x.Id != realInfo.Id).ToList();
                    //var delData = newList.Where(x => !string.IsNullOrEmpty(x.list) && x.list != "Block" && !string.IsNullOrEmpty(x.comment) && x.disabled == "false").ToList();

                    if (delData.Count() > 1)
                    {
                        foreach (var item in delData)
                        {
                            await DeleteMethod(connection, item.Id);
                        }
                    }

                    var cus_Id = realInfo.Id;
                    var cus_address = realInfo.address;
                    var cus_SubId = realInfo.comment;

                    // disabled the user start (disable : true)
                    await UserDisableOrEnableMethod(connection, cus_Id, true);
                    // disabled the user End

                    // Add Block infromation Start
                    await addUserMethod(connection, blockPlan, cus_address, cus_SubId);
                    // Add Block infromation End

                    
                }
                else
                {
                    //foreach (var item in newList)
                    //{
                    //    await DeleteMethod(connection, item.Id);
                    //}
                    throw new Exception("Proper data is not get");
                }

                

                var apiResponse = new ApiResponse()
                {
                    Message = "User is blocked",
                    Status = "success",
                    StatusCode = 200,
                    Data = newList
                };
                await InsertRequestResponse(requestModel, apiResponse, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
                return apiResponse;
            }
        }
        catch (Exception ex)
        {
            string errormessage = await errorMethord(ex, methordName);

            var response = new ApiResponse()
            {
                Message = errormessage,
                Status = "Error",
                StatusCode = 400
            };
            await InsertRequestResponse(requestModel, response, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
            return response;
        }
    }

    public ApiResponse BlockUserInfoFromMikrotikRouterSyncMethod(BlockUserFromMikrotikRouterRequestModel requestModel)
    {
        var methordName = "CamsService/BlockUserInfoFromMikrotikRouterSyncMethod";
        try
        {
            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);
                
                var newList = new List<GetMikrotikRouterUserInfoResponseModel>();
                var blockPlan = "Block";
                var realInfo = new GetMikrotikRouterUserInfoResponseModel();

                
                var getInfo = connection.CreateCommandAndParameters("/ip/firewall/address-list/print",
                "address", requestModel.CustomerIp);
                var getCustomer = getInfo.ExecuteList();
                if (getCustomer.Count() == 0)
                {
                    throw new Exception("User not found.");
                }
                else
                {
                    foreach (var item in getCustomer)
                    {
                        var singleInfo = new GetMikrotikRouterUserInfoResponseModel();
                        singleInfo = ParseModel(item.Words, requestModel.RouterIp).Result;
                        if (singleInfo.comment != null)
                        {
                            newList.Add(singleInfo);
                        }
                    }
                }

                realInfo = newList.FirstOrDefault(x => !string.IsNullOrEmpty(x.list) && x.list != "Block" && !string.IsNullOrEmpty(x.comment));
                if (realInfo != null)
                {
                    //var delData = newList.Where(x=> !string.IsNullOrEmpty(x.list) && x.list != "Block" && !string.IsNullOrEmpty(x.comment) && x.disabled == "false").ToList();
                    var delData = newList.Where(x => x.Id != realInfo.Id).ToList();

                    if (delData.Count() > 0)
                    {
                        foreach (var item in delData)
                        {
                            DeleteMethod(connection, item.Id);
                        }
                    }

                    var cus_Id = realInfo.Id;
                    var cus_address = realInfo.address;
                    var cus_SubId = realInfo.comment;

                    // disabled the user start (disable : true)
                   
                    var updateCmd = connection.CreateCommandAndParameters("/ip/firewall/address-list/set",
                       "disabled", "true", TikSpecialProperties.Id, cus_Id);
                    updateCmd.ExecuteNonQuery();
                    // disabled the user End

                    // Add Block infromation Start
                    var command = connection.CreateCommandAndParameters("/ip/firewall/address-list/add",
                    "list", blockPlan, "address", cus_address, "comment", cus_SubId);
                    command.ExecuteNonQuery();
                    // Add Block infromation End

                }
                else
                {
                    //foreach (var item in newList)
                    //{
                    //    DeleteMethod(connection, item.Id);
                    //}
                    throw new Exception("Proper data is not get");
                }

                var apiResponse = new ApiResponse()
                {
                    Message = "User is blocked",
                    Status = "success",
                    StatusCode = 200,
                    Data = newList
                };
                InsertRequestResponseSync(requestModel, apiResponse, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
                return apiResponse;
            }
        }
        catch (Exception ex)
        {
            //string errormessage = await errorMethord(ex, methordName);
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Exception " + errormessage);
            
            var response = new ApiResponse()
            {
                Message = errormessage,
                Status = "Error",
                StatusCode = 400
            };
            InsertRequestResponseSync(requestModel, response, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
            return response;
        }
    }

    public async Task<ApiResponse> UnblockUserInfoFromMikrotikRouter(
        UnblockUserFromMikrotikRouterRequestModel requestModel)
    {
        var methordName = "CamsService/UnblockUserInfoFromMikrotikRouter";
        try
        {
            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);
                var newList = new List<GetMikrotikRouterUserInfoResponseModel>();
                
                //Get user original information
                var getCustomer = await getUserByIpMethod(connection, requestModel.CustomerIp);
                if (getCustomer.Count() == 0)
                {
                    throw new Exception("user not found.");
                }

                foreach (var item in getCustomer)
                {
                    var singleInfo = new GetMikrotikRouterUserInfoResponseModel();
                    singleInfo = ParseModel(item.Words, requestModel.RouterIp).Result;
                    if (singleInfo.comment != null)
                    {
                        newList.Add(singleInfo);
                    }
                }

                var blockData = newList.FirstOrDefault(x => x.list == "Block");
                if(blockData != null)
                {
                    await DeleteMethod(connection, blockData.Id);
                }
                var enableData = newList.FirstOrDefault(x => x.list != "Block" && !string.IsNullOrEmpty(x.comment));
                if (enableData != null)
                {
                    await UserDisableOrEnableMethod(connection, enableData.Id, false);
                }
               

                var apiResponse = new ApiResponse()
                {
                    Message = "User is unblocked",
                    Status = "success",
                    StatusCode = 200,
                    Data = newList
                };
                await InsertRequestResponse(requestModel, apiResponse, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
                return apiResponse;
            }
        }
        catch (Exception ex)
        {
            string errormessage = await errorMethord(ex, methordName);

            var response = new ApiResponse()
            {
                Message = errormessage,
                Status = "Error",
                StatusCode = 400
            };
            await InsertRequestResponse(requestModel, response, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
            return response;
        }
    }

    public ApiResponse UnblockUserInfoFromMikrotikRouterSyncMethod(UnblockUserFromMikrotikRouterRequestModel requestModel)
    {
        var methordName = "CamsService/UnblockUserInfoFromMikrotikRouter";
        try
        {
            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);
                ////var blockResponse = await BlockCheckMethod(connection, requestModel.CustomerIp);
                //var blockPlan = "Block";
                //var getBlockInfo = connection.CreateCommandAndParameters("/ip/firewall/address-list/print",
                //    "address", requestModel.CustomerIp, "list", blockPlan);
                //var blockResponse = getBlockInfo.ExecuteList();
                //if (blockResponse.Count() != 0)
                //{
                //    var blockCusId = blockResponse.Single().GetId();
                //    // Delete the user block informaion
                //    //await DeleteMethod(connection, blockCusId);
                //    var deletePlan = connection.CreateCommandAndParameters("/ip/firewall/address-list/remove",
                //    TikSpecialProperties.Id, blockCusId);
                //    deletePlan.ExecuteNonQuery();
                //}
                //else
                //{
                //    throw new Exception("User is not block yet or user is not exist.");
                //}

                ////Get user original information
                ////var getCustomer = await getUserByIpMethod(connection, requestModel.CustomerIp);
                //var getInfo = connection.CreateCommandAndParameters("/ip/firewall/address-list/print",
                //"address", requestModel.CustomerIp);
                //var getCustomer = getInfo.ExecuteList();
                //if (getCustomer.Count() == 0)
                //{
                //    throw new Exception("user not found.");
                //}

                //var cusId = getCustomer.Single().GetId();

                //// Enable the user
                ////await UserDisableOrEnableMethod(connection, cusId, false);

                //var updateCmd = connection.CreateCommandAndParameters("/ip/firewall/address-list/set",
                //"disabled", "false", TikSpecialProperties.Id, cusId);
                //updateCmd.ExecuteNonQuery();

                //var apiResponse = new ApiResponse()
                //{
                //    Message = "User is unblocked",
                //    Status = "success",
                //    StatusCode = 200
                //};
                //InsertRequestResponseSync(requestModel, apiResponse, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
                //return apiResponse;

                var newList = new List<GetMikrotikRouterUserInfoResponseModel>();

                //Get user original information
                //var getCustomer = await getUserByIpMethod(connection, requestModel.CustomerIp);
                var getInfo = connection.CreateCommandAndParameters("/ip/firewall/address-list/print",
                "address", requestModel.CustomerIp);
                var getCustomer = getInfo.ExecuteList();
                if (getCustomer.Count() == 0)
                {
                    throw new Exception("user not found.");
                }

                foreach (var item in getCustomer)
                {
                    var singleInfo = new GetMikrotikRouterUserInfoResponseModel();
                    singleInfo = ParseModel(item.Words, requestModel.RouterIp).Result;
                    if (singleInfo.comment != null)
                    {
                        newList.Add(singleInfo);
                    }
                }

                var blockData = newList.FirstOrDefault(x => x.list == "Block");
                if (blockData != null)
                {
                    DeleteMethod(connection, blockData.Id);
                }
                var enableData = newList.FirstOrDefault(x => x.list != "Block" && !string.IsNullOrEmpty(x.comment));
                if (enableData != null)
                {
                    UserDisableOrEnableMethod(connection, enableData.Id, false);
                }


                var apiResponse = new ApiResponse()
                {
                    Message = "User is unblocked",
                    Status = "success",
                    StatusCode = 200,
                    Data = newList
                };
                InsertRequestResponseSync(requestModel, apiResponse, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
                return apiResponse;
            }
        }
        catch (Exception ex)
        {
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Exception " + errormessage);

            var response = new ApiResponse()
            {
                Message = errormessage,
                Status = "Error",
                StatusCode = 400
            };
            InsertRequestResponseSync(requestModel, response, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
            return response;
        }
    }



    public async Task<ApiResponse> DeleteUserInfoFromMikrotikRouter(
        DeleteUserFromMikrotikRouerRequestModel requestModel)
    {
        var methordName = "CamsService/DeleteUserInfoFromMikrotikRouter";
        try
        {
            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);

                var newList = new List<GetMikrotikRouterUserInfoResponseModel>();
                var getCustomer = await getUserByIpMethod(connection, requestModel.CustomerIp);
                if (getCustomer.Count() == 0)
                {
                    throw new Exception("user not found");
                }
                foreach (var item in getCustomer)
                {
                    var singleInfo = new GetMikrotikRouterUserInfoResponseModel();
                    singleInfo = await ParseModel(item.Words, requestModel.RouterIp);
                    newList.Add(singleInfo);
                    await DeleteMethod(connection, singleInfo.Id);
                }               

                var apiResponse = new ApiResponse()
                {
                    Message = "User is deleted",
                    Status = "success",
                    StatusCode = 200,
                    Data = newList
                };
                await InsertRequestResponse(requestModel, apiResponse, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
                return apiResponse;
            }
        }
        catch (Exception ex)
        {
            string errormessage = await errorMethord(ex, methordName);

            var response = new ApiResponse()
            {
                Message = errormessage,
                Status = "Error",
                StatusCode = 400
            };
            await InsertRequestResponse(requestModel, response, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
            return response;
        }
    }


    private async Task<GetMikrotikRouterUserInfoResponseModel> ParseModel(IReadOnlyDictionary<string, string> words, string routerIp)
    {
        var newData = new GetMikrotikRouterUserInfoResponseModel();
        foreach (var info in words)
        {
            //row.Add(info.Key, info.Value);
            if (info.Key == ".id")
            {
                newData.Id = info.Value;
            }
            else if (info.Key == "list")
            {
                newData.list = info.Value;
            }
            else if (info.Key == "address")
            {
                newData.address = info.Value;
            }
            else if (info.Key == "creation-time")
            {
                newData.CreationTime = info.Value;
            }
            else if (info.Key == "dynamic")
            {
                newData.dynamic = info.Value;
            }
            else if (info.Key == "disabled")
            {
                newData.disabled = info.Value;
            }
            else if (info.Key == "comment")
            {
                newData.comment = info.Value;
            }
        }
        newData.RouterIp = routerIp;
        newData.CreatedAt = DateTime.Now;

        return newData;
    }

    private async Task<GetMikrotikRouterInterfaceModel> ParseModelInterface(IReadOnlyDictionary<string, string> words, string routerIp)
    {
        var newData = new GetMikrotikRouterInterfaceModel();
        foreach (var info in words)
        {
            //row.Add(info.Key, info.Value);
            if (info.Key == ".id")
            {
                newData.ItemId = info.Value;
            }
            else if (info.Key == "name")
            {
                newData.Name = info.Value;
            }
            else if (info.Key == "default-name")
            {
                newData.DefaultName = info.Value;
            }
            else if (info.Key == "type")
            {
                newData.Type = info.Value;
            }
            else if (info.Key == "mtu")
            {
                newData.MTU = info.Value;
            }
            else if (info.Key == "actual-mtu")
            {
                newData.ActualMTU = info.Value;
            }
            else if (info.Key == "l2mtu")
            {
                newData.L2MTU = info.Value;
            }
            else if (info.Key == "max-l2mtu")
            {
                newData.MaxL2MTU = info.Value;
            }
            else if (info.Key == "mac-address")
            {
                newData.MacAddress = info.Value;
            }
            else if (info.Key == "last-link-down-time")
            {
                newData.LastLinkDownTime = info.Value;
            }
            else if (info.Key == "last-link-up-time")
            {
                newData.LastLinkUpTime = info.Value;
            }
            else if (info.Key == "link-downs")
            {
                newData.LinkDowns = info.Value;
            }
            else if (info.Key == "rx-byte")
            {
                newData.RXByte = info.Value;
            }
            else if (info.Key == "tx-byte")
            {
                newData.TXByte = info.Value;
            }
            else if (info.Key == "rx-packet")
            {
                newData.RXPacket = info.Value;
            }
            else if (info.Key == "tx-packet")
            {
                newData.TXPacket = info.Value;
            }
            else if (info.Key == "rx-drop")
            {
                newData.RXDrop = info.Value;
            }
            else if (info.Key == "tx-drop")
            {
                newData.TXDrop = info.Value;
            }
            else if (info.Key == "tx-queue-Drop")
            {
                newData.TXQueueDrop = info.Value;
            }
            else if (info.Key == "rx-error")
            {
                newData.RXError = info.Value;
            }
            else if (info.Key == "tx-error")
            {
                newData.TXError = info.Value;
            }
            else if (info.Key == "fp-rx-byte")
            {
                newData.FpRxByte = info.Value;
            }
            else if (info.Key == "fp-tx-byte")
            {
                newData.FpTxByte = info.Value;
            }
            else if (info.Key == "fp-rx-Packet")
            {
                newData.FpRxPacket = info.Value;
            }
            else if (info.Key == "fp-tx-Packet")
            {
                newData.FpTxPacket = info.Value;
            }
            else if (info.Key == "running")
            {
                newData.Running = info.Value;
            }
            else if (info.Key == "disabled")
            {
                newData.Disabled = info.Value;
            }
            
        }
        newData.RouterIp = routerIp;
        newData.CreatedAt = DateTime.Now;

        return newData;
    }

    private async Task<GetAllMikrotikRouterQueueInfoResponseModel> ParseQueueDataModel(IReadOnlyDictionary<string, string> words, string routerIp)
    {
        var newData = new GetAllMikrotikRouterQueueInfoResponseModel();
        foreach (var info in words)
        {
            //row.Add(info.Key, info.Value);
            if (info.Key == ".id")
            {
                newData.id = info.Value;
            }
            else if (info.Key == "name")
            {
                newData.name = info.Value;
            }
            else if (info.Key == "target")
            {
                newData.target = info.Value;
            }
            else if (info.Key == "parent")
            {
                newData.parent = info.Value;
            }
            else if (info.Key == "priority")
            {
                newData.priority = info.Value;
            }
            else if (info.Key == "queue")
            {
                newData.queue = info.Value;
            }
            else if (info.Key == "limit-at")
            {
                newData.limitat = info.Value;
            }
            else if (info.Key == "max-limit")
            {
                newData.maxlimit = info.Value;
            }
            else if (info.Key == "burst-limit")
            {
                newData.burstlimit = info.Value;
            }
            else if (info.Key == "burst-threshold")
            {
                newData.burstthreshold = info.Value;
            }
            else if (info.Key == "burst-time")
            {
                newData.bursttime = info.Value;
            }
            else if (info.Key == "bucket-size")
            {
                newData.bucketsize = info.Value;
            }
            else if (info.Key == "bytes")
            {
                newData.bytes = info.Value;
            }
            else if (info.Key == "total-bytes")
            {
                newData.totalbytes = info.Value;
            }
            else if (info.Key == "packets")
            {
                newData.packets = info.Value;
            }
            else if (info.Key == "total-packets")
            {
                newData.totalpackets = info.Value;
            }
            else if (info.Key == "dropped")
            {
                newData.dropped = info.Value;
            }
            else if (info.Key == "total-dropped")
            {
                newData.totaldropped = info.Value;
            }
            else if (info.Key == "rate")
            {
                newData.rate = info.Value;
            }
            else if (info.Key == "total-rate")
            {
                newData.totalrate = info.Value;
            }
            else if (info.Key == "packet-rate")
            {
                newData.packetrate = info.Value;
            }
            else if (info.Key == "total-packet-rate")
            {
                newData.totalpacketrate = info.Value;
            }
            else if (info.Key == "queued-packets")
            {
                newData.queuedpackets = info.Value;
            }
            else if (info.Key == "total-queued-packets")
            {
                newData.totalqueuedpackets = info.Value;
            }
            else if (info.Key == "queued-bytes")
            {
                newData.queuedbytes = info.Value;
            }
            else if (info.Key == "total-queued-bytes")
            {
                newData.totalqueuedpackets = info.Value;
            }
            else if (info.Key == "invalid")
            {
                newData.invalid = info.Value;
            }
            else if (info.Key == "dynamic")
            {
                newData.dynamic = info.Value;
            }
            else if (info.Key == "disabled")
            {
                newData.disabled = info.Value;
            }
            
        }
        newData.RouterIp = routerIp;
        newData.CreatedAt = DateTime.Now;

        return newData;
    }

    private async Task<IEnumerable<ITikReSentence>> BlockCheckMethod(ITikConnection connection, string userIp)
    {
        var blockPlan = "Block";
        var getBlockInfo = connection.CreateCommandAndParameters("/ip/firewall/address-list/print",
            "address", userIp, "list", blockPlan);
        var blockResponse = getBlockInfo.ExecuteList();
        return blockResponse;
    }

    private async Task DeleteMethod(ITikConnection connection, string cusId)
    {
        var deletePlan = connection.CreateCommandAndParameters("/ip/firewall/address-list/remove",
            TikSpecialProperties.Id, cusId);
        deletePlan.ExecuteNonQuery();
    }

    private async Task<IEnumerable<ITikReSentence>> getUserByIpMethod(ITikConnection connection, string userIp)
    {
        var getInfo = connection.CreateCommandAndParameters("/ip/firewall/address-list/print",
            "address", userIp);
        var response = getInfo.ExecuteList();
        return response;
    }

    private  IEnumerable<ITikReSentence> getAllUsersMethod(ITikConnection connection, MikrotikRouterFilterParams model)
    {
        var strDisable = "";
        if (model.Disable == "t")
        {
            strDisable = "true";
        }
        else if(model.Disable == "f")
        {
            strDisable = "true";
        }
        else
        {
            strDisable = "";
        }
        int valueCount = 0;
        if (!string.IsNullOrEmpty(model.Package))
        {
            valueCount += 2;
        }
        if (!string.IsNullOrEmpty(strDisable))
        {
            valueCount += 2;
        }
        if (!string.IsNullOrEmpty(model.CustomerIP))
        {
            valueCount += 2;
        }
        if (!string.IsNullOrEmpty(model.CustomerId))
        {
            valueCount += 2;
        }

        string[] pram = new string[valueCount];

        int setIndex = 0;
        if (!string.IsNullOrEmpty(model.Package))
        {
            pram[setIndex] = "list";
            setIndex += 1;
            pram[setIndex] = model.Package;
        }
        if (!string.IsNullOrEmpty(strDisable))
        {
            if (setIndex != 0)
            {
                setIndex += 1;
            }
            pram[setIndex] = "disabled";
            setIndex += 1;
            pram[setIndex] = strDisable;
        }
        if (!string.IsNullOrEmpty(model.CustomerIP))
        {
            if (setIndex != 0)
            {
                setIndex += 1;
            }
            pram[setIndex] = "address";
            setIndex += 1;
            pram[setIndex] = model.CustomerIP;
        }
        if (!string.IsNullOrEmpty(model.CustomerId))
        {
            if (setIndex != 0)
            {
                setIndex += 1;
            }
            pram[setIndex] = "comment";
            setIndex += 1;
            pram[setIndex] = model.CustomerId;
        }



        if (pram.Length > 0)
        {
            var getInfo = connection.CreateCommandAndParameters("/ip/firewall/address-list/print", pram);
            var response = getInfo.ExecuteList();
            return response;
        }
        else
        {
            var getInfo = connection.CreateCommandAndParameters("/ip/firewall/address-list/print");
            var response = getInfo.ExecuteList();
            return response;
        }       
        
    }

    private async Task UserDisableOrEnableMethod(ITikConnection connection, string cus_Id, bool isDisable)
    {
        var updateCmd = connection.CreateCommandAndParameters("/ip/firewall/address-list/set",
            "disabled", isDisable ? "true":"false",
            TikSpecialProperties.Id, cus_Id);
        updateCmd.ExecuteNonQuery();
    }

    private async Task addUserMethod(ITikConnection connection, string packagePlan, string cus_id, string subId)
    {
        var command = connection.CreateCommandAndParameters("/ip/firewall/address-list/add",
            "list", packagePlan,
            "address", cus_id,
            "comment", subId);
        command.ExecuteNonQuery();
    }

    private async Task<string> errorMethord(Exception ex, string info)
    {
        string errormessage = "Error : " + ex.Message.ToString();
        _logger.LogInformation("Method Name : " + info + ", Exception " + errormessage);
        await _systemService.ErrorLogEntry(info, info, errormessage);
        //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
        return errormessage;
    }

    private async Task InsertRequestResponse(object request, object response, string cusIp, string routerIp, string methordName, string userId, string subId = null)
    {
        var errorMethordName = "CamsService/InsertRequestResponse";
        try
        {
            var reqResModel = new MikrotikRequestResponse()
            {
                Request = JsonConvert.SerializeObject(request),
                Response = JsonConvert.SerializeObject(response),
                MethordName = methordName,
                CustomerIp = cusIp,
                RouterIp = routerIp,
                CreatedAt = DateTime.Now,
                UserId = userId,
                SubId = subId
            };
            await _camsContext.MikrotikRequestResponses.AddAsync(reqResModel);
            await _camsContext.SaveChangesAsync();
            _logger.LogInformation("Insert" + JsonConvert.SerializeObject(reqResModel));
        }
        catch (Exception ex)
        {
            string errormessage = errorMethord(ex, errorMethordName).Result;
        }
    }

    private void InsertRequestResponseSync(object request, object response, string cusIp, string routerIp, string methordName, string userId, string subId = null)
    {
        var errorMethordName = "CamsService/InsertRequestResponseSync";
        try
        {
            var reqResModel = new MikrotikRequestResponse()
            {
                Request = JsonConvert.SerializeObject(request),
                Response = System.Convert.ToString(JsonConvert.SerializeObject(response)),
                MethordName = methordName,
                CustomerIp = cusIp,
                RouterIp = routerIp,
                CreatedAt = DateTime.Now,
                UserId = userId,
                SubId = subId
            };
            _camsContext.MikrotikRequestResponses.Add(reqResModel);
            _camsContext.SaveChanges();
            _logger.LogInformation("Insert" + JsonConvert.SerializeObject(reqResModel));
        }
        catch (Exception ex)
        {
            string errormessage = errorMethord(ex, errorMethordName).Result;
        }
    }

    public ApiResponse BlockListsUserInfoFromMikrotikRouterSyncMethod(BlockUserListsFromMikrotikRouterRequestModel requestModel)
    {
        var methordName = "CamsService/BlockListsUserInfoFromMikrotikRouterSyncMethod";
        try
        {
            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);

                var blockPlan = "Block";
                var cus_address = requestModel.CustomerIp;
                var cus_SubId = requestModel.SubId;
                string packagePlan = requestModel.Package;
                
                // Checking customer Block request exist

                var newList = new List<GetMikrotikRouterUserInfoResponseModel>();
                var realInfo = new GetMikrotikRouterUserInfoResponseModel();

                var getInfo = connection.CreateCommandAndParameters("/ip/firewall/address-list/print",
                "address", requestModel.CustomerIp);
                var getCustomer = getInfo.ExecuteList();
                if (getCustomer.Count() > 0)
                {
                    foreach (var item in getCustomer)
                    {
                        var singleInfo = new GetMikrotikRouterUserInfoResponseModel();
                        singleInfo = ParseModel(item.Words, requestModel.RouterIp).Result;
                        if (singleInfo.comment != null)
                        {
                            newList.Add(singleInfo);
                        }
                    }

                    realInfo = newList.FirstOrDefault(x => !string.IsNullOrEmpty(x.list) && x.list == "Block");

                    if (realInfo != null) {
                        var apiBlockExistResponse = new ApiResponse()
                        {
                            Message = "User is already blocked",
                            Status = "success",
                            StatusCode = 200,
                            Data = ""
                        };
                        InsertRequestResponseSync(requestModel, apiBlockExistResponse, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
                        return apiBlockExistResponse;
                    }
                    
                    //throw new Exception("User block resqest already exist.");
                }

                var commandPackage = connection.CreateCommandAndParameters("/ip/firewall/address-list/add",
                    "list", packagePlan,
                    "address", cus_address,
                    "comment", cus_SubId);
                commandPackage.ExecuteNonQuery();

                var updateCmd = connection.CreateCommandAndParameters("/ip/firewall/address-list/set",
                       "disabled", "true", TikSpecialProperties.Id, cus_SubId);
                updateCmd.ExecuteNonQuery();

                var command = connection.CreateCommandAndParameters("/ip/firewall/address-list/add",
                "list", blockPlan, "address", cus_address, "comment", cus_SubId);
                command.ExecuteNonQuery();

                var apiResponse = new ApiResponse()
                {
                    Message = "User is blocked",
                    Status = "success",
                    StatusCode = 200,
                    Data = ""
                };
                InsertRequestResponseSync(requestModel, apiResponse, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
                return apiResponse;
            }
        }
        catch (Exception ex)
        {
            //string errormessage = await errorMethord(ex, methordName);
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Exception " + errormessage);

            var response = new ApiResponse()
            {
                Message = errormessage,
                Status = "Error",
                StatusCode = 400
            };
            InsertRequestResponseSync(requestModel, response, requestModel.CustomerIp, requestModel.RouterIp, methordName, requestModel.CallerId);
            return response;
        }
    }

    public ApiResponse GetAllQueueFromMikrotikRouter(GetAllQueueRequestModel requestModel)
    {
        requestModel.Target = requestModel.Target + "/32";
        var methordName = "CamsService/GetAllQueueFromMikrotikRouter";
        try
        {
            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);

                var newList = new List<GetAllMikrotikRouterQueueInfoResponseModel>();
                IEnumerable<ITikReSentence> getCustomer;

                int valueCount = 0;
                if (!string.IsNullOrEmpty(requestModel.Target))
                {
                    valueCount += 2;
                }
                if (!string.IsNullOrEmpty(requestModel.Name))
                {
                    valueCount += 2;
                }
                

                string[] pram = new string[valueCount];

                int setIndex = 0;
                if (!string.IsNullOrEmpty(requestModel.Target))
                {
                    pram[setIndex] = "target";
                    setIndex += 1;
                    pram[setIndex] = requestModel.Target;
                }
                if (!string.IsNullOrEmpty(requestModel.Name))
                {
                    if (setIndex != 0)
                    {
                        setIndex += 1;
                    }
                    pram[setIndex] = "name";
                    setIndex += 1;
                    pram[setIndex] = requestModel.Name;
                }


                if (pram.Length > 0)
                {
                    var getInfo = connection.CreateCommandAndParameters("/queue/simple/print", pram);
                    getCustomer = getInfo.ExecuteList();
                }
                else
                {
                    var getInfo = connection.CreateCommandAndParameters("/queue/simple/print");
                    getCustomer = getInfo.ExecuteList();
                }
                if (getCustomer.Count() == 0)
                {
                    throw new Exception("user not found");
                }
                foreach (var item in getCustomer)
                {
                    var singleInfo = new GetAllMikrotikRouterQueueInfoResponseModel();
                    singleInfo = ParseQueueDataModel(item.Words, requestModel.RouterIp).Result;
                    newList.Add(singleInfo);
                }

                var apiResponse = new ApiResponse()
                {
                    Message = "User is deleted",
                    Status = "success",
                    StatusCode = 200,
                    Data = newList
                };
                InsertRequestResponseSync(requestModel, apiResponse, null, requestModel.RouterIp, methordName, requestModel.CallerId);
                return apiResponse;
            }
        }
        catch (Exception ex)
        {
            string errormessage = errorMethord(ex, methordName).Result;

            var response = new ApiResponse()
            {
                Message = errormessage,
                Status = "Error",
                StatusCode = 400
            };
            InsertRequestResponseSync(requestModel, response, null, requestModel.RouterIp, methordName, requestModel.CallerId);
            return response;
        }
    }

    public ApiResponse SetQueueInfoIntoMikrotikRouter(SetQueueIntoMikrotikRouterRequestModel model)
    {
        model.Target = model.Target + "/32";
        var methordName = "CamsService/SetQueueInfoIntoMikrotikRouter";
        try
        {
            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(model.RouterIp, model.UserName, model.Password);

                var newList = new List<GetAllMikrotikRouterQueueInfoResponseModel>();

                var getInfoByName = connection.CreateCommandAndParameters("/queue/simple/print", "name", model.Name);
                var getCustomerByName = getInfoByName.ExecuteList();                
                if(getCustomerByName.Count() > 0)
                {
                    var nameInfo = removedQueueData(connection, getCustomerByName);
                    newList.AddRange(nameInfo);
                }
                var getInfoTarget = connection.CreateCommandAndParameters("/queue/simple/print", "target", model.Target);
                var getCustomerByTarget = getInfoTarget.ExecuteList();                
                if (getCustomerByTarget.Count() > 0)
                {
                    var targetInfo = removedQueueData(connection, getCustomerByTarget);
                    newList.AddRange(targetInfo);
                }
                var command = connection.CreateCommandAndParameters("/queue/simple/add",
                                "name", model.Name,
                                "target", model.Target,
                                "max-limit", model.Maxlimit);
                command.ExecuteNonQuery();

                var response = new ApiResponse()
                {
                    Message = "create data",
                    Status = "success",
                    StatusCode = 200,
                    Data = newList
                };
                InsertRequestResponseSync(model, response, model.Target, model.RouterIp, methordName, model.CallerId);
                return response;
            }
        }
        catch (Exception ex)
        {
            string errormessage = errorMethord(ex, methordName).Result;
            var response = new ApiResponse()
            {
                Message = errormessage,
                Status = "Error",
                StatusCode = 400
            };
            InsertRequestResponseSync(model, response, model.Target, model.RouterIp, methordName, model.CallerId);
            return response;
        }
    }

    private List<GetAllMikrotikRouterQueueInfoResponseModel> removedQueueData(ITikConnection connection, IEnumerable<ITikReSentence> getCustomer)
    {
        var newList = new List<GetAllMikrotikRouterQueueInfoResponseModel>();
        if (getCustomer.Count() > 0)
        {
            foreach (var item in getCustomer)
            {
                var singleInfo = new GetAllMikrotikRouterQueueInfoResponseModel();
                singleInfo = ParseQueueDataModel(item.Words, null).Result;
                newList.Add(singleInfo);
                DeleteQueue(connection, singleInfo.id);
            }
        }
        return newList;
    }

    private void DeleteQueue(ITikConnection connection, string id)
    {
        var deletePlan = connection.CreateCommandAndParameters("/queue/simple/remove", TikSpecialProperties.Id, id);
        deletePlan.ExecuteNonQuery();
    }

    public async Task<ApiResponse> MikrotikRouterInterfaceData(MikrotikRouterFilterParams requestModel)
    {
        var methordName = "CamsService/TestMethod";
        try
        {

            using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
            {
                connection.Open(requestModel.RouterIp, requestModel.UserName, requestModel.Password);
                var newList = new List<GetMikrotikRouterInterfaceModel>();
                var userInfoInsideRouter = new GetMikrotikRouterUserInfoResponseModel();

                //var response = getAllUsersMethod(connection, requestModel);
                var getInfo = connection.CreateCommandAndParameters("/interface/monitor-traffic");///interface/print
                var response = getInfo.ExecuteList();
                if (response.Count() == 0)
                {
                    throw new Exception("User not found.");
                }
                else
                {
                    foreach (var item in response)
                    {
                        var singleInfo = new GetMikrotikRouterInterfaceModel();
                        singleInfo = ParseModelInterface(item.Words, requestModel.RouterIp).Result;
                        newList.Add(singleInfo);
                    }
                }
                _camsContext.MikrotikRouterInterface.AddRange(newList);
                _camsContext.SaveChanges();
                var apiResponse = new ApiResponse()
                {
                    Message = "get data",
                    Status = "success",
                    StatusCode = 200,
                    Data = newList
                };
                InsertRequestResponseSync(requestModel, apiResponse, requestModel.CustomerIP, requestModel.RouterIp, methordName, requestModel.CallerId);
                return apiResponse;
            }
        }
        catch (Exception ex)
        {
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Exception " + errormessage);
            var response = new ApiResponse()
            {
                Message = ex.Message,
                Status = "Error",
                StatusCode = 400
            };
             InsertRequestResponseSync(requestModel, response, requestModel.CustomerIP, requestModel.RouterIp, methordName, requestModel.CallerId);
            return response;
        }
    }
}
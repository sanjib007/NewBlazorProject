using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using tik4net.Objects.User;

namespace L3T.Infrastructure.Helpers.Implementation.FieldForce
{

    public class RsmChecklistService : IRsmChecklistService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MisDBContext _misDBContext;
        private readonly RsmDbContext _rsmDBContext;
        private readonly ILogger<InstallationTicketService> _logger;
        private readonly FFWriteDBContext _ffWriteDBContext;
        private IConfiguration _configuration;
        private readonly IMailSenderService _mailSenderService;
        public RsmChecklistService(
            IHttpClientFactory httpClientFactory,
            MisDBContext misDBContext,
            RsmDbContext RsmConnection,
            ILogger<InstallationTicketService> logger,
            FFWriteDBContext ffWriteDBContext,
            IConfiguration iconfig,
            IMailSenderService mailSenderService)
        {
            _httpClientFactory = httpClientFactory;
            _misDBContext = misDBContext;
            _rsmDBContext = RsmConnection;
            _logger = logger;
            _ffWriteDBContext = ffWriteDBContext;
            _mailSenderService = mailSenderService;
            _configuration = iconfig;
        }

        public async Task<ApiResponse> AllDataForRSMCheckList(string clientID, string ip, string userId)
        {
            var methodName = "RsmChecklistService/AllDataForRSMCheckList";
            try
            {
                var info = new AllDataForRSMCheckListViewModel();
                var checkList = await GetChecklistData(ip, userId);
                info.ChecklistResponse = (List<ChecklistResponseModel>)checkList.Data;
                var routerType = await GetRouterTypeData(ip, userId);
                info.RouterTypeResponse = (List<RouterTypeResponseModel>)routerType.Data;
                var controllerOwner = await GetControllerOwnerData(ip, userId);
                info.ControllerOwnerResponse = (List<ControllerOwnerResponseModel>)controllerOwner.Data;
                var singleAP = await GetSingleApData(ip, userId);
                info.SingleApResponse = (List<SingleApResponseModel>)singleAP.Data;
                var multiAp = await GetMultipleApData(ip, userId);
                info.MultipleApResponse = (List<MultipleApResponseModel>)multiAp.Data;  
                var channelWidth20MHz = await GetChannelWidth20MHzData(ip, userId);
                info.ChannelWidth20MHzResponse = (List<ChannelWidth20MHzResponseModel>)channelWidth20MHz.Data;
                var ghzEnabledData = await GetGhzEnabledData(ip, userId);
                info.GhzEnabledResponse = (List<GhzEnabledResponseModel>)ghzEnabledData.Data;
                var channelWidthAutoData = await GetChannelWidthAutoData(ip, userId);
                info.ChannelWidthAutoResponse = (List<ChannelWidthAutoResponseModel>)channelWidthAutoData.Data;
                var channelbetween149_161Data = await GetChannelbetween149_161Data(ip, userId);
                info.Channelbetween149_161Response = (List<Channelbetween149_161ResponseModel>)channelbetween149_161Data.Data;
                var checkListDetail = await GetRsmChecklistDetailsByClientId(clientID, ip, userId);
                info.RsmChecklistDetail = (RsmChecklistDetailsModel)checkListDetail.Data;

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(null, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse> GetChecklistData(string ip, string userId)
        {
            var methodName = "RsmChecklistService/GetChecklistData";
            try
            {
                var query = "SELECT ID, CheckList FROM RSM_CLIST";
                var info = await _rsmDBContext.GetChecklist.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("Checklist Data not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(null, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }


        public async Task<ApiResponse> GetRouterTypeData(string ip, string userId)
        {
            var methodName = "RsmChecklistService/GetRouterTypeData";
            try
            {
                var query = "SELECT ID, RouterType FROM RSM_CLRouterType";
                var info = await _rsmDBContext.GetRouterType.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("Router Type Data not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(null, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> GetControllerOwnerData(string ip, string userId)
        {
            var methodName = "RsmChecklistService/GetControllerOwnerData";
            try
            {
                var query = "SELECT ID, ControllerOwner FROM RSM_CLControllerOwner";
                var info = await _rsmDBContext.GetControllerOwner.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("Controller Owner Data not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(null, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> GetSingleApData(string ip, string userId)
        {
            var methodName = "RsmChecklistService/GetSingleApData";
            try
            {
                var query = "SELECT ID, SingleAP FROM RSM_CLSingleAP";
                var info = await _rsmDBContext.GetSingleAp.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("SingleAP Data not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(null, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetMultipleApData(string ip, string userId)
        {
            var methodName = "RsmChecklistService/GetMultipleApData";
            try
            {
                var query = "SELECT ID, MultipleAP FROM RSM_CLMultipleAP";
                var info = await _rsmDBContext.GetMultipleAp.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("MultipleAP Data not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(null, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetChannelWidth20MHzData(string ip, string userId)
        {
            var methodName = "RsmChecklistService/GetChannelWidth20MHzData";
            try
            {
                var query = "SELECT ID, ChannelWidth20MHz FROM RSM_CLChannelWidth20MHz";
                var info = await _rsmDBContext.GetChannelWidth20MHz.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("ChannelWidth20MHz Data not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(null, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetGhzEnabledData(string ip, string userId)
        {
            var methodName = "RsmChecklistService/GetGhzEnabledData";
            try
            {
                var query = "SELECT ID, GhzEnabled FROM RSM_CLGhzEnabled";
                var info = await _rsmDBContext.GetGhzEnabled.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("GhzEnabled Data not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(null, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetChannelWidthAutoData(string ip, string userId)
        {
            var methodName = "RsmChecklistService/GetChannelWidthAutoData";
            try
            {
                var query = "SELECT ID, ChannelWidthAuto FROM RSM_CLChannelWidthAuto";
                var info = await _rsmDBContext.GetChannelWidthAuto.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("ChannelWidthAuto Data not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(null, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetChannelbetween149_161Data(string ip, string userId)
        {
            var methodName = "RsmChecklistService/GetChannelbetween149_161Data";
            try
            {
                var query = "SELECT ID, Channelbetween149_161 FROM RSM_CLChannelbetween149_161";
                var info = await _rsmDBContext.GetChannelbetween149_161.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("Channelbetween149_161 Data not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(null, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetRsmChecklistDetailsByClientId(string clientId, string ip, string userId)
        {
            var methodName = "RsmChecklistService/GetRsmChecklistDetailsByClientId";
            try
            {
                var query = "select ClientID,brslno,OpticalLaser,ONUisConnectedwithCompatibleAdapter,ONUlabeledwithSubscriberID,Cat6cableused, " +
                    "ClientuseWiFiRouter,Deafultloginpasswordchanged,PasswrodisconfiguredasPrescribebyLink3,RouterFirwareUptoDate, " +
                    "UpnpDiabled,WPSdisabled,WPA2securityEnabledtoAccessWiFi,RemoteManagementPortEnabled,RouterpositionedINproperplace, " +
                    "RouterisConnectedwithCompatibleAdapter,OnuportSpeedFE_GE,SpectrumAnalyzerusedRFchannelchecked,RouterType,ControllerOwner, " +
                    "GhzEnabled2_4,SingleAP,MultipleAP,ChannelWidth,GhzEnabled5,ChannelWidthAutoor40MHz,Channelbetween149_161AvilableAndSelected, " +
                    "Link3DNSusedinWANconfiguration,Link3DNSusedinDHCPconfiguration,RouterSupportIPv6,RouterWanRecivedIPV6fromLink3, " +
                    "LANdevicerecivingIPV6,Canbrowseipv6,NTPServer123,RouterSupportScheduleReboot,ScheduleRebootConfigured,InternetSpeedTest, " +
                    "InternetSpeedUploadFile,BDIXSpeedTest,BDIXSpeedTestUploadFile,WifiAnalyzer2_4GHz,WifiAnalyzer2_4GHzUploadFile,WifiAnalyzer5GHz, " +
                    "WifiAnalyzer5GHzUploadFile From RSM_CheckListDetails Where ClientID = '" + clientId + "'";
                var info = await _rsmDBContext.GetRsmChecklistInfos.FromSqlRaw(query).FirstOrDefaultAsync();
                if(info == null)
                {
                    info = new RsmChecklistDetailsModel()
                    {
                        ClientID = "",
                        brslno = 0,
                        OpticalLaser = 0,
                        ONUisConnectedwithCompatibleAdapter = 0,
                        ONUlabeledwithSubscriberID = 0,
                        Cat6cableused = 0,
                        ClientuseWiFiRouter = 0,
                        Deafultloginpasswordchanged = 0,
                        PasswrodisconfiguredasPrescribebyLink3 = 0,
                        RouterFirwareUptoDate = 0,
                        UpnpDiabled = 0,
                        WPSdisabled = 0,
                        WPA2securityEnabledtoAccessWiFi = 0,
                        RemoteManagementPortEnabled = 0,
                        RouterpositionedINproperplace = 0,
                        RouterisConnectedwithCompatibleAdapter = 0,
                        OnuportSpeedFE_GE = 0,
                        SpectrumAnalyzerusedRFchannelchecked = 0,
                        RouterType = 0,
                        ControllerOwner = 0,
                        GhzEnabled2_4 = 0,
                        SingleAP = 0,
                        MultipleAP = 0,
                        ChannelWidth = 0,
                        GhzEnabled5 = 0,
                        ChannelWidthAutoor40MHz = 0,
                        Channelbetween149_161AvilableAndSelected = 0,
                        Link3DNSusedinWANconfiguration = 0,
                        Link3DNSusedinDHCPconfiguration = 0,
                        RouterSupportIPv6 = 0,
                        RouterWanRecivedIPV6fromLink3 = 0,
                        LANdevicerecivingIPV6 = 0,
                        Canbrowseipv6 = 0,
                        NTPServer123 = 0,
                        RouterSupportScheduleReboot = 0,
                        ScheduleRebootConfigured = 0,
                        InternetSpeedTest = 0,
                        InternetSpeedUploadFile = "",
                        BDIXSpeedTest = 0,
                        BDIXSpeedTestUploadFile = "",
                        WifiAnalyzer2_4GHz = 0,
                        WifiAnalyzer2_4GHzUploadFile = "",
                        WifiAnalyzer5GHz = 0,
                        WifiAnalyzer5GHzUploadFile = ""
                    };
                }
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(clientId, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(clientId, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> SaveRsmChecklistData(RsmCheckListRequestModel model, string userId, string ip)
        {
            var methodName = "ChecklisRsmChecklistServicetService/SaveRsmChecklistData";
            try
            {
                await Validation(model);
                var query = "SELECT * FROM RSM_CheckListDetails WHERE ClientID = '" + model.ClientID + "'";
                var info = await _rsmDBContext.GetRsmChecklistInfos.FromSqlRaw(query).FirstOrDefaultAsync();
                string statusMessage = "";
                if(model.InternetSpeedUploadFileDetails != null)
                {
                    model.InternetSpeedFileName = model.InternetSpeedUploadFileDetails.FileName;
                    model.InternetSpeedUploadFile = model.ClientID + "_" + model.InternetSpeedFileName;
                }
                if (model.BDIXSpeedTestUploadFileDetails != null)
                {
                    model.BDIXSpeedTestFileName = model.BDIXSpeedTestUploadFileDetails.FileName;
                    model.BDIXSpeedTestUploadFile = model.ClientID + "_" + model.BDIXSpeedTestFileName;
                }
                if (model.WifiAnalyzer2_4GHzUploadFileFileDetails != null)
                {
                    model.WifiAnalyzer2_4GHzFileName = model.WifiAnalyzer2_4GHzUploadFileFileDetails.FileName;
                    model.WifiAnalyzer2_4GHzUploadFile = model.ClientID + "_" + model.WifiAnalyzer2_4GHzFileName;
                }
                if (model.WifiAnalyzer5GHzUploadFileDetails != null)
                {
                    model.WifiAnalyzer5GHzFileName = model.WifiAnalyzer5GHzUploadFileDetails.FileName;
                    model.WifiAnalyzer5GHzUploadFile = model.ClientID + "_" + model.WifiAnalyzer5GHzFileName;
                }              
                

                if (info != null)
                {
                    var updateQuery = "UPDATE RSM_CheckListDetails SET OpticalLaser = '" + model.OpticalLaser + "', " +
                        "ONUisConnectedwithCompatibleAdapter = '" + model.ONUisConnectedwithCompatibleAdapter + "', " +
                        "ONUlabeledwithSubscriberID = '" + model.ONUlabeledwithSubscriberID + "', Cat6cableused = '" + model.Cat6cableused + "', " +
                        "ClientuseWiFiRouter = '" + model.ClientuseWiFiRouter + "', Deafultloginpasswordchanged = '" + model.Deafultloginpasswordchanged + "', " +
                        "PasswrodisconfiguredasPrescribebyLink3 = '" + model.PasswrodisconfiguredasPrescribebyLink3 + "', " +
                        "RouterFirwareUptoDate = '" + model.RouterFirwareUptoDate + "', UpnpDiabled = '" + model.UpnpDiabled + "', " +
                        "WPSdisabled = '" + model.WPSdisabled + "', WPA2securityEnabledtoAccessWiFi = '" + model.WPA2securityEnabledtoAccessWiFi + "', " +
                        "RemoteManagementPortEnabled = '" + model.RemoteManagementPortEnabled + "', RouterpositionedINproperplace =  " +
                        "'" + model.RouterpositionedINproperplace + "', RouterisConnectedwithCompatibleAdapter = '" + model.RouterisConnectedwithCompatibleAdapter + "', " +
                        "OnuportSpeedFE_GE = '" + model.OnuportSpeedFE_GE + "',SpectrumAnalyzerusedRFchannelchecked= '" + model.SpectrumAnalyzerusedRFchannelchecked + "', " +
                        "RouterType='" + model.RouterType + "',ControllerOwner= '" + model.ControllerOwner + "', GhzEnabled2_4='" + model.GhzEnabled2_4 + "', " +
                        "SingleAP='" + model.SingleAP + "',MultipleAP='" + model.MultipleAP + "',ChannelWidth='" + model.ChannelWidth + "', GhzEnabled5='" + model.GhzEnabled5 + "', " +
                        "ChannelWidthAutoor40MHz='" + model.ChannelWidthAutoor40MHz + "', Channelbetween149_161AvilableAndSelected='" + model.Channelbetween149_161AvilableAndSelected + "', " +
                        "Link3DNSusedinWANconfiguration='" + model.Link3DNSusedinWANconfiguration + "', Link3DNSusedinDHCPconfiguration='" +
                        model.Link3DNSusedinDHCPconfiguration + "', RouterSupportIPv6='" + model.RouterSupportIPv6 + "', RouterWanRecivedIPV6fromLink3='" + model.RouterWanRecivedIPV6fromLink3 +
                        "', LANdevicerecivingIPV6='" + model.LANdevicerecivingIPV6 + "', Canbrowseipv6='" + model.Canbrowseipv6 + "', NTPServer123='" + model.NTPServer123 + "', " +
                        "RouterSupportScheduleReboot='" + model.RouterSupportScheduleReboot + "', ScheduleRebootConfigured='" + model.ScheduleRebootConfigured + "', " +
                        "Reamarks='" + model.Reamarks + "', InternetSpeedTest='" + model.InternetSpeedTest + "', InternetSpeedRemarks='" + model.InternetSpeedRemarks + "', " +
                        "BDIXSpeedTest='" + model.BDIXSpeedTest + "', BDIXSpeedTestRemarks='" + model.BDIXSpeedTestRemarks + "', WifiAnalyzer2_4GHz='" + model.WifiAnalyzer2_4GHz + "'," +
                        "WifiAnalyzer2_4GHzRemarks='" + model.WifiAnalyzer2_4GHzRemarks + "', WifiAnalyzer5GHz='" + model.WifiAnalyzer5GHz + "', WifiAnalyzer5GHzRemarks='" + model.WifiAnalyzer5GHzRemarks +
                        "', UpdateBy='"+userId+ "', UpdateDate= GETDATE()  WHERE ClientID = '" + model.ClientID + "' AND brslno = '" + model.brslno + "'";

                    int items = await _rsmDBContext.Database.ExecuteSqlRawAsync(updateQuery);
                    string uploadPath = _configuration.GetValue<string>("FilePath:Path");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    if (model.InternetSpeedUploadFileDetails != null)
                    {
                        string InternetSpeedFileWithPath = Path.Combine(uploadPath, model.InternetSpeedFileName);
                        using (var stream = new FileStream(InternetSpeedFileWithPath, FileMode.Create))
                        {
                            model.InternetSpeedUploadFileDetails.CopyTo(stream);
                        }
                    }
                    if (model.BDIXSpeedTestUploadFileDetails != null)
                    {
                        string BDIXSpeedTestFileWithPath = Path.Combine(uploadPath, model.BDIXSpeedTestFileName);
                        using (var stream = new FileStream(BDIXSpeedTestFileWithPath, FileMode.Create))
                        {
                            model.BDIXSpeedTestUploadFileDetails.CopyTo(stream);
                        }
                    }
                    if (model.WifiAnalyzer2_4GHzUploadFileFileDetails != null)
                    {
                        string WifiAnalyzer2_4GHzFileWithPath = Path.Combine(uploadPath, model.WifiAnalyzer2_4GHzFileName);
                        using (var stream = new FileStream(WifiAnalyzer2_4GHzFileWithPath, FileMode.Create))
                        {
                            model.WifiAnalyzer2_4GHzUploadFileFileDetails.CopyTo(stream);
                        }
                    }
                    if (model.WifiAnalyzer5GHzUploadFileDetails != null)
                    {
                        string WifiAnalyzer5GHzFileWithPath = Path.Combine(uploadPath, model.WifiAnalyzer5GHzFileName);
                        using (var stream = new FileStream(WifiAnalyzer5GHzFileWithPath, FileMode.Create))
                        {
                            model.WifiAnalyzer5GHzUploadFileDetails.CopyTo(stream);
                        }
                    }

                    if (items == 1)
                    {
                        statusMessage = "data has been updated successfully";
                    }
                    else
                    {
                        throw new Exception("something went wrong");
                    }
                }
                else
                {
                    var insertQuery = "INSERT INTO RSM_CheckListDetails (ClientID, brslno, OpticalLaser, ONUisConnectedwithCompatibleAdapter, ONUlabeledwithSubscriberID, " +
                        "Cat6cableused, ClientuseWiFiRouter, Deafultloginpasswordchanged, PasswrodisconfiguredasPrescribebyLink3, RouterFirwareUptoDate, UpnpDiabled, " +
                        "WPSdisabled, WPA2securityEnabledtoAccessWiFi, RemoteManagementPortEnabled, RouterpositionedINproperplace,RouterisConnectedwithCompatibleAdapter," +
                        "OnuportSpeedFE_GE, SpectrumAnalyzerusedRFchannelchecked, RouterType, ControllerOwner, GhzEnabled2_4,SingleAP,MultipleAP, ChannelWidth, GhzEnabled5," +
                        "ChannelWidthAutoor40MHz,Channelbetween149_161AvilableAndSelected, Link3DNSusedinWANconfiguration, Link3DNSusedinDHCPconfiguration, RouterSupportIPv6, " +
                        "RouterWanRecivedIPV6fromLink3, LANdevicerecivingIPV6, Canbrowseipv6,NTPServer123, RouterSupportScheduleReboot, ScheduleRebootConfigured, Reamarks, InternetSpeedTest, " +
                        "InternetSpeedRemarks, InternetSpeedFileName, InternetSpeedUploadFile, BDIXSpeedTest, BDIXSpeedTestRemarks,BDIXSpeedTestFileName, BDIXSpeedTestUploadFile, " +
                        "WifiAnalyzer2_4GHz, WifiAnalyzer2_4GHzRemarks,WifiAnalyzer2_4GHzFileName, WifiAnalyzer2_4GHzUploadFile, WifiAnalyzer5GHz,WifiAnalyzer5GHzRemarks, " +
                        "WifiAnalyzer5GHzFileName, WifiAnalyzer5GHzUploadFile, EntryBy) VALUES('" + model.ClientID + "','" + model.brslno +
                        "','" + model.OpticalLaser + "','" + model.ONUisConnectedwithCompatibleAdapter + "','" + model.ONUlabeledwithSubscriberID +
                        "','" + model.Cat6cableused + "','" + model.ClientuseWiFiRouter + "','" + model.Deafultloginpasswordchanged + "','" + model.PasswrodisconfiguredasPrescribebyLink3 +
                        "','" + model.RouterFirwareUptoDate + "','" + model.UpnpDiabled + "','" + model.WPSdisabled + "','" + model.WPA2securityEnabledtoAccessWiFi +
                        "','" + model.RemoteManagementPortEnabled + "','" + model.RouterpositionedINproperplace + "','" + model.RouterisConnectedwithCompatibleAdapter +
                        "','" + model.OnuportSpeedFE_GE + "','" + model.SpectrumAnalyzerusedRFchannelchecked + "','" + model.RouterType + "','" +
                        model.ControllerOwner + "','" + model.GhzEnabled2_4 + "','" + model.SingleAP + "','" + model.MultipleAP + "','" +
                        model.ChannelWidth + "','" + model.GhzEnabled5 + "','" + model.ChannelWidthAutoor40MHz + "','" + model.Channelbetween149_161AvilableAndSelected +
                        "','" + model.Link3DNSusedinWANconfiguration + "','" + model.Link3DNSusedinDHCPconfiguration + "','" + model.RouterSupportIPv6 + "','" +
                        model.RouterWanRecivedIPV6fromLink3 + "','" + model.LANdevicerecivingIPV6 + "','" + model.Canbrowseipv6 + "','" + model.NTPServer123 + "','" + model.RouterSupportScheduleReboot +
                        "','" + model.ScheduleRebootConfigured + "','" + model.Reamarks + "','" + model.InternetSpeedTest + "','" + model.InternetSpeedRemarks +
                        "','" + model.InternetSpeedFileName + "','" + model.InternetSpeedUploadFile + "','" + model.BDIXSpeedTest + "','" +
                        model.BDIXSpeedTestRemarks + "','" + model.BDIXSpeedTestFileName + "','" + model.BDIXSpeedTestUploadFile + "','" +
                        model.WifiAnalyzer2_4GHz + "','" + model.WifiAnalyzer2_4GHzRemarks + "','" + model.WifiAnalyzer2_4GHzFileName + "','" + model.WifiAnalyzer2_4GHzUploadFile +
                        "','" + model.WifiAnalyzer5GHz + "','" + model.WifiAnalyzer5GHzRemarks + "','" + model.WifiAnalyzer5GHzFileName + "','" +
                        model.WifiAnalyzer5GHzUploadFile + "','" + userId + "')";

                    int items = await _rsmDBContext.Database.ExecuteSqlRawAsync(insertQuery);

                    string uploadPath = _configuration.GetValue<string>("FilePath:Path");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string InternetSpeedFileWithPath = Path.Combine(uploadPath, model.InternetSpeedFileName);
                    using (var stream = new FileStream(InternetSpeedFileWithPath, FileMode.Create))
                    {
                        model.InternetSpeedUploadFileDetails.CopyTo(stream);
                    }
                    string BDIXSpeedTestFileWithPath = Path.Combine(uploadPath, model.BDIXSpeedTestFileName);
                    using (var stream = new FileStream(BDIXSpeedTestFileWithPath, FileMode.Create))
                    {
                        model.BDIXSpeedTestUploadFileDetails.CopyTo(stream);
                    }                   

                    if (items == 1)
                    {
                        statusMessage = "data has been inserted successfully";
                    }
                    else
                    {
                        throw new Exception("something went wrong");
                    }
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = statusMessage,
                    Data = null
                };
                await InsertRequestResponse(model, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        private async Task Validation(RsmCheckListRequestModel model)
        {
            if (model.OpticalLaser == "0")
            {
                throw new Exception("Select Optical Laser is lass than -24 dBm.");
            }
            if (model.ONUisConnectedwithCompatibleAdapter == "0")
            {
                throw new Exception("Select ONU is connected with compatible adapter?");
            }
            if (model.ONUlabeledwithSubscriberID == "0")
            {
                throw new Exception("Select ONU labeled with Subscriber ID ?");
            }
            if (model.Cat6cableused == "0")
            {
                throw new Exception("Select Cat-6 cable used ?");
            }
            if (model.ClientuseWiFiRouter == "0")
            {
                throw new Exception("Select Client use WiFi Router?");
            }
            if (model.Deafultloginpasswordchanged == "0")
            {
                throw new Exception("Select Deafult login password changed ?");
            }
            if (model.PasswrodisconfiguredasPrescribebyLink3 == "0")
            {
                throw new Exception("Select Passwrod is configured as Prescribe by Link3");
            }
            if (model.RouterFirwareUptoDate == "0")
            {
                throw new Exception("Select Router Firware Up-to-Date ?");
            }
            if (model.UpnpDiabled == "0")
            {
                throw new Exception("Select Upnp Diabled ?");
            }
            if (model.WPSdisabled == "0")
            {
                throw new Exception("Select WPS Disabled ?");
            }
            if (model.WPA2securityEnabledtoAccessWiFi == "0")
            {
                throw new Exception("Select WPA2 security enabled to access WiFi?");
            }
            if (model.RemoteManagementPortEnabled == "0")
            {
                throw new Exception("Select Remote Management Port enabled & configure to 8282.");
            }
            if (model.RouterpositionedINproperplace == "0")
            {
                throw new Exception("Select Router positioned in proper place.");
            }
            if (model.RouterisConnectedwithCompatibleAdapter == "0")
            {
                throw new Exception("Select Router is connected with compatible adapter?");
            }
            if (model.OnuportSpeedFE_GE == "0")
            {
                throw new Exception("Select Onu port Speed FE/GE (fasted option in Router) ensured by FONoC");
            }
            if (model.SpectrumAnalyzerusedRFchannelchecked == "0")
            {
                throw new Exception("Select Spectrum Analyzer used RF channel checked and slelected the best Channel.");
            }
            if (model.RouterType == "0")
            {
                throw new Exception("Select Router Type.");
            }
            if (model.RouterType == "3")
            {
                if (model.ControllerOwner == "0")
                {
                    throw new Exception("Select Controller Owner.");
                }

            }
            if (model.GhzEnabled2_4 == "0")
            {
                throw new Exception("Select 2.4 Ghz Enabled.");
            }
            if (model.SingleAP == "0")
            {
                throw new Exception("Select For Single AP/Router Channel selected as auto.");
            }
            if (model.MultipleAP == "0")
            {
                throw new Exception("Select For multiple AP/Router or RF Congession, Channel selected.");
            }
            if (model.ChannelWidth == "0")
            {
                throw new Exception("Select Channel Width (20 MHz preferred,Incase of Congession or multiple AP).");
            }
            if (model.GhzEnabled5 == "0")
            {
                throw new Exception("Select 5 Ghz Enabled");
            }
            if (model.GhzEnabled5 == "1")
            {
                if (model.ChannelWidthAutoor40MHz == "0")
                {
                    throw new Exception("Select Channel Width Auto or 40 MHz.");
                }
                if (model.Channelbetween149_161AvilableAndSelected == "0")
                {
                    throw new Exception("Select Channel between 149-161 avilable and selected.");
                }
            }
            if (model.Link3DNSusedinWANconfiguration == "0")
            {
                throw new Exception("Select Link3 DNS used in WAN configuration.");
            }
            if (model.Link3DNSusedinDHCPconfiguration == "0")
            {
                throw new Exception("Select Link3 DNS used in DHCP configuration.");
            }
            if (model.RouterSupportIPv6 == "0")
            {
                throw new Exception("Select Router Support IPv6 ?");
            }
            if (model.RouterSupportIPv6 == "1")
            {
                if (model.RouterWanRecivedIPV6fromLink3 == "0")
                {
                    throw new Exception("Select Router Wan Recived IPV6 from Link3 (start with 2400).");
                }
                if (model.LANdevicerecivingIPV6 == "0")
                {
                    throw new Exception("Select LAN device reciving IPV6 (start with 2400).");
                }
                if (model.Canbrowseipv6 == "0")
                {
                    throw new Exception("Select Can browse ipv6.google.com ?");
                }
            }
            if (model.NTPServer123 == "0")
            {
                throw new Exception("Select NTP server 123.200.0.252 configured.");
            }
            if (model.RouterSupportScheduleReboot == "0")
            {
                throw new Exception("Select Router Support Schedule Reboot.");
            }
            if (model.RouterSupportScheduleReboot == "1")
            {
                if (model.ScheduleRebootConfigured == "0")
                {
                    throw new Exception("Select Schedule Reboot configured.");
                }
            }

            if (model.ClientuseWiFiRouter == "2")
            {
                if (model.InternetSpeedTest == "0")
                {
                    throw new Exception("Select internet speed test match.");
                }
                if (model.InternetSpeedTest == "2")
                {
                    if (string.IsNullOrEmpty(model.InternetSpeedRemarks))
                    {
                        throw new Exception("Write internet speed test match remarks.");
                    }
                    if (model.InternetSpeedUploadFileDetails == null)
                    {
                        throw new Exception("Invalid internet speed File upload");
                    }
                }

                if (model.BDIXSpeedTest == "0")
                {
                    throw new Exception("Select BDIX speed test match.");
                }
                if (model.BDIXSpeedTest == "2")
                {
                    if (string.IsNullOrEmpty(model.BDIXSpeedTestRemarks))
                    {
                        throw new Exception("Write BDIX speed test match remarks.");
                    }
                    if (model.BDIXSpeedTestUploadFileDetails == null)
                    {
                        throw new Exception("Invalid BDIX File upload");
                    }
                }
            }
        }

        private async Task<string> getEmailMessageBody(string ticketid)
        {
            var methodName = "ChecklistService/getEmailMessageBody";
            try
            {
                var mailformet_query = "SELECT CTID, MailBcc, MailBody, MailCC, MailSubject, Mailfrom, Mailto, " +
                    "Status FROM tblComplainEmailFormat  WITH(NOLOCK) WHERE  (CTID = '" + ticketid + "')";
                var complainEmailFormat = await _misDBContext.tblComplainEmailFormat.FromSqlRaw(mailformet_query).FirstOrDefaultAsync();
                if (complainEmailFormat == null)
                {
                    throw new Exception("Mail Contain is not found.");
                }

                return complainEmailFormat.MailBody;
            }
            catch (Exception ex)
            {

                await errorMethord(ex, methodName);
                throw new Exception(ex.Message);
            }
        }

        private async Task InsertRequestResponse(object request, object response, string methodName, string requestedIP, string userId, string errorLog)
        {
            var errorMethordName = "ChecklistService/InsertRequestResponse";
            try
            {
                var reqResModel = new FFRequestResponseModel()
                {
                    CreatedAt = DateTime.Now,
                    Request = JsonConvert.SerializeObject(request),
                    Response = JsonConvert.SerializeObject(response),
                    RequestedIP = requestedIP,
                    MethodName = methodName,
                    UserId = userId,
                    ErrorLog = errorLog
                };
                await _ffWriteDBContext.fFRequestResponseModels.AddAsync(reqResModel);
                await _ffWriteDBContext.SaveChangesAsync();
                _logger.LogInformation("Insert" + JsonConvert.SerializeObject(reqResModel));
            }
            catch (Exception ex)
            {
                string errormessage = errorMethord(ex, errorMethordName).Result;
            }
        }

        private async Task<string> errorMethord(Exception ex, string info)
        {
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Method Name : " + info + ", Exception " + errormessage);
            //await _systemService.ErrorLogEntry(info, info, errormessage);
            //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
            return errormessage;
        }
    }
}

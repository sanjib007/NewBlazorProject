using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using L3T.Infrastructure.Helpers.Models.SmsNotification;
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

namespace L3T.Infrastructure.Helpers.Implementation.FieldForce
{
   
    public class ChecklistService : IChecklistService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MisDBContext _misDBContext;
        private readonly ILogger<InstallationTicketService> _logger;
        private readonly FFWriteDBContext _ffWriteDBContext;
        private IConfiguration _configuration;
        private readonly IMailSenderService _mailSenderService;
        public ChecklistService(
            IHttpClientFactory httpClientFactory,
            MisDBContext misDBContext,
            ILogger<InstallationTicketService> logger,
            FFWriteDBContext ffWriteDBContext,
            IConfiguration iconfig,
            IMailSenderService mailSenderService)
        {
            _httpClientFactory = httpClientFactory;
            _misDBContext = misDBContext;
            _logger = logger;
            _ffWriteDBContext = ffWriteDBContext;
            _mailSenderService = mailSenderService;
            _configuration = iconfig;
        }


        public async Task<ApiResponse> GetMisChecklistDetailsByTicketId(string ticketid, string ip)
        {
            var methodName = "ChecklistService/GetMisChecklistDetailsByTicketId";
            try
            {
                var query = "select m.ClientID,m.brslno,OpticalLaser,ONUisConnectedwithCompatibleAdapter,ONUlabeledwithSubscriberID,Cat6cableused, " +
                    "ClientuseWiFiRouter,Deafultloginpasswordchanged,PasswrodisconfiguredasPrescribebyLink3,RouterFirwareUptoDate, " +
                    "UpnpDiabled,WPSdisabled,WPA2securityEnabledtoAccessWiFi,RemoteManagementPortEnabled,RouterpositionedINproperplace, " +
                    "RouterisConnectedwithCompatibleAdapter,OnuportSpeedFE_GE,SpectrumAnalyzerusedRFchannelchecked,RouterType,ControllerOwner, " +
                    "GhzEnabled2_4,SingleAP,MultipleAP,ChannelWidth,GhzEnabled5,ChannelWidthAutoor40MHz,Channelbetween149_161AvilableAndSelected, " +
                    "Link3DNSusedinWANconfiguration,Link3DNSusedinDHCPconfiguration,RouterSupportIPv6,RouterWanRecivedIPV6fromLink3, " +
                    "LANdevicerecivingIPV6,Canbrowseipv6,NTPServer123,RouterSupportScheduleReboot,ScheduleRebootConfigured,InternetSpeedTest, " +
                    "InternetSpeedUploadFile,BDIXSpeedTest,BDIXSpeedTestUploadFile,WifiAnalyzer2_4GHz,Reamarks,WifiAnalyzer5GHz " +
                    "From Post_Installation p WITH(NOLOCK) inner join clientDatabaseMain c ON p.Cli_code = c.brCliCode and p.CliAdrNewCode " +
                    "= c.brAdrNewCode inner join Mis_CheckListDetails m on c.brCliCode = m.ClientID and c.brSlNo = m.brslno where " +
                    "p.TrackingInfo = '" + ticketid + "'";
                var info = await _misDBContext.GetMisChecklistInfos.FromSqlRaw(query).FirstOrDefaultAsync();


                if (info == null)
                {
                    throw new Exception("MIS_Checklist info not found.");
                }


                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(ticketid, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketid, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> GetCustomerInfoByTicketId(string ticketid, string ip)
        {
            var methodName = "InstallationTicketService/GetCustomerInfoByTicketId";
            try
            {
                var query = "select c.HeadOfficeName,c.brName,c.brCliCode,c.brSlNo From Post_Installation p " +
                    "WITH(NOLOCK) inner join clientDatabaseMain c ON p.Cli_code = c.brCliCode and p.CliAdrNewCode " +
                    "= c.brAdrNewCode where p.TrackingInfo = '" + ticketid + "'";
                var info = await _misDBContext.GetCustomerInfos.FromSqlRaw(query).FirstOrDefaultAsync();

                // GetAllPendingTicketByAssignUserResponseModel getTicket = await _misDBContext.Post_Instalation.Where(x => x.RefNo == ticketid).AsNoTracking().FirstOrDefaultAsync();
                if (info == null)
                {
                    throw new Exception("Hardware info not found.");
                }

                info.customer = info.brCliCode + "," + info.brSlNo;

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(ticketid, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketid, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }



        }



        public async Task<ApiResponse> GetChecklistData(string ip)
        {
            var methodName = "ChecklistService/GetChecklistData";
            try
            {
                var query = "SELECT ID, CheckList FROM RSM.WFA2.dbo.RSM_CLIST";
                var info = await _misDBContext.GetChecklist.FromSqlRaw(query).ToListAsync();

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


        public async Task<ApiResponse> GetRouterTypeData(string ip)
        {
            var methodName = "ChecklistService/GetRouterTypeData";
            try
            {
                var query = "SELECT ID, RouterType FROM RSM.WFA2.dbo.RSM_CLRouterType";
                var info = await _misDBContext.GetRouterType.FromSqlRaw(query).ToListAsync();

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




        public async Task<ApiResponse> GetControllerOwnerData(string ip)
        {
            var methodName = "ChecklistService/GetControllerOwnerData";
            try
            {
                var query = "SELECT ID, ControllerOwner FROM RSM.WFA2.dbo.RSM_CLControllerOwner";
                var info = await _misDBContext.GetControllerOwner.FromSqlRaw(query).ToListAsync();

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



        public async Task<ApiResponse> GetSingleApData(string ip)
        {
            var methodName = "ChecklistService/GetSingleApData";
            try
            {
                var query = "SELECT ID, SingleAP FROM RSM.WFA2.dbo.RSM_CLSingleAP";
                var info = await _misDBContext.GetSingleAp.FromSqlRaw(query).ToListAsync();

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

        public async Task<ApiResponse> GetMultipleApData(string ip)
        {
            var methodName = "ChecklistService/GetMultipleApData";
            try
            {
                var query = "SELECT ID, MultipleAP FROM RSM.WFA2.dbo.RSM_CLMultipleAP";
                var info = await _misDBContext.GetMultipleAp.FromSqlRaw(query).ToListAsync();

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



        public async Task<ApiResponse> GetChannelWidth20MHzData(string ip)
        {
            var methodName = "ChecklistService/GetChannelWidth20MHzData";
            try
            {
                var query = "SELECT ID, ChannelWidth20MHz FROM RSM.WFA2.dbo.RSM_CLChannelWidth20MHz";
                var info = await _misDBContext.GetChannelWidth20MHz.FromSqlRaw(query).ToListAsync();

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


        public async Task<ApiResponse> GetGhzEnabledData(string ip)
        {
            var methodName = "ChecklistService/GetGhzEnabledData";
            try
            {
                var query = "SELECT ID, GhzEnabled FROM RSM.WFA2.dbo.RSM_CLGhzEnabled";
                var info = await _misDBContext.GetGhzEnabled.FromSqlRaw(query).ToListAsync();

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


        public async Task<ApiResponse> GetChannelWidthAutoData(string ip)
        {
            var methodName = "ChecklistService/GetChannelWidthAutoData";
            try
            {
                var query = "SELECT ID, ChannelWidthAuto FROM RSM.WFA2.dbo.RSM_CLChannelWidthAuto";
                var info = await _misDBContext.GetChannelWidthAuto.FromSqlRaw(query).ToListAsync();

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


        public async Task<ApiResponse> GetChannelbetween149_161Data(string ip)
        {
            var methodName = "ChecklistService/GetChannelbetween149_161Data";
            try
            {
                var query = "SELECT ID, Channelbetween149_161 FROM RSM.WFA2.dbo.RSM_CLChannelbetween149_161";
                var info = await _misDBContext.GetChannelbetween149_161.FromSqlRaw(query).ToListAsync();

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

        public async Task<ApiResponse> SaveChecklistData(MisCheckListRequestModel model, string ip)
        {
            var methodName = "ChecklistService/SaveChecklistData";
            try
            {
                var query = "SELECT * FROM Mis_CheckListDetails WHERE ClientID = '" + model.ClientID +"' and brslno='"+ model.brslno + "'";
                var info = await _misDBContext.GetMisChecklistInfos.FromSqlRaw(query).FirstOrDefaultAsync();
                string statusMessage = "";
                
                model.InternetSpeedFileName = model.InternetSpeedUploadFileDetails.FileName;
                model.InternetSpeedUploadFile = model.ClientID + "_" + model.InternetSpeedFileName;
                model.BDIXSpeedTestFileName = model.BDIXSpeedTestUploadFileDetails.FileName;
                model.BDIXSpeedTestUploadFile = model.ClientID + "_" + model.BDIXSpeedTestFileName;
                model.WifiAnalyzer2_4GHzFileName = model.WifiAnalyzer2_4GHzUploadFileFileDetails.FileName;  
                model.WifiAnalyzer2_4GHzUploadFile = model.ClientID + "_" + model.WifiAnalyzer2_4GHzFileName;
                model.WifiAnalyzer5GHzFileName = model.WifiAnalyzer5GHzUploadFileDetails.FileName;  
                model.WifiAnalyzer5GHzUploadFile = model.ClientID + "_" + model.WifiAnalyzer5GHzFileName;


                if (info != null)
                {
                    var updateQuery = "UPDATE Mis_CheckListDetails SET OpticalLaser = '" + model.OpticalLaser +"', "+
                        "ONUisConnectedwithCompatibleAdapter = '" + model.ONUisConnectedwithCompatibleAdapter +"', "+
                        "ONUlabeledwithSubscriberID = '" + model.ONUlabeledwithSubscriberID +"', Cat6cableused = '" +model.Cat6cableused +"', "+
                        "ClientuseWiFiRouter = '" +model.ClientuseWiFiRouter +"', Deafultloginpasswordchanged = '" +model.Deafultloginpasswordchanged +"', "+
                        "PasswrodisconfiguredasPrescribebyLink3 = '" +model.PasswrodisconfiguredasPrescribebyLink3 +"', "+
                        "RouterFirwareUptoDate = '" +model.RouterFirwareUptoDate +"', UpnpDiabled = '" + model.UpnpDiabled +"', "+
                        "WPSdisabled = '" +model.WPSdisabled +"', WPA2securityEnabledtoAccessWiFi = '"+model.WPA2securityEnabledtoAccessWiFi +"', "+
                        "RemoteManagementPortEnabled = '" +model.RemoteManagementPortEnabled +"', RouterpositionedINproperplace =  "+
                        "'" + model.RouterpositionedINproperplace +"', RouterisConnectedwithCompatibleAdapter = '" +model.RouterisConnectedwithCompatibleAdapter +"', "+
                        "OnuportSpeedFE_GE = '"+ model.OnuportSpeedFE_GE +"',SpectrumAnalyzerusedRFchannelchecked= '" +model.SpectrumAnalyzerusedRFchannelchecked +"', "+
                        "RouterType='" +model.RouterType +"',ControllerOwner= '" + model.ControllerOwner +"', GhzEnabled2_4='" +model.GhzEnabled2_4 +"', "+
                        "SingleAP='" +model.SingleAP +"',MultipleAP='" + model.MultipleAP +"',ChannelWidth='" +model.ChannelWidth +"', GhzEnabled5='" +model.GhzEnabled5 +"', "+
                        "ChannelWidthAutoor40MHz='" +model.ChannelWidthAutoor40MHz +"', Channelbetween149_161AvilableAndSelected='" +model.Channelbetween149_161AvilableAndSelected +"', "+
                        "Link3DNSusedinWANconfiguration='" +model.Link3DNSusedinWANconfiguration +"', Link3DNSusedinDHCPconfiguration='" +
                        model.Link3DNSusedinDHCPconfiguration + "', RouterSupportIPv6='" +model.RouterSupportIPv6 +"', RouterWanRecivedIPV6fromLink3='" +model.RouterWanRecivedIPV6fromLink3 + 
                        "', LANdevicerecivingIPV6='" + model.LANdevicerecivingIPV6 +"', Canbrowseipv6='" +model.Canbrowseipv6 +"', NTPServer123='" +model.NTPServer123 + "', "+
                        "RouterSupportScheduleReboot='" +model.RouterSupportScheduleReboot +"', ScheduleRebootConfigured='" +model.ScheduleRebootConfigured +"', "+
                        "Reamarks='" +model.Reamarks +"', InternetSpeedTest='" +model.InternetSpeedTest +"', InternetSpeedRemarks='" +model.InternetSpeedRemarks +"', "+
                        "BDIXSpeedTest='" + model.BDIXSpeedTest +"', BDIXSpeedTestRemarks='" +model.BDIXSpeedTestRemarks +"', WifiAnalyzer2_4GHz='" +model.WifiAnalyzer2_4GHz +"',"+
                        "WifiAnalyzer2_4GHzRemarks='"+model.WifiAnalyzer2_4GHzRemarks +"', WifiAnalyzer5GHz='" +model.WifiAnalyzer5GHz +"', WifiAnalyzer5GHzRemarks='" +model.WifiAnalyzer5GHzRemarks +
                        "' WHERE ClientID = '" + model.ClientID + "' AND brslno = '" + model.brslno +"'";

                    int items = await _misDBContext.Database.ExecuteSqlRawAsync(updateQuery);
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
                    string WifiAnalyzer2_4GHzFileWithPath = Path.Combine(uploadPath, model.WifiAnalyzer2_4GHzFileName);
                    using (var stream = new FileStream(WifiAnalyzer2_4GHzFileWithPath, FileMode.Create))
                    {
                        model.WifiAnalyzer2_4GHzUploadFileFileDetails.CopyTo(stream);
                    }
                    string WifiAnalyzer5GHzFileWithPath = Path.Combine(uploadPath, model.WifiAnalyzer5GHzFileName);
                    using (var stream = new FileStream(WifiAnalyzer5GHzFileWithPath, FileMode.Create))
                    {
                        model.WifiAnalyzer5GHzUploadFileDetails.CopyTo(stream);
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
                    var insertQuery = "INSERT INTO Mis_CheckListDetails (ClientID, brslno, OpticalLaser, ONUisConnectedwithCompatibleAdapter, ONUlabeledwithSubscriberID, "+
                        "Cat6cableused, ClientuseWiFiRouter, Deafultloginpasswordchanged, PasswrodisconfiguredasPrescribebyLink3, RouterFirwareUptoDate, UpnpDiabled, "+
                        "WPSdisabled, WPA2securityEnabledtoAccessWiFi, RemoteManagementPortEnabled, RouterpositionedINproperplace,RouterisConnectedwithCompatibleAdapter,"+
                        "OnuportSpeedFE_GE, SpectrumAnalyzerusedRFchannelchecked, RouterType, ControllerOwner, GhzEnabled2_4,SingleAP,MultipleAP, ChannelWidth, GhzEnabled5,"+
                        "ChannelWidthAutoor40MHz,Channelbetween149_161AvilableAndSelected, Link3DNSusedinWANconfiguration, Link3DNSusedinDHCPconfiguration, RouterSupportIPv6, "+
                        "RouterWanRecivedIPV6fromLink3, LANdevicerecivingIPV6, Canbrowseipv6,NTPServer123, RouterSupportScheduleReboot, ScheduleRebootConfigured, Reamarks, InternetSpeedTest, "+
                        "InternetSpeedRemarks, InternetSpeedFileName, InternetSpeedUploadFile, BDIXSpeedTest, BDIXSpeedTestRemarks,BDIXSpeedTestFileName, BDIXSpeedTestUploadFile, "+
                        "WifiAnalyzer2_4GHz, WifiAnalyzer2_4GHzRemarks,WifiAnalyzer2_4GHzFileName, WifiAnalyzer2_4GHzUploadFile, WifiAnalyzer5GHz,WifiAnalyzer5GHzRemarks, "+
                        "WifiAnalyzer5GHzFileName, WifiAnalyzer5GHzUploadFile, EntryBy) VALUES('" +model.ClientID + "','" + model.brslno +
                        "','" + model.OpticalLaser +"','" +model.ONUisConnectedwithCompatibleAdapter +"','" + model.ONUlabeledwithSubscriberID + 
                        "','" + model.Cat6cableused +"','" +model.ClientuseWiFiRouter +"','" +model.Deafultloginpasswordchanged + "','" +model.PasswrodisconfiguredasPrescribebyLink3 + 
                        "','" +model.RouterFirwareUptoDate + "','" +model.UpnpDiabled +"','" +model.WPSdisabled +"','" +model.WPA2securityEnabledtoAccessWiFi + 
                        "','" +model.RemoteManagementPortEnabled + "','" + model.RouterpositionedINproperplace +"','" +model.RouterisConnectedwithCompatibleAdapter +
                        "','" +model.OnuportSpeedFE_GE + "','" +model.SpectrumAnalyzerusedRFchannelchecked + "','" +model.RouterType + "','" + 
                        model.ControllerOwner + "','" +model.GhzEnabled2_4 +"','" + model.SingleAP + "','" +model.MultipleAP + "','" +
                        model.ChannelWidth + "','" + model.GhzEnabled5 +"','" + model.ChannelWidthAutoor40MHz +"','" + model.Channelbetween149_161AvilableAndSelected + 
                        "','" +model.Link3DNSusedinWANconfiguration +"','" +model.Link3DNSusedinDHCPconfiguration + "','" + model.RouterSupportIPv6 +"','" + 
                        model.RouterWanRecivedIPV6fromLink3 +"','" +model.LANdevicerecivingIPV6 +"','" +model.Canbrowseipv6 +"','" +model.NTPServer123 +"','" +model.RouterSupportScheduleReboot + 
                        "','" +model.ScheduleRebootConfigured +"','" + model.Reamarks + "','" + model.InternetSpeedTest +"','" +model.InternetSpeedRemarks + 
                        "','" +model.InternetSpeedFileName +"','" + model.InternetSpeedUploadFile + "','" + model.BDIXSpeedTest + "','" +
                        model.BDIXSpeedTestRemarks + "','" +model.BDIXSpeedTestFileName +"','" + model.BDIXSpeedTestUploadFile +"','" +                  
                        model.WifiAnalyzer2_4GHz + "','" + model.WifiAnalyzer2_4GHzRemarks +"','" +model.WifiAnalyzer2_4GHzFileName +"','" +model.WifiAnalyzer2_4GHzUploadFile +
                        "','" +model.WifiAnalyzer5GHz +"','" +model.WifiAnalyzer5GHzRemarks +"','" +model.WifiAnalyzer5GHzFileName + "','" +
                        model.WifiAnalyzer5GHzUploadFile + "','" +model.EntryBy +"')";

                    int items = await _misDBContext.Database.ExecuteSqlRawAsync(insertQuery);
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
       


        public async Task<ApiResponse> UploadAndSaveChecklistFile(FileUploadModel uploadModel, ClaimsPrincipal user, string ip)
        {
            var methodName = "ChecklistService/UploadAndSaveChecklistFile";
            try
            {
                string uploadPath = _configuration.GetValue<string>("FilePath:Path");
               // string path = Path.Combine(Directory.GetCurrentDirectory(), @"E:\\MIS\\PresaleAttachfile");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                FileInfo fileInfo = new FileInfo(uploadModel.FileDetails.FileName);
                string fileName = uploadModel.FileDetails.FileName;
                string fileExtension = fileInfo.Extension;
                string fileNameWithPath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    uploadModel.FileDetails.CopyTo(stream);
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "File has been uploded successfully.",
                    Data = null
                };
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

        public async Task<ApiResponse> GetMisChecklistDetailsB2BByTicketId(string ticketid, string ip)
        {
            var methodName = "ChecklistService/GetMisChecklistDetailsB2BByTicketId";
            try
            {
                var query = "select m.ClientID,m.brslno,TeamEqupmentStatus, IpvlanbwStatus, ClientAccessSchedule, TowerStatusRadioLink, PhysicalMediaPrimaryLink, " +
                    "PhysicalMediaSecondaryLink,PatchcordCat6,ConnectedRouterFirewal, Devicemmarkedprimarysecondary,Upsbackup,RoutingStatus, "+
                    "ConfigurationPartManaged, GettingProperInterface,ConfigureRouterProperly,Testedallreachability,Testedallredundency, "+
                    "BwstatusFromMedia, LanCableNetworkExists, PacketLossGateway, bwstatuslan,LanProblem,WifiType,WiFiConfigurationManaged, "+
                    "FrequencyBand,RouterPositionokUserArea, GhzEnabled2_4,SingleAP,MultipleAP,ChannelWidth,GhzEnabled5,ChannelWidthAutoor40MHz, "+
                    "Channelbetween149_161AvilableAndSelected,RouterFirwareUptoDate,RemoteManagementPortEnabled,InternetSpeedTest, "+
                    "InternetSpeedUploadFile,BDIXSpeedTest,BDIXSpeedTestUploadFile,WifiAnalyzer2_4GHz,Reamarks,WifiAnalyzer5GHz, "+
                    "RSSIinDbmExample10,RSSIinDbmExample60 From Post_Installation p WITH(NOLOCK) inner join clientDatabaseMain c "+
                    "ON p.Cli_code = c.brCliCode and p.CliAdrNewCode = c.brAdrNewCode inner join Mis_CheckListDetails m on "+
                    "c.brCliCode = m.ClientID and c.brSlNo = m.brslno where p.TrackingInfo = '" + ticketid + "'";
                var info = await _misDBContext.GetMisChecklistB2BInfos.FromSqlRaw(query).FirstOrDefaultAsync();


                if (info == null)
                {
                    throw new Exception("MIS_Checklist B2B info not found.");
                }


                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(ticketid, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketid, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetChecklistB2BData(string ip)
        {
            var methodName = "ChecklistService/GetChecklistB2BData";
            try
            {
                var query = "SELECT Id, CheckList,ddlname FROM WFA2.dbo.tbl_Checklist order by Id desc";
                var info = await _misDBContext.GetChecklistB2BData.FromSqlRaw(query).ToListAsync();

                //var query = "SELECT distinct ddlname FROM WFA2.dbo.tbl_Checklist";
                //var info = await _misDBContext.GetChecklistB2BData.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("ChecklistB2B Data not found.");
                }

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


        public async Task<ApiResponse> SaveChecklistB2BData(MisCheckListRequestModel model, string ip)
        {
            var methodName = "ChecklistService/SaveChecklistB2BData";
            try
            {
                var query = "SELECT * FROM Mis_CheckListDetails WHERE ClientID = '" + model.ClientID + "' and brslno='" + model.brslno + "'";
                var info = await _misDBContext.GetMisChecklistInfos.FromSqlRaw(query).FirstOrDefaultAsync();
                string statusMessage = "";

                model.InternetSpeedFileName = model.InternetSpeedUploadFileDetails.FileName;
                model.InternetSpeedUploadFile = model.ClientID + "_" + model.InternetSpeedFileName;
                model.BDIXSpeedTestFileName = model.BDIXSpeedTestUploadFileDetails.FileName;
                model.BDIXSpeedTestUploadFile = model.ClientID + "_" + model.BDIXSpeedTestFileName;
                model.WifiAnalyzer2_4GHzFileName = model.WifiAnalyzer2_4GHzUploadFileFileDetails.FileName;
                model.WifiAnalyzer2_4GHzUploadFile = model.ClientID + "_" + model.WifiAnalyzer2_4GHzFileName;
                model.WifiAnalyzer5GHzFileName = model.WifiAnalyzer5GHzUploadFileDetails.FileName;
                model.WifiAnalyzer5GHzUploadFile = model.ClientID + "_" + model.WifiAnalyzer5GHzFileName;


                if (info != null)
                {
                    var updateQuery = "UPDATE Mis_CheckListDetails SET TeamEqupmentStatus = '" + model.TeamEqupmentStatus + "', " +
                        "IpvlanbwStatus = '" + model.IpvlanbwStatus + "', " +
                        "ClientAccessSchedule = '" + model.ClientAccessSchedule + "', TowerStatusRadioLink = '" + model.TowerStatusRadioLink + "', " +
                        "PhysicalMediaPrimaryLink = '" + model.PhysicalMediaPrimaryLink + "', PhysicalMediaSecondaryLink = '" + model.PhysicalMediaSecondaryLink + "', " +
                        "PatchcordCat6 = '" + model.PatchcordCat6 + "', " +
                        "ConnectedRouterFirewal = '" + model.ConnectedRouterFirewal + "', Devicemmarkedprimarysecondary = '" + model.devicemmarkedprimarysecondary + "', " +
                        "Upsbackup = '" + model.upsbackup + "', RoutingStatus = '" + model.RoutingStatus + "', " +
                        "ConfigurationPartManaged = '" + model.ConfigurationPartManaged + "', GettingProperInterface =  " +
                        "'" + model.GettingProperInterface + "', ConfigureRouterProperly = '" + model.ConfigureRouterProperly + "', " +
                        "Testedallreachability = '" + model.Testedallreachability + "',Testedallredundency= '" + model.Testedallredundency + "', " +
                        "BwstatusFromMedia='" + model.BwstatusFromMedia + "',LanCableNetworkExists= '" + model.LanCableNetworkExists + "', PacketLossGateway='" + model.PacketLossGateway + "', " +
                        "bwstatuslan='" + model.bwstatuslan + "',LanProblem='" + model.LanProblem + "',WifiType='" + model.WifiType + "', FrequencyBand='" + model.FrequencyBand + "', " +
                        "WiFiConfigurationManaged='" + model.WiFiConfigurationManaged + "', RouterPositionokUserArea='" + model.RouterPositionokUserArea + "', " +
                        "GhzEnabled2_4='" + model.GhzEnabled2_4 + "', SingleAP='" +
                        model.SingleAP + "', MultipleAP='" + model.MultipleAP + "', ChannelWidth='" + model.ChannelWidth +
                        "', GhzEnabled5='" + model.GhzEnabled5 + "', ChannelWidthAutoor40MHz='" + model.ChannelWidthAutoor40MHz + "', Channelbetween149_161AvilableAndSelected='" + model.Channelbetween149_161AvilableAndSelected + "', " +
                        "RouterFirwareUptoDate='" + model.RouterFirwareUptoDate + "', RemoteManagementPortEnabled='" + model.RemoteManagementPortEnabled + "', " +
                        "Reamarks='" + model.Reamarks + "', InternetSpeedTest='" + model.InternetSpeedTest + "', InternetSpeedRemarks='" + model.InternetSpeedRemarks + "', " +
                        "BDIXSpeedTest='" + model.BDIXSpeedTest + "', BDIXSpeedTestRemarks='" + model.BDIXSpeedTestRemarks + "', WifiAnalyzer2_4GHz='" + model.WifiAnalyzer2_4GHz + "'," +
                        "WifiAnalyzer2_4GHzRemarks='" + model.WifiAnalyzer2_4GHzRemarks + "', WifiAnalyzer5GHz='" + model.WifiAnalyzer5GHz + "', RSSIinDbmExample10='" + model.RSSIinDbmExample10 +
                        "', RSSIinDbmExample60='"+model.RSSIinDbmExample60+"' WHERE ClientID = '" + model.ClientID + "' AND brslno = '" + model.brslno + "'";

                    int items = await _misDBContext.Database.ExecuteSqlRawAsync(updateQuery);
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
                    string WifiAnalyzer2_4GHzFileWithPath = Path.Combine(uploadPath, model.WifiAnalyzer2_4GHzFileName);
                    using (var stream = new FileStream(WifiAnalyzer2_4GHzFileWithPath, FileMode.Create))
                    {
                        model.WifiAnalyzer2_4GHzUploadFileFileDetails.CopyTo(stream);
                    }
                    string WifiAnalyzer5GHzFileWithPath = Path.Combine(uploadPath, model.WifiAnalyzer5GHzFileName);
                    using (var stream = new FileStream(WifiAnalyzer5GHzFileWithPath, FileMode.Create))
                    {
                        model.WifiAnalyzer5GHzUploadFileDetails.CopyTo(stream);
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
                    var insertQuery = "INSERT INTO Mis_CheckListDetails (ClientID, brslno, TeamEqupmentStatus, IpvlanbwStatus, ClientAccessSchedule, " +
                        "TowerStatusRadioLink, PhysicalMediaPrimaryLink, PhysicalMediaSecondaryLink, PatchcordCat6, ConnectedRouterFirewal, Devicemmarkedprimarysecondary, " +
                        "Upsbackup, RoutingStatus, ConfigurationPartManaged, GettingProperInterface,ConfigureRouterProperly," +
                        "Testedallreachability, Testedallredundency, BwstatusFromMedia, LanCableNetworkExists, PacketLossGateway,bwstatuslan,LanProblem, WifiType, FrequencyBand," +
                        "WiFiConfigurationManaged,RouterPositionokUserArea, GhzEnabled2_4, SingleAP, MultipleAP, " +
                        "ChannelWidth, GhzEnabled5, ChannelWidthAutoor40MHz,Channelbetween149_161AvilableAndSelected, RouterFirwareUptoDate, RemoteManagementPortEnabled, Reamarks, InternetSpeedTest, " +
                        "InternetSpeedRemarks, InternetSpeedFileName, InternetSpeedUploadFile, BDIXSpeedTest, BDIXSpeedTestRemarks,BDIXSpeedTestFileName, BDIXSpeedTestUploadFile, " +
                        "WifiAnalyzer2_4GHz, WifiAnalyzer2_4GHzRemarks,WifiAnalyzer2_4GHzFileName, WifiAnalyzer2_4GHzUploadFile, WifiAnalyzer5GHz,WifiAnalyzer5GHzRemarks, " +
                        "WifiAnalyzer5GHzFileName, WifiAnalyzer5GHzUploadFile,RSSIinDbmExample10,RSSIinDbmExample60, EntryBy) VALUES('" + model.ClientID + "','" + model.brslno +
                        "','" + model.TeamEqupmentStatus + "','" + model.IpvlanbwStatus + "','" + model.ClientAccessSchedule +
                        "','" + model.TowerStatusRadioLink + "','" + model.PhysicalMediaPrimaryLink + "','" + model.PhysicalMediaSecondaryLink + "','" + model.PatchcordCat6 +
                        "','" + model.ConnectedRouterFirewal + "','" + model.devicemmarkedprimarysecondary + "','" + model.upsbackup + "','" + model.RoutingStatus +
                        "','" + model.ConfigurationPartManaged + "','" + model.GettingProperInterface + "','" + model.ConfigureRouterProperly +
                        "','" + model.Testedallreachability + "','" + model.Testedallredundency + "','" + model.BwstatusFromMedia + "','" +
                        model.LanCableNetworkExists + "','" + model.PacketLossGateway + "','" + model.bwstatuslan + "','" + model.LanProblem + "','" +
                        model.WifiType + "','" + model.FrequencyBand + "','" + model.WiFiConfigurationManaged + "','" + model.RouterPositionokUserArea +
                        "','" + model.GhzEnabled2_4 + "','" + model.SingleAP + "','" + model.MultipleAP + "','" +
                        model.ChannelWidth + "','" + model.GhzEnabled5 + "','" + model.ChannelWidthAutoor40MHz + "','" + model.Channelbetween149_161AvilableAndSelected + "','" + model.RouterFirwareUptoDate +
                        "','" + model.RemoteManagementPortEnabled + "','" + model.Reamarks + "','" + model.InternetSpeedTest + "','" + model.InternetSpeedRemarks +
                        "','" + model.InternetSpeedFileName + "','" + model.InternetSpeedUploadFile + "','" + model.BDIXSpeedTest + "','" +
                        model.BDIXSpeedTestRemarks + "','" + model.BDIXSpeedTestFileName + "','" + model.BDIXSpeedTestUploadFile + "','" +
                        model.WifiAnalyzer2_4GHz + "','" + model.WifiAnalyzer2_4GHzRemarks + "','" + model.WifiAnalyzer2_4GHzFileName + "','" + model.WifiAnalyzer2_4GHzUploadFile +
                        "','" + model.WifiAnalyzer5GHz + "','" + model.WifiAnalyzer5GHzRemarks + "','" + model.WifiAnalyzer5GHzFileName + "','" +
                        model.WifiAnalyzer5GHzUploadFile + "','" + model.RSSIinDbmExample10 + "','"+ model.RSSIinDbmExample60 + "','"+ model.EntryBy+"')";

                    int items = await _misDBContext.Database.ExecuteSqlRawAsync(insertQuery);
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

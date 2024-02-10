using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex;
using L3T.Infrastructure.Helpers.Implementation.Email;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.MailConfiguration;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using tik4net.Objects.User;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace L3T.Infrastructure.Helpers.Implementation.FieldForce
{

    public class InstallationTicketService : IInstallationTicketService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MisDBContext _misDBContext;
        private readonly RsmDbContext _rsmDBContext;
        private readonly ILogger<InstallationTicketService> _logger;
        private readonly FFWriteDBContext _ffWriteDBContext;
        private IConfiguration _configuration;
        private readonly IMailSenderService _mailSenderService;
        public InstallationTicketService(
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

        public async Task<ApiResponse> GetSubscriptionInfoByTicketId(string ticketid, string userId, string ip)
        {
            var methodName = "InstallationTicketService/GetSubscriptionInfoByTicketId";
            try
            {
                var allInfo = new InstallationTicketInformationViewModel();

                var query = "select p.CompanyName,p.CommisionDate,p.EntryDate, p.SalesPerson,p.Mname,p.Shortdocuments,p.Initiator,p.BranchName, " +
                            "p.BtsName,p.Cli_code,p.Cli_Adr_Code,p.InsService,p.InStatus,c.contact_det,c.Contact_Designation,c.phone_no,c.email_id, " +
                            "c.brAddress1,c.brAddress2, c.brAreaGroup, c.brArea,c.brwebsite,c.ClientRefarence,c.MqID, CASE " +
                            "WHEN c.brstatussla = 1 THEN 'Done' WHEN c.brstatussla = 2 THEN 'Pending and required' WHEN c.brstatussla " +
                            "= 3 THEN 'Not Required' WHEN c.brstatussla = 0 THEN 'None' End as brstatussla, c.brsladate,c.brdateinception, " +
                            "c.brcompanyname,c.branchmanager, c.brMktGroup, c.brCategory,c.brNature,c.brBusinessType,c.mrtg_link,c.brCliCode, " +
                            "c.brAdrCode,c.brSlNo,c.i_ins_date,c.i_bill_date,c.i_seller, c.brAdrNewCode From Post_Installation p " +
                            "WITH(NOLOCK) inner join clientDatabaseMain c ON p.Cli_code = c.brCliCode and p.CliAdrNewCode " +
                            "= c.brAdrNewCode where p.TrackingInfo = '" + ticketid + "'";

                var subscription = await _misDBContext.GetSubscriptionInfos.FromSqlRaw(query).FirstOrDefaultAsync();

                // GetAllPendingTicketByAssignUserResponseModel getTicket = await _misDBContext.Post_Instalation.Where(x => x.RefNo == ticketid).AsNoTracking().FirstOrDefaultAsync();
                if (subscription == null)
                {
                    throw new Exception("Subscription info not found.");
                }

                var query2 = " SELECT brName, contact_det, phone_no, email_id, brAddress1, brAddress2, brAreaGroup, brArea " +
                "FROM clientDatabaseP2PAddress  WHERE brCliCode = '" + subscription.brCliCode + "' AND brSlNo = '" + subscription.brSlNo + "'";
                var p2pAddressInfo = await _misDBContext.GetClientP2PAddressInfos.FromSqlRaw(query2).FirstOrDefaultAsync();

                if (p2pAddressInfo != null)
                {
                    subscription.p2p_brName = p2pAddressInfo.brName;
                    subscription.p2p_contact_det = p2pAddressInfo.contact_det;
                    subscription.p2p_phone_no = p2pAddressInfo.phone_no;
                    subscription.p2p_email_id = p2pAddressInfo.email_id;
                    subscription.p2p_brAddress1 = p2pAddressInfo.brAddress1;
                    subscription.p2p_brAddress2 = p2pAddressInfo.brAddress2;
                    subscription.p2p_brAreaGroup = p2pAddressInfo.brAreaGroup;
                    subscription.p2p_brArea = p2pAddressInfo.brArea;
                }

                var query3 = " select br_contact_name,br_adr1,br_adr2,br_area,br_sub_area,br_contact_num,br_contact_email " +
                "from clientBillingAddress where br_cli_code = '" + subscription.brCliCode + "' and br_sl_no = '" + subscription.brSlNo + "'";
                var billingInfo = await _misDBContext.GetClientBillingAddressInfos.FromSqlRaw(query3).FirstOrDefaultAsync();

                if (billingInfo != null)
                {
                    subscription.billingAddress = billingInfo.br_contact_name + "," + billingInfo.br_adr1 + "," + billingInfo.br_adr2
                        + "," + billingInfo.br_area + "," + billingInfo.br_sub_area + "," + billingInfo.br_contact_num + "," + billingInfo.br_contact_email;
                }

                allInfo.SubscriptionInfo = subscription;
                var pendingReson = await GetPendingReasonListForAddComment(ip);
                allInfo.PendingReasonResponse = (List<PendingReasonResponseModel>)pendingReson.Data;

                var serviceName = await GetSendMailServiceListForAddComment(ticketid, userId, ip);
                allInfo.SendMailServiceLis = (List<Cli_PendingServiceNameModel>)serviceName.Data;

                var pendingReasonSelect_sql = "select * from Cli_Mktpending  where RefNO='" + ticketid + "' and PendingReason<>'12'";
                var pendingReasonSelectedValue = await _misDBContext.Cli_Mktpending.FromSqlRaw(pendingReasonSelect_sql).FirstOrDefaultAsync();
                if(pendingReasonSelectedValue != null)
                {
                    allInfo.PendingReasonSelectedValue = pendingReasonSelectedValue.PendingReason;
                }
                else
                {
                    allInfo.PendingReasonSelectedValue = 0;
                }
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = allInfo
                };
                await InsertRequestResponse(ticketid, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketid, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }



        }



        public async Task<ApiResponse> GetHardwareInfoByTicketId(string ticketid, string ip)
        {
            var methodName = "InstallationTicketService/GetHardwareInfoByTicketId";
            try
            {
                var query = "SELECT Otc,Mrc, " +
                  "CASE WHEN BillingCycleID = 2 THEN 'Monthly' WHEN BillingCycleID = 3 THEN 'Quarterly' " +
                 " WHEN BillingCycleID = 4 THEN 'Half- Yearly' WHEN BillingCycleID = 5 THEN 'Yearly' " +
                 "END AS BillingCycle, CASE WHEN BillingType = 'Y' THEN 'Post Paid'  WHEN BillingType = 'N' THEN 'Pre Paid' " +
                 "END AS BillingType,VatProcess,Comments from Post_MainIns WITH(NOLOCK) where RefNO = '" + ticketid + "' AND ServiceID = '6'";
                var info = await _misDBContext.GetHardwareInfos.FromSqlRaw(query).FirstOrDefaultAsync();

                // GetAllPendingTicketByAssignUserResponseModel getTicket = await _misDBContext.Post_Instalation.Where(x => x.RefNo == ticketid).AsNoTracking().FirstOrDefaultAsync();
                if (info == null)
                {
                    throw new Exception("Hardware info not found.");
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



        public async Task<ApiResponse> GetInternetInfoByTicketId(string ticketid, string ip)
        {
            var methodName = "InstallationTicketService/GetInternetInfoByTicketId";
            try
            {
                var query = "SELECT i.InternetBandwidthCIR, i.InternetBandwidthMIR, i.VsatBandwidthDownCir, i.VsatBandwidthDownMir, " +
                    "i.NoteBandwith, c.brCliCode, c.brSlNo FROM Post_Installation p WITH(NOLOCK) Inner Join clientDatabaseMain c " +
                    "on p.CliAdrNewCode = c.brAdrNewCode inner join ClientTechnicalInfo i on c.brCliCode = i.ClientCode " +
                    "AND c.brSlNo = i.ClientSlNo where p.TrackingInfo = '" + ticketid + "'";
                var info = await _misDBContext.GetBandwidthInfos.FromSqlRaw(query).FirstOrDefaultAsync();

                // GetAllPendingTicketByAssignUserResponseModel getTicket = await _misDBContext.Post_Instalation.Where(x => x.RefNo == ticketid).AsNoTracking().FirstOrDefaultAsync();
                if (info == null)
                {
                    throw new Exception("Bandwidth info not found.");
                }


                var query2 = "SELECT dbo.BtsSetup.BtsSetupName FROM dbo.Post_MainIns WITH(NOLOCK) INNER JOIN " +
                      "dbo.BtsSetup ON dbo.Post_MainIns.BtsID = dbo.BtsSetup.BtsSetupID where RefNO = '" + ticketid + "'";
                var Btsinfo = await _misDBContext.GetBtsSetupInfos.FromSqlRaw(query2).FirstOrDefaultAsync();



                var query3 = "SELECT ClientDatabaseMediaSetup.MediaName, clientDatabaseMediaDetails.brCliCode, " +
                    "clientDatabaseMediaDetails.brSlNo, clientDatabaseMediaDetails.brCliAdrCode FROM " +
                    "clientDatabaseMediaDetails WITH(NOLOCK) INNER JOIN ClientDatabaseMediaSetup ON clientDatabaseMediaDetails.MedID " +
                    "= ClientDatabaseMediaSetup.MedID where brCliCode='" + info.brCliCode + "' and brSlNo='" + info.brSlNo + "'";
                var mediainfo = await _misDBContext.GetMediaInfos.FromSqlRaw(query3).FirstOrDefaultAsync();


                var query4 = "SELECT Otc,Mrc, " +
                  "CASE WHEN BillingCycleID = 2 THEN 'Monthly' WHEN BillingCycleID = 3 THEN 'Quarterly' " +
                 " WHEN BillingCycleID = 4 THEN 'Half- Yearly' WHEN BillingCycleID = 5 THEN 'Yearly' WHEN BillingCycleID = 99 THEN 'Not Applicable' " +
                 "END AS BillingCycle, CASE WHEN BillingType = 'Y' THEN 'Post Paid'  WHEN BillingType = 'N' THEN 'Pre Paid' " +
                 "END AS BillingType,VatProcess,Comments from Post_MainIns WITH(NOLOCK) where RefNO = '" + ticketid + "'  AND ServiceID = '1'";

                var postMainInsinfo = await _misDBContext.GetHardwareInfos.FromSqlRaw(query4).FirstOrDefaultAsync();
                InternetInfoResponseModel internetInfo;
                if (postMainInsinfo != null)
                {
                    internetInfo = new InternetInfoResponseModel()
                    {
                        Otc = postMainInsinfo.Otc,
                        Mrc = postMainInsinfo.Mrc,
                        BillingCycle = postMainInsinfo.BillingCycle,
                        BillingType = postMainInsinfo.BillingType,
                        VatProcess = postMainInsinfo.VatProcess,
                        Comments = postMainInsinfo.Comments,
                        InternetBandwidthCIR = info.InternetBandwidthCIR,
                        InternetBandwidthMIR = info.InternetBandwidthMIR,
                        VsatBandwidthDownCir = info.VsatBandwidthDownCir,
                        VsatBandwidthDownMir = info.VsatBandwidthDownMir,
                        NoteBandwith = info.NoteBandwith,
                        BtsSetupName = Btsinfo != null ? Btsinfo.BtsSetupName : "",
                        MediaName = mediainfo != null ? mediainfo.MediaName : ""
                    };
                }
                else
                {
                    internetInfo = new InternetInfoResponseModel()
                    {

                        InternetBandwidthCIR = info.InternetBandwidthCIR,
                        InternetBandwidthMIR = info.InternetBandwidthMIR,
                        VsatBandwidthDownCir = info.VsatBandwidthDownCir,
                        VsatBandwidthDownMir = info.VsatBandwidthDownMir,
                        NoteBandwith = info.NoteBandwith,
                        BtsSetupName = Btsinfo != null ? Btsinfo.BtsSetupName : "",
                        MediaName = mediainfo != null ? mediainfo.MediaName : ""
                    };
                }
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = internetInfo
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



        public async Task<ApiResponse> GetIpTelephonyInfoByTicketId(string ticketid, string ip)
        {
            var methodName = "InstallationTicketService/GetIpTelephonyInfoByTicketId";
            try
            {
                var query = "SELECT Otc,Mrc, CASE WHEN BillingCycleID = 2 THEN 'Monthly' WHEN " +
                    "BillingCycleID = 3 THEN 'Quarterly' WHEN BillingCycleID = 4 THEN 'Half- Yearly' WHEN " +
                    "BillingCycleID = 5 THEN 'Yearly' END AS BillingCycle, CASE WHEN BillingType = 'Y' THEN " +
                    "'Post Paid'  WHEN BillingType = 'N' THEN 'Pre Paid' END AS BillingType, CASE WHEN " +
                    "PackageID = 1 THEN 'Retail' WHEN PackageID = 2 THEN 'Corporate' WHEN PackageID = 0 THEN '' END AS Package, " +
                    "CASE WHEN PulseID = 1 THEN '1 Second' WHEN PulseID = 2 THEN '15 Second' WHEN PulseID = 3 THEN '30 Second' " +
                    "WHEN PulseID = 4 THEN '60 Second' WHEN PulseID = 5 THEN 'None' WHEN PulseID = 6 THEN '10 Second' " +
                    "WHEN PulseID = 0 THEN '' END AS Pulse,NoOfLines,VatProcess,Comments from Post_MainIns WITH(NOLOCK) " +
                    "where RefNO  = '" + ticketid + "' AND ServiceID = '14'";

                var info = await _misDBContext.GetIpTelephonyInfos.FromSqlRaw(query).FirstOrDefaultAsync();

                if (info == null)
                {
                    throw new Exception("IpTelephony info not found.");
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

        public async Task<ApiResponse> GetMktAndBillingInfoByTicketId(string ticketid, string ip)
        {
            var methodName = "InstallationTicketService/GetMktAndBillingInfoByTicketId";
            try
            {
                var query = "select c.DistributorSubscriberID,c.RRPSubscriberID, p.InsService, p.Cli_code, " +
                    "p.Cli_Adr_Code From Post_Installation p WITH(NOLOCK) inner join clientDatabaseMain c " +
                    "ON p.Cli_code = c.brCliCode and p.CliAdrNewCode = c.brAdrNewCode where p.TrackingInfo = '" + ticketid + "'";

                var info = await _misDBContext.GetMktAndBillingInfos.FromSqlRaw(query).FirstOrDefaultAsync();

                if (info == null)
                {
                    throw new Exception("Mkt and Billing info not found.");
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


        public async Task<ApiResponse> GetAllTeamNameByTicketId(string userId, string ticketid, string ip)
        {
            var methodName = "InstallationTicketService/GetAllTeamNameByTicketId";
            try
            {
                var query = "select distinct Service from view_pendin where userid='" + userId + "' and RefNo='" + ticketid + "' " +
                    "and Status='INI'";

                var info = await _misDBContext.GetTeamNames.FromSqlRaw(query).FirstOrDefaultAsync();

                if (info == null)
                {
                    throw new Exception("Team Name not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(ticketid, response, methodName, ip, userId, null);
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


        public async Task<ApiResponse> UpdateInstallationScheduleData(string userId, installationScheduleRequestModel model, string ip)
        {
            var methodName = "InstallationTicketService/UpdateInstallationScheduleData";
            try
            {
                var query = "update cli_mktpending set ScheduleDate=Convert(Datetime,'" + model.ScheduleDate + "',103),Sceduleby='" + userId + "' where refno= '" + model.TicketRefNo + "'";

                int items = await _misDBContext.Database.ExecuteSqlRawAsync(query);
                string statusMessage = "";

                if (items == 1)
                {
                    statusMessage = "data has been Updated successfully";
                }
                else
                {
                    throw new Exception("something went wrong");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = statusMessage,
                    Data = null
                };
                await InsertRequestResponse(model.TicketRefNo, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model.TicketRefNo, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> GetPendingCategoryList(string ip)
        {
            var methodName = "InstallationTicketService/GetPendingCategoryList";
            try
            {
                var query = "select Id, CategoryName from tbl_Pending_Category where status = '1'";

                var info = await _misDBContext.GetPendingCategories.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("Pending Category not found.");
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


        public async Task<ApiResponse> GetPendingReasonList(string categoryId, string ip)
        {
            var methodName = "InstallationTicketService/GetPendingReasonList";
            try
            {
                var query = "select ID,PendingReson from RSM_ReasonPendingInstallation where " +
                    "status=1 and  Category_id = '" + categoryId + "'";

                var info = await _misDBContext.GetPendingResons.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("Pending Reason not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(categoryId, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(categoryId, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> GetServiceCheckboxListData(string ticketId, string ip)
        {
            var methodName = "InstallationTicketService/GetServiceCheckboxList";
            try
            {
                var query = "SELECT Service FROM[Cli_Pending] where RefNo = '" + ticketId + "' and Service!= 'CC' " +
                    " and Service!= 'READY FOR INVOICE' and Service!= 'SD REVIEW' and Service!= 'MKT' and Status = 'INI' " +
                    "Group by Service ORDER BY[Service]";

                var info = await _misDBContext.GetCheckboxList.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("Service Checkbox List not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(ticketId, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketId, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> UpdateCommentData(UpdateCommentRequestModel model, ClaimsPrincipal user, string ip)
        {
            var methodName = "InstallationTicketService/UpdateCommentData";
            try
            {
                var l3id = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();

                var query = "update Cli_Mktpending set PendingReason='" + model.ReasonId + "',LastComments='" + model.CommentText.Replace("'", "''") +
                      "',LastCommentsDate=Convert(Datetime,'" + model.CommentDate + "',103) where Refno='" + model.TicketRefNo + "'";

                int status = await _misDBContext.Database.ExecuteSqlRawAsync(query);

                string mailTemplate = await getEmailTemplate(model, user);
                var logQuery = "update Cli_EmailLog set MailBody='" + mailTemplate.Replace("'", "''") + "' where CTID='" + model.TicketRefNo + "'";
                int logStatus = await _misDBContext.Database.ExecuteSqlRawAsync(logQuery);

                var companyNameQuery = "select CompanyName From Post_Installation where TrackingInfo='" + model.TicketRefNo + "'";
                var companyName = await _misDBContext.getCompanyName.FromSqlRaw(companyNameQuery).FirstOrDefaultAsync();
                string subject = "[PENDING INSTALLATION:]" + model.TicketRefNo + "[" + companyName.CompanyName + "]";

                string fromAddress = _configuration["ApplicationSettings:FromAddress"];
                List<string> toAddress = new List<string>();
                foreach (var item in model.CheckboxList)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var teamEmailQuery = "SELECT Team_Email from tbl_Team_info where Team_Name='" + item.Replace("&amp;", "&") + "'";
                        var teamEmail = await _misDBContext.getTeamEmail.FromSqlRaw(teamEmailQuery).FirstOrDefaultAsync();
                        if (teamEmail != null)
                        {
                            toAddress.Add(teamEmail.Team_Email.ToString());
                        }
                    }
                }

                var email = model.Mail.Split(';', ',');
                if (email.Length > 0)
                {
                    foreach (var to in email)
                    {
                        if (to != "")
                        {
                            toAddress.Add(to);
                        }

                    }
                }

                var additionalEmail = model.AdditionalMail.Split(';', ',');
                if (additionalEmail.Length > 0)
                {
                    foreach (var to in additionalEmail)
                    {
                        if (to != "")
                        {
                            toAddress.Add(to);
                        }

                    }
                }
                if (toAddress != null)
                {
                    await _mailSenderService.SendMail(subject, mailTemplate, fromAddress, toAddress, null, null);
                }
                else
                {
                    throw new Exception("Please select checkbox or enter a mail address");
                }
                var taskHistoryQuery = "INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, " +
                                      "Remarks,TaskStatus) VALUES('" + model.TicketRefNo + "', '" + l3id + "', 'MIS', '" +
                                      "Installation','" + model.CommentText.Replace("'", "''") + "','Comments Add')";
                int taskHistoryStatus = await _rsmDBContext.Database.ExecuteSqlRawAsync(taskHistoryQuery);

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = null
                };
                await InsertRequestResponse(model.TicketRefNo, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model.TicketRefNo, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> SendSmsText(SendSmsRequestModel model, ClaimsPrincipal user, string ip)
        {
            var methodName = "InstallationTicketService/SendSmsText";
            try
            {
                string text = "Dear Customer, please be informed, your internet service is handed " +
                    "over on " + model.CustomerPhoneNo + ". If you have any more queries, please let us know at " + model.ContactPhoneNo + ". Link3";

                var httpRequestMessage = new HttpRequestMessage(
                            HttpMethod.Get,
                            "http://sms.link3.net/http_api/post_api.php?api_id=LS0033&api_password=01715568606&mobile=" + model.CustomerPhoneNo + "&message=" + text.Replace("'", "''") + "");

                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                }

                var l3id = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();
                var taskHistoryQuery = "INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, " +
                                      "Remarks,TaskStatus) VALUES('" + model.TicketRefNo + "', '" + l3id + "', 'MIS', '" +
                                      "Installation','" + model.additionalEmail.Replace("'", "''") + "','Send SMS')";

                int taskHistoryStatus = await _rsmDBContext.Database.ExecuteSqlRawAsync(taskHistoryQuery);

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = null
                };
                await InsertRequestResponse(model.TicketRefNo, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model.TicketRefNo, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetInstallationCommentData(string ticketid, string ip)
        {
            var methodName = "InstallationTicketService/GetMktAndBillingInfoByTicketId";
            try
            {
                var query = "select MailBody from Cli_EmailLog where CTID='" + ticketid + "'";

                var info = await _misDBContext.GetInstallationCommentInfo.FromSqlRaw(query).FirstOrDefaultAsync();

                if (info == null)
                {
                    throw new Exception("Installation Comment info not found.");
                }

                //var response = new ApiResponse()
                //{
                //    Status = "Success",
                //    StatusCode = 200,
                //    Message = "data get successfully.",
                //    Data = info
                //};

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = info.MailBody,
                    Data = null
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



        public async Task<ApiResponse> GetGenaralInfoData(string ticketid, string ip)
        {
            var methodName = "InstallationTicketService/GetGenaralInfoData";
            try
            {
                //var query = "select MailBody from Cli_EmailLog where CTID='" + ticketid + "'";
                var supportOfficeQuery = "SELECT SupportOfficeID, SupportOfficeName FROM SupportOffice";

                var supportOfficeData = await _misDBContext.GetSupportOfficeInfo.FromSqlRaw(supportOfficeQuery).ToListAsync();

                var wireSetupQuery = "SELECT WireID, WireName FROM ClientDatabaseWireSetup ";

                var wireSetupData = await _misDBContext.GetWireSetupInfo.FromSqlRaw(wireSetupQuery).ToListAsync();

                var technologySetupQuery = "SELECT TechID,TechnologyName FROM ClientDatabseTechnologySetup";

                var technologySetupData = await _misDBContext.GetTechnologySetupInfo.FromSqlRaw(technologySetupQuery).ToListAsync();

                var mediaSetupQuery = "SELECT MedID,MediaName FROM ClientDatabaseMediaSetup";

                var mediaSetupData = await _misDBContext.GetMediaSetupInfo.FromSqlRaw(mediaSetupQuery).ToListAsync();

                var generalInfoQuery = "select brDns1,brDns2,brsmtp,brpop3,note_for_bts from Post_Installation p " +
                    "inner join clientDatabaseMain c on p.Cli_code=c.brCliCode and p.CliAdrNewCode=c.brAdrNewCode inner join ClientTechnicalInfo cl on " +
                    "c.brCliCode=cl.ClientCode and c.brSlNo=cl.ClientSlNo where p.TrackingInfo='" + ticketid + "'";

                var generalInfoData = await _misDBContext.GetGeneralInfo.FromSqlRaw(generalInfoQuery).FirstOrDefaultAsync();
                generalInfoData.supportOfficeList = supportOfficeData;
                generalInfoData.wireSetupList = wireSetupData;
                generalInfoData.technologySetupList = technologySetupData;
                generalInfoData.mediaSetupList = mediaSetupData;

                if (generalInfoData == null)
                {
                    throw new Exception("general Info not found.");
                }


                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = generalInfoData
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


        public async Task<ApiResponse> DoneGeneralInfoData(GeneralInfoDoneRequestModel model, ClaimsPrincipal user, string ip)
        {
            var methodName = "InstallationTicketService/DoneGeneralInfoData";
            try
            {
                string statusMessage = "";
                var userId = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();

                var query = "SELECT ISNULL(MAX(CompleteID),0)+1 as MaxID from Cli_InstallationCompleteByTeam";
                var maxId = await _misDBContext.GetMaxIdInfo.FromSqlRaw(query).FirstOrDefaultAsync();

                var installationTeamQuery = "INSERT INTO Cli_InstallationCompleteByTeam (CompleteID, TrackingInfo, TeamName) VALUES ('" +
                maxId.MaxID + "', '" + model.TicketId + "', '" + model.TeamName + "')";
                int installationTeamStatus = await _misDBContext.Database.ExecuteSqlRawAsync(installationTeamQuery);
                statusMessage = installationTeamStatus == 1 ? statusMessage = "data has been Inserted successfully" : throw new Exception("something went wrong");

                var engineerPortfolioQuery = "INSERT INTO tbl_EngineerPortFolio(EngineerName, ComplainFttxPhoneSupport, ComplainFttxPhysicalSupport, " +
                    "ComplainCorporatePhoneSupport, ComplainCorporatePhysicalSupport, ComplainOutofStation, ComplainServer, ComplainRadio, " +
                    "ComplainForward, ComplainFollowUp, ComplainCustomerMeeting, Installation, ShiftingRemote, ShiftingPhysical, ShiftingFollowUp, " +
                    "DismentalRemote, DismentalPhysical, DismentalFollowUp, ReActiveRemote,ReActivatePhysical, ReActivateFollowUp, MaintananceRemote, " +
                    "MaintanancePhysical, MaintananceFollowup, EntryDate)VALUES('" + userId + "', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0',convert(datetime,'" + model.CompleteDate + "',103))";
                int engineerPortfolioStatus = await _misDBContext.Database.ExecuteSqlRawAsync(engineerPortfolioQuery);
                statusMessage = engineerPortfolioStatus == 1 ? statusMessage = "data has been Inserted successfully" : throw new Exception("something went wrong");


                var technicalInfoQuery = "UPDATE ClientTechnicalInfo SET brDns1 = '" + model.brdns1 + "', brDns2 = '" +
                model.brdns2 + "', brsmtp = '" +
                model.brsmtp + "', brpop3 = '" + model.brpop3 + "' WHERE ClientCode = '" +
                model.CliCode + "' AND ClientSlNo = '" + model.SlNo + "'";
                int technicalInfoStatus = await _misDBContext.Database.ExecuteSqlRawAsync(technicalInfoQuery);
                statusMessage = technicalInfoStatus == 1 ? statusMessage = "data has been Updated successfully" : throw new Exception("something went wrong");


                var clientInfoQuery = "update clientDatabaseMain set brSupportOfficeId='" + model.SupportOfficeId + "', brSupportOffice='" +
                model.SupportOfficeName + "' where brCliCode='" + model.CliCode + "' and brSlNo='" + model.SlNo + "'";
                int clientInfoStatus = await _misDBContext.Database.ExecuteSqlRawAsync(clientInfoQuery);
                //statusMessage = clientInfoStatus == 1 ? statusMessage = "data has been Updated successfully" : throw new Exception("something went wrong");

                var taskHistoryQuery = "INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, " +
                                      "Remarks,TaskStatus) VALUES('" + model.TicketId + "', '" + userId + "', 'MIS', '" +
                                      "Installation','','Close')";
                int taskHistoryStatus = await _rsmDBContext.Database.ExecuteSqlRawAsync(taskHistoryQuery);
                statusMessage = taskHistoryStatus == 1 ? statusMessage = "All data has been Updated successfully" : throw new Exception("something went wrong");

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = statusMessage,
                    Data = null
                };
                await InsertRequestResponse(model.TicketId, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model.TicketId, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }



        public async Task<ApiResponse> UpdateGeneralInfoData(GeneralInfoUpdateModel model, ClaimsPrincipal user, string ip)
        {
            var methodName = "InstallationTicketService/UpdateGeneralInfoData";
            try
            {
                string statusMessage = "";
                var userId = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();

                var technicalInfoQuery = "UPDATE ClientTechnicalInfo SET brDns1 = '" + model.brdns1 + "', brDns2 = '" +
                model.brdns2 + "', brsmtp = '" +
                model.brsmtp + "', brpop3 = '" + model.brpop3 + "' WHERE ClientCode = '" +
                model.CliCode + "' AND ClientSlNo = '" + model.SlNo + "'";
                int technicalInfoStatus = await _misDBContext.Database.ExecuteSqlRawAsync(technicalInfoQuery);
                statusMessage = technicalInfoStatus == 1 ? statusMessage = "data has been Updated successfully" : throw new Exception("something went wrong");

                var clientInfoQuery = "update clientDatabaseMain set brSupportOfficeId='" + model.SupportOfficeId + "', brSupportOffice='" +
                model.SupportOfficeName + "' where brCliCode='" + model.CliCode + "' and brSlNo='" + model.SlNo + "'";
                int clientInfoStatus = await _misDBContext.Database.ExecuteSqlRawAsync(clientInfoQuery);
                statusMessage = clientInfoStatus == 1 ? statusMessage = "data has been Updated successfully" : throw new Exception("something went wrong");

                string mailBodyText = await getEmailText(model, user);

                var emailLogQuery = "UPDATE Cli_EmailLog set MailBody='" + mailBodyText.Replace("'", "''") + "' where CTID='" + model.TicketId + "'";
                int emailLogInfoStatus = await _misDBContext.Database.ExecuteSqlRawAsync(emailLogQuery);
                statusMessage = emailLogInfoStatus == 1 ? statusMessage = "data has been Updated successfully" : throw new Exception("something went wrong");

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = statusMessage,
                    Data = null
                };
                await InsertRequestResponse(model.TicketId, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model.TicketId, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> GetIpTelephonyData(string ticketid, string ip)
        {
            var methodName = "InstallationTicketService/GetIpTelephonyData";
            try
            {
                var IpTelephonyInfoQuery = "SELECT TrackingInfo, ClientCode, ClientSL, PhoneNo, NotForIP, Comments, " +
                    "ISP, CommencementDate, ExpiryDate, CPE from Post_Installation p inner join clientDatabaseMain c " +
                    "on p.Cli_code = c.brCliCode and p.CliAdrNewCode=c.brAdrNewCode inner join ClientDatabaseIPDetails cl on c.brCliCode = cl.ClientCode " +
                    "and c.brSlNo = cl.ClientSL where p.TrackingInfo = '" + ticketid + "'";

                var IpTelephonyInfoData = await _misDBContext.GetIpTelephonyEditInfo.FromSqlRaw(IpTelephonyInfoQuery).FirstOrDefaultAsync();

                if (IpTelephonyInfoData == null)
                {
                    throw new Exception("IP Telephony Info not found.");
                }

                IpTelephonyInfoData.IpTelephonyCpeList = GetCpeList();

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "All data get successfully.",
                    Data = IpTelephonyInfoData
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

        public async Task<ApiResponse> DoneIpTelephonyInfoData(IpTelephonyDoneRequestModel model, ClaimsPrincipal user, string ip)
        {
            var methodName = "InstallationTicketService/DoneIpTelephonyInfoData";
            try
            {
                string statusMessage = "";
                var userId = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();

                var query = "SELECT ISNULL(MAX(CompleteID),0)+1 as MaxID from Cli_InstallationCompleteByTeam";
                var maxId = await _misDBContext.GetMaxIdInfo.FromSqlRaw(query).FirstOrDefaultAsync();

                var installationTeamQuery = "INSERT INTO Cli_InstallationCompleteByTeam (CompleteID, TrackingInfo, TeamName) VALUES ('" +
                maxId.MaxID + "', '" + model.TicketId + "', '" + model.TeamName + "')";
                int installationTeamStatus = await _misDBContext.Database.ExecuteSqlRawAsync(installationTeamQuery);
                statusMessage = installationTeamStatus == 1 ? statusMessage = "data has been Inserted successfully" : throw new Exception("something went wrong");

                var engineerPortfolioQuery = "INSERT INTO tbl_EngineerPortFolio(EngineerName, ComplainFttxPhoneSupport, ComplainFttxPhysicalSupport, " +
                    "ComplainCorporatePhoneSupport, ComplainCorporatePhysicalSupport, ComplainOutofStation, ComplainServer, ComplainRadio, " +
                    "ComplainForward, ComplainFollowUp, ComplainCustomerMeeting, Installation, ShiftingRemote, ShiftingPhysical, ShiftingFollowUp, " +
                    "DismentalRemote, DismentalPhysical, DismentalFollowUp, ReActiveRemote,ReActivatePhysical, ReActivateFollowUp, MaintananceRemote, " +
                    "MaintanancePhysical, MaintananceFollowup, EntryDate)VALUES('" + userId + "', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0',convert(datetime,'" + DateTime.Now + "',103))";
                int engineerPortfolioStatus = await _misDBContext.Database.ExecuteSqlRawAsync(engineerPortfolioQuery);
                statusMessage = engineerPortfolioStatus == 1 ? statusMessage = "data has been Inserted successfully" : throw new Exception("something went wrong");

                var ipDetailsQuery = "SELECT  * FROM ClientDatabaseIPDetails where ClientCode='" + model.CliCode + "' and ClientSL='" + model.SlNo + "'";
                var ipDetailsData = await _misDBContext.GetIpDetailsInfo.FromSqlRaw(ipDetailsQuery).FirstOrDefaultAsync();
                if (ipDetailsData != null)
                {
                    var clientIpDetailsQuery = "update ClientDatabaseIPDetails set PhoneNo='" + model.PhoneNo + "', NotForIP='" + model.NotForIP + "',Comments='" +
                    model.Comments.Replace("'", "''") + "', ISP='" + model.ISP + "',CommencementDate=Convert(Datetime,'" +
                    model.CommencementDate + "',103),ExpiryDate=Convert(Datetime,'" + model.ExpiryDate + "',103),CPE='" +
                    model.CpeText + "' where ClientCode='" + model.CliCode + "' and ClientSL='" + model.SlNo + "'";
                    int clientIpDetailsStatus = await _misDBContext.Database.ExecuteSqlRawAsync(clientIpDetailsQuery);
                    statusMessage = clientIpDetailsStatus == 1 ? statusMessage = "All data has been Updated successfully" : throw new Exception("something went wrong");
                }
                else
                {
                    var clientIpDetailsQuery = "INSERT INTO ClientDatabaseIPDetails (ClientCode, ClientSL, PhoneNo, NotForIP, Comments, ISP, CommencementDate, ExpiryDate, CPE) VALUES ('" +
                    model.CliCode + "','" + Convert.ToInt32(model.SlNo) + "','" + model.PhoneNo + "','" + model.NotForIP + "','" +
                    model.Comments.Replace("'", "''") + "','" + model.ISP + "',CONVERT(datetime,'" +
                    model.CommencementDate + "',103),CONVERT(datetime,'" + model.ExpiryDate + "',103),'" + model.CpeText + "')";
                    int clientIpDetailsStatus = await _misDBContext.Database.ExecuteSqlRawAsync(clientIpDetailsQuery);
                }

                //var taskHistoryQuery = "INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, " +
                //                      "Remarks,TaskStatus) VALUES('" + model.TicketId + "', '" + userId + "', 'MIS', '" +
                //                      "Installation','IP Telephony','Close')";
                //int taskHistoryStatus = await _rsmDBContext.Database.ExecuteSqlRawAsync(taskHistoryQuery);
                //statusMessage = taskHistoryStatus == 1 ? statusMessage = "All data has been Updated successfully" : throw new Exception("something went wrong");

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = statusMessage,
                    Data = null
                };
                await InsertRequestResponse(model.TicketId, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model.TicketId, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> UpdateIpTelephonyInfoData(IpTelephonyUpdateRequestModel model, ClaimsPrincipal user, string ip)
        {
            var methodName = "InstallationTicketService/UpdateIpTelephonyInfoData";
            try
            {
                string statusMessage = "";
                var userId = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();

                var ipDetailsQuery = "SELECT  * FROM ClientDatabaseIPDetails where ClientCode='" + model.CliCode + "' and ClientSL='" + model.SlNo + "'";
                var ipDetailsData = await _misDBContext.GetIpDetailsInfo.FromSqlRaw(ipDetailsQuery).FirstOrDefaultAsync();
                if (ipDetailsData != null)
                {
                    var clientIpDetailsQuery = "update ClientDatabaseIPDetails set PhoneNo='" + model.PhoneNo + "', NotForIP='" + model.NotForIP + "',Comments='" +
                    model.Comments.Replace("'", "''") + "', ISP='" + model.ISP + "',CommencementDate=Convert(Datetime,'" +
                    model.CommencementDate + "',103),ExpiryDate=Convert(Datetime,'" + model.ExpiryDate + "',103),CPE='" +
                    model.CpeText + "' where ClientCode='" + model.CliCode + "' and ClientSL='" + model.SlNo + "'";
                    int clientIpDetailsStatus = await _misDBContext.Database.ExecuteSqlRawAsync(clientIpDetailsQuery);
                    statusMessage = clientIpDetailsStatus == 1 ? statusMessage = "All data has been Updated successfully" : throw new Exception("something went wrong");
                }
                else
                {
                    var clientIpDetailsQuery = "INSERT INTO ClientDatabaseIPDetails (ClientCode, ClientSL, PhoneNo, NotForIP, Comments, ISP, CommencementDate, ExpiryDate, CPE) VALUES ('" +
                    model.CliCode + "','" + Convert.ToInt32(model.SlNo) + "','" + model.PhoneNo + "','" + model.NotForIP + "','" +
                    model.Comments.Replace("'", "''") + "','" + model.ISP + "',CONVERT(datetime,'" +
                    model.CommencementDate + "',103),CONVERT(datetime,'" + model.ExpiryDate + "',103),'" + model.CpeText + "')";
                    int clientIpDetailsStatus = await _misDBContext.Database.ExecuteSqlRawAsync(clientIpDetailsQuery);
                    statusMessage = clientIpDetailsStatus == 1 ? statusMessage = "All data has been Updated successfully" : throw new Exception("something went wrong");
                }

                string mailBodyText = await getEmailTextInfo(model.TeamName, model.TicketId, model.Comments, user);

                var emailLogQuery = "UPDATE Cli_EmailLog set MailBody='" + mailBodyText.Replace("'", "''") + "' where CTID='" + model.TicketId + "'";
                int emailLogInfoStatus = await _misDBContext.Database.ExecuteSqlRawAsync(emailLogQuery);
                statusMessage = emailLogInfoStatus == 1 ? statusMessage = "data has been Updated successfully" : throw new Exception("something went wrong");



                //var taskHistoryQuery = "INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, " +
                //                      "Remarks,TaskStatus) VALUES('" + model.TicketId + "', '" + userId + "', 'MIS', '" +
                //                      "Installation','IP Telephony','Update')";
                //int taskHistoryStatus = await _rsmDBContext.Database.ExecuteSqlRawAsync(taskHistoryQuery);
                //statusMessage = taskHistoryStatus == 1 ? statusMessage = "All data has been Updated successfully" : throw new Exception("something went wrong");

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = statusMessage,
                    Data = null
                };
                await InsertRequestResponse(model.TicketId, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model.TicketId, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> DoneHardwareInfoData(HardwareInfoDoneRequestModel model, ClaimsPrincipal user, string ip)
        {
            var methodName = "InstallationTicketService/DoneHardwareInfoData";
            try
            {
                string statusMessage = "";
                var userId = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();

                var query = "SELECT ISNULL(MAX(CompleteID),0)+1 as MaxID from Cli_InstallationCompleteByTeam";
                var maxId = await _misDBContext.GetMaxIdInfo.FromSqlRaw(query).FirstOrDefaultAsync();

                var installationTeamQuery = "INSERT INTO Cli_InstallationCompleteByTeam (CompleteID, TrackingInfo, TeamName, CompleteDate) VALUES ('" +
                maxId.MaxID + "', '" + model.TicketId + "', '" + model.TeamName + "',Convert(Datime,'" + DateTime.Now.Date + "',103)')";
                int installationTeamStatus = await _misDBContext.Database.ExecuteSqlRawAsync(installationTeamQuery);
                statusMessage = installationTeamStatus == 1 ? statusMessage = "data has been Inserted successfully" : throw new Exception("something went wrong");

                var engineerPortfolioQuery = "INSERT INTO tbl_EngineerPortFolio(EngineerName, ComplainFttxPhoneSupport, ComplainFttxPhysicalSupport, " +
                    "ComplainCorporatePhoneSupport, ComplainCorporatePhysicalSupport, ComplainOutofStation, ComplainServer, ComplainRadio, " +
                    "ComplainForward, ComplainFollowUp, ComplainCustomerMeeting, Installation, ShiftingRemote, ShiftingPhysical, ShiftingFollowUp, " +
                    "DismentalRemote, DismentalPhysical, DismentalFollowUp, ReActiveRemote,ReActivatePhysical, ReActivateFollowUp, MaintananceRemote, " +
                    "MaintanancePhysical, MaintananceFollowup, EntryDate)VALUES('" + userId + "', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0',convert(datetime,'" + DateTime.Now + "',103))";
                int engineerPortfolioStatus = await _misDBContext.Database.ExecuteSqlRawAsync(engineerPortfolioQuery);
                statusMessage = engineerPortfolioStatus == 1 ? statusMessage = "data has been Inserted successfully" : throw new Exception("something went wrong");



                //var taskHistoryQuery = "INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, " +
                //                      "Remarks,TaskStatus) VALUES('" + model.TicketId + "', '" + userId + "', 'MIS', '" +
                //                      "Installation','Hardware','Close')";
                //int taskHistoryStatus = await _rsmDBContext.Database.ExecuteSqlRawAsync(taskHistoryQuery);
                //statusMessage = taskHistoryStatus == 1 ? statusMessage = "All data has been Updated successfully" : throw new Exception("something went wrong");

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = statusMessage,
                    Data = null
                };
                await InsertRequestResponse(model.TicketId, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model.TicketId, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> UpdateHardwareInfoData(HardwareInfoUpdateRequestModel model, ClaimsPrincipal user, string ip)
        {
            var methodName = "InstallationTicketService/UpdateHardwareInfoData";
            try
            {
                string statusMessage = "";
                var userId = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();

                string mailBodyText = await getEmailTextInfo(model.TeamName, model.TicketId, model.Comments, user);

                var emailLogQuery = "UPDATE Cli_EmailLog set MailBody='" + mailBodyText.Replace("'", "''") + "' where CTID='" + model.TicketId + "'";
                int emailLogInfoStatus = await _misDBContext.Database.ExecuteSqlRawAsync(emailLogQuery);
                statusMessage = emailLogInfoStatus == 1 ? statusMessage = "data has been Updated successfully" : throw new Exception("something went wrong");


                //var taskHistoryQuery = "INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, " +
                //                      "Remarks,TaskStatus) VALUES('" + model.TicketId + "', '" + userId + "', 'MIS', '" +
                //                      "Installation','Hardware','Update')";
                //int taskHistoryStatus = await _rsmDBContext.Database.ExecuteSqlRawAsync(taskHistoryQuery);
                //statusMessage = taskHistoryStatus == 1 ? statusMessage = "All data has been Updated successfully" : throw new Exception("something went wrong");

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = statusMessage,
                    Data = null
                };
                await InsertRequestResponse(model.TicketId, response, methodName, ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model.TicketId, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }


        public async Task<ApiResponse> GetIntranetInfoData(string ticketid, string ip)
        {
            var methodName = "InstallationTicketService/GetIntranetInfoData";
            try
            {
                var response = new ApiResponse();
                var intranetInfoQuery = "SELECT LastmileBandwidth, IntercityBandwidth,private_ip,private_gateway,private_musk,Pvlan,mrtg_link " +
                "from Post_Installation p inner join clientDatabaseMain c on p.Cli_code = c.brCliCode and p.CliAdrNewCode = c.brAdrNewCode inner join " +
                "ClientTechnicalInfo cl on c.brCliCode = cl.ClientCode inner join Post_MainIns pm on p.TrackingInfo = pm.RefNO " +
                "and c.brSlNo = cl.ClientSlNo where(pm.ServiceID = '2' or pm.ServiceID = '13') and p.TrackingInfo = '" + ticketid + "'";

                var intranetInfoData = await _misDBContext.GetIntranetInfo.FromSqlRaw(intranetInfoQuery).FirstOrDefaultAsync();

                if (intranetInfoData == null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data Not Found",
                        Data = intranetInfoData
                    };
                    await InsertRequestResponse(ticketid, response, methodName, ip, null, null);
                    return response;
                }

                var btsSetupList = "SELECT BtsSetupID, BtsSetupName FROM BtsSetup ORDER BY BtsSetupName";
                var btsSetupListData = await _misDBContext.GetBtsSetupList.FromSqlRaw(btsSetupList).ToListAsync();

                intranetInfoData.BtsSetupList = btsSetupListData;

                var btsSelectedIdQuery = "SELECT item_id from Post_Installation p inner join clientDatabaseMain c on p.Cli_code " +
                    "= c.brCliCode and p.CliAdrNewCode = c.brAdrNewCode inner join clientDatabaseItemDet cd on c.brCliCode = cd.brCliCode " +
                    "and c.brSlNo = cd.brSlNo WHERE cd.itm_type = 'BTS' and p.TrackingInfo = '" + ticketid + "'";

                var btsSelectedIdData = await _misDBContext.GetBtsSelectedId.FromSqlRaw(btsSelectedIdQuery).FirstOrDefaultAsync();

                intranetInfoData.BtsSelectedId = btsSelectedIdData.item_id;


                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "All data get successfully.",
                    Data = intranetInfoData
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

        public async Task<ApiResponse> GetNewGoInternetInfoData(string ticketid, string ip)
        {
            var methodName = "InstallationTicketService/GetNewGoInternetInfoData";
            try
            {
                var intranetInfoQuery = "SELECT LastmileBandwidth, IntercityBandwidth,private_ip,private_gateway,private_musk,Pvlan,mrtg_link " +
                "from Post_Installation p inner join clientDatabaseMain c on p.Cli_code = c.brCliCode and p.CliAdrNewCode = c.brAdrNewCode inner join " +
                "ClientTechnicalInfo cl on c.brCliCode = cl.ClientCode inner join Post_MainIns pm on p.TrackingInfo = pm.RefNO " +
                "and c.brSlNo = cl.ClientSlNo where(pm.ServiceID = '2' or pm.ServiceID = '13') and p.TrackingInfo = '" + ticketid + "'";

                var intranetInfoData = await _misDBContext.GetIntranetInfo.FromSqlRaw(intranetInfoQuery).FirstOrDefaultAsync();


                if (intranetInfoData == null)
                {
                    throw new Exception("Intranet Info not found.");
                }

                var btsSetupList = "SELECT BtsSetupID, BtsSetupName FROM BtsSetup ORDER BY BtsSetupName";
                var btsSetupListData = await _misDBContext.GetBtsSetupList.FromSqlRaw(btsSetupList).ToListAsync();

                intranetInfoData.BtsSetupList = btsSetupListData;


                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "All data get successfully.",
                    Data = intranetInfoData
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

        public async Task<ApiResponse> GetPendingReasonListForAddComment(string ip)
        {
            var methodName = "InstallationTicketService/GetPendingReasonListForAddComment";
            try
            {
                var query = "select ID,PendingReson from RSM_ReasonPendingInstallation where status='1' order by PendingReson";

                var info = await _misDBContext.GetPendingResons.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("Pending Reason not found.");
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

        public async Task<ApiResponse> GetSendMailServiceListForAddComment(string ticketid, string userId, string ip)
        {
            var methodName = "InstallationTicketService/GetSendMailServiceListForAddComment";
            try
            {
                var query = "SELECT [Service] FROM [Cli_Pending] where RefNo='" + ticketid + "' and Service!='CC' and Service!='READY FOR INVOICE' and Service!='SD REVIEW' and Service!='MKT' and Status='INI' Group by Service ORDER BY [Service]";

                var info = await _misDBContext.cli_PendingServiceName.FromSqlRaw(query).ToListAsync();

                if (info == null)
                {
                    throw new Exception("service name is not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "data get successfully.",
                    Data = info
                };
                await InsertRequestResponse(ticketid, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketid, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> SeveAddComment(MisInstallationTickeAddCommentRequestModel model)
        {
            var methodName = "InstallationTicketService/SeveAddComment";
            try
            {
                if (model.PendingReasonText.Contains("Select"))
                {
                    throw new Exception("Please select Pending Reason");
                }
                var commentInfo = await DataLog(model);
                var newComment = model.AddComments.Replace("'", "''");

                    string sp_sqlstr = "EXEC SP_AddCommentMisInstallationTickeAPI '" + model.TicketNo + "', '" + model.UserId + "', '" + commentInfo.Replace("'", "''") + "'," +
                    "'" + model.PendingReasonValue + "' ,'" + newComment.Replace("'", "''") + "'";

                var result = await _misDBContext.SP_AddCommentMisInstallationTickeAPI.FromSqlRaw(sp_sqlstr).AsNoTracking().ToListAsync();

                var body = commentInfo;
                var subject = "[PENDING INSTALLATION:]" + model.TicketNo + "[" + model.companyName + "]";
                var from = model.UserEmail; //new MailAddress(Session["uidMail"].ToString(), Session[StaticData.sessionUserName].ToString());
                var displayName = model.UserName;

                List<string> toAddress = new List<string>();
                List<string> ccAddress = new List<string>();

                foreach (string lst in model.chkpenteam)
                {
                    string sql = @"SELECT Team_id, Team_Name, Team_Desc, Team_Email, Team_IPost, Team_taskclose, Team_TOpen, "
                        + " Team_TForward, Team_InsSolve, Team_ViewAll, Tstatus, Status  from tbl_Team_info where Team_Name='" + lst.Replace("&amp;", "&") + "'";
                    var getTeam_infoData = await _misDBContext.tbl_Team_Infos.FromSqlRaw(sql).FirstOrDefaultAsync();

                    toAddress.Add(getTeam_infoData.Team_Email);
                }

                if (!string.IsNullOrEmpty(model.AdditionalMail))
                {
                    var toAdd = model.AdditionalMail.Split(';', ',');
                    for (int i = 0; i < toAdd.Length; i++)
                    {
                        if (toAdd[i] != "" && toAdd[i].Contains("@"))
                        {
                            toAddress.Add(toAdd[i].ToString());
                        }
                    }
                }

                await _mailSenderService.SendMail(subject, body, from, toAddress, null, null, displayName);

                var commandSql = @"INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, 
                                      Remarks,TaskStatus) VALUES('" + model.TicketNo + "','" + model.UserId + "','MIS','Installation'," +
                                      "'" + model.AddComments.Replace("'", "''") + "','Comments Add')";
                await _rsmDBContext.Database.ExecuteSqlRawAsync(commandSql);


                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Comment is added."
                };
                await InsertRequestResponse(model, response, methodName, model.Ip, null, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, model.Ip, model.UserId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task<string> DataLog(MisInstallationTickeAddCommentRequestModel model)
        {
            try
            {
                string strSql = " SELECT CTID, Mailfrom, Mailto, MailCC, MailBcc, MailCategory, MailSubject, MailBody, MailSentTime, Status, id "
                        + " FROM Cli_EmailLog where CTID = '" + model.TicketNo + "' ";
                var getData = await _misDBContext.Cli_EmailLogs.FromSqlRaw(strSql).AsNoTracking().FirstOrDefaultAsync();
                if(getData == null)
                {
                    getData = new Cli_EmailLogResponseModel();
                    getData.MailBody = "";
                }
                if(model.UserId == "L3T794")
                {

                    string asx = DateTime.Now.ToString();
                    string cfv = "............CTO COMMENTS..............";
                    string info = "";

                    info = info + cfv + "\n";
                    info = info + "Comments By :" + model.UserName + "\n";
                    info = info + "Date    :" + asx + "\n";
                    info = info + "Comments  :" + model.AddComments + "\n\n\n";

                    info = info + getData.MailBody + "\n";

                    return info;
                }
                else
                {
                    string view_pendin_strSql = "select distinct Service from view_pendin where userid='" + model.UserId + "' " +
                        "and RefNo='" + model.TicketNo + "' and Status='INI'";
                    var view_pendin = await _misDBContext.view_pendin.FromSqlRaw(view_pendin_strSql).AsNoTracking().FirstOrDefaultAsync();
                    if (view_pendin == null)
                    {
                        view_pendin = new View_pendinModel();
                        view_pendin.Service = "";
                    }
                    string asx = DateTime.Now.ToString();
                    string cfv = "............COMMENTS FROM :" + view_pendin.Service + "..............";
                    string info = "";

                    info = info + cfv + "\n";
                    info = info + "Comments By  :" + model.UserId + ":" + model.UserName + ",Cell No:" + model.UserCellNo + "\n";
                    info = info + "Designation  :" + model.DesignationName + "\n";
                    info = info + "Department   :" + model.DepartmentName + "\n";
                    info = info + "Date         :" + asx + "\n";
                    info = info + "Comments     :" + model.AddComments + "\n\n\n";

                    info = info + getData.MailBody + "\n";

                    return info;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private async Task<string> getEmailMessageBody(string ticketid)
        {
            var methodName = "InstallationTicketService/getEmailMessageBody";
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

        private async Task<string> getEmailTemplate(UpdateCommentRequestModel model, ClaimsPrincipal user)
        {
            var methodName = "InstallationTicketService/getEmailTemplate";
            try
            {
                string mailTemplate = "";
                var l3id = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();
                var userEmail = user.Claims.Where(c => c.Type == "email").Select(c => c.Value).FirstOrDefault();
                var userFullName = user.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
                var designation = user.Claims.Where(c => c.Type == "Designation").Select(c => c.Value).FirstOrDefault();
                var department = user.Claims.Where(c => c.Type == "Department").Select(c => c.Value).FirstOrDefault();
                var phoneNo = user.Claims.Where(c => c.Type == "PhoneNo").Select(c => c.Value).FirstOrDefault();
                var username = user.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();

                if (l3id == "L3T794")
                {
                    var mailBodyQuery = "select MailBody from Cli_EmailLog where CTID='" + model.TicketRefNo + "'";
                    var mailBody = await _misDBContext.getMailInfo.FromSqlRaw(mailBodyQuery).FirstOrDefaultAsync();

                    string headLine = "............CTO COMMENTS..............";
                    mailTemplate = mailTemplate + headLine + "\n";
                    mailTemplate = mailTemplate + "Comments By :" + username + "\n";
                    mailTemplate = mailTemplate + "Date    :" + DateTime.Now.ToString() + "\n";
                    mailTemplate = mailTemplate + "Comments  :" + model.CommentText + "\n\n\n";

                    mailTemplate = mailTemplate + mailBody + "\n";
                }
                else
                {
                    var mailBodyQuery = "select MailBody from Cli_EmailLog where CTID='" + model.TicketRefNo + "'";
                    var mailBody = await _misDBContext.getMailInfo.FromSqlRaw(mailBodyQuery).FirstOrDefaultAsync();

                    var ServiceInfoQuery = "select distinct Service from view_pendin where userid='" + l3id + "' and RefNo='" + model.TicketRefNo + "' and Status='INI'";
                    var ServiceInfo = await _misDBContext.GetCheckboxList.FromSqlRaw(ServiceInfoQuery).FirstOrDefaultAsync();

                    string headLine = "............COMMENTS FROM :" + ServiceInfo + "..............";
                    mailTemplate = mailTemplate + headLine + "\n";
                    mailTemplate = mailTemplate + "Comments By :" + username + ",Cell No:" + phoneNo + "\n";
                    mailTemplate = mailTemplate + "Designation  :" + designation + "\n";
                    mailTemplate = mailTemplate + "Department   :" + department + "\n";
                    mailTemplate = mailTemplate + "Date    :" + DateTime.Now.ToString() + "\n";
                    mailTemplate = mailTemplate + "Comments  :" + model.CommentText + "\n\n\n";

                    mailTemplate = mailTemplate + mailBody + "\n";

                }


                return mailTemplate;
            }
            catch (Exception ex)
            {

                await errorMethord(ex, methodName);
                throw new Exception(ex.Message);
            }
        }


        private async Task<string> getEmailText(GeneralInfoUpdateModel model, ClaimsPrincipal user)
        {
            var methodName = "InstallationTicketService/getEmailText";
            try
            {
                string mailTemplate = "";
                var l3id = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();
                var userEmail = user.Claims.Where(c => c.Type == "email").Select(c => c.Value).FirstOrDefault();
                var userFullName = user.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
                var designation = user.Claims.Where(c => c.Type == "Designation").Select(c => c.Value).FirstOrDefault();
                var department = user.Claims.Where(c => c.Type == "Department").Select(c => c.Value).FirstOrDefault();
                var phoneNo = user.Claims.Where(c => c.Type == "PhoneNo").Select(c => c.Value).FirstOrDefault();
                var username = user.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();

                var mailBodyQuery = "select MailBody from Cli_EmailLog where CTID='" + model.TicketId + "'";
                var mailBody = await _misDBContext.getMailInfo.FromSqlRaw(mailBodyQuery).FirstOrDefaultAsync();

                mailTemplate = "............Update from" + " " + model.TeamName + "..............";
                mailTemplate = mailTemplate + "Updates By :" + l3id + ":" + username + ",Cell No:" + phoneNo + "\n";
                mailTemplate = mailTemplate + "Designation  :" + designation + "\n";
                mailTemplate = mailTemplate + "Department   :" + department + "\n";
                mailTemplate = mailTemplate + "update Date    :" + DateTime.Now.ToString() + "\n";
                mailTemplate = mailTemplate + "Comments  :" + model.Comments + "\n\n\n";

                mailTemplate = mailTemplate + mailBody + "\n";

                return mailTemplate;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                throw new Exception(ex.Message);
            }
        }

        private async Task<string> getEmailTextInfo(string teamName, string ticketId, string comments, ClaimsPrincipal user)
        {
            var methodName = "InstallationTicketService/getEmailTextInfo";
            try
            {
                string mailTemplate = "";
                var l3id = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();
                var userEmail = user.Claims.Where(c => c.Type == "email").Select(c => c.Value).FirstOrDefault();
                var userFullName = user.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
                var designation = user.Claims.Where(c => c.Type == "Designation").Select(c => c.Value).FirstOrDefault();
                var department = user.Claims.Where(c => c.Type == "Department").Select(c => c.Value).FirstOrDefault();
                var phoneNo = user.Claims.Where(c => c.Type == "PhoneNo").Select(c => c.Value).FirstOrDefault();
                var username = user.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();

                var mailBodyQuery = "select MailBody from Cli_EmailLog where CTID='" + ticketId + "'";
                var mailBody = await _misDBContext.getMailInfo.FromSqlRaw(mailBodyQuery).FirstOrDefaultAsync();

                mailTemplate = "............Update from" + " " + teamName + "..............";
                mailTemplate = mailTemplate + "Updates By :" + l3id + ":" + username + ",Cell No:" + phoneNo + "\n";
                mailTemplate = mailTemplate + "Designation  :" + designation + "\n";
                mailTemplate = mailTemplate + "Department   :" + department + "\n";
                mailTemplate = mailTemplate + "update Date    :" + DateTime.Now.ToString() + "\n";
                mailTemplate = mailTemplate + "Comments  :" + comments + "\n\n\n";

                mailTemplate = mailTemplate + mailBody + "\n";

                return mailTemplate;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                throw new Exception(ex.Message);
            }
        }

        private List<IpTelephonyCpeModel> GetCpeList()
        {
            List<IpTelephonyCpeModel> CpeListData = new List<IpTelephonyCpeModel> {
                new IpTelephonyCpeModel()
                {
                    Value="1",
                    Text="Soft Phone"
                },
                 new IpTelephonyCpeModel()
                {
                    Value="2",
                    Text="Web Dialer"
                },
                  new IpTelephonyCpeModel()
                {
                    Value="3",
                    Text="SIP Phone"
                },
                new IpTelephonyCpeModel()
                {
                    Value="4",
                    Text="ATA"
                },
                new IpTelephonyCpeModel()
                {
                    Value="5",
                    Text="Analog Phone Set"
                },
                new IpTelephonyCpeModel()
                {
                    Value="6",
                    Text="Media Gw"
                },
                 new IpTelephonyCpeModel()
                {
                    Value="7",
                    Text="IP PABX"
                },
            };

            return CpeListData;
        }


        private async Task InsertRequestResponse(object request, object response, string methodName, string requestedIP, string userId, string errorLog)
        {
            var errorMethordName = "InstallationTicketService/InsertRequestResponse";
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
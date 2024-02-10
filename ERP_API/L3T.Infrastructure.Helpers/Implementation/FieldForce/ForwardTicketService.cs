using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.ExtraModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace L3T.Infrastructure.Helpers.Implementation.FieldForce
{
    public class ForwardTicketService : IForwardTicketService
    {
        private readonly MisDBContext _misDBContext;
        private readonly ILogger<FieldForceService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly FFWriteDBContext _ffWriteDBContext;
        private readonly IMailSenderService _mailSenderService;

        public ForwardTicketService(MisDBContext misDBContext,
            ILogger<FieldForceService> logger,
            HttpClient httpClient,
            IConfiguration configuration,
            FFWriteDBContext ffWriteDBContext,
            IMailSenderService mailSenderService)
        {
            _misDBContext = misDBContext;
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _ffWriteDBContext = ffWriteDBContext;
            _mailSenderService = mailSenderService;
        }

        public async Task<ApiResponse> ForwardTicketDetails(string ticketId, string ip, string userId)
        {
            var methodName = "ForwardTicketService/ForwardTicketDetails";
            try
            {
                var ForwardDetail_query = "SELECT t_Project.project_Id, t_Project.project_Title, t_Project.project_Description, " +
                    "t_Client.client_CompanyName, t_Project.project_category, t_Project.Team_id " +
                    "FROM t_Project  WITH(NOLOCK) INNER JOIN t_Client  WITH(NOLOCK) ON t_Project.project_Client_ID = t_Client.client_ID  " +
                    "WHERE  (t_Project.project_Id = '" + ticketId + "')";

                var ForwardDetail = await _misDBContext.FowordTicketDetails.FromSqlRaw(ForwardDetail_query).FirstOrDefaultAsync();
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Success",
                    Data = ForwardDetail
                };
                await InsertRequestResponse(ticketId, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketId, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse> Category(string ip, string userId)
        {
            var methodName = "ForwardTicketService/Catagory";
            try
            {
                var catagory_query = "SELECT C_id, Com_Category FROM Tbl_Com_Category   WITH(NOLOCK) ORDER BY Com_Category";
                var catagoryList = await _misDBContext.tblComCategory.FromSqlRaw(catagory_query).ToListAsync();
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Success",
                    Data = catagoryList
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
        public async Task<ApiResponse> FowardToList(string ip, string userId)
        {
            var methodName = "ForwardTicketService/FowordToList";
            try
            {
                var fowordTo_query = "select Team_id, Team_Name from tbl_Team_info  WITH(NOLOCK)  where Team_id not in('T-043','T-029','T-030','T-031','T-032', " +
                    "'T-033','T-160','T-010','T-011','T-013','T-014','T-021','T-034', 'T-035','T-043','T-048','T-052','T-060','T-064', " +
                    "'T-092','T-093','T-094','T-099','T-103','T-106','T-111','T-112', 'T-114','T-115','T-150','T-152','T-153','T-160'," +
                    "'T-161')  order by Team_Name";

                var fowordToList = await _misDBContext.TblTeamInfo.FromSqlRaw(fowordTo_query).ToListAsync();
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Success",
                    Data = fowordToList
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

        public async Task<ApiResponse> ForwardTicket(ForwardTicketRequestModel model, ClaimsPrincipal user, string ip)
        {
            var methodName = "ForwardTicketService/ForwardTicket";
            var userId = user.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            try
            {

                var l3id = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();
                var userEmail = user.Claims.Where(c => c.Type == "email").Select(c => c.Value).FirstOrDefault();
                var userFullName = user.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
                var designation = user.Claims.Where(c => c.Type == "Designation").Select(c => c.Value).FirstOrDefault();
                var department = user.Claims.Where(c => c.Type == "Department").Select(c => c.Value).FirstOrDefault();
                var phoneNo = user.Claims.Where(c => c.Type == "PhoneNo").Select(c => c.Value).FirstOrDefault();

                var getPermission_query = "select * from tbl_team_mem_permission  WITH(NOLOCK)  where Team_id='T-015' and Emp_id='" + l3id + "'";
                var getPermission = await _misDBContext.TblTeamMemPermission.FromSqlRaw(getPermission_query).CountAsync();

                if (getPermission > 0)
                {
                    if (model.SupportType == "NA")
                    {
                        throw new Exception("Please Select Support Type");
                    }
                }
                int sta = 0;
                string cat = "";
                string fs = "N";
                string mailboby = "", elogCategory = "", subj = "";

                var tickitComplainInfoQuery = "SELECT AutoSL, Clientcategory, DistributorTaskStatus, comp_info_Category, comp_info_Led_Status, " +
                    "comp_info_Receive_ActualTime, comp_info_Receive_Date, comp_info_Receive_Time, comp_info_Source_Information, " +
                    "comp_info_attachments, comp_info_client_Media, comp_info_client_adr, comp_info_client_adrcode, comp_info_client_id, " +
                    "comp_info_client_name, comp_info_client_slno, comp_info_com_name, comp_info_comm, comp_info_complain, comp_info_con_email, " +
                    "comp_info_con_phone_no, comp_info_contact_person, comp_info_deadline, comp_info_email_to_client, comp_info_hold_on, " +
                    "comp_info_last_update, comp_info_manually_email, comp_info_mkt_person, comp_info_postponed_flg, comp_info_postponed_hour, " +
                    "comp_info_postponed_time, comp_info_receive_by, comp_info_ref_no, comp_info_related_dept," +
                    " comp_info_resolve_status, comp_info_service_code, comp_info_service_desc, comp_info_type, receiveby, state " +
                    "FROM tbl_complain_info  WITH(NOLOCK)  WHERE (comp_info_ref_no = '" + model.TicketId + "')";

                var GetDataByTicketRefno = await _misDBContext.TblComplainInfo.FromSqlRaw(tickitComplainInfoQuery).FirstOrDefaultAsync();

                if (GetDataByTicketRefno == null)
                {
                    throw new Exception("Complain is not found.");
                }
                sta = GetDataByTicketRefno.state;

                string TeamLeader = "Not Yet Assign";

                if (model.Category == "Problem Solved")
                {
                    cat = "Problem Not Solved";
                }
                else
                {
                    cat = model.Category;
                }

                if (model.SolvedAndForward == true)
                {
                    fs = "Y";
                }

                var mailformet_query = "SELECT CTID, MailBcc, MailBody, MailCC, MailSubject, Mailfrom, " +
                    "Mailto, Status FROM tblComplainEmailFormat  WITH(NOLOCK)  WHERE  (CTID = '" + model.TicketId + "')";
                var complainEmailFormat = await _misDBContext.tblComplainEmailFormat.FromSqlRaw(mailformet_query).FirstOrDefaultAsync();
                var dtcl = 0;
                if (complainEmailFormat != null)
                {
                    dtcl = 1;
                    mailboby = complainEmailFormat.MailBody;
                }
                string fromAdd = userEmail;
                string toAdd = "sdd@link3.net";
                subj = "CTID ::  " + model.ClientName + "[" + model.TicketId + "]";
                string mbody;

                mbody = "---------------------[Ticket forwarded]--------------------" + "\n\n";
                mbody = mbody + "Ticket was forwarded to       : " + model.ForwardToText + "\n";
                mbody = mbody + "After Informing to            : " + model.InformingPerson + "\n";
                mbody = mbody + "Forward by                    : " + l3id + ":" + userFullName + ",Cell No:" + phoneNo + "\n";
                mbody = mbody + "Department                    : " + department + "\n";
                mbody = mbody + "Date & Time                   : " + model.ForwardTime + "[" + System.DateTime.Now + "]" + "\n";
                mbody = mbody + "Comments                      : " + model.Comments + "\n\n";
                mbody = mbody + mailboby;

                mailboby = mbody;

                if (model.EmailToTicketRealatedEmployee)
                {
                    await _mailSenderService.SingleSendMail(subj, mbody, fromAdd, toAdd);
                }


                var closeComplainDetails_Query = "SELECT AutoID, ComplainID, PendingStatus, SolvedDatetime, TeamID, " +
                    "TicketReceivedTime FROM tbl_CloseComplainDetails  WITH(NOLOCK)  WHERE (ComplainID = '" + model.TicketId + "')";
                var closeComplainDetails = await _misDBContext.tblCloseComplainDetails.FromSqlRaw(closeComplainDetails_Query).FirstOrDefaultAsync();

                var dtmem_query = "select * from tbl_team_mem_permission  WITH(NOLOCK)  where Emp_id='" + l3id + "'";
                var dtmem = await _misDBContext.TblTeamMemPermission.FromSqlRaw(dtmem_query).FirstOrDefaultAsync();

                string tmemid = null;
                if (dtmem != null)
                {
                    tmemid = dtmem.Team_id;
                }


                var clientdatabasemain_query = "select * from clientdatabasemain  WITH(NOLOCK)  where brclicode='" + GetDataByTicketRefno.comp_info_client_id + "' and DistributorID<>''";
                var clientdatabasemain = await _misDBContext.ClientDatabaseMain.FromSqlRaw(clientdatabasemain_query).CountAsync();
                var tdq = "";
                if (clientdatabasemain > 0)
                {
                    tdq = "Active";
                }
                var forward_date = model.ForwardTime.ToShortDateString();
                var forward_time = model.ForwardTime.ToShortTimeString();
                var task_Status = "Close";
                var txtteamid = "";

                string sql = "EXEC ForwardTicketApi @TicketId, @task_Status, @ForwardToText, " +
                    "@ForwardToValue,@Category, @cat, @sta, @ForwardTime, @forward_date, @forward_time, " +
                    "@informingPerson, @dtcl, @tmemid, @comp_info_client_id, @comp_info_com_name, " +
                    "@comp_info_related_dept, @fs, @comp_info_Receive_Date, @mailboby, @tdq, @Comments, " +
                    "@supportType, @userId, @userEmail, @userFullName, @txtteamid";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    // Create parameters
                    new SqlParameter { ParameterName = "@TicketId", Value = model.TicketId },
                    new SqlParameter { ParameterName = "@task_Status", Value = task_Status },
                    new SqlParameter { ParameterName = "@ForwardToText", Value = model.ForwardToText },
                    new SqlParameter { ParameterName = "@ForwardToValue", Value = model.ForwardToValue },
                    new SqlParameter { ParameterName = "@Category", Value = model.Category },
                    new SqlParameter { ParameterName = "@cat", Value = cat },
                    new SqlParameter { ParameterName = "@sta", Value = sta },
                    new SqlParameter { ParameterName = "@ForwardTime", Value = model.ForwardTime },
                    new SqlParameter { ParameterName = "@forward_date", Value = forward_date },
                    new SqlParameter { ParameterName = "@forward_time", Value = forward_time },
                    new SqlParameter { ParameterName = "@informingPerson", Value = model.InformingPerson },
                    new SqlParameter { ParameterName = "@dtcl", Value = dtcl },
                    new SqlParameter { ParameterName = "@tmemid", Value = GetDataByTicketRefno.comp_info_com_name },
                    new SqlParameter { ParameterName = "@comp_info_client_id", Value = GetDataByTicketRefno.comp_info_client_id },
                    new SqlParameter { ParameterName = "@comp_info_com_name", Value = GetDataByTicketRefno.comp_info_com_name },
                    new SqlParameter { ParameterName = "@comp_info_related_dept", Value = GetDataByTicketRefno.comp_info_related_dept },
                    new SqlParameter { ParameterName = "@fs", Value = fs },
                    new SqlParameter { ParameterName = "@comp_info_Receive_Date", Value = GetDataByTicketRefno.comp_info_Receive_Date },
                    new SqlParameter { ParameterName = "@mailboby", Value = mailboby },
                    new SqlParameter { ParameterName = "@tdq", Value = tdq },
                    new SqlParameter { ParameterName = "@Comments", Value = model.Comments },
                    new SqlParameter { ParameterName = "@supportType", Value = model.SupportType },
                    new SqlParameter { ParameterName = "@userId", Value = l3id },
                    new SqlParameter { ParameterName = "@userEmail", Value = userEmail },
                    new SqlParameter { ParameterName = "@userFullName", Value = userFullName },
                    new SqlParameter { ParameterName = "@txtteamid", Value = txtteamid },

                };

                var result = await _misDBContext.ForwardTicketApi.FromSqlRaw(sql, parms.ToArray()).ToListAsync();
                if (result.Count > 0)
                {
                    if (result[0].SuccessOrErrorMessage != "Success")
                    {
                        throw new Exception(result[0].SuccessOrErrorMessage);
                    }
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Success",
                    Data = result
                };
                await InsertRequestResponse(model, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> ComplainInformation(string ticketId, ClaimsPrincipal user, string ip)
        {
            var methodName = "ForwardTicketService/ComplainInformation";
            var userId = user.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            try
            {
                var complainInfo = new ComplainInformationModel();


                var l3id = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();
                var userEmail = user.Claims.Where(c => c.Type == "email").Select(c => c.Value).FirstOrDefault();
                var userFullName = user.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
                var designation = user.Claims.Where(c => c.Type == "Designation").Select(c => c.Value).FirstOrDefault();
                var department = user.Claims.Where(c => c.Type == "Department").Select(c => c.Value).FirstOrDefault();
                var phoneNo = user.Claims.Where(c => c.Type == "PhoneNo").Select(c => c.Value).FirstOrDefault();


                var tickitComplainInfoQuery = "SELECT AutoSL, Clientcategory, DistributorTaskStatus, comp_info_Category, comp_info_Led_Status, " +
                    "comp_info_Receive_ActualTime, comp_info_Receive_Date, comp_info_Receive_Time, comp_info_Source_Information, " +
                    "comp_info_attachments, comp_info_client_Media, comp_info_client_adr, comp_info_client_adrcode, comp_info_client_id, " +
                    "comp_info_client_name, comp_info_client_slno, comp_info_com_name, comp_info_comm, comp_info_complain, comp_info_con_email, " +
                    "comp_info_con_phone_no, comp_info_contact_person, comp_info_deadline, comp_info_email_to_client, comp_info_hold_on, " +
                    "comp_info_last_update, comp_info_manually_email, comp_info_mkt_person, comp_info_postponed_flg, comp_info_postponed_hour, " +
                    "comp_info_postponed_time, comp_info_receive_by, comp_info_ref_no, comp_info_related_dept," +
                    " comp_info_resolve_status, comp_info_service_code, comp_info_service_desc, comp_info_type, receiveby, state " +
                    "FROM tbl_complain_info  WITH(NOLOCK)  WHERE (comp_info_ref_no = '" + ticketId + "')";

                var GetDataByTicketRefno = await _misDBContext.TblComplainInfo.FromSqlRaw(tickitComplainInfoQuery).FirstOrDefaultAsync();

                if (GetDataByTicketRefno == null)
                {
                    throw new Exception("Complain is not found.");
                }

                string sql = "EXEC ComplainTicketInformationAPI @Client_id, @ClinetSlNo, @UserId";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    // Create parameters
                    new SqlParameter { ParameterName = "@Client_id", Value = GetDataByTicketRefno.comp_info_client_id },
                    new SqlParameter { ParameterName = "@ClinetSlNo", Value = GetDataByTicketRefno.comp_info_client_slno },
                    new SqlParameter { ParameterName = "@UserId", Value = l3id },
                };

                var result = await _misDBContext.ComplainTicketInformationAPI.FromSqlRaw(sql, parms.ToArray()).ToListAsync();
                if (result.Count > 0)
                {
                    if (result[0].SuccessOrErrorMessage != "Success")
                    {
                        throw new Exception(result[0].SuccessOrErrorMessage);
                    }
                }
                var aData = result.FirstOrDefault();

                complainInfo.lbldue = aData.Amount;
                complainInfo.DistributorName = aData.DistributorName;
                complainInfo.DistributorSubscriberCode = aData.DistributorSubscriberCode;
                complainInfo.RRPName = aData.RRPName;
                complainInfo.RRPSubscriberCode = aData.RRPSubscriberCode;
                complainInfo.ClientCode = GetDataByTicketRefno.comp_info_client_id + "\t " + aData.brAdrCode + "\t " + GetDataByTicketRefno.comp_info_client_slno;
                complainInfo.RSMToMISMigrateID = aData.RSMToMISMigrateID;
                complainInfo.RSMToMISMigratePassword = aData.RSMToMISMigratePassword;
                complainInfo.RadiusExpiredDate = aData.RadiusExpiredDate;
                complainInfo.RSMToMISMigrateRadiusPassword = aData.RSMToMISMigrateRadiusPassword;
                complainInfo.ClientGroup = aData.ClientGroup;
                complainInfo.SecurityDeposit = aData.SecurityDeposit;
                complainInfo.Reference = aData.Reference;
                complainInfo.ClientName = aData.ClientName;
                complainInfo.BranchName = aData.BranchName;
                complainInfo.ContactPerson = aData.ContactPerson;
                complainInfo.ContactPersonDesignation = aData.ContactPersonDesignation;
                complainInfo.ContactNumber = aData.ContactNumber;
                complainInfo.ContactMail = aData.ContactMail;
                complainInfo.AddressLine_1 = aData.AddressLine_1;
                complainInfo.AddressLine_2 = aData.AddressLine_1;
                complainInfo.ParentArea = aData.ParentArea;
                complainInfo.Area = aData.Area;
                complainInfo.WebSite = aData.WebSite;
                complainInfo.StatusOfSLA = aData.StatusOfSLA;
                complainInfo.DateOfInception = aData.DateOfInception;
                complainInfo.ClientCategory = aData.ClientCategory;
                complainInfo.RevenueCategory = aData.RevenueCategory;
                complainInfo.BusinessType = aData.BusinessType;
                complainInfo.SupportOffice = aData.SupportOffice;
                complainInfo.BillingAddress = aData.BillingAddress;
                complainInfo.BillDeliveryOption = aData.BillDeliveryOption;

                // IP and DNS
                complainInfo.PublicIP = aData.PublicIP;
                complainInfo.Gateway = aData.Gateway;
                complainInfo.SubnetMask = aData.SubnetMask;
                complainInfo.PrivateIP = aData.PrivateIP;
                complainInfo.PrivateGateway = aData.PrivateGateway;
                complainInfo.PrivateSubnetMask = aData.PrivateSubnetMask;
                complainInfo.DNS1 = aData.DNS1;
                complainInfo.DNS2 = aData.DNS2;
                complainInfo.SMTP = aData.SMTP;
                complainInfo.POP3 = aData.POP3;
                complainInfo.Catagory = aData.Catagory;
                complainInfo.ServiceNarration = aData.ServiceNarration;
                complainInfo.ItemName = aData.ItemName;
                complainInfo.BillingAmount = aData.BillingAmount;
                complainInfo.VatName = aData.VatName;
                complainInfo.LockUnlock = aData.LockUnlock;
                complainInfo.ServiceStatus = aData.ServiceStatus;


                //Internet
                complainInfo.Service_Type = "INTERNET";
                complainInfo.Bandwidth_CIR = aData.Bandwidth_CIR;
                complainInfo.Bandwidth_MIR = aData.Bandwidth_MIR;
                complainInfo.ITC_Bandwidth_DownP_CIR = aData.ITC_Bandwidth_DownP_CIR;
                complainInfo.ITC_Bandwidth_DownP_MIR = aData.ITC_Bandwidth_DownP_MIR;
                complainInfo.ITC_Bandwidth_UP_CIR = aData.ITC_Bandwidth_UP_CIR;
                complainInfo.ITC_Bandwidth_UP_MIR = aData.ITC_Bandwidth_UP_MIR;
                complainInfo.Connectivity_Media = aData.Connectivity_Media;

                complainInfo.RouterSwitchIP = aData.RouterSwitchIP;
                complainInfo.Passward = aData.Passward;
                complainInfo.UserName = aData.UserName;
                complainInfo.HostName = aData.HostName;
                complainInfo.BrasIP = aData.RouterSwitchIP;
                complainInfo.subcribeExpireDate = aData.subcribeExpireDate;

                if (!string.IsNullOrEmpty(complainInfo.RouterSwitchIP) && !string.IsNullOrEmpty(complainInfo.Passward) && !string.IsNullOrEmpty(complainInfo.UserName))
                {
                    var infoData = await getDataFromMikrotikRouter(complainInfo.RouterSwitchIP, complainInfo.UserName, complainInfo.Passward, complainInfo.PublicIP, l3id);
                    complainInfo.CustomerPackage = infoData.list;
                    complainInfo.CustomerIP = infoData.address;
                }

                var str = "";
                if (!string.IsNullOrEmpty(aData.sta) && aData.sta.Length > 2)
                {
                    str = aData.sta.Remove(aData.sta.Length - 2, 1);
                }
                complainInfo.BTS = str + "[" + aData.note_for_bts + "]";
                complainInfo.BTSName = str + "[" + aData.note_for_bts + "]";
                if (aData.StatusOfSLA == "Done")
                {
                    complainInfo.SLADate = aData.SLADate.ToString().Replace("12:00:00 AM", "");
                }
                else
                {
                    complainInfo.SLADate = "";
                }

                if (aData.DistributorID != null)
                {
                    complainInfo.DistributorSubscriberCode = aData.DistributorSubscriberCode + " (Radius)";
                }

                var serviceList_query = "SELECT item_desc FROM clientDatabaseItemDet  WITH(NOLOCK)  " +
                                        " WHERE brCliCode = '" + GetDataByTicketRefno.comp_info_client_id + "' " +
                                        "AND brSlNo = " + GetDataByTicketRefno.comp_info_client_slno + " AND itm_type = 'SER'";
                var resultData = await _misDBContext.tbl_ClientDatabaseItemDetModels.FromSqlRaw(serviceList_query).ToListAsync();
                var serviceList = new List<string>();
                if (resultData != null)
                {
                    foreach (var item in resultData)
                    {
                        serviceList.Add(item.item_desc);
                    }
                }
                complainInfo.Service = serviceList;
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Success",
                    Data = complainInfo
                };
                await InsertRequestResponse(ticketId, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketId, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task<Data> getDataFromMikrotikRouter(string routerIP, string username, string password, string clientIp, string userId)
        {
            var methodName = "ForwardTicketService/getDataFromMikrotikRouter";
            try
            {
                var infoModel = new MikrotikRouter()
                {
                    RouterIp = routerIP,
                    UserName = username,
                    Password = password,
                    CustomerIp = clientIp,
                    CallerId = "From Field Force " + userId
                };
                var data = JsonConvert.SerializeObject(infoModel);
                var requestContent = new StringContent(data, Encoding.UTF8, "application/json");
                _httpClient.BaseAddress = new Uri(_configuration.GetValue<string>("Url:baseUrl"));
                var response = await _httpClient.PostAsync("Mikrotik/GetUserInfo", requestContent);
                var result = response.EnsureSuccessStatusCode();
                if (result.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var routerInfo = JsonConvert.DeserializeObject<GetUserInfo>(content);
                    return routerInfo.data.FirstOrDefault();
                }

                throw new Exception("Something is Wrong.");
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                throw new Exception(ex.Message);
            }
        }


        private async Task InsertRequestResponse(object request, object response, string methodName, string requestedId, string userId, string errorLog)
        {
            var errorMethordName = "ForwardTicketService/InsertRequestResponse";
            try
            {
                var reqResModel = new FFRequestResponseModel()
                {
                    CreatedAt = DateTime.Now,
                    Request = JsonConvert.SerializeObject(request),
                    Response = JsonConvert.SerializeObject(response),
                    RequestedIP = requestedId,
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
                string errormessage = await errorMethord(ex, errorMethordName);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
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

        public async Task<ApiResponse> PushNotificationForTicketAssignOrForward(PushNotificationRequestModel requestModel, string Ip)
        {
            string methodName = "PushNotificationForTicketAssignOrForward";

            try
            {
                // get empployee information



                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Success",
                    Data = null
                };
                await InsertRequestResponse(requestModel?.TicketId, response, methodName, Ip, requestModel?.RequiestedByUserId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                throw new Exception(ex.Message);
            }
        }
    }
}

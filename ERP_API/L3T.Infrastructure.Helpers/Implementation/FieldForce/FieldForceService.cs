using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using L3T.Utility.Helper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Security.Claims;

namespace L3T.Infrastructure.Helpers.Implementation.FieldForce
{
    public class FieldForceService : IFieldForceService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MisDBContext _misDBContext;
        private readonly ILogger<FieldForceService> _logger;
        private readonly FFWriteDBContext _ffWriteDBContext;
        private readonly IMailSenderService _mailSenderService;
        private readonly RsmDbContext _rsmDbContext;

        public FieldForceService(
            IHttpClientFactory httpClientFactory,
            MisDBContext misDBContext,
            ILogger<FieldForceService> logger,
            FFWriteDBContext ffWriteDBContext,
            IMailSenderService mailSenderService,
            RsmDbContext rsmDbContext)
        {
            _httpClientFactory = httpClientFactory;
            _misDBContext = misDBContext;
            _logger = logger;
            _ffWriteDBContext = ffWriteDBContext;
            _mailSenderService = mailSenderService;
            _rsmDbContext = rsmDbContext;
        }
        public async Task<ApiResponse> AddCoordinates(AddCoordinatesRequestModel model, string ip)
        {
            var methodName = "FieldForceService/AddCoordinates";
            model.Lon = model.Lon.Replace("-", "");
            try
            {
                await InsertLatLonData(model);
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Success",
                };

                return response;

                //var httpRequestMessage = new HttpRequestMessage(
                //            HttpMethod.Get,
                //            "http://crm.link3.net/api/rest/addCoordinates.php?device_ID="+ model.DviceID + "&lat="+ model.Lat + "&lon=-"+ model.Lon + "&uid=" + model.Uid);

                //var httpClient = _httpClientFactory.CreateClient();
                //var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                //if (httpResponseMessage.IsSuccessStatusCode)
                //{
                //    var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();

                //    var result = JsonConvert.DeserializeObject<AddCoordinatesResponseModel>(contentStream);
                //    var response = new ApiResponse()
                //    {
                //        Status = "Success",
                //        StatusCode = 200,
                //        Message = "Success",
                //        Data = result
                //    };
                //    //await InsertRequestResponse(model, response, methodName, ip,null,null);
                //    return response;
                //}
                //throw new Exception("Something is Wrong.");
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, ip, "Web", ex.Message);
                //await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task InsertLatLonData(AddCoordinatesRequestModel model)
        {
            var data = new LatLonModel()
            {
                Latitude = model.Lat,
                Longitude = model.Lon,
                Emp_id = model.Uid,
                Device_Id = model.DviceID,
                Date_added = DateTime.Now,
            };
            await _ffWriteDBContext.LatLon.AddAsync(data);
            await _ffWriteDBContext.SaveChangesAsync();
        }

        public async Task<ApiResponse> GetLocationForAllUser(string date, string formTime, string toTime, string ip)
        {
            var methodName = "FieldForceService/GetLocationForAllUser";
            try
            {
                if (string.IsNullOrEmpty(date))
                {
                    throw new Exception("Date is required.");
                }

                var getData_query = "";
                var fromDateTime = date + " " + formTime;
                var toDateTime = date + " " + toTime;

                if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(formTime) && !string.IsNullOrEmpty(toTime))
                {
                    getData_query = "SELECT * FROM LatLon " +
                        "where id in (SELECT max(id) as id FROM LatLon " +
                        "where Date_added BETWEEN '" + fromDateTime + "' AND '" + toDateTime + "' group by emp_id)" +
                        " order by Date_added desc";
                }
                else
                {
                    getData_query = "SELECT * FROM LatLon " +
                        "where id in (SELECT max(id) as id FROM LatLon " +
                        "where CAST(Date_added as date) = '" + date + "' group by emp_id) " +
                        "order by Date_added desc";
                }

                var data = await _ffWriteDBContext.LatLon.FromSqlRaw(getData_query).AsNoTracking().ToListAsync();
                var response = new ApiResponse()
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "get data",
                    Data = data
                };
                await InsertRequestResponse(date, response, methodName, ip, "Web", null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(date, ex, methodName, ip, "Web", ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetLocationForAUser(string userId, string date, string? fromTime, string? toTime, string ip)
        {
            var methodName = "FieldForceService/GetLocationForAUser";
            var newObj = new
            {
                userId = userId,
                date = date,
                fromTime = fromTime,
                toTime = toTime
            };
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User Id is required.");
                }
                if (date == null)
                {
                    throw new Exception("Date is required.");
                }
                var getData_query = "";

                if (date != null && toTime != null && fromTime != null)
                {
                    var strFromDate = date + " " + fromTime + ":00:00";
                    var strToDate = date + " " + toTime + ":59:59";
                    getData_query = "SELECT * FROM LatLon where emp_id = '" + userId + "' " +
                                    "AND Date_added BETWEEN '" + strFromDate + "' AND '" + strToDate + "' " +
                                    "order by Date_added desc";
                }
                else
                {
                    getData_query = "SELECT * FROM LatLon where emp_id = '" + userId + "' " +
                                    "AND CAST(Date_added as date) = '" + date + "' " +
                                    "order by Date_added desc";
                }

                var data = await _ffWriteDBContext.LatLon.FromSqlRaw(getData_query).AsNoTracking().ToListAsync();
                var response = new ApiResponse()
                {
                    StatusCode = 200,
                    Status = "Success",
                    Message = "get data",
                    Data = data
                };
                await InsertRequestResponse(newObj, response, methodName, ip, "Web", null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(newObj, ex, methodName, ip, "Web", ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetATicketByTicketId(string ticketid, string userId, string ip)
        {
            var methodName = "FieldForceService/GetATicketByTicketId";
            try
            {
                //var getallTicket = await _misDBContext.viewAllTicketPending.Where(x => DbFunctions.Like(x.AssignEngineer, "%"+ userId + "%")).ToListAsync();
                GetAllPendingTicketByAssignUserResponseModel getTicket = await _misDBContext.ViewAllTicketForFieldForce.Where(x => x.RefNo == ticketid).AsNoTracking().FirstOrDefaultAsync();
                if (getTicket == null)
                {
                    throw new Exception("Ticket is not found.");
                }
                var mailBody = await getEmailMessageBody(ticketid);
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = mailBody,
                    Data = getTicket
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

        public async Task<ApiResponse> GetAllAssignTicketForUser(string userId, string ip)
        {
            var methodName = "FieldForceService/GetAllAssignTicketForUser";
            try
            {
                //var getallTicket = await _misDBContext.viewAllTicketPending.Where(x => DbFunctions.Like(x.AssignEngineer, "%"+ userId + "%")).ToListAsync();
                List<GetAllPendingTicketByAssignUserResponseModel> getallTicket = await _misDBContext.ViewAllTicketForFieldForce.Where(x => x.AssignEngineer.Contains(userId) && x.TicketGenerateDate >= DateTime.Now.AddDays(-7)).AsNoTracking().ToListAsync();
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = getallTicket
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



        public async Task<ApiResponse> GetAllAssignTicketForUserWithCount(string userId, string ip, int LastDays = 7)
        {
            var methodName = "FieldForceService/GetAllAssignTicketForUserWithCount";
            try
            {
                var sqlQueryStr = "SELECT * FROM [dbo].[ViewAllTicketForFieldForce] " +
                    "WHERE AssignEngineer Like '%" + userId + "%' AND TicketGenerateDate >= DATEADD(Day, -" + LastDays + ",GETDATE()) " +
                    "ORDER BY TicketGenerateDate ASC";
                List<GetAllPendingTicketByAssignUserResponseModel> getallTicket = await _misDBContext.ViewAllTicketForFieldForce.FromSqlRaw(sqlQueryStr).ToListAsync();
                //List<GetAllPendingTicketByAssignUserResponseModel> getallTicket = await _misDBContext.ViewAllTicketForFieldForce.Where(x => x.AssignEngineer.Contains(userId) && x.TicketGenerateDate >= DateTime.Now.AddDays(-LastDays)).ToListAsync();
                List<GetAllPendingTicketByAssignUserResponseModel> getallTicketPending = new List<GetAllPendingTicketByAssignUserResponseModel>();
                List<GetAllPendingTicketByAssignUserResponseModel> getallTicketClose = new List<GetAllPendingTicketByAssignUserResponseModel>();

                foreach (var item in getallTicket)
                {
                    if (item.comp_info_resolve_status == "Close")
                    {
                        getallTicketClose.Add(item);
                    }
                    else if (item.comp_info_resolve_status == "Pending")
                    {
                        getallTicketPending.Add(item);
                    }
                }

                var newResponse = new ApiResponseForTicketListAndCount()
                {
                    AllCount = getallTicket.Count,
                    AllData = getallTicket,
                    CloseCount = getallTicketClose.Count,
                    CloseData = getallTicketClose,
                    PendingCount = getallTicketPending.Count,
                    ProcessingData = getallTicketPending,
                };



                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = newResponse
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

        public async Task<ApiResponse> GetClosingNature(string userId, string ip)
        {
            var methodName = "FieldForceService/GetClosingNature";
            try
            {
                List<tbl_ClosingNatureResponseModel> getData = await _misDBContext.tbl_ClosingNature.AsNoTracking().ToListAsync();
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = getData
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

        public async Task<ApiResponse> GetReasonForOutage(long closingNatureId, string userId, string ip)
        {
            var methodName = "FieldForceService/GetClosingNature";
            try
            {
                List<tbl_ReasonForOutageResoponseModel> getData = await _misDBContext.tbl_ReasonForOutage.Where(x => x.ClosingNatureID == closingNatureId).AsNoTracking().ToListAsync();
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = getData
                };
                await InsertRequestResponse(closingNatureId, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(closingNatureId, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSupportDealyReason(string userId, string ip)
        {
            var methodName = "FieldForceService/GetSupportDealyReason";
            try
            {
                List<tbl_SupportDelayResonResopnseModel> getData = await _misDBContext.tbl_SupportDelayReson.AsNoTracking().ToListAsync();
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = getData
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

        public async Task<ApiResponse> GetSupportType(string userId, string ip)
        {
            List<string> supportType = new List<string>() {
                "Phone Support",
                "Physical Support",
                "Back Office Support" ,
                "Phone from Physical",
                "Traceless Customer",
                "Ticket Close Without Support"
            };
            return new ApiResponse()
            {
                Status = "Success",
                StatusCode = 200,
                Message = "get data",
                Data = supportType
            };
        }

        public async Task<ApiResponse> ChangeEngineer(string ticketId, string userId, string ip)
        {
            var methodName = "FieldForceService/ChangeEngineer";
            try
            {
                var query = "SELECT t_Project.Team_id as Team_id FROM t_Project WITH(NOLOCK) " +
                    "INNER JOIN t_Client WITH(NOLOCK) ON t_Project.project_Client_ID = t_Client.client_ID " +
                    "where t_Project.project_Id='" + ticketId + "'";
                var teamId = await _misDBContext.GetTeamId.FromSqlRaw<GetTeamIdModel>(query).FirstOrDefaultAsync();

                var query1 = "select Emp_id, (Emp_id+'::'+Emp_name) as Employee_Name from tbl_team_mem_permission WITH(NOLOCK) " +
                    "where Emp_id is not null and Team_id='" + teamId.Team_id + "' and Emp_id not in " +
                    "(select employeeProject_Employee_Id  from t_EmployeeProject where employeeProject_Project_Id ='" + ticketId + "' )";
                var chengeEngList = await _misDBContext.GetChangeEmginnerList.FromSqlRaw<GetChangeEngListModel>(query1).AsNoTracking().ToListAsync();

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Enginner List",
                    Data = chengeEngList
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

        public async Task<ApiResponse> InitialServiceRestoredNotification(string ticketId, ClaimsPrincipal user, string ip)
        {
            var methodName = "FieldForceService/IniInitialServiceRestoredNotification";
            var userId = user.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            try
            {
                //var mailTemp = "Dear Sir,We are pleased to inform you that your service has been restored.Restoration Date &amp;Time &nbsp; &nbsp; &nbsp; :&lt; 30 / 01 / 2013 11:55:15 AM from input field&gt;</p><p>Please contact us at 09678123123 or e-mail at support@link3.net without any hesitation for further queries or concerns regarding our services.<br>Regards,</p><p><SIGNATURE><br>Designation, Department from MIS</p><p>24 / 7 NMC &amp; amp; Helpdesk: 09678123123<br>Link3 Technologies Ltd.<br>\" runat=\"server\" Height=\"240px\" TextMode=\"MultiLine\"&nbsp;<br>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Width = \"572px\" Visible = \"False\" &gt; Dear Sir,&nbsp;</p><p>We are pleased to inform you that your service has been restored.</p><p>Restoration Date &amp;amp; Time: &amp;lt; 30 / 01 / 2013 11:55:15 AM from input field&amp;gt;</p><p>Please contact us at 09678123123 or e-mail at support@link3.net without any hesitation for further queries or concerns regarding our services.</p><p>Regards,</p><p>&amp;lt; SIGNATURE &amp; gt;</p><p>Designation, Department from&nbsp;MIS</p><p>Link3 Technologies Ltd.</p>";
                var mailTemp = "Dear Sir, \n\n We are pleased to inform you that your service has been restored. " +
                    "\n Restoration Date & Time :<DATE_TIME> " +
                    "\n Please contact us at 09678123123 or e-mail at support@link3.net without any " +
                    "hesitation for further queries or concerns regarding our services. " +
                    "\n\n Regards, " +
                    "\n <SIGNATURE> " +
                    "\n <DESIGNATION_DEPARTMENT> " +
                    "\n 24/7 NMC & Helpdesk:09678123123 " +
                    "\n Link3 Technologies Ltd. \n\n";
                var text1 = mailTemp.Replace("<DATE_TIME>", Convert.ToString(DateTime.Now.ToString()));

                var fullName = user.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
                var text2 = text1.Replace("<SIGNATURE>", fullName);

                var designation = user.Claims.Where(c => c.Type == "Designation").Select(c => c.Value).FirstOrDefault();
                var department = user.Claims.Where(c => c.Type == "Department").Select(c => c.Value).FirstOrDefault();
                var text3 = text2.Replace("<DESIGNATION_DEPARTMENT>", designation + ", " + department);

                var query = "select c.comp_info_com_name, c.comp_info_ref_no, c.comp_info_con_email, d.brCliCode, d.brSlNo, " +
                    "d.phone_no, m.MailData from tbl_complain_info c WITH(NOLOCK) inner join clientDatabaseMain d WITH(NOLOCK) on c.comp_info_client_id = d.brCliCode " +
                    "and c.comp_info_client_slno = d.brSlNo Left join tbl_MailData m WITH(NOLOCK) on c.comp_info_ref_no = m.RefNo " +
                    "where c.comp_info_ref_no ='" + ticketId + "'";

                var teamId = await _misDBContext.InitialServiceRestoredNotification.FromSqlRaw<InitialServiceRestoredNotificationModel>(query).AsNoTracking().FirstOrDefaultAsync();
                var data = new InitialServiceRestoredNotificationResoponseModel()
                {
                    TXTMobileNo = teamId.phone_no,
                    CustomerToEmails = string.IsNullOrEmpty(teamId.MailData) ? teamId.comp_info_con_email : teamId.MailData,
                    CustomerCCEmailsCC = "support@link3.net;",
                    TxtMailTemplate = text3
                };
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Initial service restore",
                    Data = data
                };

                await InsertRequestResponse(ticketId, response, methodName, ip, userId, null);

                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketId, ex, methodName, ip, userId, null);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> ResolvedDetailsMail(string ticketId, ClaimsPrincipal user, string ip)
        {
            var methodName = "FieldForceService/MailTo";
            var userId = user.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            try
            {
                //var mailTemp = "Dear Sir,We are pleased to inform you that your service has been restored.Restoration Date &amp;Time &nbsp; &nbsp; &nbsp; :&lt; 30 / 01 / 2013 11:55:15 AM from input field&gt;</p><p>Please contact us at 09678123123 or e-mail at support@link3.net without any hesitation for further queries or concerns regarding our services.<br>Regards,</p><p><SIGNATURE><br>Designation, Department from MIS</p><p>24 / 7 NMC &amp; amp; Helpdesk: 09678123123<br>Link3 Technologies Ltd.<br>\" runat=\"server\" Height=\"240px\" TextMode=\"MultiLine\"&nbsp;<br>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Width = \"572px\" Visible = \"False\" &gt; Dear Sir,&nbsp;</p><p>We are pleased to inform you that your service has been restored.</p><p>Restoration Date &amp;amp; Time: &amp;lt; 30 / 01 / 2013 11:55:15 AM from input field&amp;gt;</p><p>Please contact us at 09678123123 or e-mail at support@link3.net without any hesitation for further queries or concerns regarding our services.</p><p>Regards,</p><p>&amp;lt; SIGNATURE &amp; gt;</p><p>Designation, Department from&nbsp;MIS</p><p>Link3 Technologies Ltd.</p>";
                var mailTemp = "Dear Sir, " +
                                "\n\n We are pleased to inform you that your service has been restored, details are given below." +
                                "\n\n Solution details:" +
                                "\n <RFO>" +
                                "\n Fault Occurred: <AAAAAAAAA>" +
                                "\n Restoration Date & Time       :<Final_Resolve_Date_&_Time>" +
                                "\n Total downtime: <from_link_down_to_Resolve_Date_&_Time>" +
                                "\n Actual Cause of interruption: <Complain_Category>" +
                                "\n Action Taken: <Resolve_Comments>" +
                                "\n\n Thanks for your patience and cooperation.Please contact us at 09678123123 or e - mail at support@link3.net without any hesitation for further queries or concerns regarding our services." +
                                "\n Regards," +
                                "\n\n  <SIGNATURE>" +
                                "\n <Designation_Department>" +
                                "\n\n  Link3 Technologies Ltd.";

                var complainAccessPermission_query = "SELECT AccessPermissionDatetime, Comments, ComplainID, ComplainRecordDatetime, LinkDownAt, " +
                    "ResulationTime FROM tbl_ComplainAccessPermission WITH(NOLOCK) WHERE  (ComplainID = '" + ticketId + "')";
                var complainAccessPermission = await _misDBContext.ComplainAccessPermission.FromSqlRaw<ComplainAccessPermissionModel>(complainAccessPermission_query).AsNoTracking().FirstOrDefaultAsync();

                DateTime dt = Convert.ToDateTime(complainAccessPermission.LinkDownAt);
                DateTime dt2 = Convert.ToDateTime(complainAccessPermission.ResulationTime == null ? DateTime.Now : Convert.ToDateTime(complainAccessPermission.ResulationTime));
                TimeSpan ts = dt2 - dt;
                int h = ts.Hours;
                int m = ts.Minutes;
                string tt = h + " H:" + m + " M";

                var text_1 = mailTemp.Replace("<RFO>", "");
                var text_2 = text_1.Replace("<AAAAAAAAA>", Convert.ToString(dt));
                var text_3 = text_2.Replace("<Final_Resolve_Date_&_Time>", Convert.ToString(dt2));
                var text_4 = text_3.Replace("<from_link_down_to_Resolve_Date_&_Time>", tt);
                var text_5 = text_4.Replace("<Complain_Category>", "");
                var text_6 = text_5.Replace("<Resolve_Comments>", "");



                var fullName = user.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
                var text2 = text_5.Replace("<SIGNATURE>", fullName);

                var designation = user.Claims.Where(c => c.Type == "Designation").Select(c => c.Value).FirstOrDefault();
                var department = user.Claims.Where(c => c.Type == "Department").Select(c => c.Value).FirstOrDefault();
                var text3 = text2.Replace("<DESIGNATION_DEPARTMENT>", designation + ", " + department);

                var teamId_query = "select c.comp_info_com_name, c.comp_info_ref_no, c.comp_info_con_email " +
                    "from tbl_complain_info c WITH(NOLOCK)  where c.comp_info_ref_no ='" + ticketId + "'";

                var teamId = await _misDBContext.TblComplainInfoModel.FromSqlRaw<tbl_complain_info_Model>(teamId_query).AsNoTracking().FirstOrDefaultAsync();
                var data = new ComplainAccessPermissionResponseModel()
                {
                    MailTo = teamId.comp_info_con_email,
                    MailCC = "support@link3.net;",
                    MailBody = text3
                };
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Mail to data",
                    Data = data
                };
                await InsertRequestResponse(ticketId, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketId, ex, methodName, ip, userId, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + "Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetTicketLog(string ticketId, string userId, string ip)
        {
            var methodName = "FieldForceService/GetTicketLog";
            try
            {
                string mailboby = await getEmailMessageBody(ticketId);

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = mailboby
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

        public async Task<ApiResponse> ResolvedTicket(ResolvedTicketRequestModel model, ClaimsPrincipal user, string ip)
        {
            var methodName = "FieldForceService/ResolvedTicket";
            var userId = user.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            try
            {
                var mailResult = new MailResponseModel();
                string statustask = "Close";
                string gts = "Close";
                string finalsts = "Close";
                string hstatus = "Close";


                var l3id = user.Claims.Where(c => c.Type == "L3Id").Select(c => c.Value).FirstOrDefault();
                var userEmail = user.Claims.Where(c => c.Type == "email").Select(c => c.Value).FirstOrDefault();
                var userFullName = user.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
                var designation = user.Claims.Where(c => c.Type == "Designation").Select(c => c.Value).FirstOrDefault();
                var department = user.Claims.Where(c => c.Type == "Department").Select(c => c.Value).FirstOrDefault();
                var phoneNo = user.Claims.Where(c => c.Type == "PhoneNo").Select(c => c.Value).FirstOrDefault();

                var permissionCheck = await _misDBContext.TblTeamMemPermission.FromSqlRaw("select * from tbl_team_mem_permission  WITH(NOLOCK)  where Team_id in('T-001','T-005') and Emp_id='" + l3id + "'").CountAsync();
                if (permissionCheck > 0)
                {
                    var engCheck = await _misDBContext.tbl_ClosingNature.FromSqlRaw("select * from tbl_ComplainTaskAssign  WITH(NOLOCK)  where refno='" + model.TicketRefNo + "'").CountAsync();
                    if (engCheck > 0)
                    {
                        throw new Exception("Engineer Not assign");
                    }
                }

                var tickitComplainInfoQuery = "SELECT AutoSL, Clientcategory, DistributorTaskStatus, comp_info_Category, comp_info_Led_Status, " +
                    "comp_info_Receive_ActualTime, comp_info_Receive_Date, comp_info_Receive_Time, comp_info_Source_Information, " +
                    "comp_info_attachments, comp_info_client_Media, comp_info_client_adr, comp_info_client_adrcode, comp_info_client_id, " +
                    "comp_info_client_name, comp_info_client_slno, comp_info_com_name, comp_info_comm, comp_info_complain, comp_info_con_email, " +
                    "comp_info_con_phone_no, comp_info_contact_person, comp_info_deadline, comp_info_email_to_client, comp_info_hold_on, " +
                    "comp_info_last_update, comp_info_manually_email, comp_info_mkt_person, comp_info_postponed_flg, comp_info_postponed_hour, " +
                    "comp_info_postponed_time, comp_info_receive_by, comp_info_ref_no, comp_info_related_dept," +
                    " comp_info_resolve_status, comp_info_service_code, comp_info_service_desc, comp_info_type, receiveby, state " +
                    "FROM tbl_complain_info WITH(NOLOCK) WHERE (comp_info_ref_no = '" + model.TicketRefNo + "')";

                var GetDataByTicketRefno = await _misDBContext.TblComplainInfo.FromSqlRaw(tickitComplainInfoQuery).AsNoTracking().FirstOrDefaultAsync();

                if (GetDataByTicketRefno == null)
                {
                    throw new Exception("Complain is not found.");
                }
                var teamid = GetDataByTicketRefno.comp_info_related_dept;
                DateTime complainReceiveTime = Convert.ToDateTime(GetDataByTicketRefno.comp_info_Receive_ActualTime.ToString());
                DateTime curDateTime = DateTime.Now;
                TimeSpan complainDelayTime = curDateTime - complainReceiveTime;

                string issueDelayDayTime = complainDelayTime.ToString("G", new CultureInfo("en-US"));
                string[] issueDelayDayTimeList = issueDelayDayTime.Split(new Char[] { ':' });
                int totalIssueDelayHours = int.Parse(issueDelayDayTimeList[0]) * 24 + int.Parse(issueDelayDayTimeList[1]);

                DateTime officeStartTime = new DateTime(curDateTime.Year, curDateTime.Month, curDateTime.Day, 08, 00, 00);
                TimeSpan currentTimeDiff = curDateTime - officeStartTime;
                string currentDelayTime = currentTimeDiff.ToString("G", new CultureInfo("en-US"));
                string[] currentDelayTimeList = currentDelayTime.Split(new Char[] { ':' });
                int currentDelayHours = int.Parse(currentDelayTimeList[0]) * 24 + int.Parse(currentDelayTimeList[1]);

                if ((currentDelayHours >= 4) && (totalIssueDelayHours >= 4))
                {
                    if (string.IsNullOrWhiteSpace(model.ResonForSupportDelay))
                    {
                        throw new Exception("Select Missing Reson for Support Delay");
                    }
                }

                model.Comments = model.Comments + "\nResolved by :" + l3id + ":" + userFullName + ",Cell No:" + phoneNo + "\n";

                DateTime starttime = model.AccessPermissionDateTime;
                DateTime t1 = model.AccessPermissionDateTime;

                DateTime t2 = model.DPTResolveDateTime;

                int ar_time_diff = DateTime.Compare(t2, t1);
                if (ar_time_diff <= 0)
                {
                    throw new Exception("Resolved date must be greater than start date.");
                }

                int sta = GetDataByTicketRefno.state;

                string task_name = "", task_description = "", start_date = null;

                var ttype = await _misDBContext.TProject.FromSqlRaw("SELECT DISTINCT project_Title FROM t_Project  WITH(NOLOCK)  WHERE (project_Id = '" + model.TicketRefNo + "')").FirstOrDefaultAsync();
                //var getMaxTicketId = await _misDBContext.SP_MaxTicketId.FromSqlRaw("exec max_Task_Ref_No").ToListAsync();
                var taskid = ""; //getMaxTicketId[0].TicketId;
                var query_getProjectByRef = "SELECT t_Project.project_Id, t_Project.project_Title, t_Project.project_StartTime, " +
                    "t_Client.client_CompanyName, t_Project.project_Status, t_Project.project_Description, t_Project.project_EstimateTime, " +
                    "t_Project.Project_Type, t_Project.project_Client_ID, t_Project.project_category, t_Project.Team_id " +
                    "FROM t_Project WITH(NOLOCK)  INNER JOIN t_Client  WITH(NOLOCK) ON t_Project.project_Client_ID = t_Client.client_ID " +
                    "where t_Project.project_Id='" + model.TicketRefNo + "'";

                var getProjectByRef = await _misDBContext.GetProjectByRefNo.FromSqlRaw(query_getProjectByRef).AsNoTracking().FirstOrDefaultAsync();

                if (getProjectByRef != null)
                {
                    task_name = Convert.ToString(getProjectByRef.project_Title);
                    task_description = Convert.ToString(getProjectByRef.project_Description);
                    start_date = Convert.ToDateTime(getProjectByRef.project_StartTime).ToString();
                }

                long outageReson = 0;
                long supportReson = 0;
                if (!string.IsNullOrEmpty(model.ResonForOutage))
                {
                    outageReson = Convert.ToInt64(model.ResonForOutage);
                }
                if (string.IsNullOrEmpty(model.ResonForSupportDelay))
                {
                    supportReson = Convert.ToInt64(model.ResonForSupportDelay);
                }

                var mailBody = await EmailtoTeam(model.TicketRefNo, "Close", l3id, userFullName, phoneNo, department, model.Comments);

                int fttxphys = 0;
                int fttxphones = 0;
                int corpphys = 0;
                int corpphons = 0;
                string clic = GetDataByTicketRefno.Clientcategory;
                if (clic == "RETAIL (SOHO)" || clic == "RETAIL (HOME)")
                {
                    if (model.SupportType == "Phone Support")
                    {
                        fttxphones = 1;
                    }
                    else
                    {
                        fttxphys = 1;
                    }

                }
                else
                {
                    if (model.SupportType == "Phone Support")
                    {
                        corpphons = 1;
                    }
                    else
                    {
                        corpphys = 1;
                    }
                }
                bool tfbls = false;
                string fg = "";
                if (model.CheckboxEmailTo == true)
                {
                    // Ticket_Log();
                    if (GetDataByTicketRefno.comp_info_client_id != "08.01.001.00676")
                    {
                        string mailboby = await getEmailMessageBody(model.TicketRefNo);

                        string info = "";

                        info = info + "Mailing Status : " + "Initial Service Restored Notification" + "\n";
                        info = info + "Mail sent by   : " + userFullName + "\n";
                        info = info + "Department     : " + department + "\n";
                        info = info + "Date Time      : " + DateTime.Now + "\n\n";
                        info = info + mailboby + "\n";

                        fg = info;
                    }

                    if (GetDataByTicketRefno.comp_info_client_id != "08.01.001.00676")
                    {
                        if (tfbls == false)
                        {
                            mailResult = await SendMailToClient(model.TxtMailToTemplate, model.TicketRefNo, model.CustomerToEmails, model.CustomerCCEmailsCC, GetDataByTicketRefno.comp_info_com_name);
                            await InsertSmsData(model.DPTResolveDateTime, model.TicketRefNo, GetDataByTicketRefno.comp_info_com_name, GetDataByTicketRefno.comp_info_client_id, model.TXTMobileNo);
                        }
                    }

                }

                if (model.CheckboxResolveDetailsMail == true)
                {
                    if (GetDataByTicketRefno.comp_info_client_id != "08.01.001.00676")
                    {
                        if (tfbls == false)
                        {

                            var text = model.TxtTemplate2;

                            if (string.IsNullOrEmpty(model.ResonForOutage))
                            {
                                text = model.ResonForOutage.Replace("<RFO>", " ");
                            }
                            else
                            {
                                text = model.ResonForOutage.Replace("<RFO>", "Reson for outage (RFO) : " + model.ResonForOutage);
                            }
                            //ResolveMail
                            await ResolvedMail(model.TxtTemplate2, GetDataByTicketRefno.comp_info_com_name, model.TicketRefNo, model.TxtMailToTemplate, model.TxtMailToTemplateCC, userEmail, userFullName);
                        }
                    }
                }


                var query_getDataByProjectId = "SELECT task_Credit, task_Description, task_Employee_Id, task_Employee_Name, task_EstimateTime, " +
                    "task_Id, task_Name, task_Project_Id, task_Project_Title, task_StartDate, task_Status, task_team_leaderAction, task_type " +
                    "FROM t_MultiTask  WITH(NOLOCK) WHERE (task_Project_Id = '" + model.TicketRefNo + "')";
                var getDataByProjectId = await _misDBContext.t_MultiTask.FromSqlRaw(query_getDataByProjectId).AsNoTracking().FirstOrDefaultAsync();
                var getDataByProjectIdNotNull = getDataByProjectId == null ? "true" : "false";


                var dtMem_Query = "SELECT AutoSL, Assign_emp, Close_task, Emp_id, Emp_name, Final_post, LockUnlock_Prm, " +
                    "Post_reply, Team_Name, Team_id, Ticket_close, Ticket_forward, Ticket_insSolve, Ticket_open, " +
                    "Ticket_viewAll FROM tbl_team_mem_permission  WITH(NOLOCK) WHERE (Emp_id = '" + l3id + "') AND (Team_id IN ('T-001', 'T-005'))";
                var dtMem = await _misDBContext.TblTeamMemPermission.FromSqlRaw(dtMem_Query).AsNoTracking().FirstOrDefaultAsync();
                var dtMemNotNull = dtMem != null ? "true" : "false";
                var tmemid = "";
                if (dtMem != null)
                {
                    tmemid = dtMem.Team_id;
                }

                var strChangeAccessPermission = model.ChangeAccessPermission == true ? "true" : "false";
                var checkboxCloseNature = model.CheckboxClosingNature == true ? "true" : "false";

                var checkEng = model.CheckEngneer == true ? "true" : "false";

                if (model.CheckEngneer == true)
                {
                    var get_T_employeeProjectList = await _misDBContext.t_EmployeeProject.Where(x => x.employeeProject_Project_Id == model.TicketRefNo).ToListAsync();

                    _misDBContext.t_EmployeeProject.RemoveRange(get_T_employeeProjectList);

                    var newList = new List<t_EmployeeProjectModel>();

                    foreach (var aData in model.SelectedEmployeeListBox)
                    {
                        var aInfo = new t_EmployeeProjectModel()
                        {
                            employeeProject_Project_Id = model.TicketRefNo,
                            employeeProject_Employee_Id = aData,
                            employeeProject_AssigenDate = DateTime.Now
                        };
                        newList.Add(aInfo);
                    }

                    await _misDBContext.t_EmployeeProject.AddRangeAsync(newList);

                }


                // resolved sp call
                string sql = "EXEC ResolvedTicketForApi @getDataByProjectIdNotNull, @taskid, @project_Title, " +
                    "@task_name, @task_description, @start_date, @userFullName, @TicketRefNo, @userId," +
                    " @CheckboxClosingNature, @ClosingNature, @comp_info_client_id, @comp_info_com_name, " +
                    "@comp_info_Category, @comp_info_related_dept, @comp_info_Receive_Date, @outageReson, " +
                    "@supportReson, @close, @dtMemNotNull, @teamid, @Comments, @sta, @ChangeAccessPermission, " +
                    "@starttime, @SupportType, @DPTResolveDateTime, @fttxphones, @fttxphys, @corpphons, @corpphys, @fg, @mailBody";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    // Create parameters
                    new SqlParameter { ParameterName = "@getDataByProjectIdNotNull", Value = getDataByProjectIdNotNull },
                    new SqlParameter { ParameterName = "@taskid", Value = taskid },
                    new SqlParameter { ParameterName = "@project_Title", Value = ttype.project_Title },
                    new SqlParameter { ParameterName = "@task_name", Value = task_name },
                    new SqlParameter { ParameterName = "@task_description", Value = task_description },
                    new SqlParameter { ParameterName = "@start_date", Value = start_date },
                    new SqlParameter { ParameterName = "@userFullName", Value = userFullName },
                    new SqlParameter { ParameterName = "@TicketRefNo", Value = model.TicketRefNo },
                    new SqlParameter { ParameterName = "@userId", Value = l3id },
                    new SqlParameter { ParameterName = "@CheckboxClosingNature", Value = checkboxCloseNature },
                    new SqlParameter { ParameterName = "@ClosingNature", Value = model.ClosingNature },
                    new SqlParameter { ParameterName = "@comp_info_client_id", Value = GetDataByTicketRefno.comp_info_client_id },
                    new SqlParameter { ParameterName = "@comp_info_com_name", Value = GetDataByTicketRefno.comp_info_com_name },
                    new SqlParameter { ParameterName = "@comp_info_Category", Value = GetDataByTicketRefno.comp_info_Category },
                    new SqlParameter { ParameterName = "@comp_info_related_dept", Value = GetDataByTicketRefno.comp_info_related_dept },
                    new SqlParameter { ParameterName = "@comp_info_Receive_Date", Value = GetDataByTicketRefno.comp_info_Receive_Date },
                    new SqlParameter { ParameterName = "@outageReson", Value = outageReson },
                    new SqlParameter { ParameterName = "@supportReson", Value = supportReson },
                    new SqlParameter { ParameterName = "@close", Value = "Close" },
                    new SqlParameter { ParameterName = "@dtMemNotNull", Value = dtMemNotNull },
                    new SqlParameter { ParameterName = "@teamid", Value = teamid },
                    new SqlParameter { ParameterName = "@Comments", Value = model.Comments },
                    new SqlParameter { ParameterName = "@sta", Value = sta },
                    new SqlParameter { ParameterName = "@ChangeAccessPermission", Value = strChangeAccessPermission },
                    new SqlParameter { ParameterName = "@starttime", Value = starttime.ToString() },
                    new SqlParameter { ParameterName = "@SupportType", Value = model.SupportType },
                    new SqlParameter { ParameterName = "@DPTResolveDateTime", Value = model.DPTResolveDateTime },
                    new SqlParameter { ParameterName = "@fttxphones", Value = fttxphones },
                    new SqlParameter { ParameterName = "@fttxphys", Value = fttxphys },
                    new SqlParameter { ParameterName = "@corpphons", Value = corpphons },
                    new SqlParameter { ParameterName = "@corpphys", Value = corpphys },
                    new SqlParameter { ParameterName = "@fg", Value = fg },
                    new SqlParameter { ParameterName = "@mailBody", Value = mailBody },
                };

                var result = await _misDBContext.ResolvedTicketForApi.FromSqlRaw(sql, parms.ToArray()).AsNoTracking().ToListAsync();
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
                    Message = "Resolved Success",
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

        private async Task ResolvedMail(string txttemplate2, string lblcliname, string ticketId, string txtmailtotemplate0, string txtmailtotemplateCC, string uidMail, string uName)
        {
            string body = txttemplate2;
            string subject = "Details: Service Restored at :" + lblcliname + ", " + "[" + ticketId + "]";
            string fromAddress = "support@link3.net";
            List<string> toAddress = new List<string>();
            var mm = txtmailtotemplate0.Split(';', ',');

            if (mm.Length > 0)
            {
                foreach (var to in mm)
                {
                    if (to != "")
                    {
                        toAddress.Add(to);
                    }

                }
            }
            List<string> ccAddress = new List<string>();
            var CCadd = txtmailtotemplateCC.Split(';', ',');
            if (CCadd.Length > 0)
            {
                foreach (var cc in CCadd)
                {
                    if (cc != "")
                    {
                        ccAddress.Add(cc);
                    }

                }
            }

            await _mailSenderService.SendMail(subject, body, fromAddress, toAddress, ccAddress, null);
        }
        private async Task<MailResponseModel> SendMailToClient(string txtMailTemplate, string ticketId, string CustomerToEmails, string CustomerCCEmailsCC, string comp_info_com_name)
        {
            string a = txtMailTemplate.Replace("<30/01/2013 11:55:15 AM from input field>", DateTime.Now.ToString());
            string mailBody = "";

            var getDataByRefNo_query = "SELECT MailBody, MailSubject, RefNo FROM tbl_ComplainMailLooping  WITH(NOLOCK) WHERE (RefNo = '" + ticketId + "')";
            var getDataByRefNo = await _misDBContext.TblComplainMailLooping.FromSqlRaw(getDataByRefNo_query).AsNoTracking().FirstOrDefaultAsync();

            var finalBody = "";
            var finalSubject = "";
            if (getDataByRefNo != null)
            {
                string subject = getDataByRefNo.MailSubject;
                string body = getDataByRefNo.MailBody;

                mailBody = a + "\n";
                mailBody = mailBody + "Subject  : " + subject + "\n";
                mailBody = mailBody + body + "\n";


                finalBody = mailBody;
                finalSubject = "Service restored at  :" + subject + "";

            }
            else
            {
                finalBody = a.ToString();
                finalSubject = "Service restored at  :" + comp_info_com_name + ", " + "[" + ticketId + "]";
            }

            var fromAddress = "support@link3.net";

            List<string> toAddress = new List<string>();
            var mm = CustomerToEmails.Split(';', ',');

            if (mm.Length > 0)
            {
                foreach (var to in mm)
                {
                    if (to != "")
                    {
                        toAddress.Add(to);
                    }

                }
            }
            List<string> ccAddress = new List<string>();
            var CCadd = CustomerCCEmailsCC.Split(';', ',');
            if (CCadd.Length > 0)
            {
                foreach (var cc in CCadd)
                {
                    if (cc != "")
                    {
                        ccAddress.Add(cc);
                    }

                }
            }
            await _mailSenderService.SendMail(finalSubject, finalBody, fromAddress, toAddress, ccAddress, null);
            var response = new MailResponseModel()
            {
                Body = finalBody,
                Subject = finalSubject,
            };
            return response;
        }

        private async Task InsertSmsData(DateTime DPTResolveDateTime, string TicketId, string comp_info_com_name, string comp_info_client_id, string TXTMobileNo)
        {
            var methodName = "FieldForceService/InsertSmsData";

            var smsText = "Sir, Your service at <Customer NAme, Br.Name, [TKI]> has been restored at <datetimeformat>.[LINK3]";
            string ov = DPTResolveDateTime.ToString();
            string cna = comp_info_com_name + " [" + TicketId + "]";

            string vv = smsText.Replace("<Customer NAme, Br.Name, [TKI]>", cna);
            var finalMessage = vv.Replace("<datetimeformat>", ov);

            string s1 = @"প্রিয় গ্রাহক আপনার সার্ভিস পুনরুদ্ধার করা হয়েছে। সাময়িক অসুবিধার জন্য অত্যন্ত দুঃখিত । প্রাপ্ত সার্ভিস সম্পর্কে মতামত জানাতে ক্লিক করুন";


            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(comp_info_client_id + "/" + TicketId + "/" + comp_info_com_name);
            string enc = Convert.ToBase64String(encbuff);
            string urlenc = "";// Server.UrlEncode(enc);

            string ap = @"https://selfcare.link3.net/feedback/" + urlenc + " প্রশ্নের জন্য কল করুন 09678123123। আমাদের সাথে থাকার জন্য ধন্যবাদ।";
            string jk = s1 + " " + ap;


            try
            {
                var httpRequestMessage = new HttpRequestMessage(
                            HttpMethod.Get,
                            "http://sms.link3.net/http_api/post_api.php?api_id=LS0033&api_password=01715568606&mobile=" + TXTMobileNo + "&message=" + jk.Replace("'", "''") + "");

                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                var str = "DPTResolveDateTime: " + DPTResolveDateTime + "; TicketId: " + TicketId + "; comp_info_com_name: " + comp_info_com_name + "; comp_info_client_id: " + comp_info_client_id + "; TXTMobileNo: " + TXTMobileNo;
                await InsertRequestResponse(str, ex, methodName, null, null, ex.Message);
            }
        }

        private async Task<string> EmailtoTeam(string ticketId, string sts, string userId, string name, string phone, string department, string txtcomments)
        {
            string mailboby = "", elogCategory = "", subj = "";

            mailboby = await getEmailMessageBody(ticketId);

            string mbody;

            if (sts == "Resolve")
            {
                mbody = "--------------[ Resolved and Forward to Controller Group Action ]------------" + "\n\n";
            }
            else
            {
                mbody = "--------------[ Ticket Close ]------------" + "\n\n";
            }

            mbody = mbody + "Completely Close, Close by    : " + userId + ":" + name + ",Cell No:" + phone + "\n";
            mbody = mbody + "Departmet                     : " + department + "\n";
            mbody = mbody + "Solution Description          : " + txtcomments + "\n";
            mbody = mbody + "Time                          : " + "[" + DateTime.Now + "]" + "\n\n";
            mbody = mbody + mailboby;

            mailboby = mbody;

            return mailboby;
        }

        private async Task<string> getEmailMessageBody(string ticketid)
        {
            var methodName = "FieldForceService/getEmailMessageBody";
            try
            {
                var mailformet_query = "SELECT CTID, MailBcc, MailBody, MailCC, MailSubject, Mailfrom, Mailto, " +
                    "Status FROM tblComplainEmailFormat  WITH(NOLOCK) WHERE  (CTID = '" + ticketid + "')";
                var complainEmailFormat = await _misDBContext.tblComplainEmailFormat.FromSqlRaw(mailformet_query).AsNoTracking().FirstOrDefaultAsync();
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
            var errorMethordName = "FieldForceService/InsertRequestResponse";
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

        public async Task<ApiResponse> GetInternetTechnologyDataFromMedia(string ip, ClaimsPrincipal user)
        {
            var methodName = "FieldForceService/GetInternetTechnologyDataFromMedia";
            try
            {
                List<ClientDatabaseWireSetupResponseModel> getData = await _misDBContext.ClientDatabaseWireSetup.AsNoTracking().ToListAsync();
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = getData
                };
                await InsertRequestResponse(null, response, methodName, ip, user.GetClaimUserId(), null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetClientDatabseTechnologySetupData(string ip, ClaimsPrincipal user)
        {
            var methodName = "FieldForceService/GetClientDatabseTechnologySetupData";
            try
            {
                List<ClientDatabseTechnologySetupResponseModel> getData = await _misDBContext.ClientDatabseTechnologySetup.AsNoTracking().ToListAsync();
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = getData
                };
                await InsertRequestResponse(null, response, methodName, ip, user.GetClaimUserId(), null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetClientDatabaseMediaSetupData(string ip, ClaimsPrincipal user)
        {
            var methodName = "FieldForceService/GetClientDatabaseMediaSetupData";
            try
            {
                List<ClientDatabaseMediaSetupResponseModel> getData = await _misDBContext.ClientDatabaseMediaSetup.AsNoTracking().ToListAsync();
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = getData
                };
                await InsertRequestResponse(null, response, methodName, ip, user.GetClaimUserId(), null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSupportOfficeData(string ip, ClaimsPrincipal user)
        {
            var methodName = "FieldForceService/GetSupportOfficeData";
            try
            {
                //List<SupportOfficeResponseModel> getData = await _misDBContext.SupportOffice.sq .ToListAsync();
                string strSql = " SELECT SupportOfficeID, SupportOfficeName, EntryDate, EntryUserID, ModifiedDate, ModifyingUserID "
                    + " FROM SupportOffice ORDER BY SupportOfficeName";
                //var GetDataByTicketRefno = await _misDBContext.TblComplainInfo.FromSqlRaw(tickitComplainInfoQuery).FirstOrDefaultAsync();

                var getData = await _misDBContext.SupportOffice.FromSqlRaw(strSql).AsNoTracking().ToListAsync();

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = getData
                };
                await InsertRequestResponse(null, response, methodName, ip, user.GetClaimUserId(), null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAssignedMisInstallationTicketList(string userId, string ip)
        {
            var methodName = "FieldForceService/GetAssignedMisInstallationTicketList";
            try
            {

                //string strSql = " select vwmktp.Mkt_group, vwmktp.Pending_for, vwmktp.CompanyName, vwmktp.TrackingInfo, "
                //    + " vwmktp.SalesPerson, vwmktp.EntryDate, vwmktp.PStatus, vwmktp.updatetime, vwmktp.BranchName, "
                //    + " vwmktp.CommisionDate, vwmktp.Cli_code, vwmktp.Cli_Adr_Code, vwmktp.CliAdrNewCode, vwmktp.EngName, "
                //    + " vwmktp.TicketFollowUp, vwmktp.MediaName, vwmktp.Pending_for_team, vwmktp.brAreaGroup, "
                //    + " vwmktp.brSupportOffice, vwmktp.brArea, vwmktp.cli_category, vwmktp.PendingReson, vwmktp.brAddress1 brAddress, "
                //    + " vwmktp.phone_no, tassign.Emp_ID, tassign.Emp_Name, tassign.Team_Name, tassign.TeamAssignDate, "
                //    + " tassign.AssignBy, tassign.Status,  tassign.SL TeamAssignSlNo from [dbo].[mkt_pending] vwmktp "
                //    + " inner join [dbo].[Cli_TeamAssign] tassign on vwmktp.TrackingInfo = tassign.TrackingInfo where "
                //    + " vwmktp.PStatus = 'INI' and cast(vwmktp.EntryDate as date) between (GETDATE()-7) and GETDATE() "
                //    + " and Emp_ID = '" + userId + "'order by vwmktp.EntryDate asc ";

                string strSql = " select vwmktp.Mkt_group, vwmktp.Pending_for, vwmktp.CompanyName, vwmktp.TrackingInfo, "
                    + " vwmktp.SalesPerson, vwmktp.EntryDate, vwmktp.PStatus, vwmktp.updatetime, vwmktp.BranchName, "
                    + " vwmktp.CommisionDate, vwmktp.Cli_code, vwmktp.Cli_Adr_Code, vwmktp.CliAdrNewCode, vwmktp.EngName, "
                    + " vwmktp.TicketFollowUp, vwmktp.MediaName, vwmktp.Pending_for_team, vwmktp.brAreaGroup, "
                    + " vwmktp.brSupportOffice, vwmktp.brArea, vwmktp.cli_category, vwmktp.PendingReson, vwmktp.brAddress1 brAddress, "
                    + " vwmktp.phone_no, tassign.Emp_ID, tassign.Emp_Name, tassign.Team_Name, tassign.TeamAssignDate, "
                    + " tassign.AssignBy, tassign.Status,  tassign.SL TeamAssignSlNo from [dbo].[mkt_pending] vwmktp "
                    + " inner join [dbo].[Cli_TeamAssign] tassign on vwmktp.TrackingInfo = tassign.TrackingInfo where "
                    + " vwmktp.PStatus = 'INI' and cast(vwmktp.EntryDate as date) between (GETDATE()-30) and GETDATE() "
                    + " and Emp_ID = '" + userId + "'order by vwmktp.EntryDate asc ";


                var getData = await _misDBContext.MisInstallationTicketList.FromSqlRaw(strSql).AsNoTracking().ToListAsync();

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = getData
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


        public async Task<ApiResponse> UpdateMediaInfoDetails(MediaInformationRequestModel requestModel, ClaimsPrincipal user, string ip)
        {
            var methodName = "FieldForceService/UpdateMediaInfoDetails";
            var response = new ApiResponse();
            int items = 0;
            try
            {
                string strGetSql = " SELECT MedID, TechID, Tempd, WireID, brCliAdrCode, brCliCode, brSlNo "
                    + " FROM clientDatabaseMediaDetails WHERE (brCliCode = '" + requestModel.brCliCode + "') "
                    + " AND (brSlNo = " + Convert.ToInt32(requestModel.brSlNo) + ") ";
                //var getData = await _misDBContext.ClientDatabaseMediaDetails.FromSqlRaw(strGetSql).DefaultIfEmpty().ToListAsync();
                var getData = await _misDBContext.ClientDatabaseMediaDetails.FromSqlRaw(strGetSql).AsNoTracking().ToListAsync();
                if (getData.Count > 0)
                {
                    string strUpdateSql = " UPDATE clientDatabaseMediaDetails SET brCliAdrCode = '" + requestModel.brAdrNewCode + "', "
                    + " WireID = " + Convert.ToInt32(requestModel.internetTechnologyWireId) + ", TechID = " + Convert.ToInt32(requestModel.internetMediaTechId) + ", "
                    + " MedID = " + Convert.ToInt32(requestModel.internetMediaId) + " WHERE (brCliCode = '" + requestModel.brCliCode + "') "
                    + " AND (brSlNo = " + Convert.ToInt32(requestModel.brSlNo) + ") ";

                    items = await _misDBContext.Database.ExecuteSqlRawAsync(strUpdateSql);
                    if (items > 0)
                    {

                    }
                    else
                    {
                        response = new ApiResponse()
                        {
                            Status = "Failed",
                            StatusCode = 400,
                            Message = "Update process failed.",
                            Data = null
                        };
                        return response;
                    }
                }
                else
                {
                    string strSqlInsert = @"insert into clientDatabaseMediaDetails_Test (brCliCode,brSlNo,brCliAdrCode,WireID,TechID,MedID) "
                        + " values  ('" + requestModel.brCliCode + "', " + Convert.ToInt32(requestModel.brSlNo) + ", "
                        + " '" + requestModel.brAdrNewCode + "', " + Convert.ToInt32(requestModel.internetTechnologyWireId) + ", "
                        + " " + Convert.ToInt32(requestModel.internetMediaTechId) + ", " + Convert.ToInt32(requestModel.internetMediaId) + ") ";

                    items = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlInsert);
                    if (items > 0)
                    {

                    }
                    else
                    {
                        response = new ApiResponse()
                        {
                            Status = "Failed",
                            StatusCode = 400,
                            Message = "Insert process failed.",
                            Data = null
                        };
                        return response;
                    }
                }

                // update post installation media name
                string strPostInsmediaUpdate = " UPDATE Post_Installation SET MediaName = '" + requestModel.internetMediaTypeName + "' WHERE (TrackingInfo = '" + requestModel.TrackingInfo + "') ";
                int updateMediaResult = await _misDBContext.Database.ExecuteSqlRawAsync(strPostInsmediaUpdate);

                if (updateMediaResult > 0)
                {
                    string SupportType = "Installation";
                    string taskstatus = "Media Update";
                    string remarks = "";
                    string strInsertUserLogHistory = @"INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, 
                                      Remarks,TaskStatus) VALUES('" + requestModel.TrackingInfo + "','" + user.GetClaimUserId() + "','Installation','"
                                      + SupportType + "','" + remarks + "','" + taskstatus + "')";
                    int historyInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strInsertUserLogHistory);
                    if (historyInsertResult > 0)
                    {
                        response = new ApiResponse()
                        {
                            Status = "Success",
                            StatusCode = 200,
                            Message = "Insert process successfully completed.",
                            Data = null
                        };
                    }
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "Insert process failed.",
                        Data = null
                    };
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> SupportofficeDataUpdateInClientDatabase(SupportOfficeInfoRequestModel model,
            ClaimsPrincipal user, string ip)
        {
            var methodName = "FieldForceService/SupportofficeDataUpdateInClientDatabase";
            var response = new ApiResponse();
            string SupportType = "Installation";
            string taskstatus = "Support Office Update";
            string remarks = "";
            try
            {
                string strUpdateResult = " update clientDatabaseMain set brSupportOfficeId = '" + model.brSupportOfficeId + "', "
                    + " brSupportOffice = '" + model.brSupportOffice + "' where brCliCode = '" + model.brCliCode + "' "
                    + " and brSlNo = '" + model.brSlNo + "' ";
                int updateItem = await _misDBContext.Database.ExecuteSqlRawAsync(strUpdateResult);

                string strInsertUserLogHistory = @"INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, 
                                      Remarks,TaskStatus) VALUES('" + model.TrackingInfo + "','" + user.GetClaimUserId() + "','Installation','"
                                          + SupportType + "','" + remarks + "','" + taskstatus + "')";
                int historyInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strInsertUserLogHistory);

                if (historyInsertResult > 0)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Insert process successfully completed.",
                        Data = null
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetNetworkTypeList(ClaimsPrincipal user, string ip)
        {
            var methodName = "FieldForceService/GetNetworkTypeList";
            var response = new ApiResponse();

            try
            {
                string strSql = " SELECT CablePathID, CablePathName FROM tbl_BackboneCablePathTypeSetup ORDER BY CablePathName";
                var getData = await _misDBContext.BackboneCablePathTypes.FromSqlRaw(strSql).AsNoTracking().ToListAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Get Network type data successfully.",
                        Data = getData
                    };

                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 400,
                        Message = "No data found.",
                        Data = null
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetClientDatabaseMainInfoByAddressCode(ClaimsPrincipal user, string ip, string clientAddressCode)
        {
            var methodName = "FieldForceService/GetClientDatabaseMainInfoByAddressCode";
            var response = new ApiResponse();
            try
            {
                string strSql = " SELECT [brCliCode],[brSlNo],[brAdrCode],[brAdrNewCode],[brName],[brGroupId],[brGroup] "
                    + " ,[brMktGrpId],[brMktGroup],[brClientTypeId],[brClientType],[brAddressTypeID],[brAddressTypeName] "
                    + " ,[brBusinessTypeId],[brBusinessType],[brCategoryId],[brCategory],[brNatureId],[brNature],[brStatus] "
                    + " ,[brInsStatus],[brCrStatus],[brBlStatus],[brSearch],[SdStatus],[brBlStdate],[brLastBlDate],[brAddress1] "
                    + " ,[brAddress2],[brAreaGroupId],[brAreaGroup],[brAreaId],[brArea],[brPostalArea],[brwebsite],[brstatussla] "
                    + " ,[brsladate],[brdateinception],[brcompanyname],[branchmanager],[brSupportOfficeId],[brSupportOffice] "
                    + " ,[contact_det],[Contact_Designation],[phone_no],[fax_no],[email_id],[mrtg_link],[note_for_services] "
                    + " ,[bw_as_client],[add_for_p2p],[note_for_bts],[cur_status],[final_update_from],[final_update_by] "
                    + " ,[final_update_date],[i_bill_date],[i_seller],[i_acc_manager],[i_ins_ref_no],[i_ins_date],[i_ins_engg] "
                    + " ,[i_bill_mgr],[i_terms_cond],[sll],[UpdStatus],[ClientUpdStatus],[sdststemp],[HeadOfficeName],[BranchName] "
                    + " ,[LockUnlock],[SMSMobileNo],[ClientRefarence],[NewE1POP],[VPNTunnel],[ICBWProvider],[SofDate],[SubscriberType] "
                    + " ,[Salution],[GenderType],[MiddleName],[LastName],[NickName],[Occupation],[MQStatus],[BillDeliverd],[MqID] "
                    + " ,[MqActiveInactive],[SubscriberPassword],[SOFCompleteBy],[SOFCompleteDate],[DistributorID],[PackagePlan] "
                    + " ,[DistributorSubscriberID],[RRPID],[RRPSubscriberID],[SMEPkgID],[DisPackageID],[TimeBased],[SAFNumber] "
                    + " ,[ProofTypeID],[ProofID_No] FROM[dbo].[clientDatabaseMain] "
                    // + " WHERE(brAdrNewCode = 'BR-378888')
                    + " WHERE(brAdrNewCode = '" + clientAddressCode + "') ";
                var getData = await _misDBContext.ClientDatabaseMainResponseModel.FromSqlRaw(strSql).AsNoTracking().ToListAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully.",
                        Data = getData
                    };

                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 400,
                        Message = "No data found.",
                        Data = null
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetClientBillingAddressInfoByClientCodeAndSerialNo(string userId, string ip,
            string brClientCode, int brSerialNumber)
        {
            var methodName = "FieldForceService/GetClientBillingAddressInfoByClientCodeAndSerialNo";
            var response = new ApiResponse();
            try
            {
                string strSql = " SELECT [br_cli_code], [br_sl_no], [BrAdrCode], [br_name], [br_contact_name], [br_contact_num], "
                    + " [br_contact_email], [br_adr1], [br_adr2], [br_area], [br_sub_area], [br_postal_area], [UpdStatus], "
                    + " [RoadNo], [Sector], [Block], [Division], [District], [Thana], [ParentArea], [SubArea], [PostalCode], "
                    + " [LandMark], [BuildingName] FROM [dbo].[clientBillingAddress] "
                    //  + " where br_cli_code='08.01.001.149920' and br_sl_no='1'
                    + " where br_cli_code = '" + brClientCode + "' and br_sl_no = " + brSerialNumber + " ";
                var getData = await _misDBContext.ClientBillingAddress.FromSqlRaw(strSql).AsNoTracking().ToListAsync();
                if (getData.Count != 0)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully.",
                        Data = getData
                    };

                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 400,
                        Message = "No data found.",
                        Data = null
                    };

                    return response;
                }

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetClientTechnicalInfo(string brClientCode, int brSerialNumber, string userId, string ip)
        {
            var methodName = "FieldForceService/GetClientTechnicalInfo";
            var response = new ApiResponse();

            try
            {
                string strSql = " SELECT [brCliCode],[brSlNo],[brAdrCode],[brAdrNewCode],[brName],[brGroupId],[brGroup], "
                    + " [brMktGrpId],[brMktGroup],[brClientTypeId],[brClientType],[brAddressTypeID],[brAddressTypeName], "
                    + " [brBusinessTypeId],[brBusinessType],[brCategoryId],[brCategory],[brNatureId],[brNature],[brStatus], "
                    + " [brInsStatus],[brCrStatus],[brBlStatus],[brSearch],[SdStatus],[brBlStdate],[brLastBlDate], "
                    + " [brAddress1],[brAddress2],[brAreaGroupId],[brAreaGroup],[brAreaId],[brArea],[brPostalArea], "
                    + " [brwebsite],[brstatussla],[brsladate],[brdateinception],[brcompanyname],[branchmanager], "
                    + " [brSupportOfficeId],[brSupportOffice],[contact_det],[Contact_Designation],[phone_no],[fax_no], "
                    + " [email_id],[mrtg_link],[note_for_services],[bw_as_client],[add_for_p2p],[note_for_bts], "
                    + " [cur_status],[final_update_from],[final_update_by],[final_update_date],[i_bill_date],[i_seller], "
                    + " [i_acc_manager],[i_ins_ref_no],[i_ins_date],[i_ins_engg],[i_bill_mgr],[i_terms_cond],[sll], "
                    + " [UpdStatus],[ClientUpdStatus],[sdststemp],[HeadOfficeName],[BranchName],[LockUnlock],[SMSMobileNo], "
                    + " [ClientRefarence],[NewE1POP],[VPNTunnel],[ICBWProvider],[SofDate],[SubscriberType],[Salution], "
                    + " [GenderType],[MiddleName],[LastName],[NickName],[Occupation],[MQStatus],[BillDeliverd],[MqID], "
                    + " [MqActiveInactive],[SubscriberPassword],[SOFCompleteBy],[SOFCompleteDate],[DistributorID], "
                    + " [PackagePlan],[DistributorSubscriberID],[RRPID],[RRPSubscriberID],[SMEPkgID],[DisPackageID], "
                    + " [TimeBased],[SAFNumber],[ProofTypeID],[ProofID_No] FROM [dbo].[clientDatabaseMain] "
                    + " where brCliCode ='" + brClientCode + "' and brSlNo= " + brSerialNumber + " ";
                var getClientDbmaindata = await _misDBContext.ClientDatabaseMainResponseModel.FromSqlRaw(strSql).AsNoTracking().FirstOrDefaultAsync();

                var strsqlTechnical = " SELECT BandwidthType, ClientAddrerssCode, ClientCode, ClientSlNo, DeviceName, "
                    + " IntercityBandwidth, InternetBandwidthCIR, InternetBandwidthMIR, LastmileBandwidth, "
                    + " NoteBandwith, Pvlan, TechInfoID, TrackingInfo, VsatBandwidthDownCir, VsatBandwidthDownMir, "
                    + " VsatBandwidthUpCir, VsatBandwidthUpMir, brDns1, brDns2, brpop3, brsmtp, bw_as_client, "
                    + " f_bts_sw_router, f_con_terminate_to, f_mac, f_media_con, f_media_converter_switch_port, "
                    + " f_note_for_fiber, f_oltchasisNo, f_onu, f_onuNo, f_pon, f_port, f_splitter, f_vlan_id, "
                    + " gate_way, private_gateway, private_ip, private_musk, r_Bandwidth, r_ThroughPutInternetMulti, "
                    + " r_ThroughPutInternetSingle, r_ThroughPutIntranetMulti, r_ThroughPutIntranetSingle, "
                    + " r_air_distance, r_bts_sw_router, r_connected_to, r_equip, r_frequency, r_gateway, "
                    + " r_note_for_radio, r_radio_ip, r_rssi, r_snr, r_subnetmask, r_tower_hi, r_vlan_id, "
                    + " real_ip, sll, [ChangesStatus], subnet_musk, LanPort, ClientGateway, ClientIp, ClientSubnetMusk, "
                    + " OLDBWMIR, OldBWCIR, f_Laser FROM ClientTechnicalInfo "
                    + " WHERE (ClientCode = '" + brClientCode + "') AND (ClientSlNo = " + brSerialNumber + ")";
                var getDataTechnical = await _misDBContext.ClientTechnicalInfos.FromSqlRaw(strsqlTechnical).AsNoTracking().FirstOrDefaultAsync();

                string strSqlEquipment = " SELECT [cli_code], [cli_sl_no], [equipments], [description], [slno], [quantity], "
                    + " [ownership], [TechName], [HandoverDate], [CoordinatorName], [Throuputtest], [Tag], [unique_id]  FROM [dbo].[clientDatabaseEquipment] "
                    + " where  cli_code = '" + brClientCode + "'and cli_sl_no = " + brSerialNumber + " ";
                var getDataEquipment = await _misDBContext.ClientDatabaseEquipments.FromSqlRaw(strSqlEquipment).AsNoTracking().ToListAsync();

                string strSqlItemDetails = " SELECT [brCliCode], [brSlNo], [itm_type], [item_id], [item_desc], [ServiceStatus], [Slno] FROM [dbo].[clientDatabaseItemDet] "
                    + " where brCliCode='" + brClientCode + "' and brSlNo='" + brSerialNumber + "' and itm_type='BTS'";
                var getItemDetails = await _misDBContext.ClientDatabaseItemDets.FromSqlRaw(strSqlItemDetails).AsNoTracking().ToListAsync();

                string strSqlSubSpliter = " SELECT [SpliterID],[BtsID],[OltName],[PON],[Port],[PortCapacity],[SpliterCapacity],"
                    + " [SpliterLocation],[CustomerName],[CableNo],[LinkPath],[Remarks],[EntryUserID], "
                    + " [EntryDate],[UpdateUserID],[UpdateDate],[CustomerCode],[CustomerSl],[Shifted], "
                    + " [EncloserNo],[OLTBrand],[UTPClient] FROM [dbo].[tbl_SubSpliterEntry] "
                    + " WHERE CustomerCode = '" + brClientCode + "' AND CustomerSl = " + brSerialNumber + " AND Shifted = 'No' ";
                var getSubSpliterData = await _misDBContext.tbl_SubSpliterEntrys.FromSqlRaw(strSqlSubSpliter).AsNoTracking().FirstOrDefaultAsync();

                string strSqlDarkFiber = " SELECT [AutoID],[ClientCode],[ClientSl],[ClientName],[LocationName1],[AddressLine1], "
                    + " [AddressLine2],[LocationName2],[p2pAddress1],[p2pAddress2],[NoOfCore],[CoreName],[CablePathType], "
                    + " [LinkDistance],[LinkPath],[MailComplain],[Remarks],[EntryDate],[EntryUserID],[UpdateDate],[UpdateUserID] "
                    + " FROM [dbo].[tbl_darkfiber_client] WHERE (ClientCode = '" + brClientCode + "') AND (ClientSl = " + brSerialNumber + ") ";
                var getDarkFiberData = await _misDBContext.tbl_darkfiber_clients.FromSqlRaw(strSqlDarkFiber).AsNoTracking().ToListAsync();

                string strSqlNttn = " SELECT [AutoID],[SubscriberID],[SlNO],[CableNetworkID],[NTTNNameID],[Typeofp2mlinkID], "
                    + " [CoreName],[SCR_LinkID],[SummitLinkID],[BahonCoreID],[Remarks] FROM [dbo].[tbl_NttnDetails] "
                    + " WHERE (SubscriberID = '" + brClientCode + "') AND (SlNO = " + brSerialNumber + ") ";
                var getNttnData = await _misDBContext.tbl_NttnDetails.FromSqlRaw(strSqlNttn).AsNoTracking().FirstOrDefaultAsync();

                var responseModel = new NetworkUpdateInfoResponseModel()
                {
                    ClientDatabaseMainModel = getClientDbmaindata,
                    ClientTechnicalInfoModel = getDataTechnical,
                    ClientDatabaseEquipmentListModel = getDataEquipment,
                    ClientDatabaseItemDetListModel = getItemDetails,
                    tbl_SubSpliterEntryModel = getSubSpliterData,
                    tbl_Darkfiber_ClientListModel = getDarkFiberData,
                    tbl_NttnDetailsModel = getNttnData
                };

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Data get successfully.",
                    Data = responseModel
                };
                return response;

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }


        }

        public async Task<ApiResponse> GetInstallationCompletionFormData(string brClientCode, int brSerialNumber,
            string userId, string ip, string clientName)
        {
            var methodName = "FieldForceService/GetInstallationCompletionFormData";
            var response = new ApiResponse();

            try
            {
                string strSql = " SELECT [brCliCode],[brSlNo],[brAdrCode],[brAdrNewCode],[brName],[brGroupId],[brGroup], "
                    + " [brMktGrpId],[brMktGroup],[brClientTypeId],[brClientType],[brAddressTypeID],[brAddressTypeName], "
                    + " [brBusinessTypeId],[brBusinessType],[brCategoryId],[brCategory],[brNatureId],[brNature],[brStatus], "
                    + " [brInsStatus],[brCrStatus],[brBlStatus],[brSearch],[SdStatus],[brBlStdate],[brLastBlDate], "
                    + " [brAddress1],[brAddress2],[brAreaGroupId],[brAreaGroup],[brAreaId],[brArea],[brPostalArea], "
                    + " [brwebsite],[brstatussla],[brsladate],[brdateinception],[brcompanyname],[branchmanager], "
                    + " [brSupportOfficeId],[brSupportOffice],[contact_det],[Contact_Designation],[phone_no],[fax_no], "
                    + " [email_id],[mrtg_link],[note_for_services],[bw_as_client],[add_for_p2p],[note_for_bts], "
                    + " [cur_status],[final_update_from],[final_update_by],[final_update_date],[i_bill_date],[i_seller], "
                    + " [i_acc_manager],[i_ins_ref_no],[i_ins_date],[i_ins_engg],[i_bill_mgr],[i_terms_cond],[sll], "
                    + " [UpdStatus],[ClientUpdStatus],[sdststemp],[HeadOfficeName],[BranchName],[LockUnlock],[SMSMobileNo], "
                    + " [ClientRefarence],[NewE1POP],[VPNTunnel],[ICBWProvider],[SofDate],[SubscriberType],[Salution], "
                    + " [GenderType],[MiddleName],[LastName],[NickName],[Occupation],[MQStatus],[BillDeliverd],[MqID], "
                    + " [MqActiveInactive],[SubscriberPassword],[SOFCompleteBy],[SOFCompleteDate],[DistributorID], "
                    + " [PackagePlan],[DistributorSubscriberID],[RRPID],[RRPSubscriberID],[SMEPkgID],[DisPackageID], "
                    + " [TimeBased],[SAFNumber],[ProofTypeID],[ProofID_No] FROM [dbo].[clientDatabaseMain] "
                    + " where brCliCode ='" + brClientCode + "' and brSlNo= " + brSerialNumber + " ";
                var getClientDbmaindata = await _misDBContext.ClientDatabaseMainResponseModel.FromSqlRaw(strSql).AsNoTracking().FirstOrDefaultAsync();
                // + " from clientDatabaseMain where brCliCode = '08.01.001.150253'and brSlNo = '1'

                string strSqlItemDetails = " SELECT [brCliCode], [brSlNo], [itm_type], [item_id], [item_desc], [ServiceStatus], [Slno] FROM [dbo].[clientDatabaseItemDet] "
                    + " where brCliCode='" + brClientCode + "' and brSlNo='" + brSerialNumber + "' and itm_type='SER'";
                var getItemDetails = await _misDBContext.ClientDatabaseItemDets.FromSqlRaw(strSqlItemDetails).AsNoTracking().ToListAsync();

                string strSqlDatabaseMediaSetup = " SELECT ClientDatabaseMediaSetup.MediaName, clientDatabaseMediaDetails.brCliCode, clientDatabaseMediaDetails.brSlNo, "
                    + " clientDatabaseMediaDetails.brCliAdrCode FROM clientDatabaseMediaDetails INNER JOIN ClientDatabaseMediaSetup "
                    + " ON clientDatabaseMediaDetails.MedID = ClientDatabaseMediaSetup.MedID "
                    + " where brCliCode ='" + brClientCode + "' and brSlNo = " + brSerialNumber + " ";
                var getDatabaseMediasetup = await _misDBContext.InstallationCompletionMediaInfos.FromSqlRaw(strSqlDatabaseMediaSetup).AsNoTracking().FirstOrDefaultAsync();

                var strsqlTechnical = " SELECT BandwidthType, ClientAddrerssCode, ClientCode, ClientSlNo, DeviceName, "
                    + " IntercityBandwidth, InternetBandwidthCIR, InternetBandwidthMIR, LastmileBandwidth, "
                    + " NoteBandwith, Pvlan, TechInfoID, TrackingInfo, VsatBandwidthDownCir, VsatBandwidthDownMir, "
                    + " VsatBandwidthUpCir, VsatBandwidthUpMir, brDns1, brDns2, brpop3, brsmtp, bw_as_client, "
                    + " f_bts_sw_router, f_con_terminate_to, f_mac, f_media_con, f_media_converter_switch_port, "
                    + " f_note_for_fiber, f_oltchasisNo, f_onu, f_onuNo, f_pon, f_port, f_splitter, f_vlan_id, "
                    + " gate_way, private_gateway, private_ip, private_musk, r_Bandwidth, r_ThroughPutInternetMulti, "
                    + " r_ThroughPutInternetSingle, r_ThroughPutIntranetMulti, r_ThroughPutIntranetSingle, "
                    + " r_air_distance, r_bts_sw_router, r_connected_to, r_equip, r_frequency, r_gateway, "
                    + " r_note_for_radio, r_radio_ip, r_rssi, r_snr, r_subnetmask, r_tower_hi, r_vlan_id, "
                    + " real_ip, sll, [ChangesStatus], subnet_musk, LanPort, ClientGateway, ClientIp, ClientSubnetMusk, "
                    + " OLDBWMIR, OldBWCIR, f_Laser FROM ClientTechnicalInfo "
                    + " WHERE (ClientCode = '" + brClientCode + "') AND (ClientSlNo = " + brSerialNumber + ")";
                var getTechnicalData = await _misDBContext.ClientTechnicalInfos.FromSqlRaw(strsqlTechnical).AsNoTracking().FirstOrDefaultAsync();

                string strSqlEquipment = " SELECT [cli_code], [cli_sl_no], [equipments], [description], [slno], [quantity], "
                    + " [ownership], [TechName], [HandoverDate], [CoordinatorName], [Throuputtest], [Tag], [unique_id]  FROM [dbo].[clientDatabaseEquipment] "
                    + " where  cli_code = '" + brClientCode + "'and cli_sl_no = " + brSerialNumber + " ";
                var getEquipmentData = await _misDBContext.ClientDatabaseEquipments.FromSqlRaw(strSqlEquipment).AsNoTracking().ToListAsync();

                var responseModel = new InstallationCompletionFormResponseModel()
                {
                    ClientDatabaseMainModel = getClientDbmaindata,
                    ClientDatabaseItemDetListModel = getItemDetails,
                    InstallationCompletionMediaInfoModel = getDatabaseMediasetup,
                    ClientTechnicalInfoModel = getTechnicalData,
                    ClientDatabaseEquipmentListModel = getEquipmentData
                };

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Data get successfully.",
                    Data = responseModel
                };
                return response;



            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }


        }

        public async Task<ApiResponse> GetTicketPriorityListByTicketId(string ip, ClaimsPrincipal user, string ticketId)
        {
            var methodName = "FieldForceService/GetTicketPriorityListByTicketId";
            var response = new ApiResponse();
            try
            {
                string strSqlGetList = " Select * from Cli_Pending "             //  Select Service,PrioritySet,SLNo from Cli_Pending
                                                                                 //+ " where refno='TKI-16Feb23-0287581' and status<>'Complete' \r\n\t\torder by PrioritySet"
                    + " where refno='" + ticketId + "' and status<>'Complete' order by PrioritySet";
                var getData = await _misDBContext.Cli_PendingModel.FromSqlRaw(strSqlGetList).AsNoTracking().ToListAsync();
                if (getData.Count > 0)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully.",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "No data found",
                        Data = null
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateTicketPriorityStatusByTcketId(string userId, string ip, string ticketId,
                    int priorityStatus, int pendingListSlNo, string serviceType)
        {
            var methodName = "FieldForceService/UpdateTicketPriorityStatusByTcketId";
            var response = new ApiResponse();

            try
            {
                string hk = "select distinct Service from view_pendin where userid='" + userId + "' and RefNo='" + ticketId + "' and Status='INI'";
                var getData = await _misDBContext.ViewPendingServiceResponseModel.FromSqlRaw(hk).AsNoTracking().FirstOrDefaultAsync();
                if (getData != null)
                {
                    if (getData.Service == serviceType)
                    {
                        // update Cli_Pending set PrioritySet = '1' where SLNo = '1148916'
                        string strUpdateSql = "update Cli_Pending set  PrioritySet=" + priorityStatus + " where SLNo =" + pendingListSlNo + " ";
                        var result = await _misDBContext.Database.ExecuteSqlRawAsync(strUpdateSql);
                        if (result > 0)
                        {
                            response = new ApiResponse()
                            {
                                Status = "Success",
                                StatusCode = 200,
                                Message = "Data updated successfully",
                                Data = null
                            };
                            return response;
                        }
                        else
                        {
                            response = new ApiResponse()
                            {
                                Status = "Failed",
                                StatusCode = 400,
                                Message = "Update failed.!",
                                Data = null
                            };
                            return response;
                        }


                    }
                    else
                    {
                        response = new ApiResponse()
                        {
                            Status = "Failed",
                            StatusCode = 400,
                            Message = "You can not set the Priority!",
                            Data = null
                        };
                        return response;
                    }
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetMailLogByTicketId(ClaimsPrincipal user, string ip, string ticketId)
        {
            var methodName = "FieldForceService/GetMailLogByTicketId";
            var response = new ApiResponse();
            try
            {
                string strSql = @"select MailBody  from Cli_EmailLog where CTID='" + ticketId + "'";
                var result = await _misDBContext.MailLogModel.FromSqlRaw(strSql).AsNoTracking().FirstOrDefaultAsync();
                if (result != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = result
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetParentData(string ip, ClaimsPrincipal user, string userId, string ticketId)
        {
            var methodName = "FieldForceService/GetParentData";
            var response = new ApiResponse();

            try
            {
                string strSqlAreaGroup = " SELECT AreaGroupID, DistrictID, AreaGroupName, EntryDate, EntryBy, UpdateDate, "
                    + " UpdateBy, DivisionSetup, SupportOfficeID FROM AreaGroup order by AreaGroupName ";
                var resultAreaGroup = await _misDBContext.AreaGroups.FromSqlRaw(strSqlAreaGroup).AsNoTracking().ToListAsync();



                string strSqlArea = " SELECT [AreaID], [AreaGroupID], [AreaName], [EntryDate], [EntryBy], [UpdateDate], [UpdateBy] FROM [dbo].[Area] ";
                var resultAreaData = await _misDBContext.Areas.FromSqlRaw(strSqlArea).ToListAsync();

                string strSqlClientStatus = " SELECT [ClientStatusSlaID], [StatusName]  FROM [dbo].[ClientStatusSla] ";
                var resultClientStatus = await _misDBContext.ClientStatusSlas.FromSqlRaw(strSqlClientStatus).AsNoTracking().ToListAsync();

                string strSqlClientNatureSetup = " SELECT NatureTypeID, NatureTypeName, EntryDate, EntryUserID, ModifiedDate, ModifyingUserID "
                    + " FROM ClientNatureSetup ORDER BY NatureTypeName ";
                var resultClientNatureSetup = await _misDBContext.ClientNatureSetups.FromSqlRaw(strSqlClientNatureSetup).AsNoTracking().ToListAsync();

                string strSqlClientCategorySetup = " SELECT CategoryID, CategoryName, ModifyingUserID, ModifiedDate, EntryDate, EntryUserID, Status "
                    + " FROM  clientCategorySetup  ORDER BY CategoryName ";
                var resultClientCategorySetup = await _misDBContext.ClientCategorySetups.FromSqlRaw(strSqlClientCategorySetup).AsNoTracking().ToListAsync();

                string strSqlSupportOffice = " SELECT SupportOfficeID, SupportOfficeName, EntryDate, EntryUserID, ModifiedDate, ModifyingUserID "
                    + " FROM SupportOffice ORDER BY SupportOfficeName ";
                var resultSupportOffice = await _misDBContext.SupportOffice.FromSqlRaw(strSqlSupportOffice).AsNoTracking().ToListAsync();

                string strSqlClientDatabaseWireSetup = " SELECT WireID, WireName FROM ClientDatabaseWireSetup ORDER BY WireID ";
                var resultClientDatabaseWireSetup = await _misDBContext.ClientDatabaseWireSetup.FromSqlRaw(strSqlClientDatabaseWireSetup).AsNoTracking().ToListAsync();

                string strSqlClientDatabseTechnologySetup = " SELECT ClientDatabseTechnologySetup.* FROM ClientDatabseTechnologySetup ";
                var resultClientDatabseTechnologySetup = await _misDBContext.ClientDatabseTechnologySetup.FromSqlRaw(strSqlClientDatabseTechnologySetup).AsNoTracking().ToListAsync();

                string strSqlClientDatabaseMediaSetup = " SELECT ClientDatabaseMediaSetup.* FROM  ClientDatabaseMediaSetup ";
                var resultClientDatabaseMediaSetup = await _misDBContext.ClientDatabaseMediaSetup.FromSqlRaw(strSqlClientDatabaseMediaSetup).AsNoTracking().ToListAsync();

                string strSqlClientGroupSetup = " SELECT ClientGroupID, ClientGroupName, EntryDate, EntryUserID, ModifiedDate, ModifyingUserID "
                    + " FROM ClientGroupSetup  ORDER BY ClientGroupName";
                var resultClientGroupSetup = await _misDBContext.ClientGroupSetups.FromSqlRaw(strSqlClientGroupSetup).AsNoTracking().ToListAsync();

                string hk = "select distinct Service from view_pendin where userid='" + userId + "' and RefNo='" + ticketId + "' and Status='INI'";
                var getData = await _misDBContext.ViewPendingServiceResponseModel.FromSqlRaw(hk).ToListAsync();

                //string sql = @"select distinct Service from view_pendin where userid='" + Session[StaticData.sessionUserId].ToString() + "' and RefNo='" + lblrefno.Text + "' and Status='INI'";

                string strSqlNTTNSetup = " SELECT NTTNID, NTTNName FROM tbl_BackboneNTTNSetup  ORDER BY NTTNName ";
                var resultNTTNSetup = await _misDBContext.BackboneNTTNSetups.FromSqlRaw(strSqlNTTNSetup).AsNoTracking().ToListAsync();

                string strSqlTypeofp2mlinkSetup = " select Typeofp2mlinkID,Typeofp2mlink from tbl_Typeofp2mlinkSetup ";
                var resultTypeofp2mlinkSetups = await _misDBContext.Typeofp2mlinkSetups.FromSqlRaw(strSqlTypeofp2mlinkSetup).AsNoTracking().ToListAsync();

                string strSqlBackboneCablePathTypeSetup = " SELECT CablePathID, CablePathName FROM tbl_BackboneCablePathTypeSetup ORDER BY CablePathName";
                var resultBackboneCablePathTypeSetup = await _misDBContext.BackboneCablePathTypes.FromSqlRaw(strSqlBackboneCablePathTypeSetup).AsNoTracking().ToListAsync();

                string strBtsSetup = " SELECT BtsSetupID, BtsSetupName, DivisionID, EntryDate, EntryUserID, ModifiedDate, ModifyingUserID, Area, AreaID, BTSStatus, BtsName, NewBtsName "
                    + " FROM BtsSetup ORDER BY BtsSetupName";
                var resultBtsSetup = await _misDBContext.BtsSetups.FromSqlRaw(strBtsSetup).AsNoTracking().ToListAsync();

                string strSqlWebServer = " SELECT SID, ServerID, ServerIP, ServiceProvider, ServerLocation, PurchaseSpace, HostingCost FROM tbl_WebServer ";
                var getWebServerData = await _misDBContext.tbl_WebServers.FromSqlRaw(strSqlWebServer).ToListAsync();

                string strSqlTeam_info = " SELECT Team_id, Team_Name, Team_Desc, Team_Email, Team_IPost, Team_taskclose, Team_TOpen, "
                    + " Team_TForward, Team_InsSolve, Team_ViewAll, Tstatus, Status  FROM tbl_Team_info order by Team_Name ";
                var getTeam_infoData = await _misDBContext.tbl_Team_Infos.FromSqlRaw(strSqlTeam_info).ToListAsync();

                string strSqlBlgCycleSetup = " SELECT BillingCycleID, BillingCycleName, BillingCycleValue FROM blgCycleSetup WHERE (BillingCycleID <> '1') ORDER BY BillingCycleID ";
                var getBlgCycleSetupData = await _misDBContext.BlgCycleSetups.FromSqlRaw(strSqlBlgCycleSetup).AsNoTracking().ToListAsync();



                var responseModel = new MisInstallationTicketParentDataResponseModel()
                {
                    AreaGroupList = resultAreaGroup,
                    AreaList = resultAreaData,
                    ClientStatusSlaList = resultClientStatus,
                    ClientNatureSetupList = resultClientNatureSetup,
                    ClientCategorySetupList = resultClientCategorySetup,
                    SupportOfficeList = resultSupportOffice,
                    ClientDatabaseWireSetupList = resultClientDatabaseWireSetup,
                    ClientDatabseTechnologySetupList = resultClientDatabseTechnologySetup,
                    ClientDatabaseMediaSetupList = resultClientDatabaseMediaSetup,
                    ClientGroupSetupList = resultClientGroupSetup,
                    ViewPendingServiceList = getData,
                    BackboneNTTNSetupList = resultNTTNSetup,
                    Typeofp2mlinkSetupResponseModelList = resultTypeofp2mlinkSetups,
                    BackboneCablePathTypeSetupList = resultBackboneCablePathTypeSetup,
                    BtsSetupList = resultBtsSetup,
                    WebServerList = getWebServerData,
                    Team_InfoList = getTeam_infoData,
                    BlgCycleSetupList = getBlgCycleSetupData
                };

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Data get successfully.",
                    Data = responseModel
                };
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetClientInformationForNewGo(string ip, ClaimsPrincipal user, string subscriberId, int serialNo)
        {
            var methodName = "FieldForceService/GetClientInformationForNewGo";
            var response = new ApiResponse();

            try
            {
                string strSqlClientDatabaseMain = " select * from clientDatabaseMain where brCliCode='" + subscriberId + "' and brSlNo = " + serialNo + "";
                var getDataClientDatabaseMain = await _misDBContext.ClientDatabaseMainResponseModel.FromSqlRaw(strSqlClientDatabaseMain).AsNoTracking().FirstOrDefaultAsync();

                string insSql = " select a.DisPackageID,b.PackageID,a.DistributorID,c.DistributorName,PackagePlan,b.UpdatePackageName,DistributorSubscriberID, "
                    + " ServiceCode,ServiceNarration,OTCAmount,Amount,SubscriberPassword from clientDatabaseMain a "
                    + " inner join tbl_PackagePlan b on a.DisPackageID = b.packageid inner join Kh_DistributorProfile c on c.DistributorID = a.DistributorID "
                    + "     inner join cli_mktpending d on d.Refno = a.i_ins_ref_no     where a.DistributorID <> '' "
                    + " and a.brCliCode = '" + subscriberId + "' and a.brSlNo = " + serialNo + " and a.DistributorID <> '' ";

                var resultIns = await _misDBContext.MisInstallationServicePackageInfos.FromSqlRaw(insSql).AsNoTracking().FirstOrDefaultAsync();

                string insr = " select a.brCliCode,a.i_ins_ref_no,sum(b.Mrc) as MRC,sum(otc) as OTC from clientDatabaseMain a inner join Post_MainIns b "
                    + " on a.i_ins_ref_no=b.RefNO where a.DistributorID <> '' and a.brCliCode = '" + subscriberId + "' group by a.brCliCode,a.i_ins_ref_no ";
                var resultinsr = await _misDBContext.CustomerBillingInfos.FromSqlRaw(insr).AsNoTracking().AsNoTracking().FirstOrDefaultAsync();

                var responseModel = new ClientInformationForNewGoResponseModel()
                {
                    clientDatabaseMainResponseModel = getDataClientDatabaseMain,
                    misInstallationServicePackageInfo = resultIns,
                    customerBillingInfo = resultinsr
                };

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Data get successfully.",
                    Data = responseModel
                };
                return response;

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetP2PAddressForNewGo(string ip, ClaimsPrincipal user, string subscriberId)
        {
            var methodName = "FieldForceService/GetP2PAddressForNewGo";
            var response = new ApiResponse();

            try
            {
                string strSqlP2PAddress = " SELECT brCliCode, brSlNo, brAdrCode, brAdrNewCode, brName, brAddress1, brAddress2, brAreaGroupId, brAreaGroup, "
                    + " brAreaId, brArea, contact_det, Contact_Designation, phone_no, fax_no, email_id, add_for_p2p, sll "
                    + " FROM clientDatabaseP2PAddress  WHERE (brCliCode = '" + subscriberId + "') AND (brSlNo = 2) ";
                var getP2PaddressData = await _misDBContext.ClientDatabaseP2PAddresses.FromSqlRaw(strSqlP2PAddress).AsNoTracking().FirstOrDefaultAsync();

                if (getP2PaddressData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getP2PaddressData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }


            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetPostMainInstallationDataByServiceId(string ticketId, int serviceId, string ip)
        {
            var methodName = "FieldForceService/GetPostMainInstallationDataByServiceId";
            var response = new ApiResponse();
            try
            {
                string strSqlPost_MainIns = " SELECT Post_MainIns.* FROM Post_MainIns WHERE (RefNO = '" + ticketId + "') ";
                var getPost_MainInsdata = await _misDBContext.Post_MainIns.FromSqlRaw(strSqlPost_MainIns).AsNoTracking().FirstOrDefaultAsync();
                if (getPost_MainInsdata != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getPost_MainInsdata
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllServiceDataForNewGo(string ip, ClaimsPrincipal user, string ticketId, string serviceId,
            string brClientCode, int brSerialNumber, int btsId)
        {
            var methodName = "FieldForceService/GetAllServiceDataForNewGo";
            var response = new ApiResponse();
            try
            {
                string strSqlPost_MainIns = " SELECT Post_MainIns.* FROM Post_MainIns WHERE (RefNO = '" + ticketId + "') ";
                var getPost_MainInsdata = await _misDBContext.Post_MainIns.FromSqlRaw(strSqlPost_MainIns).AsNoTracking().FirstOrDefaultAsync();

                //if (serviceId == 1 || serviceId == 2 || serviceId == 22 || serviceId == 13)
                //{

                string strSqlItemDetails = " SELECT [brCliCode], [brSlNo], [itm_type], [item_id], [item_desc], [ServiceStatus], [Slno] FROM [dbo].[clientDatabaseItemDet] "
                + " where brCliCode='" + brClientCode + "' and brSlNo='" + brSerialNumber + "' and itm_type='BTS'";
                var getItemDetailsData = await _misDBContext.ClientDatabaseItemDets.FromSqlRaw(strSqlItemDetails).AsNoTracking().ToListAsync();

                string strSqlODFNameSetUp = " SELECT BtsID, CableID, EntryDate, EntryUserID, IncrementID, ODFID, ODFName, Remarks, "
                    + " UpdateDate, UpdateUserID FROM tbl_ODFNameSetUp WHERE (BtsID = '" + btsId + "') ";
                var getTbl_ODFNameSetupData = await _misDBContext.tbl_ODFNameSetups.FromSqlRaw(strSqlODFNameSetUp).AsNoTracking().FirstOrDefaultAsync();

                var strsqlTechnical = " SELECT BandwidthType, ClientAddrerssCode, ClientCode, ClientSlNo, DeviceName, "
                + " IntercityBandwidth, InternetBandwidthCIR, InternetBandwidthMIR, LastmileBandwidth, "
                + " NoteBandwith, Pvlan, TechInfoID, TrackingInfo, VsatBandwidthDownCir, VsatBandwidthDownMir, "
                + " VsatBandwidthUpCir, VsatBandwidthUpMir, brDns1, brDns2, brpop3, brsmtp, bw_as_client, "
                + " f_bts_sw_router, f_con_terminate_to, f_mac, f_media_con, f_media_converter_switch_port, "
                + " f_note_for_fiber, f_oltchasisNo, f_onu, f_onuNo, f_pon, f_port, f_splitter, f_vlan_id, "
                + " gate_way, private_gateway, private_ip, private_musk, r_Bandwidth, r_ThroughPutInternetMulti, "
                + " r_ThroughPutInternetSingle, r_ThroughPutIntranetMulti, r_ThroughPutIntranetSingle, "
                + " r_air_distance, r_bts_sw_router, r_connected_to, r_equip, r_frequency, r_gateway, "
                + " r_note_for_radio, r_radio_ip, r_rssi, r_snr, r_subnetmask, r_tower_hi, r_vlan_id, "
                + " real_ip, sll, [ChangesStatus], subnet_musk, LanPort, ClientGateway, ClientIp, ClientSubnetMusk, "
                + " OLDBWMIR, OldBWCIR, f_Laser FROM ClientTechnicalInfo "
                + " WHERE (ClientCode = '" + brClientCode + "') AND (ClientSlNo = " + brSerialNumber + ")";
                var getTechnicalData = await _misDBContext.ClientTechnicalInfos.FromSqlRaw(strsqlTechnical).AsNoTracking().FirstOrDefaultAsync();

                string strGetSql = " SELECT MedID, TechID, Tempd, WireID, brCliAdrCode, brCliCode, brSlNo "
                + " FROM clientDatabaseMediaDetails WHERE (brCliCode = '" + brClientCode + "') "
                + " AND (brSlNo = " + Convert.ToInt32(brSerialNumber) + ") ";
                var getClientDatabaseMediaDetailsData = await _misDBContext.ClientDatabaseMediaDetails.FromSqlRaw(strGetSql).AsNoTracking().ToListAsync();

                var responseModel = new AllServiceDataForNewGoResponseModel()
                {
                    Post_MainIns = getPost_MainInsdata,
                    ClientDatabaseItemDetList = getItemDetailsData,
                    tbl_ODFNameSetUp = getTbl_ODFNameSetupData,
                    ClientTechnicalInfo = getTechnicalData,
                    ClientDatabaseMediaDetailsList = getClientDatabaseMediaDetailsData
                };

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Data get successfully.",
                    Data = responseModel
                };
                return response;

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetServiceWisePermissionInfo(string ip, string userId, string ticketId)
        {
            var methodName = "FieldForceService/GetServiceWisePermissionInfo";
            var response = new ApiResponse();
            try
            {
                string strSqlTeam_mem_permission = " SELECT dbo.Cli_Pending.RefNo, dbo.Cli_Pending.Service, dbo.Cli_Pending.Status, dbo.tbl_team_mem_permission.Emp_id, "
                    + " dbo.tbl_team_mem_permission.Assign_emp FROM Cli_Pending INNER JOIN dbo.tbl_team_mem_permission "
                    + " ON dbo.Cli_Pending.Service = dbo.tbl_team_mem_permission.Team_Name "
                    //+ " where emp_id = 'L3T2098' and RefNo='TKI-28Feb23-2746955'
                    + " where emp_id = '" + userId + "' and RefNo = '" + ticketId + "' ";
                var getTeamMemberPermissionData = await _misDBContext.ServiceWisePermissions.FromSqlRaw(strSqlTeam_mem_permission).AsNoTracking().FirstOrDefaultAsync();
                if (getTeamMemberPermissionData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getTeamMemberPermissionData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetEmployeeTicketPriorityInfo(string ip, string userId, string ticketId)
        {
            var methodName = "FieldForceService/GetEmployeeTicketPriorityInfo";
            var response = new ApiResponse();
            try
            {
                string strSql = " SELECT RefNo, Service, Status, PrioritySet, Emp_id FROM ViewPririty "
                    + " where RefNo = '" + ticketId + "' and Emp_id = '" + userId + "' ";
                var getData = await _misDBContext.EmployeeTicketPrioritys.FromSqlRaw(strSql).AsNoTracking().FirstOrDefaultAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetConnectivityTrayList(string ip, ClaimsPrincipal user)
        {
            var methodName = "FieldForceService/GetConnectivityTrayList";
            var response = new ApiResponse();
            try
            {
                string strSql = @"SELECT  TrayID FROM tbl_ODFDetailsEntry group by TrayID ";
                var getData = await _misDBContext.ODFDetailsTrayIds.FromSqlRaw(strSql).AsNoTracking().ToListAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }
            }

            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetConnectivityPortList(string ip, ClaimsPrincipal user)
        {
            var methodName = "FieldForceService/GetConnectivityPortList";
            var response = new ApiResponse();
            try
            {
                string strSql = @"SELECT PortID FROM tbl_ODFDetailsEntry group by PortID";
                var getData = await _misDBContext.ODFDetailsPortIds.FromSqlRaw(strSql).AsNoTracking().ToListAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetColorRelatedDetailsData(string ip, ClaimsPrincipal user, int brSerialNumber, string brClientCode)
        {
            var methodName = "FieldForceService/GetColorRelatedDetailsData";
            var response = new ApiResponse();
            var model = new VwSplitColorNewResponseModel();
            try
            {
                string strSqlVwSplitColor = " SELECT View_SpltColorInfo.* FROM View_SpltColorInfo WHERE (CustomerID = '" + brClientCode + "') "
                    + " AND (CustomerSl = " + brSerialNumber + ") and Shifted='No' ";
                //+ " AND (CustomerSl = " + brSerialNumber + ") and Shifted='No' ORDER BY Position";
                var getVwSplitColorData = await _misDBContext.VwSplitColors.FromSqlRaw(strSqlVwSplitColor).AsNoTracking().FirstOrDefaultAsync();

                string rm = "";
                //if (getVwSplitColorData.Remarks != "")
                if (getVwSplitColorData == null)
                {
                    //rm = "; {RM:" + getVwSplitColorData.Remarks + "}";
                }
                else
                {
                    rm = "";
                    model = new VwSplitColorNewResponseModel()
                    {
                        TubeColorName = getVwSplitColorData.TubeColorName,
                        CoreColorName = getVwSplitColorData.CoreColorName,
                        autoid = getVwSplitColorData.autoid,
                        BtsID = getVwSplitColorData.BtsID,
                        OltName = getVwSplitColorData.OltName,
                        PON = getVwSplitColorData.PON,
                        Port = getVwSplitColorData.Port,
                        SplitterName = getVwSplitColorData.SplitterName,
                        CustomerID = getVwSplitColorData.CustomerID,
                        StartPoint = getVwSplitColorData.StartPoint,
                        CableType = getVwSplitColorData.CableType,
                        TubeColor = getVwSplitColorData.TubeColor,
                        CoreColor = getVwSplitColorData.CoreColor,
                        CableID = getVwSplitColorData.CableID,
                        StartMeter = getVwSplitColorData.StartMeter,
                        EndMeter = getVwSplitColorData.EndMeter,
                        Length = getVwSplitColorData.Length,
                        EndPoint = getVwSplitColorData.EndPoint,
                        Remarks = getVwSplitColorData.Remarks,
                        EntryUserID = getVwSplitColorData.EntryUserID,
                        EntryDate = getVwSplitColorData.EntryDate,
                        UpdateUserID = getVwSplitColorData.UpdateUserID,
                        UpdateDate = getVwSplitColorData.UpdateDate,
                        Position = getVwSplitColorData.Position,
                        CustomerSl = getVwSplitColorData.CustomerSl,
                        Shifted = getVwSplitColorData.Shifted
                    };

                    model.JoinColorRemarks = "[{SP: " + getVwSplitColorData.StartPoint + "};{Cable " + getVwSplitColorData.CableType + " (TC:" +
                        getVwSplitColorData.TubeColorName + ";CC:" + getVwSplitColorData.CoreColorName + ")};{CID:" +
                        getVwSplitColorData.CableID + ")" + "(SM:" + getVwSplitColorData.StartMeter + "- EM:" + getVwSplitColorData.EndMeter + " = " +
                        getVwSplitColorData.Length + "m)};" + " {EP:" + getVwSplitColorData.EndPoint + "}" +
                        rm + "]+";
                    //model = modelNew;
                }



                // ------------------already implemented above--------
                //string strSqlSubSpliter = " SELECT [SpliterID],[BtsID],[OltName],[PON],[Port],[PortCapacity],[SpliterCapacity],"
                //    + " [SpliterLocation],[CustomerName],[CableNo],[LinkPath],[Remarks],[EntryUserID], "
                //    + " [EntryDate],[UpdateUserID],[UpdateDate],[CustomerCode],[CustomerSl],[Shifted], "
                //    + " [EncloserNo],[OLTBrand],[UTPClient] FROM [dbo].[tbl_SubSpliterEntry] "
                //    + " WHERE CustomerCode = '" + brClientCode + "' AND CustomerSl = " + brSerialNumber + " AND Shifted = 'No' ";
                //var getSubSpliterData = await _misDBContext.tbl_SubSpliterEntrys.FromSqlRaw(strSqlSubSpliter).AsNoTracking().FirstOrDefaultAsync();

                //var strsqlTechnical = " SELECT BandwidthType, ClientAddrerssCode, ClientCode, ClientSlNo, DeviceName, "
                //    + " IntercityBandwidth, InternetBandwidthCIR, InternetBandwidthMIR, LastmileBandwidth, "
                //    + " NoteBandwith, Pvlan, TechInfoID, TrackingInfo, VsatBandwidthDownCir, VsatBandwidthDownMir, "
                //    + " VsatBandwidthUpCir, VsatBandwidthUpMir, brDns1, brDns2, brpop3, brsmtp, bw_as_client, "
                //    + " f_bts_sw_router, f_con_terminate_to, f_mac, f_media_con, f_media_converter_switch_port, "
                //    + " f_note_for_fiber, f_oltchasisNo, f_onu, f_onuNo, f_pon, f_port, f_splitter, f_vlan_id, "
                //    + " gate_way, private_gateway, private_ip, private_musk, r_Bandwidth, r_ThroughPutInternetMulti, "
                //    + " r_ThroughPutInternetSingle, r_ThroughPutIntranetMulti, r_ThroughPutIntranetSingle, "
                //    + " r_air_distance, r_bts_sw_router, r_connected_to, r_equip, r_frequency, r_gateway, "
                //    + " r_note_for_radio, r_radio_ip, r_rssi, r_snr, r_subnetmask, r_tower_hi, r_vlan_id, "
                //    + " real_ip, sll, [ChangesStatus], subnet_musk, LanPort, ClientGateway, ClientIp, ClientSubnetMusk, "
                //    + " OLDBWMIR, OldBWCIR, f_Laser FROM ClientTechnicalInfo "
                //    + " WHERE (ClientCode = '" + brClientCode + "') AND (ClientSlNo = " + brSerialNumber + ")";
                //var getDataTechnical = await _misDBContext.ClientTechnicalInfos.FromSqlRaw(strsqlTechnical).AsNoTracking().FirstOrDefaultAsync();

                if (getVwSplitColorData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = model
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
            //return response;
        }

        public async Task<ApiResponse> GetP2PFiberDetailsData(string ip, ClaimsPrincipal user, int brSerialNumber, string brClientCode)
        {
            var methodName = "FieldForceService/GetP2PFiberDetailsData";
            var response = new ApiResponse();

            try
            {
                string strSqlODFDetailsEntry = " SELECT Address, AssignEng, AssignEngDate, BackboneID, BtsID, CablePathTypeID, ClientBackBoneName, ClientCode, "
                    + " ClientSl, ColorID, EntryDate, EntryUserID, FiberSCRID, GooglePath, IncrementID, JoinColor, NTTNNameID, "
                    + " ODFFrom, ODFNameID, ODFTo, PortID, Remarks, Status, SummitLinkID, TrayID, TubeID, UpdateDate, UpdateUserID, BahonCoreID "
                    + " FROM tbl_ODFDetailsEntry   WHERE (ClientCode = '" + brClientCode + "') AND (ClientSl = " + brSerialNumber + ")";
                var getODFDetailsEntryData = await _misDBContext.tbl_ODFDetailsEntrys.FromSqlRaw(strSqlODFDetailsEntry).AsNoTracking().FirstOrDefaultAsync();

                //if (getODFDetailsEntryData != null)
                //{
                //    string strSqlView_JoinColor = " SELECT View_JoinColor.* FROM View_JoinColor WHERE (AutoODFID = " + getODFDetailsEntryData.IncrementID + ") ORDER BY Position ";
                //    var getDataView_JoinColor = await _misDBContext.View_JoinColors.FromSqlRaw(strSqlView_JoinColor).AsNoTracking().FirstOrDefaultAsync();
                //}

                if (getODFDetailsEntryData != null)
                {
                    string strSqlView_JoinColor = " SELECT View_JoinColor.* FROM View_JoinColor WHERE (AutoODFID = " + getODFDetailsEntryData.IncrementID + ") ORDER BY Position ";
                    var getDataView_JoinColor = await _misDBContext.View_JoinColors.FromSqlRaw(strSqlView_JoinColor).AsNoTracking().FirstOrDefaultAsync();
                    string rm = "";
                    if (getDataView_JoinColor.Remarks != "")
                    {
                        rm = ";{RM:" + getDataView_JoinColor.Remarks + "}";
                    }
                    else rm = "";
                    getDataView_JoinColor.JoinColorRemarks = "[{SP: " + getDataView_JoinColor.StartPoint + "};{Cable " + getDataView_JoinColor.CableType + " (TC:" +
                                getDataView_JoinColor.TubeColorName + ";CC:" + getDataView_JoinColor.CoreColorName + ")};{CID:" +
                                getDataView_JoinColor.CableID + ")" + "(SM:" + getDataView_JoinColor.StartMeter + "- EM:" + getDataView_JoinColor.EndMeter + " = " +
                                getDataView_JoinColor.Length + "m)};" + " {EP:" + getDataView_JoinColor.EndPoint + "};{join @(" +
                                getDataView_JoinColor.CableJoin + "(No: " + getDataView_JoinColor.JoinNo + "{Cable No: " + getDataView_JoinColor.CableNo + "}" +
                                rm + "]+";

                    P2PFiberDetailsResponseModel responseModel = new P2PFiberDetailsResponseModel()
                    {
                        ODFDetailsEntry = getODFDetailsEntryData,
                        View_JoinColors = getDataView_JoinColor
                    };

                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = responseModel
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }



            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

            //return response;
        }

        public async Task<ApiResponse> GetDarkClientInformation(string ip, ClaimsPrincipal user, int brSerialNumber, string brClientCode)
        {
            var methodName = "FieldForceService/GetDarkClientInformation";
            var response = new ApiResponse();
            try
            {
                string strSqlDarkFiber_Client = " SELECT tbl_darkfiber_client.* FROM tbl_darkfiber_client "
                    + " WHERE (ClientCode = '" + brSerialNumber + "') AND (ClientSl = " + brClientCode + ") ";
                var getDarkFiber_ClientData = await _misDBContext.tbl_darkfiber_clients.FromSqlRaw(strSqlDarkFiber_Client).FirstOrDefaultAsync();
                if (getDarkFiber_ClientData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getDarkFiber_ClientData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetDarkFiberClientColorInformation(string ip, ClaimsPrincipal user, int brSerialNumber,
                    string brClientCode, int noOfCore)
        {
            var methodName = "FieldForceService/GetDarkFiberClientColorInformation";
            var response = new ApiResponse();
            var viewDClient_ColorList = new List<View_DarkClient_ColorViewModel>();
            var mainList = new List<DarkFiberCoreViewModel>();


            try
            {
                string strSqlDarkFiber_Core = " SELECT coreid, noofcore, corename FROM tbl_darkfiber_core WHERE (noofcore = " + noOfCore + ") ";
                var getSqlDarkFiber_Core = await _misDBContext.Tbl_DarkFiber_Cores.FromSqlRaw(strSqlDarkFiber_Core).AsNoTracking().ToListAsync();
                if (getSqlDarkFiber_Core != null)
                {
                    foreach (var model in getSqlDarkFiber_Core)
                    {

                        string strSql = " SELECT autoid, CoreName, ClientCode, ClientSl, StartPoint, CableType, TubeColor, CoreColor, "
                            + " CableID, StartMeter, EndMeter, Length, EndPoint, CableJoin, Remarks,  EntryUserID, EntryDate, UpdateUserID, "
                            + " UpdateDate, Position, TubeColorName, CoreColorName FROM View_DarkClient_Color "
                            + " WHERE (ClientCode = '" + brClientCode + "') AND (ClientSl = " + brSerialNumber + ") "
                            + " AND (CoreName = '" + model.corename + "')  ";
                        var getData = await _misDBContext.View_DarkClient_Colors.FromSqlRaw(strSql).AsNoTracking().FirstOrDefaultAsync();

                        if (getData != null)
                        {
                            View_DarkClient_ColorViewModel getView_DarkClient_ColorViewModel = new View_DarkClient_ColorViewModel()
                            {
                                autoid = getData.autoid,
                                CoreName = getData.CoreName,
                                ClientCode = getData.ClientCode,
                                ClientSl = getData.ClientSl,
                                StartPoint = getData.StartPoint,
                                CableType = getData.CableType,
                                TubeColor = getData.TubeColor,
                                CoreColor = getData.CoreColor,
                                CableID = getData.CableID,
                                StartMeter = getData.StartMeter,
                                EndMeter = getData.EndMeter,
                                Length = getData.Length,
                                EndPoint = getData.EndPoint,
                                CableJoin = getData.CableJoin,
                                Remarks = getData.Remarks,
                                EntryUserID = getData.EntryUserID,
                                EntryDate = getData.EntryDate,
                                UpdateUserID = getData.UpdateUserID,
                                UpdateDate = getData.UpdateDate,
                                Position = getData.Position,
                                TubeColorName = getData.TubeColorName,
                                CoreColorName = getData.CoreColorName,
                                ColorRemarks = ""
                            };

                            string rm = "";
                            if (getView_DarkClient_ColorViewModel.Remarks != "")
                            {
                                rm = ";{RM:" + getView_DarkClient_ColorViewModel.Remarks + "}";
                            }
                            else rm = "";

                            getView_DarkClient_ColorViewModel.ColorRemarks = "[{SP: " + getData.StartPoint + "};{Cable " + getData.CableType + " (TC:" +
                                        getData.TubeColorName + ";CC:" + getData.CoreColorName + ")};{CID:" +
                                        getData.CableID + ")" + "(SM:" + getData.StartMeter + "- EM:" + getData.EndMeter + " = " +
                                        getData.Length + "m)};" + " {EP:" + getData.EndPoint + "};{join @(" +
                                        getData.CableJoin + ")}" + rm + "]+";
                            viewDClient_ColorList.Add(getView_DarkClient_ColorViewModel);
                        }

                        var aa = new DarkFiberCoreViewModel()
                        {
                            coreid = model.coreid,
                            noofcore = model.noofcore,
                            corename = model.corename,
                            View_DarkClient_ColorsList = viewDClient_ColorList
                        };

                        mainList.Add(aa);
                    }
                }
                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Data get successfully",
                    Data = mainList
                };
                return response;

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetSMSDataForNewGo(string ip, ClaimsPrincipal user, string ticketId)
        {
            var methodName = "FieldForceService/GetSMSDataForNewGo";
            var response = new ApiResponse();
            try
            {
                string strSql = "select * from Post_MainIns where RefNO='" + ticketId + "' and ServiceID='18' ";
                var getData = await _misDBContext.Post_MainIns.FromSqlRaw(strSql).AsNoTracking().ToListAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetPaymentDataForNewGo(string ip, ClaimsPrincipal user, string ticketId)
        {
            var methodName = "FieldForceService/GetPaymentDataForNewGo";
            var response = new ApiResponse();
            try
            {
                string strSql = "select * from Post_MainIns where RefNO='" + ticketId + "' and ServiceID = 19 ";
                var getData = await _misDBContext.Post_MainIns.FromSqlRaw(strSql).AsNoTracking().ToListAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetIpTvInfoForNewGo(string ip, ClaimsPrincipal user, string brClientCode)
        {
            var methodName = "FieldForceService/GetIpTvInfoForNewGo";
            var response = new ApiResponse();
            try
            {
                string strSql = "select * from tbl_IPTV_Subscriber a inner join tbl_Iptv_Package_Setup b on a.PackageID=b.PairConaxID where a.MISID = '" + brClientCode + "' ";
                var getData = await _misDBContext.IPTVs.FromSqlRaw(strSql).AsNoTracking().ToListAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetPAcakgeNameInfoForNewGo(string ip, ClaimsPrincipal user, int brSerialNumber, string brClientCode)
        {
            var methodName = "FieldForceService/GetPAcakgeNameInfoForNewGo";
            var response = new ApiResponse();
            try
            {
                string qry = @"select * from ClientTechnicalInfo a  inner join tbl_PackagePlan b on a.NoteBandwith=b.PackageName where ClientCode='" + brClientCode + "' and ClientSlNo='" + brSerialNumber + "'";
                var getData = await _misDBContext.PacakgeNameInfos.FromSqlRaw(qry).AsNoTracking().FirstOrDefaultAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetHost_IPInfoForNewGo(string ip, ClaimsPrincipal user, int brSerialNumber, string brClientCode)
        {
            var methodName = "FieldForceService/GetHost_IPInfoForNewGo";
            var response = new ApiResponse();
            try
            {
                string strSql = " select distinct ID,IPAddress,Remarks, HostName from ClientTechnicalInfo a inner join Kh_IpAddress b on "
                    + " a.real_ip=b.IPAddress and a.gate_way=b.Gateway and a.subnet_musk=b.SubnetMask "
                    + " where ClientCode='" + brClientCode + "'and ClientSlNo = " + brSerialNumber + " ";
                var getData = await _misDBContext.Host_IPInfos.FromSqlRaw(strSql).AsNoTracking().FirstOrDefaultAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetKh_IpAddressByHostNameForNewGo(string ip, ClaimsPrincipal user, string hostName)
        {
            var methodName = "FieldForceService/GetKh_IpAddressByHostNameForNewGo";
            var response = new ApiResponse();

            try
            {
                string strSql = " select ID,IPAddress,Remarks, HostName from Kh_IpAddress where HostName='" + hostName + "' and UsedStatus='N' order by ID ";
                var getData = await _misDBContext.Kh_IpAddress.FromSqlRaw(strSql).AsNoTracking().ToListAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetColorInfo(string ip, ClaimsPrincipal user)
        {
            var methodName = "FieldForceService/GetColorInfo";
            var response = new ApiResponse();
            try
            {
                string strSql = " SELECT ColorID, ColorName FROM tbl_ColorInfo ORDER BY ColorName ";
                var getData = await _misDBContext.tbl_Colors.FromSqlRaw(strSql).ToListAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }



        public async Task<ApiResponse> GetCrUploadFile(string ip, ClaimsPrincipal user, int brSerialNumber, string brClientCode)
        {
            var methodName = "FieldForceService/GetCrUploadFile";
            var response = new ApiResponse();
            try
            {
                string strSql = " SELECT View_SpltColorInfo.* FROM View_SpltColorInfo WHERE(CustomerID = '" + brClientCode + "') "
                    + " AND(CustomerSl = " + brSerialNumber + ") and Shifted = 'No' ORDER BY Position ";
                var getData = await _misDBContext.VwSplitColors.FromSqlRaw(strSql).AsNoTracking().FirstOrDefaultAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> AddToODF_JoincolorEntryCommand(string ip, ClaimsPrincipal user, tbl_Splitter_JoincolorEntryRequestModel model)
        {
            var methodName = "FieldForceService/AddToODF_JoincolorEntryCommand";
            var response = new ApiResponse();
            try
            {
                //string strSql = " SELECT autoid, BtsID, OdfID, Tray, Port, StartPoint, CableType, TubeColor, CoreColor, CableID, StartMeter, "
                //    + " EndMeter, Length, EndPoint, CableJoin, JoinNo, CableNo, Remarks, EntryUserID, EntryDate, UpdateUserID, "
                //    + " UpdateDate, Position, AutoODFID  FROM tbl_ODF_JoincolorEntry  WHERE (AutoODFID = " + model.AutoODFID + ") "
                //    + " AND (Position = " + model.Position + ") ";

                string strSql = " SELECT tbl_Splitter_JoincolorEntry.* FROM tbl_Splitter_JoincolorEntry "
                    + " WHERE(SplitterName = '" + model.SplitterName + "') AND(CustomerID = '" + model.CustomerID + "') "
                    + " AND(CustomerSl = " + model.CustomerSl + ") AND(Position = " + model.Position + ") ";

                var getData = await _misDBContext.tbl_Splitter_JoincolorEntrys.FromSqlRaw(strSql).AsNoTracking().FirstOrDefaultAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 409,
                        Message = "Data already Exist",
                        Data = getData
                    };
                    return response;
                }

                //int sm = Convert.ToInt32(txtstartmeter.Text);
                //int em = Convert.ToInt32(txtendmeter.Text);

                int sm = Convert.ToInt32(model.StartMeter);
                int em = Convert.ToInt32(model.EndMeter);
                int ln = 0;
                if (sm > em)
                {
                    ln = sm - em;
                }
                else ln = em - sm;

                //jc.InsertQuery(Convert.ToInt32(lblbts.Text), Convert.ToInt32(lblodf.Text), Convert.ToInt32(txttray.Text),
                //Convert.ToInt32(txtport.Text), txtstartpoint.Text, ddlcabletype.SelectedItem.Text,
                //Convert.ToInt32(ddltubecolor.SelectedValue), Convert.ToInt32(ddlcorecolor.SelectedValue), txtcableid.Text,
                //txtstartmeter.Text, txtendmeter.Text, ln.ToString(), txtendpoint.Text, ddlcablejoin.SelectedItem.Text, txtno.Text,
                //txtcablenumber.Text, txtRemarks.Text, Session[StaticData.sessionUserId].ToString(), DateTime.Now.Date,
                //Session[StaticData.sessionUserId].ToString(), DateTime.Now.Date, Convert.ToInt32(txtPosition.Text),
                //Convert.ToInt32(lblautoodfid.Text));

                string sqlInsert = " INSERT INTO [tbl_Splitter_JoincolorEntry] ([BtsID], [OltName], [PON], [Port], [SplitterName], "
                    + " [CustomerID], [StartPoint], [CableType], [TubeColor], [CoreColor], [CableID], [StartMeter], [EndMeter], [Length], "
                    + " [EndPoint], [Remarks], [EntryUserID], [EntryDate], [UpdateUserID], [UpdateDate], [Position], [CustomerSl]) "
                    + " VALUES (" + model.BtsID + ", '" + model.OltName + "', " + model.PON + ", " + model.Port + ", '" + model.SplitterName
                    + "', '" + model.CustomerID + "', '" + model.StartPoint + "', '" + model.CableType + "', " + model.TubeColor
                    + ", " + model.CoreColor + ", '" + model.CableID + "', '" + model.StartMeter + "', '" + model.EndMeter + "', " + ln
                    + ", '" + model.EndPoint + "', '" + model.Remarks + "', '" + model.EntryUserID + "', '" + model.EntryDate + "', '" + model.UpdateUserID
                    + "', '" + model.UpdateDate + "', " + model.Position + ", " + model.CustomerSl + ") ";

                int item = await _misDBContext.Database.ExecuteSqlRawAsync(sqlInsert);
                if (item > 0)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Insert process successfully completed.",
                        Data = null
                    };
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "Insert process failed!",
                        Data = null
                    };
                }
                return response;

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetPAcakgeNameInfoForNewGo(string ip, ClaimsPrincipal user, int autoODFID)
        {
            var methodName = "FieldForceService/GetPAcakgeNameInfoForNewGo";
            var response = new ApiResponse();
            try
            {
                string strSql = " SELECT View_JoinColor.* FROM View_JoinColor WHERE (AutoODFID = " + autoODFID + ") ORDER BY Position ";
                var getData = await _misDBContext.View_JoinColors.FromSqlRaw(strSql).AsNoTracking().FirstOrDefaultAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data get successfully",
                        Data = getData
                    };
                    return response;
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "No data found!",
                        Data = null
                    };
                    return response;
                }

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> AddTotbl_ODF_JoincolorEntry(string ip, ClaimsPrincipal user, tbl_ODF_JoincolorEntryRequestModel model)
        {
            var methodName = "FieldForceService/AddTotbl_ODF_JoincolorEntry";
            var response = new ApiResponse();
            try
            {
                string strsql = " SELECT autoid, BtsID, OdfID, Tray, Port, StartPoint, CableType, TubeColor, CoreColor, CableID, StartMeter, "
                    + " EndMeter, Length, EndPoint, CableJoin, JoinNo, CableNo, Remarks, EntryUserID, EntryDate, UpdateUserID, "
                    + " UpdateDate, Position, AutoODFID  FROM tbl_ODF_JoincolorEntry "
                    + " WHERE (AutoODFID = " + model.AutoODFID + ") AND (Position = " + model.Position + ") ";
                var getData = await _misDBContext.tbl_ODF_JoincolorEntries.FromSqlRaw(strsql).AsNoTracking().FirstOrDefaultAsync();
                if (getData != null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 409,
                        Message = "Data already Exist",
                        Data = getData
                    };
                    return response;
                }

                int sm = Convert.ToInt32(model.StartMeter);
                int em = Convert.ToInt32(model.EndMeter);
                int ln = 0;
                if (sm > em)
                {
                    ln = sm - em;
                }
                else ln = em - sm;

                //string sqlInsert = " INSERT INTO [tbl_Splitter_JoincolorEntry] ([BtsID], [OltName], [PON], [Port], [SplitterName], "
                //    + " [CustomerID], [StartPoint], [CableType], [TubeColor], [CoreColor], [CableID], [StartMeter], [EndMeter], [Length], "
                //    + " [EndPoint], [Remarks], [EntryUserID], [EntryDate], [UpdateUserID], [UpdateDate], [Position], [CustomerSl]) "
                //    + " VALUES (" + model.BtsID + ", '" + model.OltName + "', " + model.PON + ", " + model.Port + ", '" + model.SplitterName
                //    + "', '" + model.CustomerID + "', '" + model.StartPoint + "', '" + model.CableType + "', " + model.TubeColor
                //    + ", " + model.CoreColor + ", '" + model.CableID + "', '" + model.StartMeter + "', '" + model.EndMeter + "', " + model.Length
                //    + ", '" + model.EndPoint + "', '" + model.Remarks + "', '" + model.EntryUserID + "', '" + model.EntryDate + "', '" + model.UpdateUserID
                //    + "', '" + model.UpdateDate + "', " + model.Position + ", " + model.CustomerSl + ") ";

                string sqlInsert = " INSERT INTO [tbl_ODF_JoincolorEntry] ([BtsID], [OdfID], [Tray], [Port], [StartPoint], [CableType], [TubeColor], "
                    + " [CoreColor], [CableID], [StartMeter], [EndMeter], [Length], [EndPoint], [CableJoin], [JoinNo], [CableNo], [Remarks], "
                    + " [EntryUserID], [EntryDate], [UpdateUserID], [UpdateDate], [Position], [AutoODFID]) "
                    + " VALUES (" + model.BtsID + ", " + model.OdfID + ", " + model.Tray + ", " + model.Port + ", '" + model.StartPoint + "', '" + model.CableType
                    + "', " + model.TubeColor + ", " + model.CoreColor + ", '" + model.CableID + "', '" + model.StartMeter + "', '" + model.EndMeter
                    + "', '" + ln.ToString() + "', '" + model.EndPoint + "', '" + model.CableJoin + "', '" + model.JoinNo + "', '" + model.CableNo
                    + "', '" + model.Remarks + "', '" + model.EntryUserID + "', '" + Convert.ToDateTime(model.EntryDate) + "', '" + model.UpdateUserID
                    + "', '" + model.UpdateDate + "', " + model.Position + ", " + model.AutoODFID + ") ";

                int item = await _misDBContext.Database.ExecuteSqlRawAsync(sqlInsert);
                if (item > 0)
                {
                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Insert process successfully completed.",
                        Data = null
                    };
                }
                else
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 404,
                        Message = "Insert process failed!",
                        Data = null
                    };
                }
                return response;


            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetAllSpliterInfoForNewGo(string ip, ClaimsPrincipal user, string prefixText, int count, string btsId)
        {
            var methodName = "FieldForceService/GetAllSpliterInfoForNewGo";
            var response = new ApiResponse();
            var getdataNewModelList = new List<string>();

            try
            {
                prefixText = "%" + prefixText + "%";
                string strSql = " SELECT BtsID, EncloserNo, EntryDate, EntryUserID, MoreSplitter, OLTBrand, OltName, PON, Port, "
                + " PortCapacity, SpliterLeg,  SplitterCapacity, SplitterID, SplitterLocation, UpdateDate, UpdateUserID, SynchStatus "
                + " FROM tbl_MainSpliterEntry WHERE (SplitterLocation LIKE '" + prefixText + "') AND (BtsID = '" + btsId + "') AND (MoreSplitter = 'NO') "
                + " OR (BtsID = '" + btsId + "') AND (MoreSplitter = 'NO') AND (EncloserNo LIKE '" + prefixText + "') ";
                var getData = await _misDBContext.tbl_MainSpliterEntries.FromSqlRaw(strSql).ToListAsync();
                if (getData != null ) {
                    getdataNewModelList = new List<string>();
                }
                foreach (var dr in getData)
                {
                    var ViewText = dr.SplitterLocation.ToString() + ":" + dr.EncloserNo.ToString() + ":" + dr.OLTBrand.ToString() + ":" +
                                    dr.OltName.ToString() + ":" + dr.PON.ToString() + ":" + dr.Port.ToString() + ":" + dr.PortCapacity.ToString()
                                    + ":" + dr.SplitterCapacity.ToString();
                    getdataNewModelList.Add(ViewText);

                }

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Data get successfully",
                    Data = getdataNewModelList
                };
                return response;

            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> GetDataLogByIpComment(ClaimsPrincipal user, string ip, string ipComment, string ticketId, string teamName)
        {
            var methodName = "FieldForceService/GetDataLogByIpComment";
            var response = new ApiResponse();
            try
            {
                string strSql = " SELECT CTID, Mailfrom, Mailto, MailCC, MailBcc, MailCategory, MailSubject, MailBody, MailSentTime, Status, id "
                    + " FROM Cli_EmailLog where CTID = '" + ticketId + "' ";
                var getData = await _misDBContext.Cli_EmailLogs.FromSqlRaw(strSql).AsNoTracking().FirstOrDefaultAsync();

                string userId = user.GetClaimUserId().ToString();
                string userName = user.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
                string phoneNo = user.Claims.Where(c => c.Type == "PhoneNo").Select(c => c.Value).FirstOrDefault();
                string designation = user.Claims.Where(c => c.Type == "Designation").Select(c => c.Value).FirstOrDefault();
                string department = user.Claims.Where(c => c.Type == "Department").Select(c => c.Value).FirstOrDefault();


                string asx = DateTime.Now.ToString();

                string info = "";

                info = info + "............Update from" + " " + teamName + ".........." + "\n";
                info = info + "Update By            :" + userId + ":" + userName.ToString() + ",Cell No:" + phoneNo.ToString() + "\n";
                info = info + "Designation          : " + designation.ToString() + "\n";
                info = info + "Department           : " + department.ToString() + "\n";
                info = info + "update Date          :" + asx + "\n";
                info = info + "Comments             :" + ipComment.Replace("'", "''") + "\n\n\n";

                info = info + getData.MailBody + "\n";
                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Data get successfully",
                    Data = info
                };
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> UpdateP2MDataForTicketCloseForNewGo(string ip, ClaimsPrincipal user, string splitterNameFiber,
        string fiberoltbrand, string fiberoltname, int fiberpon, int fiberport, int portcapfiber, string encloserno,
        string refnoOrTicketId, string branchidOrCliCode, int slnoOrCustomerCodeSlNo, string customerName,
                    string customerBranchName, string customerAddressline1, int btsSetupId, string fiberLaser,
                    string btsName, int cableNumber, string linkPathFiber, string remarksFiber, string emailBody)
        {
            var methodName = "FieldForceService/UpdateP2MDataForTicketCloseForNewGo";
            var response = new ApiResponse();

            try
            {
                string[] ar = splitterNameFiber.Split(':');
                string splitloc = ar[0].Trim();

                int splitcap = Convert.ToInt32(ar[2].Trim());
                string cusname = branchidOrCliCode + "::" + slnoOrCustomerCodeSlNo.ToString() + ":" + customerName + ":" + customerBranchName + ":" + customerAddressline1;

                string strSqlView_SubSplliters = " SELECT View_SubSplliter.* FROM View_SubSplliter WHERE (BtsID = " + btsSetupId + ") AND (OltName = '" + fiberoltname + "') "
                    + " AND (PON = " + fiberpon + ") AND (Port = " + fiberport + ") AND (EncloserNo = '" + encloserno + "') "
                    + " AND (MqActiveInactive = '1') AND (Shifted IN ('No')) OR (BtsID = " + btsSetupId + ") AND (OltName = '" + fiberoltname + "') "
                    + " AND (PON = " + fiberpon + ") AND (Port = " + fiberport + ") AND (EncloserNo = '" + encloserno + "') "
                    + " AND (Shifted IN ('No', 'Yes')) AND (UTPClient = 'YES') ORDER BY CONVERT (int, REPLACE(CableNo, CHAR(0), '')) ";
                var getDataView_SubSplliters = await _misDBContext.View_SubSplliters.FromSqlRaw(strSqlView_SubSplliters).AsNoTracking().ToListAsync();
                if (getDataView_SubSplliters.Count > splitcap)
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 409,
                        Message = "Already complete splitter capacity!",
                        Data = null
                    };
                    return response;
                }

                string strSqlSplitter_JoincolorEntrys = " SELECT BtsID, CableID, CableType, CoreColor, CustomerID, CustomerSl, EndMeter, EndPoint, EntryDate, "
                    + " EntryUserID, Length, OltName, PON, Port, Position, Remarks, Shifted, SplitterName, StartMeter, StartPoint, "
                    + " TubeColor, UpdateDate, UpdateUserID, autoid FROM tbl_Splitter_JoincolorEntry "
                    + " WHERE (CustomerID = '" + branchidOrCliCode + "') AND (CustomerSl = " + slnoOrCustomerCodeSlNo + ") ";
                var getDatatbl_Splitter_JoincolorEntrys = await _misDBContext.tbl_Splitter_JoincolorEntrys.FromSqlRaw(strSqlSplitter_JoincolorEntrys).
                AsNoTracking().ToListAsync();

                using (IDbContextTransaction transaction = _misDBContext.Database.BeginTransaction())
                {
                    try
                    {
                        string strSqlUpdateClientTechnicalInfo = @"UPDATE ClientTechnicalInfo SET f_pon = " + fiberpon + ", f_port = " +
                        fiberport + ", f_splitter = '" + splitterNameFiber + "', f_onu = '" + fiberoltbrand + "', f_oltchasisNo = '" +
                        fiberoltname + "', f_Laser = '" + fiberLaser + "' WHERE ClientCode = '" +
                        branchidOrCliCode + "' AND ClientSlNo = " + slnoOrCustomerCodeSlNo + " ";

                        int clientTechnicalInfoUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlUpdateClientTechnicalInfo);

                        if (btsSetupId != null)
                        {
                            string strSqlDeleteClientDatabaseItemDet = @"DELETE clientDatabaseItemDet where   brCliCode='" + branchidOrCliCode + "' and brSlNo='" + slnoOrCustomerCodeSlNo + "' and  itm_type='BTS'";
                            int clientDatabaseItemDetDeleteResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlDeleteClientDatabaseItemDet);

                            string strSqlClientDatabaseItemDetInsert = @"insert into  clientDatabaseItemDet(brCliCode,brSlNo,itm_type,item_id,item_desc) values ('"
                                + branchidOrCliCode + "'," + slnoOrCustomerCodeSlNo + ",'BTS','" + btsSetupId
                                + "','" + btsName.Replace("'", "''") + "')";
                            int clientDatabaseItemDetInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlClientDatabaseItemDetInsert);

                            string strSqlView_SubSplliterSelect = @"SELECT * FROM View_SubSplliter WHERE CustomerCode = '" + branchidOrCliCode + "' AND CustomerSl = " +
                                        slnoOrCustomerCodeSlNo + " AND Shifted = 'No' ORDER BY CONVERT (int, REPLACE(CableNo, CHAR(0), ''))";
                            var getDataView_SubSplliterSelect = await _misDBContext.View_SubSplliters.FromSqlRaw(strSqlView_SubSplliterSelect).AsNoTracking().ToListAsync();

                            if (getDataView_SubSplliterSelect.Count > 0)
                            {
                                string strSqlUpdatetbl_SubSpliterEntry = @"UPDATE tbl_SubSpliterEntry SET BtsID = " + btsSetupId + ", OltName = '" +
                                fiberoltname + "', PON = " + fiberpon + ", Port = " + fiberport + ", PortCapacity = " + portcapfiber
                                + ", SpliterCapacity = " + splitcap + "', SpliterLocation = '" + splitloc + "', CustomerName = '" +
                                customerName.Replace("'", "''") + "', CableNo = " + cableNumber + ", LinkPath = '" + linkPathFiber + "', Remarks = '" +
                                remarksFiber.Replace("'", "''") + "', UpdateUserID = '" + user.GetClaimUserId().ToString() + "', UpdateDate = CONVERT(datetime,'" +
                                DateTime.Now + "',103), EncloserNo = '" + ar[1].Trim() + "', OLTBrand = '" + fiberoltbrand + "' WHERE CustomerCode = '" +
                                branchidOrCliCode + "' AND CustomerSl = " + slnoOrCustomerCodeSlNo + "' AND Shifted = 'NO' ";

                                int updatetbl_SubSpliterEntryResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlUpdatetbl_SubSpliterEntry);
                            }
                            else
                            {
                                string strSqlInsertSubSpliterEntry = @"INSERT INTO tbl_SubSpliterEntry (BtsID, OltName, PON, Port, PortCapacity, SpliterCapacity, 
                                        SpliterLocation, CustomerName, CableNo, LinkPath, Remarks, EntryUserID, EntryDate, UpdateUserID, UpdateDate,
                                        CustomerCode, CustomerSl, Shifted, EncloserNo, OLTBrand, UTPClient)VALUES (" + btsSetupId + ", '"
                                        + fiberoltname + "'," + fiberpon + "," + fiberport + "," + portcapfiber + ",'" + splitcap
                                        + "','" + splitloc + "','" + cusname.Replace("'", "''") + "', " + cableNumber + ",'" + linkPathFiber + "','" +
                                        remarksFiber.Replace("'", "''") + "','" + user.GetClaimUserId().ToString() + "',CONVERT(datetime,'" + DateTime.Now + "',103),'" +
                                        user.GetClaimUserId().ToString() + "', CONVERT(datetime,'" + DateTime.Now + "',103), '" + branchidOrCliCode + "'," +
                                        slnoOrCustomerCodeSlNo + ", 'No', '" + ar[1].Trim() + "','" + fiberoltbrand + "', 'NO')";
                                int insertSubSpliterEntryResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlInsertSubSpliterEntry);
                            }

                            string[] splitername = splitterNameFiber.Split(':');

                            if (getDatatbl_Splitter_JoincolorEntrys.Count > 0)
                            {
                                string strSqlSplitter_JoincolorEntry = @"UPDATE tbl_Splitter_JoincolorEntry SET BtsID = " + btsSetupId + ", OltName = '" + fiberoltname
                                    + "', PON = " + fiberpon + ", Port = " + fiberport + ", SplitterName = '" + splitername[0].Trim()
                                    + "' WHERE CustomerID = '" + branchidOrCliCode + "' AND CustomerSl = " + slnoOrCustomerCodeSlNo + " ";
                                int updateSplitter_JoincolorEntryResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlSplitter_JoincolorEntry);
                            }

                        }

                        string ab = emailBody;
                        string strSqlCli_EmailLogUpdate = @"UPDATE Cli_EmailLog set MailBody='" + ab.Replace("'", "''") + "' where CTID='" + refnoOrTicketId.ToString() + "'";
                        int getDataCli_EmailLogUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlCli_EmailLogUpdate);




                        //string ConnectionStr_WFA2_133 = System.Configuration.ConfigurationManager.AppSettings["WFA2ConnectionString133"].ToString();
                        //SqlConnection myConnection31 = new SqlConnection(ConnectionStr_WFA2_133);
                        //myConnection31.Open();

                        //SqlCommand myCommand31 = myConnection31.CreateCommand();
                        //SqlDataAdapter sqlDataAdapterObj31 = null;
                        //myCommand31.Connection = myConnection31;

                        //try
                        //{
                        //    myCommand31.CommandText = @"INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, 
                        //              Remarks,TaskStatus) VALUES('" + TicketID + "','" + EntryUser + "','MIS','" +
                        //                          SupportType + "','" + remarks.Replace("'", "''") + "','" + taskstatus + "')";
                        //    myCommand31.ExecuteNonQuery();
                        //}

                        //  cls.userinserloghistory(lblrefno.Text, Session[StaticData.sessionUserId].ToString(), "Installation", "FI BTS PON/PORT", "update");
                        // public void userinserloghistory(string TicketID, string EntryUser, string SupportType, string remarks, string taskstatus)

                        string SupportType = "Installation";
                        string remarks = "FI BTS PON/PORT";
                        string taskstatus = "update";

                        string strSqlRsmInsertUserTaskHistory = @"INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, 
                                      Remarks,TaskStatus) VALUES('" + refnoOrTicketId + "','" + user.GetClaimUserId().ToString() + "','MIS','" +
                                                  SupportType + "','" + remarks.Replace("'", "''") + "','" + taskstatus + "')";
                        int getRsmInsertUserTaskHistoryResult = await _rsmDbContext.Database.ExecuteSqlRawAsync(strSqlRsmInsertUserTaskHistory);


                        var aa = _misDBContext.SaveChangesAsync();
                        var bb = _rsmDbContext.SaveChangesAsync();
                        transaction.Commit();

                        response = new ApiResponse()
                        {
                            Status = "Success",
                            StatusCode = 200,
                            Message = "Process executed successfully",
                            Data = null
                        };
                        return response;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        await errorMethord(ex, methodName);
                        //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                        await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                        await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse> UpdateP2PDataForTicketCloseForNewGo(string ip, ClaimsPrincipal user, string refnoOrTicketId,
            string branchidOrCliCode, int slnoOrCustomerCodeSlNo, string emailBody, int cablePathID_DDLcablnetwork,
            string Typeofp2mlink_DDLtypeofp2mlinkText, string p2pSwitchRouIP, string p2pSwRouPortNew, string p2pLaserNew,
            string p2PMCTypeInfo, string btsSetupName, int btsSetupId, string customerName, string customerBranchName,
            string customerAddressline1, string linkpathp2p_GooglePath, string remarksp2pText, int autoOFIID_IncrementID)
        {
            var methodName = "FieldForceService/UpdateP2PDataForTicketCloseForNewGo";
            var response = new ApiResponse();
            using (IDbContextTransaction transaction = _misDBContext.Database.BeginTransaction())
            {
                try
                {
                    string ab = emailBody;

                    if (cablePathID_DDLcablnetwork == 1 || Typeofp2mlink_DDLtypeofp2mlinkText == "Link3 Own")
                    {
                        string strSqlUpdateClientTechnicalInfo = @"UPDATE ClientTechnicalInfo SET  f_media_con = '" + p2PMCTypeInfo + "', f_bts_sw_router = '" +
                        p2pSwitchRouIP + "', f_media_converter_switch_port = '" + p2pSwRouPortNew + "', f_Laser = '" +
                        p2pLaserNew + "' WHERE ClientCode ='" + branchidOrCliCode + "' AND ClientSlNo = " + slnoOrCustomerCodeSlNo + " ";

                        int updateClientTechnicalInfoResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlUpdateClientTechnicalInfo);
                    }

                    if (btsSetupName != "")
                    {
                        string strSqlclientDatabaseItemDet = @"DELETE clientDatabaseItemDet where   brCliCode='" + branchidOrCliCode + "' and brSlNo = " + slnoOrCustomerCodeSlNo + " and  itm_type='BTS'";
                        int deleteclientDatabaseItemDetResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlclientDatabaseItemDet);

                        string strSqlClientDatabaseItemDetInsert = @"INSERT INTO clientDatabaseItemDet (brCliCode, brSlNo, itm_type, item_id, item_desc) VALUES ('" + branchidOrCliCode + "','" + Convert.ToInt32(slnoOrCustomerCodeSlNo) + "','BTS','" + Convert.ToInt32(btsSetupId) + "','" + btsSetupName + "')";
                        int clientDatabaseItemDetInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlClientDatabaseItemDetInsert);
                    }

                    string strSqlUpdateCli_EmailLog = @"UPDATE Cli_EmailLog set MailBody='" + ab.Replace("'", "''") + "' where CTID='" + refnoOrTicketId.ToString() + "'";
                    int updateCli_EmailLogResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlUpdateCli_EmailLog);

                    string[] c = branchidOrCliCode.Split('.');
                    string clibackbone = c[3].Trim() + ":::" + slnoOrCustomerCodeSlNo + ":" + customerName + ":" + customerBranchName + ":" + customerAddressline1;

                    string strSql = @"UPDATE  tbl_ODFDetailsEntry SET  ClientBackBoneName ='" + clibackbone.Replace("'", "''") + "', GooglePath = '" +
                    linkpathp2p_GooglePath + "', Remarks ='" + remarksp2pText.Replace("'", "''") + "', AssignEng = '" +
                    user.GetClaimUserId().ToString() + "', AssignEngDate =Convert(Datetime,'" + DateTime.Now.Date + "',103), UpdateUserID ='" +
                    user.GetClaimUserId().ToString() + "', UpdateDate =Convert(Datetime,'" +
                    DateTime.Now + "',103), ClientCode ='" + branchidOrCliCode + "', ClientSl = " + slnoOrCustomerCodeSlNo
                    + " WHERE  IncrementID = " + autoOFIID_IncrementID + " ";


                    //cls.userinserloghistory(lblrefno.Text, Session[StaticData.sessionUserId].ToString(), "Installation", "FI Data", "Update");
                    //public void userinserloghistory(string TicketID, string EntryUser, string SupportType, string remarks, string taskstatus)

                    string SupportType = "Installation";
                    string remarks = "FI Data";
                    string taskstatus = "Update";

                    string strSqlRsmInsertUserTaskHistory = @"INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, 
                                      Remarks,TaskStatus) VALUES('" + refnoOrTicketId + "','" + user.GetClaimUserId().ToString() + "','MIS','" +
                                                  SupportType + "','" + remarks.Replace("'", "''") + "','" + taskstatus + "')";
                    int getRsmInsertUserTaskHistoryResult = await _rsmDbContext.Database.ExecuteSqlRawAsync(strSqlRsmInsertUserTaskHistory);

                    var aa = _misDBContext.SaveChangesAsync();
                    var bb = _rsmDbContext.SaveChangesAsync();
                    transaction.Commit();

                    response = new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Process executed successfully",
                        Data = null
                    };
                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    await errorMethord(ex, methodName);
                    //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                    await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                    await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                    throw new Exception(ex.Message);
                }
            }



            //return response;

        }

        public async Task<ApiResponse> DoneP2MDataForTicketCloseForNewGo(string ip, ClaimsPrincipal user, string branchidOrCliCode,
                int slnoOrCustomerCodeSlNo, string customerName, string customerBranchName, string customerAddressline1,
                int cablnetwork_CablePathID, int typeofp2mlink_Typeofp2mlinkID, string splitterName, string fiberLaser,
                    int cableNumberFiber, int nTTNID, string teamName, string comments, string ticketId, int teamId_CategorySetupId,
                    string distributorId_From_ClientDataBasemain, decimal otcAmount_ClientDbMain, string serviceNarration_ClientDbMain,
                    decimal monthlyAmount_Amount_ClientDbMain, string entityName_Hostname, string realIp_ClientTechnicalInfo,
                    string nTTN_Name, DateTime installationDate_ClientDbMain, string designationName, string departmentName,
                    DateTime billingDate, string installation_MktBilling_comment, string linkidp2m_SummitLinkId,
                    string scridp2m_FiberAtHomeSCRID, string bahoncoreid, string bahonlinkid, int btsId_FIBERMEDIADETAILSP2M,
                    string btsName_FIBERMEDIADETAILSP2M, string fiberPon_FiberMediaDetailsP2M, int fiberPort_FiberMediaDetailsP2M,
                    string fiberoltbrand_FiberMediaDetailsP2M, string fiberoltname_FiberMediaDetailsP2M, string fiberlaser_FiberMediaDetailsP2M,
                    string fiberPortCapacity_FiberMediaDetailsP2M, string linkpathFb_ConnectivityDetailsP2M, string remarksFb_ConnectivityDetailsP2M,
                    string latitude, string longiTude)
        {
            var methodName = "FieldForceService/DoneP2MDataForTicketCloseForNewGo";
            var response = new ApiResponse();

            string userId = user.GetClaimUserId();
            var userFullName = user.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();
            var userDesignation = user.Claims.Where(c => c.Type == "Designation").Select(c => c.Value).FirstOrDefault();
            var userDepartmentName = user.Claims.Where(c => c.Type == "Department").Select(c => c.Value).FirstOrDefault();
            var userPhoneNumber = user.Claims.Where(c => c.Type == "PhoneNo").Select(c => c.Value).FirstOrDefault();

            if (cablnetwork_CablePathID == null)
            {
                response = new ApiResponse()
                {
                    Status = "Failed",
                    StatusCode = 400,
                    Message = "select Cable Network Type",
                    Data = null
                };
                return response;
            }

            // if (ddlcablnetwork.SelectedItem.Value == "1" || ddltypeofp2mlink.SelectedItem.Text == "Link3 Own")
            if (cablnetwork_CablePathID == 1 || typeofp2mlink_Typeofp2mlinkID == 1)
            {
                if (splitterName == "")
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "Spliter should not blank",
                        Data = null
                    };
                    return response;
                }
                if (fiberLaser == "")
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "Please confirm LASER at Client end by a good power meter",
                        Data = null
                    };
                    return response;
                }
                if (cableNumberFiber == null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "select cable no",
                        Data = null
                    };
                    return response;
                }
            }

            //if (ddlcablnetwork.SelectedItem.Value == "2")
            if (cablnetwork_CablePathID == 2)
            {
                if (nTTNID == null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "select NTTN Name",
                        Data = null
                    };
                    return response;
                }
                if (typeofp2mlink_Typeofp2mlinkID == null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "select type of p2m link",
                        Data = null
                    };
                    return response;
                }
            }

            string cusname = branchidOrCliCode + "::" + slnoOrCustomerCodeSlNo + ":" + customerName + ":" + customerBranchName + ":" + customerAddressline1;
            string strSqlSplitter_JoincolorEntrys = " SELECT BtsID, CableID, CableType, CoreColor, CustomerID, CustomerSl, EndMeter, EndPoint, EntryDate, "
                    + " EntryUserID, Length, OltName, PON, Port, Position, Remarks, Shifted, SplitterName, StartMeter, StartPoint, "
                    + " TubeColor, UpdateDate, UpdateUserID, autoid FROM tbl_Splitter_JoincolorEntry "
                    + " WHERE (CustomerID = '" + branchidOrCliCode + "') AND (CustomerSl = " + slnoOrCustomerCodeSlNo + ") ";
            var getDatatbl_Splitter_JoincolorEntrys = await _misDBContext.tbl_Splitter_JoincolorEntrys.FromSqlRaw(strSqlSplitter_JoincolorEntrys).
                            AsNoTracking().FirstOrDefaultAsync();

            if (cablnetwork_CablePathID == 1 || typeofp2mlink_Typeofp2mlinkID == 1)
            {
                if (getDatatbl_Splitter_JoincolorEntrys == null)
                {
                    response = new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "Please enter color detail",
                        Data = null
                    };
                    return response;
                }
            }

            if (teamName == null)
            {
                response = new ApiResponse()
                {
                    Status = "Failed",
                    StatusCode = 400,
                    Message = "Please Select Team",
                    Data = null
                };
                return response;
            }

            using (IDbContextTransaction transaction = _misDBContext.Database.BeginTransaction())
            {
                try
                {
                    if (await TechnicalUpdate(comments, user, ticketId, userFullName, userId, teamId_CategorySetupId, teamName, userPhoneNumber,
                            userDesignation, userDepartmentName, ip, branchidOrCliCode, slnoOrCustomerCodeSlNo, distributorId_From_ClientDataBasemain,
                            teamId_CategorySetupId, otcAmount_ClientDbMain, serviceNarration_ClientDbMain, monthlyAmount_Amount_ClientDbMain,
                            entityName_Hostname, realIp_ClientTechnicalInfo, installationDate_ClientDbMain, designationName, departmentName,
                            billingDate, installation_MktBilling_comment) == false)
                    {
                        response = new ApiResponse()
                        {
                            Status = "Failed",
                            StatusCode = 400,
                            Message = "ERROR IN UPDATE",
                            Data = null
                        };
                        return response;
                    }

                    string strSqlInstallationCompleteByTeam = @" SELECT ISNULL(MAX(CompleteID),0)+1 as MaxID from Cli_InstallationCompleteByTeam ";
                    var getDataInstallationCompleteByTeam = await _misDBContext.Cli_InstallationCompleteByTeamMaxIds.
                        FromSqlRaw(strSqlInstallationCompleteByTeam).AsNoTracking().FirstOrDefaultAsync();
                    int mmid = 0;
                    if (getDataInstallationCompleteByTeam != null)
                    {
                        mmid = getDataInstallationCompleteByTeam.MaxID;
                    }

                    string strSqlInstallationCompleteByTeamInsert = @"INSERT INTO Cli_InstallationCompleteByTeam (CompleteID, TrackingInfo, TeamName) VALUES ('" +
                            mmid + "', '" + ticketId + "', '" + teamName + "')";
                    int getInstallationCompleteByTeamInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlInstallationCompleteByTeamInsert);

                    string strSqltbl_EngineerPortFolioInsert = @"INSERT INTO tbl_EngineerPortFolio(EngineerName, ComplainFttxPhoneSupport, ComplainFttxPhysicalSupport, ComplainCorporatePhoneSupport, ComplainCorporatePhysicalSupport, ComplainOutofStation, ComplainServer, ComplainRadio, "
                        + " ComplainForward, ComplainFollowUp, ComplainCustomerMeeting, Installation, ShiftingRemote, ShiftingPhysical, ShiftingFollowUp, DismentalRemote, DismentalPhysical, DismentalFollowUp, ReActiveRemote,   "
                        + " ReActivatePhysical, ReActivateFollowUp, MaintananceRemote, MaintanancePhysical, MaintananceFollowup, EntryDate) VALUES ('"
                        + userId + "', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0',convert(datetime,'"
                        + DateTime.Now.Date + "',103))";
                    int gettbl_EngineerPortFolioInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqltbl_EngineerPortFolioInsert);

                    if (cablnetwork_CablePathID == 2)
                    {
                        if (nTTN_Name == "Summit Communications")
                        {
                            string strsqltbl_NttnDetailsInsertSummit = @"INSERT INTO tbl_NttnDetails (SubscriberID, SlNO, CableNetworkID, NTTNNameID, Typeofp2mlinkID, CoreName, SummitLinkID) VALUES ('" +
                                branchidOrCliCode + "', '" + slnoOrCustomerCodeSlNo + "', '" + cablnetwork_CablePathID + "','" +
                                nTTNID + "','" + typeofp2mlink_Typeofp2mlinkID + "', 'Core 1', '" + linkidp2m_SummitLinkId + "')";
                            int gettbl_NttnDetailsInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strsqltbl_NttnDetailsInsertSummit);
                        }
                        if (nTTN_Name == "Fiber@Home")
                        {
                            string strsqltbl_NttnDetailsInsertFiberAtHome = @"INSERT INTO tbl_NttnDetails (SubscriberID, SlNO, CableNetworkID, NTTNNameID, Typeofp2mlinkID, CoreName, SCR_LinkID) VALUES ('" +
                                branchidOrCliCode + "', '" + slnoOrCustomerCodeSlNo + "', '" + cablnetwork_CablePathID + "','" +
                                nTTNID + "', '" + typeofp2mlink_Typeofp2mlinkID + "', 'Core 1', '" + scridp2m_FiberAtHomeSCRID + "')";
                            int gettbl_NttnDetailsInsertFiberAtHomeResult = await _misDBContext.Database.ExecuteSqlRawAsync(strsqltbl_NttnDetailsInsertFiberAtHome);
                        }
                        if (nTTN_Name == "Bahon")
                        {
                            string strsqltbl_NttnDetailsInsertBahon = @"INSERT INTO tbl_NttnDetails (SubscriberID, SlNO, CableNetworkID, NTTNNameID, Typeofp2mlinkID, CoreName, SCR_LinkID) VALUES ('" +
                                branchidOrCliCode + "', '" + slnoOrCustomerCodeSlNo + "', '" + cablnetwork_CablePathID + "','" +
                                nTTNID + "', '" + typeofp2mlink_Typeofp2mlinkID + "', '" + bahoncoreid + "', '" + bahonlinkid + "')";
                            int gettbl_NttnDetailsInsertBahonResult = await _misDBContext.Database.ExecuteSqlRawAsync(strsqltbl_NttnDetailsInsertBahon);
                        }
                    }

                    if (btsName_FIBERMEDIADETAILSP2M != null)
                    {
                        if (typeofp2mlink_Typeofp2mlinkID == 1)
                        {
                            string[] ar = splitterName.Split(':');
                            string splitloc = ar[0].Trim();
                            int splitcap = Convert.ToInt32(ar[2].Trim());

                            string strSqlClientTechnicalInfo = @"UPDATE ClientTechnicalInfo SET f_pon = '" + fiberPon_FiberMediaDetailsP2M + "', f_port = '" + fiberPort_FiberMediaDetailsP2M
                                    + "', f_splitter = '" + splitterName + "', f_onu = '" + fiberoltbrand_FiberMediaDetailsP2M + "', f_oltchasisNo = '" +
                                    fiberoltname_FiberMediaDetailsP2M + "', f_Laser = '" + fiberlaser_FiberMediaDetailsP2M + "' WHERE ClientCode = '" +
                                    branchidOrCliCode + "' AND ClientSlNo = '" + slnoOrCustomerCodeSlNo + "'";
                            int getClientTechnicalInfoResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlClientTechnicalInfo);

                            string strSqlView_SubSplliter = @"SELECT * FROM View_SubSplliter WHERE CustomerCode = '" + branchidOrCliCode + "' AND CustomerSl = '" +
                                         slnoOrCustomerCodeSlNo + "' AND Shifted = 'No' ORDER BY CONVERT (int, REPLACE(CableNo, CHAR(0), ''))";
                            var getDataView_SubSplliter = await _misDBContext.View_SubSplliters.FromSqlRaw(strSqlView_SubSplliter).AsNoTracking().FirstOrDefaultAsync();
                            if (getDataView_SubSplliter != null)
                            {
                                string strtbl_SubSpliterEntryUpdate = @"UPDATE tbl_SubSpliterEntry SET BtsID = '" + btsId_FIBERMEDIADETAILSP2M + "', OltName = '" +
                                fiberoltname_FiberMediaDetailsP2M + "', PON = '" + fiberPon_FiberMediaDetailsP2M + "', Port = '" + fiberPort_FiberMediaDetailsP2M
                                + "', PortCapacity = '" + fiberPortCapacity_FiberMediaDetailsP2M + "', SpliterCapacity = '" + splitcap + "', SpliterLocation = '" + splitloc
                                + "', CustomerName = '" + cusname.Replace("'", "''") + "', CableNo = '" + cableNumberFiber + "', LinkPath = '"
                                + linkpathFb_ConnectivityDetailsP2M + "', Remarks = '" +
                                remarksFb_ConnectivityDetailsP2M.Replace("'", "''") + "', UpdateUserID = '" + userId.ToString() + "', UpdateDate = CONVERT(datetime,'" +
                                DateTime.Now + "',103), EncloserNo = '" + ar[1].Trim() + "', OLTBrand = '" + fiberoltbrand_FiberMediaDetailsP2M + "' WHERE CustomerCode = '" +
                                branchidOrCliCode + "' AND CustomerSl = '" + slnoOrCustomerCodeSlNo + "' AND Shifted = 'NO'";

                                int gettbl_SubSpliterEntryUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strtbl_SubSpliterEntryUpdate);
                            }
                            else
                            {
                                string strSqlInserttbl_SubSpliterEntry = @"INSERT INTO tbl_SubSpliterEntry (BtsID, OltName, PON, Port, PortCapacity, SpliterCapacity, 
                                    SpliterLocation, CustomerName, CableNo, LinkPath, Remarks, EntryUserID, EntryDate, UpdateUserID, UpdateDate,
                                    CustomerCode, CustomerSl, Shifted, EncloserNo, OLTBrand, UTPClient)VALUES ('" +
                                    Convert.ToInt32(btsId_FIBERMEDIADETAILSP2M) + "', '" + fiberoltname_FiberMediaDetailsP2M + "','" +
                                    Convert.ToInt32(fiberPon_FiberMediaDetailsP2M) + "','" + Convert.ToInt32(fiberPort_FiberMediaDetailsP2M) + "','" +
                                    Convert.ToInt32(fiberPortCapacity_FiberMediaDetailsP2M) + "','" + splitcap + "','" + splitloc + "','" +
                                    cusname.Replace("'", "''") + "','" + cableNumberFiber + "','" + linkpathFb_ConnectivityDetailsP2M + "','" +
                                    remarksFb_ConnectivityDetailsP2M.Replace("'", "''") + "','" +
                                    userId.ToString() + "',CONVERT(datetime,'" + DateTime.Now + "',103),'" +
                                    userId.ToString() + "', CONVERT(datetime,'" + DateTime.Now + "',103), '" + branchidOrCliCode + "','" +
                                    Convert.ToInt32(slnoOrCustomerCodeSlNo) + "', 'No', '" + ar[1].Trim() + "','" + fiberoltbrand_FiberMediaDetailsP2M + "', 'NO')";
                                int getInserttbl_SubSpliterEntryResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlInserttbl_SubSpliterEntry);

                            }

                            string[] splitername = splitterName.Split(':');

                            if (getDatatbl_Splitter_JoincolorEntrys != null)
                            {
                                string strSqltbl_Splitter_JoincolorEntryUpdate = @"UPDATE tbl_Splitter_JoincolorEntry SET BtsID = '" +
                                     btsId_FIBERMEDIADETAILSP2M + "', OltName = '" + fiberoltname_FiberMediaDetailsP2M + "', PON = '" +
                                     fiberPon_FiberMediaDetailsP2M + "', Port = '" + fiberPortCapacity_FiberMediaDetailsP2M + "', SplitterName = '" +
                                     splitername[0].Trim() + "' WHERE CustomerID = '" +
                                     branchidOrCliCode + "' AND CustomerSl = '" + slnoOrCustomerCodeSlNo + "'";
                                int gettbl_Splitter_JoincolorEntryUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqltbl_Splitter_JoincolorEntryUpdate);

                            }
                        }

                        string strSqlClientDatabaseItemDetDelete = @"DELETE clientDatabaseItemDet where   brCliCode='" + branchidOrCliCode
                            + "' and brSlNo='" + slnoOrCustomerCodeSlNo + "' and  itm_type='BTS'";
                        int getClientDatabaseItemDetDeleteResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlClientDatabaseItemDetDelete);

                        string strSqlClientDatabaseItemDetInsert = @"insert into  clientDatabaseItemDet(brCliCode,brSlNo,itm_type,item_id,item_desc) values ('"
                                + branchidOrCliCode + "','" + slnoOrCustomerCodeSlNo + "','BTS','" + btsId_FIBERMEDIADETAILSP2M + "','"
                                + btsName_FIBERMEDIADETAILSP2M.Replace("'", "''") + "')";
                        int getClientDatabaseItemDetInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlClientDatabaseItemDetInsert);

                        string strSqltbl_GoogleMapInsert = @"INSERT INTO tbl_GoogleMap (SubscriberID, Brslno, Latitude, Longtitude) VALUES ('"
                            + branchidOrCliCode + "','" + slnoOrCustomerCodeSlNo + "','" + latitude + "','" + longiTude + "')";
                        int gettbl_GoogleMapInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqltbl_GoogleMapInsert);

                        string SupportType = "Installation";
                        string remarks = "FI Data";
                        string taskstatus = "Close";

                        string strSqltbl_UserTaskHistoryInsert = @"INSERT INTO tbl_UserTaskHistory(TicketID, EntryUser, ModuleName, SupportType, 
                                      Remarks,TaskStatus) VALUES('" + ticketId + "','" + userId + "','MIS','" +
                                      SupportType + "','" + remarks.Replace("'", "''") + "','" + taskstatus + "')";
                        int gettbl_UserTaskHistoryInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqltbl_UserTaskHistoryInsert);

                        var aa = _misDBContext.SaveChangesAsync();
                        var bb = _rsmDbContext.SaveChangesAsync();
                        transaction.Commit();

                        response = new ApiResponse()
                        {
                            Status = "Success",
                            StatusCode = 200,
                            Message = "Process executed successfully",
                            Data = null
                        };
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    await errorMethord(ex, methodName);
                    //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                    await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                    await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                    throw new Exception(ex.Message);
                }

            }
            response = new ApiResponse()
            {
                Status = "failed",
                StatusCode = 400,
                Message = "Ticket closing Process Failed.!",
                Data = null
            };
            return response;
        }


        private async Task<bool> TechnicalUpdate(string comments, ClaimsPrincipal user, string ticketId, string userFullName, string userId,
            int teamId, string teamName, string userPhoneNumber, string userDesignation, string userDepartmentName, string ip,
            string branchidOrCliCode, int slnoOrCustomerCodeSlNo, string distributorId_From_ClientDataBasemain,
            int teamId_CategorySetupId, decimal otcAmount_ClientDbMain, string serviceNarration_ClientDbMain,
            decimal monthlyAmount_Amount_ClientDbMain, string entityName_Hostname, string realIp_ClientTechnicalInfo,
            DateTime installationDate_ClientDbMain, string designationName, string departmentName, DateTime billingDate,
            string installation_MktBilling_comment)
        {
            var methodName = "FieldForceService/TechnicalUpdate";
            var response = new ApiResponse();

            DateTime datt = DateTime.Now;
            string sdt = datt.ToString("yyyy-MM-dd");
            string tt = string.Format("{0:HH:mm:ss tt}", DateTime.Now);
            string ent = sdt + " " + tt;

            // DateTime datt = DateTime.Now;
            string sdtt = datt.ToString("dd/MM/yyyy");
            string ttt = string.Format("{0:HH:mm:ss tt}", DateTime.Now);
            string entt = sdtt + " " + ttt;

            bool flg = false;
            string dft = "";
            string pP = "";
            string asx = DateTime.Now.ToString();

            string gt = "SD REVIEW";
            string ft = "INI";
            //string eid = Session[StaticData.sessionUserId].ToString();
            //string uname = Session[StaticData.sessionUserName].ToString();

            string eid = user.GetClaimUserId().ToString();
            string uname = user.Claims.Where(c => c.Type == "FullName").Select(c => c.Value).FirstOrDefault();

            string ab = "";
            int j = 0;

            using (IDbContextTransaction transaction = _misDBContext.Database.BeginTransaction())
            {
                try
                {

                    ab = await GetDataLogForP2MTicketClose(comments, ticketId, userFullName, userId, teamId, teamName, userPhoneNumber,
                        userDesignation, userDepartmentName, ip);

                    // line number: 3235

                    string strSqlCliPendingTicketInfo = @"select count(Service) as refid from Cli_Pending where RefNo='"
                            + ticketId + "' and Service='READY FOR INVOICE'";
                    var getcli_PendingTickets = await _misDBContext.cli_PendingTickets.FromSqlRaw(strSqlCliPendingTicketInfo).AsNoTracking().FirstOrDefaultAsync();
                    if (getcli_PendingTickets != null)
                    {
                        string strSqlCli_PendingRedayForInvoiceDelete = @"delete Cli_Pending where RefNo='" + ticketId + "' and Service='READY FOR INVOICE'";
                        int getCli_PendingRedayForInvoiceDeleteResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlCli_PendingRedayForInvoiceDelete);

                        string strSqlCli_PendingSdReviewUpdate = @"update Cli_Pending set Status='INI' where RefNo='" + ticketId + "' and Service='SD REVIEW'";
                        int getCli_PendingSdReviewUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlCli_PendingSdReviewUpdate);
                    }

                    string strSqlCli_PendingCountStatus = @"select count(Status) as costatus from Cli_Pending where RefNo='" + ticketId + "' and Status='INI'";
                    var getSqlCli_PendingCountStatusData = await _misDBContext.cli_PendingCounts.FromSqlRaw(strSqlCli_PendingCountStatus).AsNoTracking().FirstOrDefaultAsync();

                    j = (int)(getSqlCli_PendingCountStatusData.costatus);
                    if (j == 1)
                    {
                        string strsqlCli_PendingInsert = "insert into Cli_Pending(RefNo,Service,StartDate,Status) values('" + ticketId + "','" + gt + "','" + ent + "','" + ft + "')";
                        int getsqlCli_PendingInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strsqlCli_PendingInsert);

                        string strSqlUpdatePost_Installation = @"UPDATE Post_Installation set FinalStatus='SD REVIEW',updatetime='" + entt + "',SofDatetime='" + entt + "' where TrackingInfo='" + ticketId + "'";
                        int getUpdatePost_InstallationResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlUpdatePost_Installation);

                        string strSqlUpdateCli_ReportForInstallation = @"UPDATE Cli_ReportForInstallation SET CompleteDate ='" + ent + "', SDDReviewDate ='"
                            + ent + "', TKIStatus = 'Complete' WHERE RefNo = '" + ticketId + "'";
                        int getUpdateCli_ReportForInstallationResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlUpdateCli_ReportForInstallation);
                    }

                    string strSqlUpdateCli_PendingForCompleteStatus = @"UPDATE Cli_Pending set Status='Complete' where RefNo='" + ticketId + "' and Service='" + teamName + "'";
                    int getUpdateCli_PendingForCompleteStatusResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlUpdateCli_PendingForCompleteStatus);

                    string strSqlCli_PendingList = @"select Service,PrioritySet from Cli_Pending where RefNo='" + ticketId + "' and Status='INI' and Service !='" +
                            teamName + "' and Service not in('CC','SD','MKT') order by PrioritySet";
                    var dt1 = await _misDBContext.Cli_PendingModel.FromSqlRaw(strSqlCli_PendingList).AsNoTracking().ToListAsync();

                    foreach (var model in dt1)
                    {
                        dft = dft + model.Service.ToString() + ",";
                        pP += model.PrioritySet.ToString() + ",";
                    }

                    string strSqlCli_MktpendingCmtUpdate = @"UPDATE Cli_Mktpending set Pending_for='" + dft + "',Pending_for_team='" + pP +
                                     "',LastComments='" + comments.Replace("'", "''") + "',LastCommentsDate='" + ent + "' where Refno='" + ticketId + "'";
                    int getCli_MktpendingCmtUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlCli_MktpendingCmtUpdate);

                    if (dft.Contains("SD REVIEW,SD REVIEW,"))
                    {
                        string strSqlCli_MktpendingSdReview = @"UPDATE Cli_Mktpending set Pending_for='SD REVIEW,' where Refno='" + ticketId + "'";
                        int getCli_MktpendingSdReviewResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlCli_MktpendingSdReview);
                    }

                    string strSqlUpdateEMaillog = @"UPDATE Cli_EmailLog set MailBody='" + ab.Replace("'", "''") + "' where CTID='" + ticketId + "'";
                    int getUpdateEMaillogResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlUpdateEMaillog);

                    #region  BILLING SECTION

                    if (teamName == "ERP" || teamName == "FO NOC")
                    {
                        if (!string.IsNullOrWhiteSpace(distributorId_From_ClientDataBasemain))
                        {
                            string strSqlslsSalesDetails = "SELECT isnull(count(*), 0) as Total from L3T.dbo.slsSalesDetails where ClientID='" + branchidOrCliCode + "'";
                            var getDataslsSalesDetails = await _misDBContext.slsSalesDetailsTotals.FromSqlRaw(strSqlslsSalesDetails).AsNoTracking().FirstOrDefaultAsync();

                            int toy = getDataslsSalesDetails.Total;
                            if (toy == 0)
                            {
                                int maxIDD = 0;
                                string salesId = "";

                                //DateTime nxtdt = DateTime.Now.AddDays(1);
                                DateTime nxtdt = DateTime.Now;
                                string nextbillingmonth = nxtdt.ToString("dd/MM/yyyy");

                                string calltype = "", calltype2 = "";
                                DateTime d0t = DateTime.Now;
                                string dtf = d0t.ToString("MMyy");

                                calltype = "Monthly Bandwidth Charge";
                                calltype2 = "Installation Charge";

                                string strSqlSalesDetailsMaxId = "SELECT max(convert(int,RIGHT(SalesID, LEN(SalesID) - 10)))+1 AS SalesID from L3T.dbo.slsSalesDetails";
                                var getslsSalesDetailsMaxIds = await _misDBContext.slsSalesDetailsMaxIds.FromSqlRaw(strSqlSalesDetailsMaxId).
                                    AsNoTracking().FirstOrDefaultAsync();
                                maxIDD = Convert.ToInt32(getslsSalesDetailsMaxIds.SalesID.ToString());
                                salesId = "Link3" + dtf + "-" + maxIDD.ToString();

                                string strSqlBillingSectionslsSalesDetailsInsert = @"INSERT INTO L3T.dbo.slsSalesDetails(SalesID,TrackingInfo,BillSalesID,ClientID,BranchID,ClientCategoryID,
                                   BillRefID,[BillAddressLine1],[BillAddressLine2],[BillAreaID],[BillStatusID],[BillProcessStatusID],
                                   BillingCycleID,[BillingCycleValue],[VatAmount],[DiscountAmount],[BillStartDate],[LastBillIssueDate],
                                   LastBillPeriod,LastBillEndDate,NextBillingMonth,SalesEntryDate,EntryUserID,ModifiedDate,
                                   ModifyingUserID,SO_Hdr_HPC_Flag,T_Fl,SOFStatus,SReason,BillStatus,DistributorID) VALUES('" +
                                     salesId + "','" + ticketId + "','" + salesId + "','" +
                                       branchidOrCliCode + "','" +
                                      branchidOrCliCode + "'," +
                                     teamId_CategorySetupId + ",'" +
                                     branchidOrCliCode + "','adress1','adress2',10,'Y','A',0,0,0,0,Convert(Datetime,'" +
                                     DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                     DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                     DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                     DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                     DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                     DateTime.Now.Date + "',103),'ADM',Convert(Datetime,'" +
                                     DateTime.Now.Date + "',103),'" +
                                     user.GetClaimUserId().ToString() + "','P',2,NULL,NULL,NULL,'" +
                                     distributorId_From_ClientDataBasemain + "')";
                                int getBillingSectionslsSalesDetailsInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlBillingSectionslsSalesDetailsInsert);

                                string strSqlBillingSectionblgRevenueFromServiceInsert = @"INSERT INTO L3T.dbo.blgRevenueFromService([TrackingInfo],[SalesID],[ServiceCode],
                                [ServiceLineNo],[BillSalesID],
                                [ServiceDescription],[ServiceNarration],[ScopeID],[ServiceConfirmDate],[BillingCycleID],[BillingCycleValue],
                                [BillingModeID],[BillingServiceQty],[BillingServiceRate],[BillingAmount],[VATAmount],[DiscountAmount],
                                [BillingStatusID],[ServiceStatusID],[ServiceStatusDate],[LastBillIssueDate],[LastBillPeriod],[ProcessAllPendingBills],
                                [VATCalculationProcessID],NextBillingMonth,UsagesStartDate,UsagesEndDate,BillStartDate,BillProcessStatusID,
                                ManualBillProcessID,EntryDate,EntryUserID,ModifiedDate,ModifyingUserID,
                                StatusReasonID) VALUES('" +
                                        ticketId + "','" + salesId + "','09.001.004','1','" +
                                        salesId + "','Installation - Internet','" +
                                        calltype2 + "','2',Convert(Datetime,'" +
                                        DateTime.Now.Date + "',103),'1','0','1','1','" +
                                        otcAmount_ClientDbMain + "','" +
                                        otcAmount_ClientDbMain + "','5',0,'Y',1,convert(datetime,'" +
                                        DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                        DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                        DateTime.Now.Date + "',103),NULL,'1',Convert(Datetime,'" +
                                        nextbillingmonth + "',103),Convert(Datetime,'" +
                                        DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                        DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                        DateTime.Now.Date + "',103),'N','',Convert(Datetime,'" +
                                        DateTime.Now.Date + "',103),'" +
                                        user.GetClaimUserId().ToString() + "',Convert(Datetime,'" +
                                        DateTime.Now.Date + "',103),'" +
                                        user.GetClaimUserId().ToString() + "',1)";
                                int getBillingSectionblgRevenueFromServiceInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlBillingSectionblgRevenueFromServiceInsert);

                                string strSqlBillingSectionblgRevenueFromService2Insert = @"INSERT INTO L3T.dbo.blgRevenueFromService([TrackingInfo],[SalesID],[ServiceCode],[ServiceLineNo],[BillSalesID],
                                        [ServiceDescription],[ServiceNarration],[ScopeID],[ServiceConfirmDate],[BillingCycleID],[BillingCycleValue],
                                        [BillingModeID],[BillingServiceQty],[BillingServiceRate],[BillingAmount],[VATAmount],[DiscountAmount],
                                        [BillingStatusID],[ServiceStatusID],[ServiceStatusDate],[LastBillIssueDate],[LastBillPeriod],[ProcessAllPendingBills],
                                        [VATCalculationProcessID],NextBillingMonth,UsagesStartDate,UsagesEndDate,BillStartDate,BillProcessStatusID,
                                        ManualBillProcessID,EntryDate,EntryUserID,ModifiedDate,ModifyingUserID,
                                        StatusReasonID) VALUES('" +
                                              ticketId + "','" + salesId + "','" + branchidOrCliCode + "','2','" +
                                              salesId + "','" + serviceNarration_ClientDbMain + "','" +
                                              calltype + "','2',Convert(Datetime,'" +
                                              DateTime.Now.Date + "',103),'2','1','1','1','" +
                                              monthlyAmount_Amount_ClientDbMain + "','" +
                                              monthlyAmount_Amount_ClientDbMain + "','5',0,'Y',1,convert(datetime,'" +
                                              DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                              DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                              DateTime.Now.Date + "',103),NULL,'1',Convert(Datetime,'" +
                                              nextbillingmonth + "',103),Convert(Datetime,'" +
                                              DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                              DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                              DateTime.Now.Date + "',103),'N','',Convert(Datetime,'" +
                                              DateTime.Now.Date + "',103),'" +
                                              user.GetClaimUserId().ToString() + "',Convert(Datetime,'" +
                                              DateTime.Now.Date + "',103),'" +
                                              user.GetClaimUserId().ToString() + "',1)";
                                int getBillingSectionblgRevenueFromService2InsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlBillingSectionblgRevenueFromService2Insert);

                                // 3425
                                string strSqlKh_IpAddressUpdate = @"update Kh_IpAddress set UsedStatus='Y',SubscriberID='" + branchidOrCliCode
                                    + "',SubscriberSlNo='" + slnoOrCustomerCodeSlNo + "' where HostName='" + entityName_Hostname
                                    + "' and IPAddress='" + realIp_ClientTechnicalInfo + "'";
                                int getKh_IpAddressUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlKh_IpAddressUpdate);

                            }
                        }
                    }

                    #endregion

                    #region IPTV && bundle offer

                    if (teamName == "FO NOC" || teamName == "ERP")
                    {
                        if (distributorId_From_ClientDataBasemain == "")
                        {
                            if (teamId_CategorySetupId == 11)
                            {
                                if (realIp_ClientTechnicalInfo != "")
                                {
                                    #region iptv internet

                                    string strSqlIpTvInterNetblgRevenue = "SELECT count(*) as Total from L3T.dbo.blgRevenueFromService where TrackingInfo='" + ticketId + "' and ServiceCode<>'14.005.049'";
                                    var getIpTvInterNetblgRevenue = await _misDBContext.slsSalesDetailsTotals.FromSqlRaw(strSqlIpTvInterNetblgRevenue).AsNoTracking().FirstOrDefaultAsync();

                                    int toy = Convert.ToInt32(getIpTvInterNetblgRevenue.Total.ToString());
                                    if (toy == 0)
                                    {
                                        int maxIDD = 0;
                                        string salesId = "";

                                        DateTime nxtdt = DateTime.Now.AddDays(1);
                                        string nextbillingmonth = nxtdt.ToString("dd/MM/yyyy");

                                        string calltype = "", calltype2 = "";
                                        DateTime d0t = DateTime.Now;
                                        string dtf = d0t.ToString("MMyy");

                                        calltype = "Monthly Bandwidth Charge";
                                        calltype2 = "Installation Charge";

                                        string strSqlPackagePlan = @"select c.NoteBandwith,d.ServiceCode,d.Amount from Post_Installation a inner join clientdatabasemain b on a.cli_code=b.brclicode
                                                            inner join ClientTechnicalInfo c on c.ClientCode=b.brclicode 
                                                            inner join tbl_packageplan d on  d.PackageID=b.DisPackageID
                                                            where a.Trackinginfo='" + ticketId + "' and c.NoteBandwith<>'IPTV'";
                                        var dtservgt = await _misDBContext.IpTvPackageInfos.FromSqlRaw(strSqlPackagePlan).AsNoTracking().FirstOrDefaultAsync();

                                        var strSqlMaxIdgenIpTv = "SELECT max(convert(int,RIGHT(SalesID, LEN(SalesID) - 10)))+1 AS SalesID from L3T.dbo.slsSalesDetails";


                                        var getMaxIdgenIpTv = await _misDBContext.slsSalesDetailsMaxIds.FromSqlRaw(strSqlMaxIdgenIpTv).AsNoTracking().FirstOrDefaultAsync();
                                        maxIDD = Convert.ToInt32(getMaxIdgenIpTv.SalesID.ToString());
                                        salesId = "Link3" + dtf + "-" + maxIDD.ToString();

                                        string strSqlIpTvSalesDetailsInsert = @"INSERT INTO L3T.dbo.slsSalesDetails(SalesID,TrackingInfo,BillSalesID,ClientID,BranchID,ClientCategoryID,
                                           BillRefID,[BillAddressLine1],[BillAddressLine2],[BillAreaID],[BillStatusID],[BillProcessStatusID],
                                           BillingCycleID,[BillingCycleValue],[VatAmount],[DiscountAmount],[BillStartDate],[LastBillIssueDate],
                                           LastBillPeriod,LastBillEndDate,NextBillingMonth,SalesEntryDate,EntryUserID,ModifiedDate,
                                           ModifyingUserID,SO_Hdr_HPC_Flag,T_Fl,SOFStatus,SReason,BillStatus,DistributorID) VALUES('" +
                                             salesId + "','" + ticketId + "','" + salesId + "','" +
                                               branchidOrCliCode + "','" +
                                              branchidOrCliCode + "'," +
                                             teamId_CategorySetupId + ",'" +
                                             branchidOrCliCode + "','adress1','adress2',10,'Y','A',0,0,0,0,Convert(Datetime,'" +
                                             DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                             DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                             DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                             DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                             DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                             DateTime.Now.Date + "',103),'ADM',Convert(Datetime,'" +
                                             DateTime.Now.Date + "',103),'" +
                                             user.GetClaimUserId().ToString() + "','P',2,NULL,NULL,NULL,'9999')";

                                        int getIpTvSalesDetailsInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlIpTvSalesDetailsInsert);

                                        string strSqlIpTvRevenueFromServiceInsert = @"INSERT INTO L3T.dbo.blgRevenueFromService([TrackingInfo],[SalesID],[ServiceCode],[ServiceLineNo],[BillSalesID],
                                        [ServiceDescription],[ServiceNarration],[ScopeID],[ServiceConfirmDate],[BillingCycleID],[BillingCycleValue],
                                        [BillingModeID],[BillingServiceQty],[BillingServiceRate],[BillingAmount],[VATAmount],[DiscountAmount],
                                        [BillingStatusID],[ServiceStatusID],[ServiceStatusDate],[LastBillIssueDate],[LastBillPeriod],[ProcessAllPendingBills],
                                        [VATCalculationProcessID],NextBillingMonth,UsagesStartDate,UsagesEndDate,BillStartDate,BillProcessStatusID,
                                        ManualBillProcessID,EntryDate,EntryUserID,ModifiedDate,ModifyingUserID,
                                        StatusReasonID) VALUES('" +
                                                      ticketId + "','" + salesId + "','" + dtservgt.ServiceCode.ToString() + "','2','" +
                                                      salesId + "','" + dtservgt.NoteBandwith.ToString() + "','" +
                                                      calltype + "','1',Convert(Datetime,'" +
                                                      DateTime.Now.Date + "',103),'2','1','1','1','" +
                                                      dtservgt.Amount.ToString() + "','" +
                                                      dtservgt.Amount.ToString() + "','5',0,'Y',1,convert(datetime,'" +
                                                      DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                                      DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                                      DateTime.Now.Date + "',103),NULL,'1',Convert(Datetime,'" +
                                                      nextbillingmonth + "',103),Convert(Datetime,'" +
                                                      DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                                      DateTime.Now.Date + "',103),Convert(Datetime,'" +
                                                      DateTime.Now.Date + "',103),'N','',Convert(Datetime,'" +
                                                      DateTime.Now.Date + "',103),'" +
                                                      user.GetClaimUserId().ToString() + "',Convert(Datetime,'" +
                                                      DateTime.Now.Date + "',103),'" +
                                                      user.GetClaimUserId().ToString() + "',1)";

                                        int getIpTvRevenueFromServiceResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlIpTvRevenueFromServiceInsert);
                                    }
                                    // 3455
                                    #endregion
                                }
                            }
                        }
                    }
                    #endregion

                    string strCliPendingUpdate = @"UPDATE Cli_Pending SET  FinisDate = '" + ent + "' WHERE  RefNo = '" + ticketId + "' AND Service = '" + teamName + "'";
                    int getCliPendingUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strCliPendingUpdate);

                    string strSqlCliPendingTeam = @"SELECT RefNo, Service, Status, userid, Mkt_group, SLNo, PrioritySet, StartDate, FinisDate FROM Cli_Pending where RefNo ='" +
                        ticketId + "'  and status='INI' and PrioritySet <>0 and service<>'" + teamName + "' and startdate is not null";
                    var dcpl = await _misDBContext.Cli_PendingModel.FromSqlRaw(strSqlCliPendingTeam).AsNoTracking().FirstOrDefaultAsync();
                    string prt = "";

                    if (dcpl == null)
                    {
                        string strSqlCliPendingProioritySet = @"SELECT top 1 PrioritySet  from Cli_Pending where RefNo ='" + ticketId + "' and status='INI' and PrioritySet <>0  group by PrioritySet";
                        var dcplt = await _misDBContext.cli_PendingPrioritySets.FromSqlRaw(strSqlCli_PendingList).AsNoTracking().FirstOrDefaultAsync();
                        if (dcplt != null)
                        {
                            prt = dcplt.PrioritySet.ToString();
                            string strSqlCliPendingPrioritySet = @"SELECT RefNo, Service, Status, userid, Mkt_group, SLNo, PrioritySet, StartDate, FinisDate FROM Cli_Pending where RefNo ='" +
                                ticketId + "'  and status='INI' and PrioritySet='" + prt + "'";
                            var dst = await _misDBContext.Cli_PendingModel.FromSqlRaw(strSqlCliPendingPrioritySet).AsNoTracking().ToListAsync();
                            foreach (var dr in dst)
                            {
                                string strUpdateCli_PendingPrioritySet = @" Update Cli_Pending set StartDate='" + ent + "' where RefNo ='" + ticketId + "' and PrioritySet =" + dr.PrioritySet + " ";
                                int getUpdateCli_PendingPrioritySetResult = await _misDBContext.Database.ExecuteSqlRawAsync(strUpdateCli_PendingPrioritySet);
                            }
                        }
                    }

                    if (distributorId_From_ClientDataBasemain != "")
                    {
                        if (j == 1)
                        {
                            if (await this.TechnicalAndServiceDeliveryUpdate(installationDate_ClientDbMain, ticketId, userFullName,
                                    userId, designationName, departmentName, billingDate, installation_MktBilling_comment, ip) == false)
                            {
                                //lblerrorgeneral.Visible = true;
                                //lblerrorgeneral.Text = "ERROR UPDATE";

                                return false;
                            }
                        }
                    }

                    return true;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    await errorMethord(ex, methodName);
                    //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                    await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                    await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                    throw new Exception(ex.Message);
                }
            }

        }

        private async Task<string> GetDataLogForP2MTicketClose(string comments, string ticketId, string userFullName, string userId,
            int teamId, string teamName, string userPhoneNumber, string userDesignation, string userDepartmentName, string ip)
        {
            var methodName = "FieldForceService/GetDataLogForP2MTicketClose";
            var response = new ApiResponse();

            try
            {
                string tr = ticketId;
                //string uname = Session[StaticData.sessionUserName].ToString();
                //string userid = Session[StaticData.sessionUserId].ToString();
                string uname = userFullName;
                string userid = userId;

                string gpn = "";

                string query = @"select MailBody from Cli_EmailLog where CTID='" + ticketId + "'";
                var result = await _misDBContext.MailLogModel.FromSqlRaw(query).AsNoTracking().FirstOrDefaultAsync();
                if (result != null)
                {
                    //gpn = dtquery.Rows[0]["MailBody"].ToString();
                    gpn = result.MailBody.ToString();
                }

                string asx = DateTime.Now.ToString();

                string info = "";

                info = info + "..........." + teamName + "................" + "\n";
                //  info = info + "Job DONE, confirmed by :" + uname + "\n";
                info = info + "Job DONE, confirmed by :" + userId + ":" + userFullName + ",Cell No:" + userPhoneNumber + "\n";
                info = info + "Designation            : " + userDesignation + "\n";
                info = info + "Department             : " + userDepartmentName + "\n";
                info = info + "Posting Date           :" + asx + "\n";

                info = info + "Comments               :" + comments.Replace("'", "''") + "\n";

                info = info + gpn;

                return info;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }


        }

        public async Task<bool> TechnicalAndServiceDeliveryUpdate(DateTime installationDate_ClientDbMain, string ticketId,
            string userFullName, string userId, string designationName, string departmentName, DateTime billingDate,
            string installation_MktBilling_comment, string ip)
        {
            var methodName = "FieldForceService/TechnicalAndServiceDeliveryUpdate";
            var response = new ApiResponse();

            DateTime datt = DateTime.Now;
            string sdt = datt.ToString("yyyy-MM-dd");
            string tt = string.Format("{0:HH:mm:ss tt}", DateTime.Now);
            string ent = sdt + " " + tt;

            //   DateTime datt = DateTime.Now;
            string sdtt = datt.ToString("dd/MM/yyyy");
            string ttt = string.Format("{0:HH:mm:ss tt}", DateTime.Now);
            string entt = sdtt + " " + ttt;

            bool flg = false;
            string gt = "SD REVIEW,";
            string ft = "INI";
            string ty = "READY FOR INVOICE";
            string asx = DateTime.Now.ToString();
            //DateTime starttime = Convert.ToDateTime(i_ins_date.SelectedDate.ToShortDateString());
            DateTime starttime = Convert.ToDateTime(installationDate_ClientDbMain.Date.ToShortDateString());
            string df = starttime.ToString();

            using (IDbContextTransaction transaction = _misDBContext.Database.BeginTransaction())
            {
                try
                {

                    string ab;
                    ab = await DataLogServiceDelivery(ticketId, userFullName, userId,
                            designationName, departmentName, installationDate_ClientDbMain, billingDate,
                            installation_MktBilling_comment, ip);

                    string strSqlCliPending = @"select count(Service) as refid from Cli_Pending where RefNo='" + ticketId + "' and Service='READY FOR INVOICE'";
                    var getDataCliPendingRefId = await _misDBContext.cli_PendingTickets.FromSqlRaw(strSqlCliPending).AsNoTracking().FirstOrDefaultAsync();

                    if (Convert.ToInt32(getDataCliPendingRefId.refid) > 0)
                    {
                        // 6519
                        string strSqlDeleteCliPendingReadyForInvoice = @"delete Cli_Pending where RefNo='" + ticketId + "' and Service='READY FOR INVOICE'";
                        int getDeleteCli_PendingPrioritySetResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlDeleteCliPendingReadyForInvoice);

                        string strSqlCliPendingSdReview = @"UPDATE Cli_Pending set Status='INI' where RefNo='" + ticketId + "' and Service='SD REVIEW'";
                        int getUpdateCli_PendingSdReviewResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlCliPendingSdReview);
                    }

                    string strSqlPostInstallationUpdate = @"UPDATE Post_Installation set FinalStatus='READY FOR INVOICE', PenStatus='"
                     + ty + "',UPDATEtime='" + entt + "',SofDatetime=Convert(Datetime,'" + df
                     + "',103) where TrackingInfo='" + ticketId + "'";
                    int getPostInstallationUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlPostInstallationUpdate);

                    string strSqlCliPendingCompleteUpdate = @"UPDATE Cli_Pending set Status='Complete',FinisDate='" + ent + "' where RefNo='" + ticketId + "' and Service='SD REVIEW'";
                    int getCliPendingCompleteUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlCliPendingCompleteUpdate);

                    string strSqlMktpendingUpdate = @"UPDATE Cli_Mktpending set Pending_for='" + ty + "' where RefNo='" + ticketId + "'";
                    int getMktpendingUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlMktpendingUpdate);

                    string strSqlReportForInstallationUpdate = @"UPDATE Cli_ReportForInstallation set CompleteDate='" + ent + "' where RefNo='" + ticketId + "'";
                    int getReportForInstallationUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlReportForInstallationUpdate);

                    string strSqlCliPendingInsert = @"insert into Cli_Pending(RefNo,Service,Status) values('" + ticketId + "','" + ty + "','" + ft + "')";
                    int getCliPendingInsertResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlCliPendingInsert);

                    string strSqlEmailLogUpdate = @"UPDATE Cli_EmailLog set MailBody='" + ab.Replace("'", "''") + "' where CTID='" + ticketId + "'";
                    int getEmailLogUpdateResult = await _misDBContext.Database.ExecuteSqlRawAsync(strSqlEmailLogUpdate);

                    var aa = _misDBContext.SaveChangesAsync();
                    transaction.Commit();

                    flg = true;
                    return flg;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    await errorMethord(ex, methodName);
                    //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                    await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                    await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                    //throw new Exception(ex.Message);
                    return flg;
                }
            }

        }

        public async Task<string> DataLogServiceDelivery(string ticketId, string userfullName, string userId,
            string designationName, string departmentName, DateTime installationDate, DateTime billingDate,
            string installation_MktBilling_comment, string ip)
        {
            var methodName = "FieldForceService/DataLogServiceDelivery";
            try
            {
                // 6595
                //string tr = Session[StaticData.sessionRefNo].ToString();
                //string uname = Session[StaticData.sessionUserName].ToString();
                //string userid = Session[StaticData.sessionUserId].ToString();
                //string sd = Session[StaticData.sessionServiceDelivery].ToString();

                string tr = ticketId;
                string uname = userfullName;
                string userid = userId;
                //string sd = Session[StaticData.sessionServiceDelivery].ToString();

                string gp = "";
                string gpn = "";

                string query = @"select MailBody from Cli_EmailLog where CTID='" + ticketId + "'";
                var result = await _misDBContext.MailLogModel.FromSqlRaw(query).AsNoTracking().FirstOrDefaultAsync();

                if (result != null)
                {
                    gpn = result.MailBody.ToString();
                }
                gp = "Service Delivery";

                string asx = DateTime.Now.ToString();

                string info = "";

                info = info + "..........." + gp + "................" + "\n";
                info = info + "Job DONE, confirmed by :" + uname + "\n";
                info = info + "Designation            : " + designationName.ToString() + "\n";
                info = info + "Department             : " + departmentName + "\n";
                info = info + "Posting Date           :" + asx + "\n";
                info = info + "Installation Date      :" + installationDate.Date.ToString().Replace("12:00:00 AM", "") + "\n";
                info = info + "Billing  Date          :" + billingDate.Date.ToString().Replace("12:00:00 AM", "") + "\n";
                info = info + "Comments               :" + installation_MktBilling_comment.Replace("'", "''") + "\n";

                info = info + gpn;

                return info;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                //await InsertRequestResponse(null, ex, methodName, ip, user.GetClaimUserId(), ex.Message);
                await InsertRequestResponse(null, ex, methodName, ip, null, ex.Message);
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }


    }
}

using Elasticsearch.Net;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.BTS;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using tik4net.Objects.User;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace L3T.Infrastructure.Helpers.Implementation.FieldForce
{
    public class RSMComplainTicketService : IRSMComplainTicketService
    {
        private readonly ILogger<RSMComplainTicketService> _logger;
        private readonly RsmDbContext _rsmContext;
        private readonly FFWriteDBContext _ffWriteDBContext;
        private readonly IMailSenderService _mailSenderService;
        private readonly MisDBContext _misDBContext;

        public RSMComplainTicketService(
            ILogger<RSMComplainTicketService> logger, 
            RsmDbContext rsmContext,
            FFWriteDBContext ffWriteDBContext,
            IMailSenderService mailSenderService,
            MisDBContext misDBContext)
        {
            _logger = logger;
            _rsmContext = rsmContext;
            _ffWriteDBContext = ffWriteDBContext;
            _mailSenderService = mailSenderService;
            _misDBContext = misDBContext;
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
            await _mailSenderService.ExceptionSendMail(ex.ToString(), info + " Error Message :" + ex.Message);
            //await _systemService.ErrorLogEntry(info, info, errormessage);
            //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
            return errormessage;
        }

        public async Task<ApiResponse> GetRSMComplainTicket(string userId, string ip)
        {
            var methodName = "RSMComplainTicketService/GetRSMComplainTicket";
            try
            {
                var TeamID = "";
                string permission_sql = @"select distinct * from tbl_team_mem_permission  WITH(NOLOCK) where Emp_id='" + userId + "'";

                var permissino = await _rsmContext.Team_Mem_Permissions.FromSqlRaw(permission_sql).ToListAsync();
                if (permissino.Count > 0)
                {
                    foreach (var dr in permissino)
                    {

                        if (TeamID == "")
                        {
                            TeamID = "'" + dr.Team_id.ToString() + "'";
                        }
                        else
                        {
                            TeamID = TeamID + ",'" + dr.Team_id.ToString() + "'";
                        }
                    }
                }

                var teamIdstr = "";
                if (!string.IsNullOrEmpty(TeamID))
                {
                    teamIdstr = "and g.Team_id in(" + TeamID + ")";
                }

                var mon_sql = @"select  a.RefNo,a.CustomerID,b.HeadOfficeName,a.ReceiveDateTime,a.ForwardDate,(ed.EmpName +'/'+ed.Dept+'/'+convert(varchar,a.ActualReceiveDatetime)) as Date,t.TicketTypeName,
                        b.brAddress1,b.phone_no,a.Comments,
                        d.NatureName,b.brSupportOffice,g.Team_Name,b.Area,j.user_name,a.Complains,a.LastComments,a.ExecuteDate,
                        case when a.AssignEngineer='Yes' then 'Assign' else 'Un Assign' end as AssignEngineer,(ta.UserID+'/'+ae.user_name+'/'+Convert(varchar,ta.AssignDatetime)) as AssignEng,a.ScheduleDate,ScheduleStatus,a.ComplainSource,pt.TaskPendingStatus,isnull(a.SupportType,a.GenerateTicketSupportType) as  SupportType 
                        from RSM_Complain_Details a  WITH(NOLOCK)
                        left outer join clientDatabaseMain b  WITH(NOLOCK) on a.CustomerID=b.MqID
                        left outer join RSM_SupportOfficeWiseID c  WITH(NOLOCK) on c.Support_OfficeID=b.brSupportOfficeId 
                        left outer join RSM_NatureSetup d  WITH(NOLOCK) on d.NatureID=a.ComplainCategory
                        left outer join RSM_TicketType t  WITH(NOLOCK) on t.ID=d.TaskTypeID
                        left outer join RSM_TaskPendingTeam tp  WITH(NOLOCK) on tp.Refno=a.RefNo 
                        left outer join tbl_Team_info g  WITH(NOLOCK) on g.Team_id=tp.PendingTeamID
                        left outer join tbl_user_info j  WITH(NOLOCK) on j.userid=a.ComplainReceiveby
						 left outer join RSM_TaskAssign h  WITH(NOLOCK) on h.RefNo=a.RefNo
                         left outer join LNK.dbo.Emp_Details ed with(nolock) on ed.EmpID=a.ComplainReceiveby
                         left outer join RSM_TaskAssign ta  WITH(NOLOCK) on ta.RefNo=a.RefNo 
                         left outer join tbl_user_info ae WITH(NOLOCK) on ae.userid=ta.UserID
                          left outer join RSM_PendingTaskType pt  WITH(NOLOCK) on pt.ID=a.PendingReasonID
                        where  tp.Status='0' and h.UserID='" + userId + "'  "+ teamIdstr + 
                        " group by a.RefNo,b.brAddress1,b.phone_no,a.Comments,a.CustomerID,b.HeadOfficeName,a.ReceiveDateTime,a.ForwardDate,(ed.EmpName +'/'+ed.Dept+'/'+convert(varchar,a.ActualReceiveDatetime)),t.TicketTypeName, " +
                        " d.NatureName,b.brSupportOffice,g.Team_Name,b.Area,j.user_name,a.Complains,a.LastComments,a.ExecuteDate, " +
                        " case when a.AssignEngineer='Yes' then 'Assign' else 'Un Assign' end,(ta.UserID+'/'+ae.user_name+'/'+Convert(varchar,ta.AssignDatetime)),a.ScheduleDate,ScheduleStatus,a.ComplainSource,pt.TaskPendingStatus,isnull(a.SupportType,a.GenerateTicketSupportType) order by a.ForwardDate desc";

                var complainTicketList = await _rsmContext.RSMComplainTicketResponse.FromSqlRaw(mon_sql).ToListAsync();
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get Data",
                    Data = complainTicketList
                };
                await InsertRequestResponse(null, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(null, ex, methodName, ip, userId, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetRSMSubcriberInfo(string customerId, string userId, string ip)
        {
            var methodName = "RSMComplainTicketService/GetRSMSubcriberInfo";
            try
            {
                var newSubcriberInfo = new RSMSubcriberInformationResponseModel();
                string sp = "EXEC GetInformationRSMComplainTicket @customerId";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    // Create parameters
                    new SqlParameter { ParameterName = "@customerId", Value = customerId },
                };

                var result = await _rsmContext.GetInformationRSMComplainTicket.FromSqlRaw(sp, parms.ToArray()).AsNoTracking().ToListAsync();
                if (result.Count < 0)
                {
                    throw new Exception("data is not found");
                }
                var resultData = result.FirstOrDefault();

                newSubcriberInfo.Subscriber_Code = resultData.Subscriber_Code;
                newSubcriberInfo.IP = resultData.IP;
                newSubcriberInfo.Subscriber_Name = resultData.Subscriber_Name;
                newSubcriberInfo.Gateway = resultData.Gateway;
                newSubcriberInfo.Address = resultData.Address;
                newSubcriberInfo.Subnet_Mask = resultData.Subnet_Mask;
                newSubcriberInfo.Area = resultData.Area;
                newSubcriberInfo.VLAN = resultData.VLAN;
                newSubcriberInfo.Contact_Detail = resultData.Contact_Detail;
                newSubcriberInfo.Contact_Number = resultData.Contact_Number;
                newSubcriberInfo.OLT = resultData.OLT;
                newSubcriberInfo.OLT_IP = resultData.OLT_IP;
                newSubcriberInfo.Email = resultData.Email;
                newSubcriberInfo.PON = resultData.PON;
                newSubcriberInfo.Support_Office = resultData.Support_Office;
                newSubcriberInfo.PORT = resultData.PORT;
                newSubcriberInfo.Splitter = resultData.Splitter;
                newSubcriberInfo.Customer_MAC = resultData.Customer_MAC;
                newSubcriberInfo.ONU_MAC = resultData.ONU_MAC;
                newSubcriberInfo.ONU_PORT = resultData.ONU_PORT;
                newSubcriberInfo.ONU_ID = resultData.ONU_ID;
                newSubcriberInfo.District = resultData.District;
                newSubcriberInfo.UpazilaThana = resultData.UpazilaThana;
                newSubcriberInfo.Post_Code = resultData.Post_Code;
                newSubcriberInfo.AreaGroup = resultData.AreaGroup;
                newSubcriberInfo.House_Name = resultData.House_Name;
                newSubcriberInfo.House_No = resultData.House_No;
                newSubcriberInfo.FloorFlat_No = resultData.FloorFlat_No;
                newSubcriberInfo.Road_Name = resultData.Road_Name;
                newSubcriberInfo.Road_No = resultData.Road_No;
                newSubcriberInfo.Block_No = resultData.Block_No;
                newSubcriberInfo.Section = resultData.Section;
                newSubcriberInfo.Sector = resultData.Sector;
                newSubcriberInfo.Land_Mark = resultData.Land_Mark;                

                string subSpliterEntry_sql = @"select b.EncloserNo, b.CableNo from clientDatabaseMain a WITH(NOLOCK)
                inner join tbl_SubSpliterEntry b WITH(NOLOCK) on a.brCliCode=b.CustomerCode and a.brSlNo=b.CustomerSl 
				where a.MqID= '" + customerId + "' and b.Shifted='No'";

                var subSpliterEntry = await _misDBContext.tbl_SubSpliterEntry.FromSqlRaw(subSpliterEntry_sql).FirstOrDefaultAsync();
                if (subSpliterEntry != null)
                {
                    newSubcriberInfo.Encloser_No = subSpliterEntry.EncloserNo;
                    newSubcriberInfo.Cable_No = subSpliterEntry.CableNo;
                }                

                var sql = @"SELECT a.ColorName AS TubeColorName, c.ColorName AS CoreColorName, b.autoid, b.BtsID, b.OltName, 
                         b.PON, b.Port, b.SplitterName, b.CustomerID, b.StartPoint, b.CableType, b.TubeColor, b.CoreColor, b.CableID, b.StartMeter, 
                         b.EndMeter, b.Length, b.EndPoint, b.Remarks, b.EntryUserID, b.EntryDate, b.UpdateUserID, b.UpdateDate, b.Position, b.CustomerSl, 
                         b.Shifted  FROM tbl_ColorInfo a WITH(NOLOCK) INNER JOIN  tbl_Splitter_JoincolorEntry b WITH(NOLOCK) ON a.ColorID = b.TubeColor 
						 INNER JOIN  dbo.tbl_ColorInfo c WITH(NOLOCK) ON b.CoreColor = c.ColorID
						 inner join clientDatabaseMain m WITH(NOLOCK) on m.brCliCode=b.CustomerID and m.brSlNo=b.CustomerSl
						 where m.MqID='" + customerId + "' and b.Shifted='No' order by b.Position"; // 31 multiple data

                var colorInfoList = await _misDBContext.colorInfo.FromSqlRaw(sql).ToListAsync();

                if (colorInfoList.Count <= 0)
                {
                    throw new Exception("Color information is not found.");
                }

                var rm = "";
                string joincolor = "";
                foreach (var dr in colorInfoList)
                {
                    if (dr.Remarks.ToString() != "")
                    {
                        rm = "; {RM:" + dr.Remarks + "}";
                    }
                    else rm = "";
                    joincolor += "[{SP: " + dr.StartPoint + "};{Cable " + dr.CableType + " (TC:" +
                        dr.TubeColorName + ";CC:" + dr.CoreColorName + ")};{CID:" +
                        dr.CableID + ")" + "(SM:" + dr.StartMeter + "- EM:" + dr.EndMeter + " = " +
                        dr.Length + "m)};" + " {EP:" + dr.EndPoint + "}" +
                        rm + "]+";
                }
                newSubcriberInfo.Color = joincolor;

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get Data",
                    Data = newSubcriberInfo
                };
                await InsertRequestResponse(customerId, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(customerId, ex, methodName, ip, userId, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetRSMComplainTicketLogs(string ticketNo, string userId, string ip)
        {
            var methodName = "RSMComplainTicketService/GetRSMTicketLogs";
            try
            {
                string log_sql = @"select (a.UserID+':'+isnull(b.EmpName,'Self Care API')) as CommentsBy,
                        isnull(b.Designation+'/'+b.Dept+'/'+b.Sect+'/'+b.ContactNumber,'Self Care') as DegDepCell,a.Comments,a.CommentsDate
                        from RSM_ComplainLogDetails a WITH(NOLOCK) left outer join lnk.dbo.Emp_Details b WITH(NOLOCK) on a.UserID=b.EmpID
                        where a.RefNo='" + ticketNo + "' order by a.CommentsDate desc";

                var logs = await _rsmContext.ComplainLogDetails.FromSqlRaw(log_sql).ToListAsync();
                if (logs == null)
                {
                    throw new Exception("logs is empty.");
                }
                
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get Data",
                    Data = logs
                };
                await InsertRequestResponse(ticketNo, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketNo, ex, methodName, ip, userId, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetBillInformationFromHydra(string customerId, string userId, string ip)
        {
            var methodName = "RSMComplainTicketService/getBillInformationFromHydra";
            try
            {
                string hydraScript_sql = @"select ID, SQLText, SqlType from RSM_HydraScript where SqlType='PackageLoad'";

                var hydraScript = await _rsmContext.RSM_HydraScript.FromSqlRaw(hydraScript_sql).FirstOrDefaultAsync();
                if (hydraScript == null)
                {
                    throw new Exception("Hydra Script is empty.");
                }

                var sql_text = hydraScript.SQLText.Replace("xxxxxxx", customerId);

                var result = await PackageAndBalanceInfoFormOracalData(sql_text, customerId);
                
                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get Data",
                    Data = result
                };
                await InsertRequestResponse(customerId, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(customerId, ex, methodName, ip, userId, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetTechnicalInfoFromHydra(string customerId, string userId, string ip)
        {
            var methodName = "RSMComplainTicketService/GetTechnicalInfoFromHydra";
            try
            {
                string hydraScript_sql = @"select ID, SQLText, SqlType from RSM_HydraScript where SqlType='CustomerTechnicalInformation'";

                var hydraScript = await _rsmContext.RSM_HydraScript.FromSqlRaw(hydraScript_sql).FirstOrDefaultAsync();
                if (hydraScript == null)
                {
                    throw new Exception("Hydra Script is empty.");
                }

                var sql_text = hydraScript.SQLText.Replace("xxxxxxx", customerId);

                var result = await TeclicalInfoFormHydra(sql_text, customerId);

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get Data",
                    Data = result
                };
                await InsertRequestResponse(customerId, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(customerId, ex, methodName, ip, userId, ex.Message);
                throw new Exception(ex.Message);
            }
        }
        
        private async Task<HydraPackageAndBalanceResponseModel> PackageAndBalanceInfoFormOracalData(string sql, string customerId)
        {
            string oradb = "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.0.67)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = hydrast))); User Id=Ais_NET;Password=OxcjtkzYWxx125PnyQ3mqrR0;Connection Timeout=1800; ";
            var newObj = new HydraPackageAndBalanceResponseModel();
            var package_data_List = new List<HydraPackageInformationModel>();
            var balance_data = new HydraBalanceInfoModel();
            using (OracleConnection connection = new OracleConnection(oradb))
            {
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                OracleDataAdapter oraDataAdapterObj = new OracleDataAdapter();
                try
                {
                    command.CommandText = sql;
                    oraDataAdapterObj.SelectCommand = command;
                    DataTable dtora = new DataTable();
                    oraDataAdapterObj.Fill(dtora);
                    foreach (DataRow dr in dtora.Rows)
                    {
                        var package_data = new HydraPackageInformationModel();

                        package_data.SERVICE_NAME = dr["SERVICE_NAME"].ToString();
                        package_data.SERVICE_AMOUNT = Convert.ToInt32(dr["SERVICE_AMOUNT"]).ToString("N2");
                        package_data.MRC = "MRC";
                        package_data.SERVICE_END_DATE = dr["SERVICE_END_DATE"].ToString();
                        package_data.CUSTOMER_ID = dr["CUSTOMER_ID"].ToString();

                        package_data_List.Add(package_data);


                    }
                    command.CommandText = @"SELECT SUBJ.VC_CODE,SUBJ.N_SUBJECT_ID, ST.VC_SUBJ_STATE_NAME CUSTOMER_STATUS , NVL((SELECT MAX(BAL.N_SUM_BAL_END) FROM
                                        TABLE(SD_REPORTS_PKG.RPT_ACCOUNT_BALANCE(SUBJ.N_ACCOUNT_ID, SYSDATE,SYSDATE)) BAL),0)AS BALANCE
                                        FROM SI_SUBJ_ACCOUNTS SUBJ
                                        INNER JOIN SI_V_USERS_JR ST ON ST.N_SUBJECT_ID = SUBJ.N_SUBJECT_ID
                                        WHERE SUBJ.VC_CODE='" + customerId + "'";
                    oraDataAdapterObj.SelectCommand = command;
                    DataTable dtbal = new DataTable();
                    oraDataAdapterObj.Fill(dtbal);
                    if (dtbal.Rows.Count > 0)
                    {
                        balance_data.BALANCE = Convert.ToDecimal(dtbal.Rows[0]["BALANCE"]).ToString("N2");
                        balance_data.VC_CODE = dtbal.Rows[0]["VC_CODE"].ToString();
                        balance_data.N_SUBJECT_ID = dtbal.Rows[0]["N_SUBJECT_ID"].ToString();
                        balance_data.CUSTOMER_STATUS = dtbal.Rows[0]["CUSTOMER_STATUS"].ToString();
                    }

                    newObj.BalanceInfo = balance_data;
                    newObj.PackageInfo = package_data_List;
                    
                }
                catch (Exception ex)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new Exception(ex.Message);
                }
                connection.Close();
                connection.Dispose();
                return newObj;
            }
        }

        private async Task<ShowTechnicalInformationFromHydraModel> TeclicalInfoFormHydra(string sql, string customerId)
        {
            string oradb = "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.0.67)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = hydrast))); User Id=Ais_NET;Password=OxcjtkzYWxx125PnyQ3mqrR0;Connection Timeout=1800; ";
            var info = new ShowTechnicalInformationFromHydraModel();
            using (OracleConnection connection = new OracleConnection(oradb))
            {
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                OracleDataAdapter oraDataAdapterObj = new OracleDataAdapter();
                try
                {
                    command.CommandText = sql;
                    oraDataAdapterObj.SelectCommand = command;
                    DataTable dtora = new DataTable();
                    oraDataAdapterObj.Fill(dtora);
                    foreach (DataRow dr in dtora.Rows)
                    {
                        var vlan = dr["VLAN"].ToString();
                        if (!string.IsNullOrEmpty(vlan))
                        {
                            info.IP = dr["IPV4"].ToString();
                            string[] ipSplit = info.IP.Split('.');
                            info.Gateway = ipSplit[0] + "." + ipSplit[1] + "." + ipSplit[2] + "." + "1";
                            info.Subnet_Mask = ipSplit[0] + "." + ipSplit[1] + "." + ipSplit[2] + "." + "0/24";
                            info.VLAN = vlan;
                            info.OLT = dr["OLT"].ToString();
                            if (!string.IsNullOrEmpty(info.OLT))
                            {
                                if (dr["OLT"].ToString() != "")
                                {
                                    string[] g = dr["OLT"].ToString().Split('_');

                                    for (int k = 0; k < g.Length; k++)
                                    {
                                        if (g[k].Contains("/"))
                                        {
                                            string[] sp = g[k].Split('/');
                                            info.PON = sp[0].ToString();
                                            info.PORT = sp[1].ToString();
                                        }
                                    }
                                }
                                info.OLT_IP = dr["OLT_IP"].ToString();
                                info.BRAS = dr["BRAS"].ToString();
                                info.BRAS_IP = dr["BRAS_IP"].ToString();
                                info.Customer_MAC = dr["MAC"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new Exception(ex.Message);
                }
                connection.Close();
                connection.Dispose();
                return info;
            }
        }

        

        public async Task<ApiResponse> GetComplainTicektImportentData(string ticketId, string userId, string ip)
        {
            var methodName = "RSMComplainTicketService/GetComplainTicektImportentData";
            try
            {
                var ticketDetails_sql = "select * from RSM_Complain_Details where RefNo='" + ticketId + "'";
                var ticketDetails = await _rsmContext.RSM_Complain_Details.FromSqlRaw(ticketDetails_sql).FirstOrDefaultAsync();
                if (ticketDetails == null)
                {
                    throw new Exception("Ticket is not found");
                }
                RSMTicketSelectedDataResponseModel rSMTicketSelectedData = new RSMTicketSelectedDataResponseModel();

                List<string> closeTicketSupportType = new List<string>() {
                    "Phone Support",
                    "Physical Support",
                    "Back Office Support" ,
                    "Phone from Physical",
                    "Traceless Customer",
                    "Close Without Support"
                };
                rSMTicketSelectedData.CloseTicketSupportType = closeTicketSupportType;

                List<string> changeSupportType = new List<string>() {
                    "Phone Support",
                    "Physical Support",
                    "Traceless Customer",
                    "Customer Schedule waiting"
                };
                rSMTicketSelectedData.ChangeSupportType = changeSupportType;

                var followUPList = new List<tbl_HelpDeskCategoryModel>();

                string permission_sql = @"select distinct * from tbl_team_mem_permission WITH(NOLOCK) where Team_id='T-001' and Emp_id='" + userId + "'";
                var permissino = await _rsmContext.Team_Mem_Permissions.FromSqlRaw(permission_sql).FirstOrDefaultAsync();
                if (permissino != null)
                {
                    string qry = @"select ID, TicketsubNature from tbl_HelpDeskCategory where  TicketNatureID='4'";
                    followUPList = await _rsmContext.tbl_HelpDeskCategory.FromSqlRaw(qry).ToListAsync();
                }
                rSMTicketSelectedData.FollowUpList = followUPList;

                string pendingReasonSql = @"select ID,TaskPendingStatus from RSM_PendingTaskType where status='1' order by TaskPendingStatus";
                var pendingReasonList = await _rsmContext.RSM_PendingTaskType.FromSqlRaw(pendingReasonSql).ToListAsync();

                rSMTicketSelectedData.PendingReasonList = pendingReasonList;

                var selectReasonSql = @"select * from RSM_Complain_Details where refno='" + ticketId + "'";
                var rsm_Complain = await _rsmContext.RSM_Complain_Details.FromSqlRaw(selectReasonSql).FirstOrDefaultAsync();
                if (rsm_Complain != null)
                {
                    var pendingReasonSelectedId = rsm_Complain.PendingReasonID;

                    rSMTicketSelectedData.PendingReasonSelectedId = Convert.ToInt32(pendingReasonSelectedId);

                    DateTime pd = DateTime.Now;
                    DateTime fd = Convert.ToDateTime(rsm_Complain.ForwardDate);
                    TimeSpan ts = pd - fd;
                    int mm = Convert.ToInt32(ts.TotalMinutes);
                    if (mm > 60)
                    {
                        rSMTicketSelectedData.CauseOfDelayVisible = true;

                        string causeOfDelaySqls = @"select ID,CauseOfDelay from tbl_CasueOfDelay where SupportType='Phone Support' order by CauseOfDelay";
                        var phoneCauseOfDelayList = await _rsmContext.tbl_CauseOfDelay.FromSqlRaw(causeOfDelaySqls).ToListAsync();
                        rSMTicketSelectedData.PhoneSupportCasueOfDelayList = phoneCauseOfDelayList;
                    }

                    if (mm > 240)
                    {
                        rSMTicketSelectedData.CauseOfDelayVisible = true;

                        string causeOfDelaySqls = @"select ID,CauseOfDelay from tbl_CasueOfDelay where SupportType='Physical Support' order by CauseOfDelay";
                        var physicalCauseOfDelayList = await _rsmContext.tbl_CauseOfDelay.FromSqlRaw(causeOfDelaySqls).ToListAsync();
                        rSMTicketSelectedData.PhysicalSupportCasueOfDelayList = physicalCauseOfDelayList;
                    }
                }
                string sdq_sql = @"select c.TicketTypeName,b.NatureName from RSM_Complain_Details 
                                a inner join RSM_NatureSetup b on a.ComplainCategory=b.NatureID
                                inner join rsm_tickettype c on c.id=b.TaskTypeID
                                where a.RefNo='" + ticketId + "'";
                var ticketCategoryAndNeature = await _rsmContext.GetTicketCategoryAndNeature.FromSqlRaw(sdq_sql).FirstOrDefaultAsync();
                if (ticketCategoryAndNeature != null)
                {
                    rSMTicketSelectedData.TicketTypeName = ticketCategoryAndNeature.TicketTypeName;
                    rSMTicketSelectedData.NatureName = ticketCategoryAndNeature.NatureName;
                }
                string query = @"select ID,TicketTypeName from rsm_tickettype order by TicketTypeName";
                var ticketCategoryList = await _rsmContext.Rsm_tickettype.FromSqlRaw(query).ToListAsync();
                if (ticketCategoryList != null)
                {
                    rSMTicketSelectedData.TicketCategoriesList = ticketCategoryList;
                }

                string RSM_SupportOfficeWiseID_sql = @"select * from RSM_SupportOfficeWiseID where UserID='" + ticketId + "'";
                var RSM_SupportOfficeWiseID = await _rsmContext.RSM_SupportOfficeWiseID.FromSqlRaw(RSM_SupportOfficeWiseID_sql).FirstOrDefaultAsync();
                if(RSM_SupportOfficeWiseID != null)
                {
                    var rsm_sql = @"select top (1) a.* from RSM_Complain_Details a 
                        left outer join clientDatabaseMain b on a.CustomerID=b.MqID
                        left outer join RSM_SupportOfficeWiseID c on c.Support_OfficeID=b.brSupportOfficeId 
                        left outer join RSM_NatureSetup d on d.NatureID=a.ComplainCategory
                        left outer join RSM_TicketType t on t.ID=d.TaskTypeID
                        left outer join RSM_TaskPendingTeam tp on tp.Refno=a.RefNo 
                        where c.Support_OfficeID = '" + RSM_SupportOfficeWiseID.Support_OfficeID + "' and tp.PendingTeamID = '" + RSM_SupportOfficeWiseID.Team_ID + "' and tp.PendingTeamID<>'T-019' and tp.Status='0' and t.ID in('22','23') and a.RefNo='" + ticketId + "'";
                    var rsm_complain_detail = await _rsmContext.RSM_Complain_Details.FromSqlRaw(rsm_sql).FirstOrDefaultAsync();
                    
                    if(rsm_complain_detail != null)
                    {
                        rSMTicketSelectedData.DevicecollectionstatusVisible = true;
                        rSMTicketSelectedData.DeviceCollection = new List<string>() { "YES", "NO" };
                    }
                }
                var getSubInfo = await GetRSMSubcriberInfo(ticketDetails.CustomerID, userId, ip);
                rSMTicketSelectedData.SubcriberInformation = (RSMSubcriberInformationResponseModel)getSubInfo.Data;
                var getComplainLog = await GetRSMComplainTicketLogs(ticketId, userId, ip);
                rSMTicketSelectedData.TicketLog = (List<ComplainLogDetailsModel>)getComplainLog.Data;
                var tecnicalInfo = await GetTechnicalInfoFromHydra(ticketDetails.CustomerID, userId, ip);
                rSMTicketSelectedData.TechinicalInformation = (ShowTechnicalInformationFromHydraModel)tecnicalInfo.Data;
                var hydraBalanceAndPackageInfo = await GetBillInformationFromHydra(ticketDetails.CustomerID, userId, ip);
                rSMTicketSelectedData.HydraPackageAndBalanceInfo = (HydraPackageAndBalanceResponseModel)hydraBalanceAndPackageInfo.Data;

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = rSMTicketSelectedData
                };
                await InsertRequestResponse(ticketId, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketId, ex, methodName, ip, userId, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> CloseNatureList(int categoryId, string userId, string ip)
        {
            var methodName = "RSMComplainTicketService/CloseNatureList";
            try
            {
                string query = @"select NatureID,NatureName from RSM_NatureSetup where Status='1' and TaskTypeID='" + categoryId + "' order by NatureName";
                var closeNatureList = await _rsmContext.RSM_NatureSetup.FromSqlRaw(query).ToListAsync();
                if(closeNatureList == null)
                {
                    throw new Exception("Close nature list not found.");
                }

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = closeNatureList
                };
                await InsertRequestResponse(categoryId, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(categoryId, ex, methodName, ip, userId, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateRSMComplainPostReplay(PostReplayRequestModel model, string userId, string ip)
        {
            var methodName = "RSMComplainTicketService/UpdateRSMComplainPostReplay";
            try
            {
                var response = new ApiResponse();
                string urgentsupport = "No";
                string mbody = "------[ Intermediate Post Reply ]------" + "\n\n";

                if (model.chksupportType)
                {
                    if (string.IsNullOrEmpty(model.supporTypeName))
                    {
                        throw new Exception("Select Missing Support Type");
                    }
                    mbody = mbody + "Support Type update  : " + model.supporTypeName + "\n";
                }
                

                string genarateTicketSupportTypeQuery = "select GenerateTicketSupportType from RSM_Complain_Details where TaskStatus = '0' "+
                    "and PendingTeamID = 'T-007' and RefNo = '" + model.ticketNo+ "'";
                var genarateTicketSupportTypeData = await _rsmContext.GetGenerateTicketSupportType.FromSqlRaw(genarateTicketSupportTypeQuery).FirstOrDefaultAsync();
                if (genarateTicketSupportTypeData != null)
                {
                    mbody = mbody + "Update from  :" + genarateTicketSupportTypeData.GenerateTicketSupportType + "\n";
                }
                
                if(string.IsNullOrEmpty(model.postText)) 
                {                    
                    throw new Exception("Input missing Post Reply");
                }
                mbody = mbody + "Post Reply   : " + model.postText + "\n";

                if (model.isFollowUpVisible)
                {
                    if (string.IsNullOrEmpty(model.followUpName))
                    {
                        throw new Exception("Select missing Followup call");
                    }
                }

                if (string.IsNullOrEmpty(model.pendingReasonName))
                {
                    throw new Exception("Select missing Pending Reason");
                }

                if (model.chksupportType)
                {
                    if (model.supporTypeName == "Customer Schedule waiting")
                    {
                    
                        if (DateTime.Now > model.sheduleDate)
                        {                         
                            throw new Exception("Invalid Datetime selection");
                        }
                    }
                }
                if (model.chkIsUrgent)
                {
                    urgentsupport = "Yes";
                }
                


                //string sql = "EXEC UpdatePostReplay @ticketNo,@checkSupportType,@supporTypeName,@mBody,@followUpName,@pendingReasonId,@urgentSupport, " +
                //   "@tokenID,@userId,@sheduleDate,@dateAndTime,@textPostReplay,@isFollowUpVisible,@followUpId,@vicidialId";

                string sql = "EXEC UpdatePostReplay '"+ model.ticketNo + "','" + model.chksupportType.ToString() + "','"+ model.supporTypeName + "','" + mbody + "','"+ model.followUpName + "','"+ model.pendingReasonId + "','"+ urgentsupport + "', "+
                    "'"+ model.tokenID + "','"+ userId + "','"+ model.sheduleDate + "','"+ model.postText + "','"+ model.isFollowUpVisible + "','"+ model.followUpId + "','"+ model.vicidialId + "'";

                //List<SqlParameter> parms = new List<SqlParameter>
                //{
                //    // Create parameters
                //    new SqlParameter { ParameterName = "@ticketNo", Value = model.ticketNo },
                //    new SqlParameter { ParameterName = "@checkSupportType", Value = model.chksupportType },
                //    new SqlParameter { ParameterName = "@supporTypeName", Value = model.supporTypeName },
                //    new SqlParameter { ParameterName = "@mBody", Value = mbody },
                //    new SqlParameter { ParameterName = "@followUpName", Value = model.followUpName },
                //    new SqlParameter { ParameterName = "@pendingReasonId", Value = model.pendingReasonId },
                //    new SqlParameter { ParameterName = "@urgentSupport", Value = urgentsupport },
                //    new SqlParameter { ParameterName = "@tokenID", Value = model.tokenID },
                //    new SqlParameter { ParameterName = "@userId", Value = userId },
                //    new SqlParameter { ParameterName = "@sheduleDate", Value = timeData2 },
                //    new SqlParameter { ParameterName = "@dateAndTime", Value = timeData },
                //    new SqlParameter { ParameterName = "@textPostReplay", Value = model.postText },
                //    new SqlParameter { ParameterName = "@isFollowUpVisible", Value = model.isFollowUpVisible },
                //    new SqlParameter { ParameterName = "@followUpId", Value = model.followUpId },
                //    new SqlParameter { ParameterName = "@vicidialId", Value = model.vicidialId },
                //};

               // var result = await _rsmContext.GetPostReplayResponse.FromSqlRaw(sql, parms.ToArray()).AsNoTracking().ToListAsync();
                var result = await _rsmContext.GetPostReplayResponse.FromSqlRaw(sql).AsNoTracking().ToListAsync();
                if (result.Count > 0)
                {
                    if (result[0].SuccessOrErrorMessage != "Success")
                    {
                        throw new Exception(result[0].SuccessOrErrorMessage);
                    }
                }

                response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get Data",
                    Data = result
                };
                await InsertRequestResponse(model, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, ip, userId, ex.Message);
                throw new Exception(ex.Message);
            }
        }


        public async Task<ApiResponse> CloseTicket(RSMCloseTicketRequestModel model, string userId, string ip)
        {
            var methodName = "RSMComplainTicketService/CloseTicket";
            try
            {
                if (model.CauseofdelayVisible == true)
                {
                    if (model.CauseofdelayValue == 0)
                    {
                        throw new Exception("Please select Cause of Delay");
                    }
                }

                if (model.DevicecollectionstatusVisible == true)
                {
                    if (string.IsNullOrEmpty(model.DeviceCollectionValue))
                    {
                        throw new Exception("Select missing Device status");
                    }
                }

                string assign_sql = @"select * from RSM_TaskAssign where RefNo='" + model.TicketId + "'";
                var assign = await _rsmContext.RSM_TaskAssign.FromSqlRaw(assign_sql).FirstOrDefaultAsync();
                if (assign == null)
                {
                    throw new Exception("This ticket didnot assign Engineer. Please assign Engineer");
                }

                string mbody = ""; 
                string smsmob = "";

                mbody = "------[Ticket Closed]------" + "\n\n";
                mbody = mbody + "Closing Comments  : " + model.Comment + "\n";

                string complain_Details_sql = @"select a.RefNo, a.SourceMobileNo, a.ComplainSource, a.TaskStatus, b.Status from RSM_Complain_Details a 
                                                inner join RSM_TaskPendingTeam b on a.RefNo = b.RefNo  
                                                where  a.RefNo='" + model.TicketId + "' and (b.Status = 0 or a.taskStatus = 0)";
                var complain_Details = await _rsmContext.RSM_Complain_Details_Ticket.FromSqlRaw(complain_Details_sql).FirstOrDefaultAsync();                
                if (complain_Details == null)
                {
                    throw new Exception("Complain Details is not found.");
                }
                else
                {
                    if(complain_Details.TaskStatus == "1" && complain_Details.Status == 1)
                    {
                        throw new Exception("Some one has already close this Ticket");
                    }
                    else
                    {
                        smsmob = complain_Details.ComplainSource;
                    }
                }
                string mob = "";
                if (smsmob != "Client (by Phone)")
                {
                    mob = model.RestoreMobileNo;
                }
                else
                {
                    mob = complain_Details.SourceMobileNo;
                    if (mob == "")
                    {
                        mob = model.RestoreMobileNo;
                    }
                }
                string tid = "";
                string techid_sql = @"select a.PendingTeamID from RSM_TaskPendingTeam a inner join RSM_SupportOfficeWiseID b on a.PendingTeamID=b.Team_ID
                            where b.UserID='" + userId + "' and a.RefNo='" + model.TicketId + "' and a.Status='0' group by a.PendingTeamID";

                var techid = await _rsmContext.RSM_TaskPendingTeam.FromSqlRaw(techid_sql).FirstOrDefaultAsync();
                if (techid != null)
                {
                    tid = techid.PendingTeamID;
                }

                if (tid == "")
                {
                    string techid2_sql = @"select a.* from tbl_team_mem_permission a inner join RSM_TaskPendingTeam b on a.Team_id=b.PendingTeamID
                          where Emp_id='" + userId + "' and b.RefNo='" + model.TicketId + "' and b.Status='0'";
                    var techid2 = await _rsmContext.tbl_team_mem_permission.FromSqlRaw(techid2_sql).FirstOrDefaultAsync();
                    if (techid2 != null)
                    {
                        tid = techid2.Team_id;
                    }
                }
                if (tid == "")
                {
                    throw new Exception("You can not close this ticket, Because this ticket pending to another department");
                }

                if (model.TicketCategory != 8)
                {
                    if (model.TicketCategory != 23)
                    {
                        string RSM_SupportOfficeWiseID_sql = @"select * from RSM_SupportOfficeWiseID where UserID='" + userId + "' and Team_ID='T-077'";
                        var RSM_SupportOfficeWiseID = await _rsmContext.RSM_SupportOfficeWiseID.FromSqlRaw(RSM_SupportOfficeWiseID_sql).FirstOrDefaultAsync();
                        if (RSM_SupportOfficeWiseID != null)
                        {
                            if (string.IsNullOrEmpty(model.Color))
                            {
                                throw new Exception("Customer color is not updated.");
                            }
                        }
                    }
                }
                string time = string.Format("{0:HH:mm:ss tt}", DateTime.Now);
                string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                string strDateTime = date + " " + time;
                string TokenID = model.TokenID;

                string sp_sqlstr = "EXEC RSMCloseTicketApi '" + model.SupportType + "', '" + model.TicketId + "', '" + userId + "'," +
                    "'" + TokenID + "' ,'" + mbody.Replace("'", "''") + "', '" + strDateTime + "', '" + model.ChangeNature + "', '" + model.Comment.Replace("'", "''") + "', '" + model.CauseofdelayVisible + "'," +
                    "'" + model.CauseofdelayValue + "', '" + model.DevicecollectionstatusVisible + "', '" + model.DeviceCollectionValue + "', '" + tid + "', '" + model.CustomerID + "'";

                var result = await _rsmContext.SP_RSMCloseTicketApi.FromSqlRaw(sp_sqlstr).AsNoTracking().ToListAsync();

                //string sp_sql = @"EXEC RSMCloseTicketApi @supportType, @ticketId, @userid," +
                //    "@TokenID, @mbody, @strDateTime, @ChangeNature, @Comment, @CauseofdelayVisible," +
                //    "@CauseofdelayValue, @DevicecollectionstatusVisible, @DeviceCollectionValue, @tid, @CustomerID";


                //List<SqlParameter> paramiters = new List<SqlParameter>
                //{
                //    // Create parameters
                //    new SqlParameter { ParameterName = "@supportType", Value = model.SupportType },
                //    new SqlParameter { ParameterName = "@ticketId", Value = model.TicketId },
                //    new SqlParameter { ParameterName = "@userid", Value = userId },
                //    new SqlParameter { ParameterName = "@TokenID", Value = TokenID },
                //    new SqlParameter { ParameterName = "@mbody", Value = mbody.Replace("'", "''") },
                //    new SqlParameter { ParameterName = "@strDateTime", Value = strDateTime },
                //    new SqlParameter { ParameterName = "@ChangeNature", Value = model.ChangeNature },
                //    new SqlParameter { ParameterName = "@Comment", Value = model.Comment.Replace("'", "''") },
                //    new SqlParameter { ParameterName = "@CauseofdelayVisible", Value = model.CauseofdelayVisible },
                //    new SqlParameter { ParameterName = "@CauseofdelayValue", Value = model.CauseofdelayValue },
                //    new SqlParameter { ParameterName = "@DevicecollectionstatusVisible", Value = model.DevicecollectionstatusVisible },
                //    new SqlParameter { ParameterName = "@DeviceCollectionValue", Value = model.DeviceCollectionValue },
                //    new SqlParameter { ParameterName = "@tid", Value = tid },
                //    new SqlParameter { ParameterName = "@CustomerID", Value = model.CustomerID },
                //};

                //var result = await _rsmContext.SP_RSMCloseTicketApi.FromSqlRaw(sp_sql, paramiters.ToArray()).AsNoTracking().ToListAsync();
                if (result.Count > 0)
                {
                    if (result[0].SuccessOrErrorMessage != "Success")
                    {
                        throw new Exception(result[0].SuccessOrErrorMessage);
                    }
                }

                await BingeServiceactivate(model.TicketId, Convert.ToDecimal(model.Balance), model.CustomerID); //Hydra Database Related Work

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data"
                };
                await InsertRequestResponse(model, response, methodName, ip, userId, null);
                return response;
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(model, ex, methodName, ip, userId, ex.Message);
                throw new Exception(ex.Message);
            }
        }


        private async Task BingeServiceactivate(string ticketId, decimal balance, string customerId)
        {
            var methodName = "RSMComplainTicketService/BingeServiceactivate";
            try
            {
                string sql = @"select b.DeviceName,b.Price+350 as Price
                        from tbl_BingeServiceDetails a inner join tbl_BingeDevice b on a.ServiceName=b.DeviceName
                        where a.ReFno='" + ticketId + "' and a.Status='1'";
                var queryResult = await _rsmContext.tbl_BingeServiceDetails.FromSqlRaw(sql).FirstOrDefaultAsync();

                if (queryResult != null)
                {
                    if (queryResult.Price > balance)
                    {
                        throw new Exception("Sufficient balance not exist");
                    }
                }

                if (queryResult != null)
                {
                    string oradb = "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.0.67)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = hydrast))); User Id=Ais_NET;Password=OxcjtkzYWxx125PnyQ3mqrR0;Connection Timeout=1800; ";
                    using (OracleConnection connection = new OracleConnection(oradb))
                    {
                        connection.Open();
                        OracleCommand command = connection.CreateCommand();
                        OracleTransaction transaction;
                        transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                        command.Transaction = transaction;

                        try
                        {
                            command.CommandText = @"SELECT SUBJ.VC_CODE,SUBJ.N_SUBJECT_ID, ST.VC_SUBJ_STATE_NAME CUSTOMER_STATUS , NVL((SELECT MAX(BAL.N_SUM_BAL_END) FROM
                                        TABLE(SD_REPORTS_PKG.RPT_ACCOUNT_BALANCE(SUBJ.N_ACCOUNT_ID, SYSDATE,SYSDATE)) BAL),0)AS BALANCE
                                        FROM SI_SUBJ_ACCOUNTS SUBJ
                                        INNER JOIN SI_V_USERS_JR ST ON ST.N_SUBJECT_ID = SUBJ.N_SUBJECT_ID
                                        WHERE SUBJ.VC_CODE='" + customerId + "'";
                            OracleDataAdapter oraDataAdapterObj = new OracleDataAdapter();
                            oraDataAdapterObj.SelectCommand = command;
                            DataTable dtbal = new DataTable();
                            oraDataAdapterObj.Fill(dtbal);
                            if (dtbal.Rows.Count > 0)
                            {
                                balance = Convert.ToDecimal(dtbal.Rows[0]["BALANCE"]);
                            }

                            string hsql = @"select * from RSM_HydraScript where SqlType='Hoichoi (Free Subscription)'";
                            var hydraScript = await _rsmContext.RSM_HydraScript.FromSqlRaw(hsql).FirstOrDefaultAsync();

                            if (hydraScript != null)
                            {
                                string CustomerID = "", ContructID = "", ObjectID = "", doc_ID = "", N_Account_ID = "";

                                command.CommandText = @"select  SI.VC_CODE, SI.N_ACCOUNT_ID, SI.N_SUBJECT_ID,SG.n_doc_id, SG.n_object_id from SI_SUBJ_ACCOUNTS SI
                                                    inner join SI_SUBSCRIPTIONS SG ON SI.N_SUBJECT_ID = SG.N_SUBJECT_ID
                                                    AND SG.C_FL_CLOSED = 'N' AND SG.D_BEGIN <= SYSDATE
                                                     AND SG.N_PAR_SUBSCRIPTION_ID IS NULL AND SG.C_ACTIVE  = 'Y'
                                                     WHERE  SI.VC_CODE='" + customerId + "' " +
                                                        " group by SI.VC_CODE, SI.N_ACCOUNT_ID, SI.N_SUBJECT_ID,SG.n_doc_id, SG.n_object_id";
                                OracleDataAdapter oraDataAdapterObj2 = new OracleDataAdapter();
                                oraDataAdapterObj2.SelectCommand = command;
                                DataTable dtora = new DataTable();
                                oraDataAdapterObj2.Fill(dtora);

                                if (dtora.Rows.Count > 0)
                                {
                                    CustomerID = dtora.Rows[0]["N_SUBJECT_ID"].ToString();
                                    ContructID = dtora.Rows[0]["N_DOC_ID"].ToString();
                                    ObjectID = dtora.Rows[0]["n_object_id"].ToString();
                                    N_Account_ID = dtora.Rows[0]["N_ACCOUNT_ID"].ToString();
                                }

                                // string ddmmyy = DateTime.Now.ToString("dd/MM/yyyy h:mm:ss tt");
                                var ddmmyy = await DatetimeData();

                                string a = hydraScript.SQLText.ToString().
                                           Replace("abcabcabc", CustomerID).
                                           Replace("defdefdef", N_Account_ID).
                                           Replace("ghighighi", ObjectID).
                                           Replace("klmklmklm", ContructID).
                                           Replace("Hoichoi (Free Subscription)", "Binge").
                                           Replace("ddddddd 00:00:00", ddmmyy);

                                command.CommandText = a;
                                command.ExecuteNonQuery();

                                string a2 = hydraScript.SQLText.ToString().
                                           Replace("abcabcabc", CustomerID).
                                           Replace("defdefdef", N_Account_ID).
                                           Replace("ghighighi", ObjectID).
                                           Replace("klmklmklm", ContructID).
                                           Replace("Hoichoi (Free Subscription)", queryResult.DeviceName).
                                           Replace("ddddddd 00:00:00", ddmmyy);

                                command.CommandText = a2;
                                command.ExecuteNonQuery();

                                transaction.Commit();

                                string ssl = @"update WFA2.dbo.tbl_BingeServiceDetails set status='0' where Refno='" + ticketId + "'";
                                var result = _rsmContext.Database.ExecuteSqlRaw(ssl);

                            }

                        }

                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                await InsertRequestResponse(ticketId, ex, methodName, null, null, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task<string> DatetimeData()
        {
            DateTime dtu = DateTime.Now;
            string dformat = string.Format("{00:00}", dtu.Day) + "/" + string.Format("{00:00}", dtu.Month) + "/" + dtu.Year.ToString() + " " + string.Format("{00:00}", dtu.Hour) + ":" + string.Format("{00:00}", dtu.Minute) + ":" + string.Format("{00:00}", dtu.Second);
            return dformat;
        }



    }
}

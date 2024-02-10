using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Migrations.ClientAPIDB;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.RequestModel.Client;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client.Hydra;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client.RSM;
using L3T.Infrastructure.Helpers.Repositories.Interface.Client;
using L3T.Infrastructure.Helpers.Services.ServiceInterface;
using L3T.Utility.Helper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Repositories.Implementation.Client
{
    public class TrackingIssueRepository : ITrackingIssueRepository
    {
        private readonly MisDBContext _misDBContext;
        private readonly RsmDBContext _rsmcontext;
        private readonly HydraLocalDBContext _hydracontext;
        private readonly IClientReqResService _reqResService;
        public TrackingIssueRepository(MisDBContext misLiveDBContext, RsmDBContext rsmcontext, IClientReqResService reqResService, HydraLocalDBContext hydracontext)
        {
            _misDBContext = misLiveDBContext;
            _rsmcontext = rsmcontext;
            _reqResService = reqResService;
            _hydracontext = hydracontext;
        }

      
        public async Task<List<TicketListResponseModel>> GetAllTicketWithFilter(TicketListRequestModel model)
        {
            try
            {
                var sql = $@"exec dbo.GetAllKindOfTicket '{model.FromDate}', '{model.ToDate}', '{model.DateTimeSearchTable}', 
                            '{model.AssignEmpId}', '{model.AssignEmpName}', '{model.TicketId}', '{model.CustomerId}', '{model.CustomerName}', 
                            '{model.CustomerPhone}', '{model.CustomerEmail}', '{model.Area}', '{model.SupportOffice}', '{model.Status}', 
                            '{model.TicketCatagory}', '{model.OrderByTableName}', '{model.DescOrAsc}', {model.PageNumber}, {model.PageSize} ";
                var getMaxTicketId = await _misDBContext.TicketListResponseModel.FromSqlRaw(sql).ToListAsync();

                return getMaxTicketId;

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            

        }



        public async Task<RsmProfileResModel> RsmProfileView(string subscriber_codeUpper)
        {
            return await _rsmcontext.Rsm_Profile_View.Where(t => t.CustomerID == subscriber_codeUpper.Trim()).FirstOrDefaultAsync();
        }


        public async Task<ApiResponse> AddRepositoryTicketRsm(TicketCreateReqModel ReqModel)
        {
            var response = new ApiResponse();
            var methordName = "TrackingIssueRepository/AddRepositoryTicketRsm";
            var subscriber_code = ReqModel.subscriberCode.Replace(" ", "");
            var subscriber_codeUpper = subscriber_code.ToUpper();
            var CustomerPrefix = subscriber_codeUpper.Substring(0, 3).ToLower();
            string service_team = "", team_Name = "";
            bool isDRopCall = false;
            var priority = "";
            var dropcallId = "";


            int nid = Convert.ToInt32(ReqModel.natureId);

            service_team = _rsmcontext.RSM_NatureSetup.Where(t => t.NatureID == nid && t.Team_ID != null).FirstOrDefault().Team_ID;

            team_Name = _rsmcontext.tbl_Team_info.Where(t => t.Team_id == service_team.Trim()).FirstOrDefault().Team_Name;

            var transaction = await _rsmcontext.Database.BeginTransactionAsync();
            try
            {

                RSM_Complain_Details rsmComplainData = new RSM_Complain_Details();

                rsmComplainData.RefNo = (_rsmcontext.RSM_Complain_Details.Max(t => (int?)t.RefNo) ?? 0) + 1;
                rsmComplainData.CustomerID = ReqModel.subscriberCode.Trim();
                rsmComplainData.ReceiveDateTime = DateTime.Now;


                rsmComplainData.ActualReceiveDatetime = DateTime.Now;
                rsmComplainData.ComplainCategory = ReqModel.natureId.Trim();
                rsmComplainData.Complains = ReqModel.description + " [Priority:" + priority + "]";

                //if (string.IsNullOrWhiteSpace(dropcallId))
                //{
                //    rsmComplainData.ComplainSource = ComplainSource.SelfCare.ToString();
                //}
                //else
                //{
                //    rsmComplainData.ComplainSource = ComplainSource.DropCall.ToString();
                //    isDRopCall = true;
                //}
                rsmComplainData.ComplainSource = "Janata Wifi";

                rsmComplainData.PendingTeamID = service_team;
                rsmComplainData.ComplainReceiveby = "API";
                rsmComplainData.TaskStatus = "0";
                rsmComplainData.Comments = ReqModel.description;
                rsmComplainData.LastComments = ReqModel.description;
                rsmComplainData.LedStatus = "";
                rsmComplainData.LastUpdateDate = DateTime.Now;
                rsmComplainData.FaultOccured = DateTime.Now;
                rsmComplainData.ForwardDate = DateTime.Now;
                rsmComplainData.AssignEngineer = "No";
                rsmComplainData.CauseOfDelay = 0;
                rsmComplainData.vicidial_call_id = dropcallId ?? "";

                await _rsmcontext.RSM_Complain_Details.AddAsync(rsmComplainData);
                _rsmcontext.SaveChangesAsync();

                await transaction.CreateSavepointAsync("SavedRsmTicket"); // Use Transactio

                var rsmComplainLogDbReqModel = new RSM_ComplainLogDetails()
                {
                    Comments = ReqModel.description + " [Forward To:" + team_Name + "]",
                    CommentsDate = DateTime.Now,
                    RefNo = rsmComplainData.RefNo,
                    UserID = "API"
                };

                await _rsmcontext.RSM_ComplainLogDetails.AddAsync(rsmComplainLogDbReqModel);
                await _rsmcontext.SaveChangesAsync();

                var rsmtaskp = new RSM_TaskPendingTeam()
                {
                    RefNo = rsmComplainData.RefNo.ToString(),
                    PendingTeamID = service_team,
                    Status = 1,
                    StartDate = DateTime.Now
                };

                await _rsmcontext.RSM_TaskPendingTeam.AddAsync(rsmtaskp);

                var res = await _rsmcontext.SaveChangesAsync();


                //if (isDRopCall)
                //{

                //    string qry = @"update vicidial_closer_log set ref_no = '" + com.RefNo + "' where uniqueid = '" + tic.dropcall_id + "'";
                //    DataProcess.UpdateMySqlQuery("Data Source=203.76.96.84;Database=asterisk;User ID=cron;Password=1234", qry);
                //}

                transaction.Commit();  // Use Transaction  

                if (res > 0)
                {
                    response.Status = "Success";
                    response.StatusCode = 200;
                    response.Message = "Ticket create successfully!";
                    response.Data = rsmComplainData.RefNo.ToString();
                   // _logger.LogInformation("AddTicketRsmOrMis : " + response);
                    return response;
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackToSavepointAsync("SavedRsmTicket");
               // _logger.LogInformation("Exception " + ex.Message.ToString());
                return await _reqResService.BadRequest(ex.Message);

            }
            throw new ApplicationException("Ticket is not created successlly.");
        }



        public async Task<ApiResponse> AddRepositoryTicketMis(TicketCreateReqModel ReqModel)
        {
            var response = new ApiResponse();
            var methordName = "TrackingIssueRepository/AddRepositoryTicketMis";
            decimal categoryId = Convert.ToInt32(ReqModel.natureId);



            var clientdata = await _misDBContext.ClientDatabaseMain.Where(t => t.brCliCode == ReqModel.subscriberCode).FirstOrDefaultAsync();

            if (clientdata == null)
            {
                throw new ApplicationException("Invalid Subscriber ID.");
            }

            var misCategorydata = await _misDBContext.Tbl_Com_Category.Where(t => t.C_id == categoryId).FirstOrDefaultAsync();

            if (misCategorydata == null)
            {
                throw new ApplicationException("Category not found.");
            }

            var misTicketRefGenerate = "EXEC max_com_Ticketref";
            var generateStr = await _misDBContext.Max_com_Ticketref.FromSqlRaw(misTicketRefGenerate).AsNoTracking().ToListAsync();

            if (generateStr.Count == 0)
            {
                throw new ApplicationException("Generate Ticket Ref problem");
            }
            var aGenerateStr = generateStr.FirstOrDefault();

            //var sqlTmp = @"select  min(tid) as MinID FROM tmpTID where dflg = 'N' and pflg = 'N' ";
            //var mistmpdata = _miscontext.tmpTID.FromSqlRaw(sqlTmp).FirstOrDefault();

            //var mistmpdata = await _miscontext.TmpTID.Where(x => x.dflg == "N" && x.pflg == "N").MinAsync(t => t.tid);


            int brslno = Convert.ToInt32(clientdata.brSlNo);
            var misCategoryName = misCategorydata.Com_Category.ToString();
            var complaincomments = ReqModel.description;
            var RelatedDepId = misCategorydata.RelatedDepID;
            var RelatedDeptName = misCategorydata.RelatedDeptName.ToString();
            var BrAdrCode = clientdata.brAdrCode.ToString();

            var OfficeName = clientdata.HeadOfficeName.ToString() + ":" + clientdata.BranchName.ToString().Replace("Head Office", "");
            var Address = clientdata.brAddress1.ToString() + "," + clientdata.brAddress2.ToString() + clientdata.brAreaGroup.ToString() + "," + clientdata.brArea.ToString();
            var contact_det = clientdata.contact_det.ToString();
            var phone_no = clientdata.phone_no.ToString();
            var email_id = clientdata.email_id.ToString();
            var brCategory = clientdata.brCategory.ToString();
            var brAreaGroup = clientdata.brAreaGroup.ToString();

            var str = aGenerateStr.maxref.ToString(); //"L3-" + DateTime.Now.Date.ToString("ddMMMyyyy") + "-" + mistmpdata.ToString().PadLeft(5, '0');

            var requestModelMisTicket = new MisTicketGenerateReqModel()
            {
                misSubscriberCode = ReqModel.subscriberCode,
                brslno = brslno,
                misCategoryName = misCategoryName,
                complaincomments = complaincomments,
                RelatedDepId = RelatedDepId,
                BrAdrCode = BrAdrCode,
                RelatedDeptName = RelatedDeptName,
                OfficeName = OfficeName,
                Address = Address,
                str = str,
                contact_det = contact_det,
                phone_no = phone_no,
                email_id = email_id,
                brCategory = brCategory,
                brAreaGroup = brAreaGroup
            };

            var res = await MisTicketGenerate(requestModelMisTicket);


            if (res > 0)
            {
                var getTicketId = await _misDBContext.Tbl_complain_info.Where(x => x.comp_info_client_adrcode == ReqModel.subscriberCode).OrderBy(x => x.comp_info_last_update).LastOrDefaultAsync();

                response.Status = "Success";
                response.StatusCode = 200;
                response.Message = "Ticket create successfully!";
                response.Data = getTicketId.comp_info_ref_no.ToString();
              //  _logger.LogInformation("AddTicketRsmOrMis : " + response);
                return response;
            }
            throw new ApplicationException("Ticket is not created successlly.");
        }



        private async Task<int> MisTicketGenerate(MisTicketGenerateReqModel misTicketreq)
        {
            var transaction = _misDBContext.Database.BeginTransaction();
            try
            {
                //DateTime aDateTime = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("h:mm tt"));

                // DateTime aaDateTime = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd"));

               // int textMinId = Convert.ToInt32(misTicketreq.minId);

                string strMailbody = " ";
                strMailbody = "------------------[Ticket Open From Janata-Wifi ]-------------------\n\n";
                strMailbody = (((((strMailbody + "Ticket was forward to    : " + misTicketreq.RelatedDeptName + "\n") + "Client Name              : " + misTicketreq.OfficeName.ToString() + "\n") + "Problem Category         : " + misTicketreq.misCategoryName + "\n") + "Source of Information    : Janata-Wifi \n") + "Client inform us         : " + misTicketreq.complaincomments + "\n") + "Preliminary Findings     : N/A\n" + "LED Status               : N/A\n";
                strMailbody = strMailbody + "Fault Occured Time       : " + DateTime.Now.Date.ToString("dd/MMM/yyyy") + " " + DateTime.Now.ToString("h:mm tt") + " [" + DateTime.Now.ToString() + "]\n";
                strMailbody = strMailbody + "Complain Acknowledge Time: " + DateTime.Now.Date.ToString("dd/MMM/yyyy") + " " + DateTime.Now.ToString("h:mm tt") + " [" + DateTime.Now.ToString() + "]";



                var insetDatacomplainInfo = @$"INSERT INTO [tbl_complain_info] ([comp_info_type], 
                                            [comp_info_ref_no], [comp_info_com_name], [comp_info_client_id], 
                                            [comp_info_client_name],[comp_info_client_adr], [comp_info_contact_person], 
                                            [comp_info_con_phone_no], [comp_info_con_email], [comp_info_email_to_client],
                                            [comp_info_Receive_Date], [comp_info_Receive_Time], 
                                            [comp_info_Receive_ActualTime], [comp_info_service_code], 
                                            [comp_info_service_desc],[comp_info_Category], [comp_info_complain], 
                                            [comp_info_Source_Information], [comp_info_Led_Status], [comp_info_deadline], 
                                            [comp_info_related_dept],[comp_info_manually_email], [comp_info_receive_by], 
                                            [comp_info_comm], [comp_info_attachments], [comp_info_resolve_status], 
                                            [comp_info_client_slno],[comp_info_client_adrcode], [comp_info_client_Media], 
                                            [comp_info_mkt_person], [comp_info_last_update],[comp_info_postponed_flg], 
                                            [comp_info_postponed_hour], [comp_info_hold_on], [state], [receiveby],
                                            [Clientcategory],[CallerID]) 
                                            VALUES ('complain', @str, 
                                            @OfficeName,@misSubscriberCode, 
                                            @OfficeName1,@Address,@contact_det,
                                            @phone_no,@email_id,'N'
                                            ,Convert(datetime,'{DateTime.Now.Date.ToString("dd/MM/yyyy")}',103),
                                            '{DateTime.Now.ToString("h:mm tt")}','{DateTime.Now.ToString("yyyy-MM-dd h:mm tt")}','','',@misCategoryName,
                                            '{misTicketreq.complaincomments.Replace("'", "''")}','Janata-Wifi ','N/A', 
                                            '{DateTime.Now.ToString("yyyy-MM-dd h:mm tt")}',@RelatedDepId,'', 
                                            'L3T683:S.M. MAMUNUR RASHID', 'Please take care.','fileattachment', 'Pending',
                                            @brslno,@BrAdrCode,'','', 
                                            '{DateTime.Now.ToString("yyyy-MM-dd h:mm tt")}','N','0','Not yet Assign',
                                            '1','L3T683',@brCategory,'NA')";

                _misDBContext.Database.ExecuteSqlRaw(insetDatacomplainInfo,
                                         new SqlParameter("str", misTicketreq.str),
                                         new SqlParameter("OfficeName", misTicketreq.OfficeName),
                                         new SqlParameter("misSubscriberCode", misTicketreq.misSubscriberCode),
                                         new SqlParameter("OfficeName1", misTicketreq.OfficeName),
                                         new SqlParameter("Address", misTicketreq.Address),
                                         new SqlParameter("contact_det", misTicketreq.contact_det),
                                         new SqlParameter("phone_no", misTicketreq.phone_no),
                                         new SqlParameter("email_id", misTicketreq.email_id),
                                         new SqlParameter("misCategoryName", misTicketreq.misCategoryName),
                                         new SqlParameter("RelatedDepId", misTicketreq.RelatedDepId),
                                         new SqlParameter("brslno", misTicketreq.brslno),
                                         new SqlParameter("BrAdrCode", misTicketreq.BrAdrCode),
                                         new SqlParameter("brCategory", misTicketreq.brCategory));


                transaction.CreateSavepoint("SavedMisTicket");


                var insertDataTicketForwardHistory = @$"INSERT INTO [tbl_Ticket_ForwardHistory] ([ticket_No],
                                                    [ticket_ForwordTime], [ticket_ForwardBy], [ticket_ForwardFromTeam],
                                                    [ticket_ForwardToTeam],[ticket_ForwardToTeamName],
                                                    [ticket_ForwardComments], [ticket_Forwarddatetime], 
                                                    [ticket_ForwardActualtime], [ticket_ForwardInformingper],
                                                    [ticket_last_update], [ticket_postponed_hour], [state])
                                                    VALUES (@str,'{DateTime.Now.Date.ToString("yyyy-MM-dd h:mm tt")}',
                                                    'S.M. MAMUNUR RASHID','T-001',@RelatedDepId,
                                                    @RelatedDeptName,'Please Take Care',
                                                    '{DateTime.Now.Date.ToString("yyyy-MM-dd h:mm tt")}',
                                                    '{DateTime.Now.Date.ToString("yyyy-MM-dd h:mm tt")}','N/A',
                                                     '{DateTime.Now.Date.ToString("yyyy-MM-dd h:mm tt")}','0','1')";

                _misDBContext.Database.ExecuteSqlRaw(insertDataTicketForwardHistory,
                                         new SqlParameter("str", misTicketreq.str),
                                         new SqlParameter("RelatedDepId", misTicketreq.RelatedDepId),
                                         new SqlParameter("RelatedDeptName", misTicketreq.RelatedDeptName));



                var clientDataCheck = _misDBContext.T_Client.Where(t => t.client_ID == misTicketreq.misSubscriberCode).FirstOrDefault();

                if (clientDataCheck == null)
                {
                    var insertDataTClient = @$"INSERT INTO t_Client(client_ID,client_CompanyName,client_ContactPerson,
                                            client_Email,client_Phone,client_Location,client_ContactDate)
                                            values(@misSubscriberCode,@OfficeName,
                                            @contact_det,
                                            @email_id,@phone_no,
                                            '{misTicketreq.Address.Replace("'", "''")}',
                                            '{DateTime.Now.Date.ToString("yyyy-MM-dd h:mm tt")}')";


                    _misDBContext.Database.ExecuteSqlRaw(insertDataTClient,
                                             new SqlParameter("misSubscriberCode", misTicketreq.misSubscriberCode),
                                               new SqlParameter("OfficeName", misTicketreq.OfficeName),
                                               new SqlParameter("contact_det", misTicketreq.contact_det),
                                               new SqlParameter("email_id", misTicketreq.email_id),
                                               new SqlParameter("phone_no", misTicketreq.phone_no));


                }



                var insertDataTProject = @$"INSERT INTO [t_Project] ([project_Id], [project_Title],
                                        [project_Description], [project_StartTime], [project_EstimateTime], 
                                        [project_Client_ID],[project_Status], [Project_Type], [project_category],
                                        [Team_id], [Project_Cat]) 
                                        VALUES (@str,@OfficeName,
                                       @complaincomments,'{DateTime.Now.Date.ToString("yyyy-MM-dd h:mm tt")}',
                                        '{DateTime.Now.Date.ToString("yyyy-MM-dd h:mm tt")}',@misSubscriberCode,
                                        'Pending','com', @misCategoryName,@RelatedDepId,
                                      @misCategoryName1)";

                _misDBContext.Database.ExecuteSqlRaw(insertDataTProject,
                                       new SqlParameter("str", misTicketreq.str),
                                       new SqlParameter("OfficeName", misTicketreq.OfficeName),
                                       new SqlParameter("complaincomments", misTicketreq.complaincomments),
                                       new SqlParameter("misSubscriberCode", misTicketreq.misSubscriberCode),
                                       new SqlParameter("misCategoryName", misTicketreq.misCategoryName),
                                       new SqlParameter("RelatedDepId", misTicketreq.RelatedDepId),
                                       new SqlParameter("misCategoryName1", misTicketreq.misCategoryName));



                var insertDataClientComDet = @$"INSERT INTO [tbl_client_com_det] ([com_ref_no], [client_id],
                                               [sl_no], [Area], [Medea], [Bts], [cat]) 
                                               VALUES (@str,@misSubscriberCode,
                                                @brslno,@brAreaGroup,'','', @brCategory)";


                _misDBContext.Database.ExecuteSqlRaw(insertDataClientComDet,
                                           new SqlParameter("str", misTicketreq.str),
                                           new SqlParameter("misSubscriberCode", misTicketreq.misSubscriberCode),
                                           new SqlParameter("brslno", misTicketreq.brslno),
                                           new SqlParameter("brAreaGroup", misTicketreq.brAreaGroup),
                                           new SqlParameter("brCategory", misTicketreq.brCategory));




                var insertDataEmailFormat = @$"INSERT INTO [tblComplainEmailFormat] ([CTID], [Mailfrom], [Mailto],
                                            [MailCC], [MailBcc], [MailSubject], [MailBody], [Status]) 
                                            VALUES (@str,'mamunur.rashid@link3.net',
                                            'support@link3.net','support@link3.net', '', @OfficeName,
                                            @strMailbody ,'Sent')";



                _misDBContext.Database.ExecuteSqlRaw(insertDataEmailFormat,
                                           new SqlParameter("str", misTicketreq.str),
                                           new SqlParameter("OfficeName", misTicketreq.OfficeName),
                                           new SqlParameter("strMailbody", strMailbody));





                var insertDataAccessPermission = @$"INSERT INTO [tbl_ComplainAccessPermission] ([ComplainID],
                                                [ComplainRecordDatetime], [LinkDownAt], [AccessPermissionDatetime],[Comments])                                               
                                                VALUES (@str,'{DateTime.Now.Date.ToString("yyyy-MM-dd h:mm tt")}',
                                                '{DateTime.Now.Date.ToString("yyyy-MM-dd h:mm tt")}',
                                                '{DateTime.Now.Date.ToString("yyyy-MM-dd h:mm tt")}','')";


                var res = _misDBContext.Database.ExecuteSqlRaw(insertDataAccessPermission,
                                            new SqlParameter("str", misTicketreq.str));

                transaction.Commit();

                return res;
            }
            catch (Exception ex)
            {
                transaction.RollbackToSavepoint("SavedMisTicket"); // Use Transaction  
                throw new Exception(ex.Message);
            }
        }



        public async Task<List<RSM_ComplainLogDetailsModel>> GetRSMComplainTicketLogs(string ticketNo)
        {
                string log_sql = @"select (a.UserID+':'+isnull(b.EmpName,'Self Care API')) as CommentsBy,
                        isnull(b.Designation+'/'+b.Dept+'/'+b.Sect+'/'+b.ContactNumber,'Self Care') as DegDepCell,a.Comments,a.CommentsDate
                        from RSM_ComplainLogDetails a WITH(NOLOCK) left outer join lnk.dbo.Emp_Details b WITH(NOLOCK) on a.UserID=b.EmpID
                        where a.RefNo='" + ticketNo + "' order by a.CommentsDate desc";

                var result = await _rsmcontext.RSM_ComplainLogDetailsModel.FromSqlRaw(log_sql).ToListAsync();
                return result;
        }
        public async Task<List<RSM_NatureSetup>>GetRSM_NatureSetup()
        {
            var resultRsm =  await _rsmcontext.RSM_NatureSetup.Where(t => t.Team_ID != null).OrderBy(t => t.NatureID).ToListAsync();


            //string log_sql1 = @"select NatureID,natureName from RSM_NatureSetup order by NatureID desc";

            //var result = await _rsmcontext.RSM_NatureSetup.FromSqlRaw(log_sql1).ToListAsync();


            return resultRsm;
        }

        public async Task<List<Tbl_Com_Category>> GetMis_NatureSetup()
        {

            var misCategoryListdata = await _misDBContext.Tbl_Com_Category.Where(t => t.SelfCategory != null).OrderBy(t => t.C_id).ToListAsync();

            return misCategoryListdata;
        }

        public async Task<List<NetworkInformationResponseModel>> GetHydraNetworkInformation(string customerId)
        {

            string hydra_sql = @"select CustomerID,AssignedPackage,BrasIP,IPv4,Subnet,IPv6,VLAN,OLTPON,OLT,BTS,MACAddress,CustomerStatus,CustomerCategory from NetworkInformation where CustomerID = '"+ customerId + "' ";

            var hydraResult = await _hydracontext.NetworkInformationResponseModel.FromSqlRaw(hydra_sql).ToListAsync();

            return hydraResult;
        }


        public async Task<List<MisNetworkInformationResponseModel>> GetMisNetworkInformationResponseModel(string customerId)
        {
            string log_sql = @"select client.brCliCode as CustomerID,it.ItemName as AssignedPackage, kh.RouterSwitchIP as BrasIP,
                                cti.real_ip as IPv4,cti.subnet_musk as subnet,cti.r_vlan_id as VLAN,IPv6='', cti.f_pon as OLTPON,sub.OltName as OLT,
                                sub.BtsSetupName as BTS,cti.f_mac as MACAddress,client.brCategory as CustomerCategory

                                from  [WFA2].[dbo].[clientDatabaseMain] as client  WITH(NOLOCK) 
                                inner join  clientTechnicalInfo as cti WITH(NOLOCK) on client.brCliCode = cti.ClientCode 
                                inner join  Kh_IpAddress as kh WITH(NOLOCK) on kh.SubscriberID = cti.ClientCode 
                                inner join  l3t.dbo.slsSalesDetails  as sd WITH(NOLOCK) on sd.ClientID = client.brCliCode and sd.BranchID = client.brAdrCode
                                inner join  l3t.dbo.blgRevenueFromService  as blg WITH(NOLOCK) on blg.TrackingInfo = sd.TrackingInfo and blg.SalesID = sd.SalesID
                                inner join  l3t.dbo.slsItemDetails  as it WITH(NOLOCK) on it.ItemCode = blg.ServiceCode 
                                inner join View_SubSplliter as sub WITH(NOLOCK) on sub.CustomerCode = kh.SubscriberID and sub.CustomerSl = client.brSlNo
                                where client.brCliCode = '" + customerId + "'";
                       

            var result = await _misDBContext.MisNetworkInformationResponseModel.FromSqlRaw(log_sql).ToListAsync();
            return result;
        }

    }


}

using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Pagination;
using L3T.Infrastructure.Helpers.Repositories.Implementation.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Repositories.Interface;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Email;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using L3T.Utility.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.Service.Implementation.ChangeRequest.v1
{
    public class CrStatusService : ICrStatusService
    {
        private readonly ICrStatusRepository _crStatusRepository;
        private readonly ChangeRequestDataContext _context;
        private readonly ILogger<CrStatusRepository> _logger;
        private readonly IHostingEnvironment _environment;
        private readonly ICrApprovalFlowRepository _crApprovalFlowRepository;
        private readonly IChangeRequestedInfoRepository _changeRequestedInfoRepository;
        private readonly IAssignEmployeeRepository _assignEmployeeRepository;
        private readonly INotificationDetailsService _notificationDetailsService;
        private readonly ICRRequestResponseService _cRRequestResponseService;
        private readonly IMailSenderService _mailSenderService;
        private readonly IConfiguration _configuration;
        private readonly IChangeRequestLogService _changeRequestLogService;
        private readonly IChangeRequestLogRepository _changeRequestLogRepository;
        private readonly ICommonRepository _commonRepository;

        public CrStatusService(ICrStatusRepository crStatusRepository,
            IHostingEnvironment environment, ChangeRequestDataContext context,
            ILogger<CrStatusRepository> logger, ICrApprovalFlowRepository crApprovalFlowRepository,
            IChangeRequestedInfoRepository changeRequestedInfoRepository,
            IAssignEmployeeRepository assignEmployeeRepository,
            INotificationDetailsService notificationDetailsService,
            ICRRequestResponseService cRRequestResponseService,
            IMailSenderService mailSenderService,
            IConfiguration configuration, 
            IChangeRequestLogService changeRequestLogService, 
            IChangeRequestLogRepository changeRequestLogRepository, 
            ICommonRepository commonRepository)

        {
            _crStatusRepository = crStatusRepository;
            _logger = logger;
            _context = context;
            _environment = environment;
            _crApprovalFlowRepository = crApprovalFlowRepository;
            _changeRequestedInfoRepository = changeRequestedInfoRepository;
            _assignEmployeeRepository = assignEmployeeRepository;
            _notificationDetailsService = notificationDetailsService;
            _cRRequestResponseService = cRRequestResponseService;
            _mailSenderService = mailSenderService;
            _configuration = configuration;
            _changeRequestLogService = changeRequestLogService;
            _changeRequestLogRepository = changeRequestLogRepository;
            _commonRepository = commonRepository;
        }

        public async Task<ApiResponse> AddStatus(AddStatusReq requestModel, string userId, string ip)
        {
            var methodName = "CrStatusService/AddCrStatus";
            try
            {
                if (string.IsNullOrWhiteSpace(requestModel.StatusDisplayName))
                {
                    throw new Exception("Please enter details properly.");

                }
                var checkName = await _crStatusRepository.FirstOrDefaultAsync(x => x.Status == requestModel.Status);

                if (checkName != null)
                {
                    throw new Exception("Your provided name already exist.");

                }

                if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                }
                if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                {
                    _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }

                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\" + "CrStatusImg" + requestModel.StatusImage.FileName))
                {
                    requestModel.StatusImage.CopyTo(filestream);
                    filestream.Flush();

                }

                var requestDbModel = new CrStatus()
                {
                    StatusDisplayName = requestModel.Status,
                    StatusImage = requestModel.StatusImage.FileName,
                    Status = requestModel.Status,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId

                };
                await _crStatusRepository.CreateAsync(requestDbModel);
                if (await _context.SaveChangesAsync() > 0)
                {
                    var Message = "CR status save successfully!";
                    return await _cRRequestResponseService.CreateResponseRequest(requestModel, requestDbModel, ip, methodName, userId, Message);                     
                }
                _logger.LogInformation("Add CR status : " + requestDbModel);

                throw new Exception("CR status Save some problem.");
            }
            catch (Exception ex)
            {

                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(requestModel, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> DeleteStatus(long Id, string userId, string ip)
        {
             
            var methodName = "CrStatusService/CrStatusDelete";
            try
            {

                var res = await _crStatusRepository.FirstOrDefaultAsync(x => x.Id == Id);
                if (res != null)
                {

                    await _crStatusRepository.Delete(res);
                    await _context.SaveChangesAsync();

                    var Message = "Data Deleted Successfully.";

					return await _cRRequestResponseService.CreateResponseRequest(Id, null, ip, methodName, userId, Message);

                }
                else
                {
                    _logger.LogInformation("CR Status Delete : " + Id);
                    throw new Exception("Data not found.");

                }
                 
            }
            catch (Exception ex)
            {

                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(Id, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> GetAllStatus(string userId, string ip)
        {
             
            var methodName = "CrStatusService/GetCrStatus";
            try
            {
                var res = await _crStatusRepository.FindAsync(null);
                if (res != null)
                {
                    return await _cRRequestResponseService.CreateResponseRequest(null, res, ip, methodName, userId, "Ok");
                }
                else
                {
                    _logger.LogInformation("GetCrStatus : " + "");
                    throw new Exception("Data not found.");
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> UpdateStatus(UpdateStatusReq ReqModel, string userId, string ip)
        {
             
            var methodName = "CrStatusService/UpdateCrStatus";
            try
            {
                var getData = await _crStatusRepository.FirstOrDefaultAsync(x => x.Id == ReqModel.Id);
                if (getData != null)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                    }
                    if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                    {
                        _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    }

                    using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\" + "CrStatusImg" + ReqModel.StatusImage.FileName))
                    {
                        ReqModel.StatusImage.CopyTo(filestream);
                        filestream.Flush();

                    }

                    getData.StatusDisplayName = ReqModel.StatusDisplayName;
                    getData.StatusImage = ReqModel.StatusImage.FileName;
                    getData.Status = ReqModel.Status;
                    getData.LastModifiedBy = userId;
                    getData.LastModifiedAt = DateTime.UtcNow;

                    _crStatusRepository.UpdateLatest(getData);
                    _context.SaveChanges();

					var Message = "CR status update successfully!";

					return await _cRRequestResponseService.CreateResponseRequest(ReqModel, getData, ip, methodName, userId, Message);
				}
                else
                {

                    _logger.LogInformation("CR StatusUpdate : " + ReqModel);
                    throw new Exception("Data not found.");
                }
                 

            }
            catch (Exception ex)
            {

                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        private async Task<CrApprovalFlow> FineNextApprover(int currentApproverFlow, long CrId, List<CrApprovalFlow> approverList)
        {
            var nextApprover = new CrApprovalFlow();
            for (int i = currentApproverFlow - 1; i >= 1; i--)
            {
                nextApprover = approverList.FirstOrDefault(x => x.CrId == CrId && x.ApproverFlow == (i));
                if (nextApprover == null)
                {
                    if(i != 1)
                    {
                        continue;
                    }
                    else
                    {
                        return nextApprover;
                    }
                }
                else { 
                    return nextApprover;
                }
            }
            return nextApprover;
        }

        public async Task<ApiResponse> UpdateCRStatus(UpdateCrStatusRequestModel model, string userId, string ip)
        {
             
            var methodName = "CrStatusService/UpdateCRStatus";
            try
            {
                var allCrStatus = await _crStatusRepository.FindAsync(null);
                var getStatus = allCrStatus.FirstOrDefault(x => x.Status == model.CrStatus);
                var nextApprover = new CrApprovalFlow();
                int isNotApproveYet = 0;
                var getAllApprovalInfo = await _crApprovalFlowRepository.FindAsync(x => x.CrId == model.CrId && x.IsActive == true);
                var crInfo = await _changeRequestedInfoRepository.FindFirstOrDefaultAsync(x=> x.Id == model.CrId);
                var currentApprover = getAllApprovalInfo.FirstOrDefault(x => x.CrId == model.CrId && x.ApproverEmpId == userId);
                if (currentApprover == null)
                {
                    throw new Exception($@"Approver is not found.");
                }
                if(currentApprover.ApproverFlow != 1)
                {
                    nextApprover = await FineNextApprover(currentApprover.ApproverFlow, crInfo.Id, getAllApprovalInfo);
                }
                else
                {
                    nextApprover = null;
                }
                                
                var allLowerApprover = getAllApprovalInfo.Where(x => x.ApproverFlow > currentApprover.ApproverFlow).ToList();
                var allHigherApprover = getAllApprovalInfo.Where(x => x.ApproverFlow < currentApprover.ApproverFlow).ToList();

                if(currentApprover.Status == model.CrStatus)
                {
                    throw new Exception($@"This CR is already {getStatus.StatusDisplayName}.");
                }

                if (nextApprover != null && nextApprover.Status != AllStatus.Pending.ToString())
                {
                    throw new Exception($@"This CR is already {nextApprover.Status} by your senior officer. So you can't {getStatus.StatusDisplayName} this CR.");
                }

                if (nextApprover != null)
                {
                    crInfo.CurrentApprover = nextApprover.ApproverEmpId;
                    crInfo.CurrentApproverRole = nextApprover.ApproverRole;
                }

                var newModel = new AssignEmployeeListReqModel()
                {
                    CrId = model.CrId.ToString(),
                    PageNumber = 1,
                    PageSize = 10000,
                };
                var getAllDeveloperForThisUser = await _commonRepository.GetAllAssignEmployeeFromDB(newModel);
                var endDate = getAllDeveloperForThisUser.Max(x => x.EndDate);

                if (model.CrStatus != AllStatus.Approved.ToString())
                {
                    if (currentApprover != null)
                    {
                        currentApprover.Status = getStatus.Status;
                        currentApprover.StatusDisplayName = getStatus.StatusDisplayName;
                        currentApprover.StatusDate = DateTime.Now;
                        currentApprover.ApproverEmail = model.Email;
                        _crApprovalFlowRepository.Update(currentApprover);

                        crInfo.Status = getStatus.Status;
                        crInfo.StatusDisplayName = getStatus.StatusDisplayName;
                    }
                }
                else
                {
                    if (currentApprover.IsPrincipleApprover)
                    {
                        var employeeIsAssign = await _assignEmployeeRepository.FindAsync(x => x.CrId == model.CrId);
                        if (employeeIsAssign.Count() <= 0)
                        {
                            throw new Exception("Please assign employee before the approval.");
                        }
                    }                    

                    // if any status CR to Approved status again
                    if(currentApprover.Status != AllStatus.Approved.ToString())
                    {
                        currentApprover.Status = getStatus.Status;
                        currentApprover.StatusDisplayName = getStatus.StatusDisplayName;
                        currentApprover.StatusDate = DateTime.Now;
                        currentApprover.ApproverEmail = model.Email;
                        _crApprovalFlowRepository.Update(currentApprover);

                        if(currentApprover.ApproverFlow == 1)
                        {
                            isNotApproveYet = 1;
                        }
                        else
                        {
                            var getStatusSub = allCrStatus.FirstOrDefault(x => x.Status == AllStatus.Submitted.ToString());
                            crInfo.Status = getStatusSub.Status;
                            crInfo.StatusDisplayName= getStatusSub.StatusDisplayName;
                        }
                        
                    }
                    else
                    {
                        isNotApproveYet = getAllApprovalInfo.FindAll(x => x.Status == AllStatus.Pending.ToString()).Count();

                        if (isNotApproveYet > 0)
                        {
                            if (currentApprover != null)
                            {
                                var getStatusApp = allCrStatus.FirstOrDefault(x => x.Status == AllStatus.Approved.ToString());
                                currentApprover.Status = getStatusApp.Status;
                                currentApprover.StatusDisplayName = getStatusApp.StatusDisplayName;
                                currentApprover.StatusDate = DateTime.Now;
                                currentApprover.ApproverEmail = model.Email;
                                _crApprovalFlowRepository.Update(currentApprover);
                            }
                        }
                    }
                    

                    if (isNotApproveYet == getAllApprovalInfo.Count())
                    {
                        crInfo.ApprovedUserId = userId;
                        crInfo.ApprovedDate = DateTime.Now;
                    }

                    if (isNotApproveYet == 1)
                    {
                        crInfo.Status = getStatus.Status;
                        crInfo.StatusDisplayName = getStatus.StatusDisplayName;
                        crInfo.FinalApprovedUserId = userId;
                        crInfo.FinalApprovedDate = DateTime.Now;

                        var toUserEmail = (from pd in getAllDeveloperForThisUser
                                           join od in _context.DeveloperInformation on pd.EmpId equals od.UserId
                                 where od.User_Email != null  && pd.DeleteStatus == false && pd.CrId == model.CrId
                                 select new { od.User_Email, od.UserId }).Select(x => new DeveloperInformation { User_Email = x.User_Email, UserId = x.UserId }).ToList();


                        await MailSendForDeveloper(crInfo, toUserEmail, getAllDeveloperForThisUser);
                        var approversEmail = allLowerApprover.FindAll(x => x.ApproverEmail != null).Select(x => x.ApproverEmpId).ToList();
                        
                        await MailSend(crInfo, approversEmail, endDate);
                        var sendNotificationForThisUser = getAllDeveloperForThisUser.Select(x => x.EmpId).ToList();
                        await NotificationSend(sendNotificationForThisUser, currentApprover, crInfo, getStatus.StatusDisplayName, userId, ip, "/AllChangeRequest/" + crInfo.Id);
                    }
                }

                crInfo.LastModifiedAt = DateTime.Now;
                crInfo.LastModifiedBy = userId;
                _changeRequestedInfoRepository.Update(crInfo);

                await _context.SaveChangesAsync();
                var higherUserIds = allHigherApprover.Select(x => x.ApproverEmpId).ToList();
                await NotificationSend(higherUserIds, currentApprover, crInfo, getStatus.StatusDisplayName, userId, ip, "/AllChangeRequest/" + crInfo.Id);
                var lowerUserIds = allLowerApprover.Select(x => x.ApproverEmpId).ToList();
                await NotificationSend(lowerUserIds, currentApprover, crInfo, getStatus.StatusDisplayName, userId, ip, "/AllChangeRequest/" + crInfo.Id);

                await _notificationDetailsService.WhenApprovedCRUpdateNotificationUnreadToRead(userId, model.CrId, ip);

                
                await MailSendForIndividualApprover(crInfo, getAllApprovalInfo, currentApprover.ApproverEmail);

                var Message = "Change Request is "+getStatus.StatusDisplayName +" Successfully.";
                return await _cRRequestResponseService.CreateResponseRequest(model, null, ip, methodName, userId, Message);
                 
            }
            catch (Exception ex)
            {

                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(model, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        private async Task NotificationSend(List<string> notifyUser, CrApprovalFlow currentApprover, ChangeRequestedInfo crInfo, string crStarus, string userId, string ip, string notifyUrl = null)
        {
            var methodName = "CrStatusService/NotificationSend";
            try
            {
                if (notifyUser.Count > 0)
                {
                    var notifyList = new List<NotificationDetails>();
                    foreach (var item in notifyUser)
                    {
                        var Model = new NotificationDetails()
                        {
                            CrId = crInfo.Id,
                            ApproverEmpId = item,
                            Title = crInfo.ChangeRequestFor,
                            Message = crInfo.Subject + ". This CR is " + crStarus + " by " + currentApprover.ApproverName,
                            Image = crStarus + ".svg",
                            NotifyURL = notifyUrl,
                            Type = crInfo.ChangeRequestFor,
                            IsRead = false,
                            IsActive = true,
                            CreatedBy = userId
                        };
                        notifyList.Add(Model);
                    }
                    await _notificationDetailsService.AddNotificationDetailsRange(notifyList, userId, ip);
                    await _cRRequestResponseService.CreateResponseRequest(notifyUser, null, ip, methodName, userId, "Ok");
                }
                throw new Exception("User not found.");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                await _cRRequestResponseService.CreateResponseRequest(notifyUser, ex, ip, methodName, userId, "Error", ex.Message);
            }
        }

        private async Task MailSend(ChangeRequestedInfo crInfo, List<string> ccUser, DateTime? crCompleteDate)
        {
            var cCMailList = new List<string>();
            var toMailList = new List<string>();
            var mailBody = new StringBuilder();
            var fromMailUser = new CrApprovalFlow()
            {
                ApproverEmail = _configuration.GetValue<string>("MailConfig:fromAddress"),
                ApproverName = _configuration.GetValue<string>("MailConfig:displayName")
            };

            //string srtTextBody = $@"<p>Hello {crInfo.RequestorName},<br><br>
            //                    We hope you are doing well.<br>The Software Team from I &amp;I Department are glad to inform you that 
            //                    your CR (CR#{crInfo.Id}) with the title &quot;{crInfo.Subject}&quot; has been approved with expected 
            //                    development completion date by {crInfo.CrDate}.&nbsp;<br><br>Please communicate with Mr.Moynul Haque or 
            //                    Mr.Faisal Khan if there are any further questions or concerns.<br><br>Please be prepared for a UAT 
            //                    session before the completion date, 
            //                    that will be communicated by the SW team.<br><br>SW team will work relentlessly to ensure a robust 
            //                    solution for you within the timeline 
            //                    mentioned.<br><br>Have a great day<br><br>Thank you<br>SW Team<br>I&amp;I Department</p>";

            string srtTextBody = $@"Hello {crInfo.RequestorName}, <br><br>
                                    We hope you are doing well. <br>
                                    The Software Team from I &I Department are glad to inform you that your CR (CR#{crInfo.Id}) with the title &quot;{crInfo.Subject}&quot; 
                                    has been approved with expected development completion date by {crCompleteDate}. <br><br>
                                    
                                    Please communicate with Mr. Moynul Haque Biswas or Mr. Faisal Khan if there are any further questions or concerns. <br><br>

                                    Please be prepared for a UAT session before the completion date, that will be communicated by the SW team. <br><br>

                                    SW team will work relentlessly to ensure a robust solution for you within the timeline mentioned. <br><br>

                                    Have a great day <br>
                                    Thank you, <br>
                                    SW Team <br>
                                    I&I Department";

            mailBody.Append(srtTextBody);
            if (ccUser.Count > 0)
            {
                foreach (var cc in ccUser)
                {
                    cCMailList.Add(cc);
                }
                if (!string.IsNullOrEmpty(crInfo.Email))
                {
                    toMailList.Add(crInfo.Email);
                }
            }

            //cCMailList.Add("loton1984@gmail.com");
            //toMailList.Add("sanjib.dhar@link3.net");

            _mailSenderService.SendMail(crInfo.Subject, mailBody.ToString(), fromMailUser.ApproverEmail, toMailList, cCMailList, null, fromMailUser.ApproverName);
        }

        private async Task MailSendForIndividualApprover(ChangeRequestedInfo crInfo, List<CrApprovalFlow> approvers, string ccUser)
        {
            var cCMailList = new List<string>();
            var toMailList = new List<string>();
            var mailBody = new StringBuilder();
            var fromMailUser = new CrApprovalFlow()
            {
                ApproverEmail = _configuration.GetValue<string>("MailConfig:fromAddress"),
                ApproverName = _configuration.GetValue<string>("MailConfig:displayName")
            };

            //string srtTextBody = $@"<p>Hello {crInfo.RequestorName},<br><br>
            //                    We hope you are doing well.<br>The Software Team from I &amp;I Department are glad to inform you that 
            //                    your CR (CR#{crInfo.Id}) with the title &quot;{crInfo.Subject}&quot; has been approved with expected 
            //                    development completion date by {crInfo.CrDate}.&nbsp;<br><br>Please communicate with Mr.Moynul Haque or 
            //                    Mr.Faisal Khan if there are any further questions or concerns.<br><br>Please be prepared for a UAT 
            //                    session before the completion date, 
            //                    that will be communicated by the SW team.<br><br>SW team will work relentlessly to ensure a robust 
            //                    solution for you within the timeline 
            //                    mentioned.<br><br>Have a great day<br><br>Thank you<br>SW Team<br>I&amp;I Department</p>";

            //string srtTextBody = $@"Hello {crInfo.RequestorName}, <br><br>
            //                        We hope you are doing well. <br>
            //                        The Software Team from I &I Department are glad to inform you that your CR (CR#{crInfo.Id}) with the title &quot;{crInfo.Subject}&quot; 
            //                        has been approved with expected development completion date by {crCompleteDate}. <br><br>
                                    
            //                        Please communicate with Mr. Moynul Haque Biswas or Mr. Faisal Khan if there are any further questions or concerns. <br><br>

            //                        Please be prepared for a UAT session before the completion date, that will be communicated by the SW team. <br><br>

            //                        SW team will work relentlessly to ensure a robust solution for you within the timeline mentioned. <br><br>

            //                        Have a great day <br>
            //                        Thank you, <br>
            //                        SW Team <br>
            //                        I&I Department";

            string srtTextBody = $@"Hello {crInfo.RequestorName}, <br><br>
                                    We hope you are doing well.<br>
                                    The Software Team from I &I Department are glad to inform you that your 
                                    CR (CR#{crInfo.Id}) with the title &quot;{crInfo.Subject}&quot; has been 
                                    approved by <Approver>. <br><br>
                                    Remaining Approvers are:<br><br>";
            
            mailBody.Append(srtTextBody);

            foreach (var approver in approvers)
            {
                if(approver.StatusDate == null)
                {
                    mailBody.AppendLine($@"{approver.ApproverName} ({approver.Status}) <br>");
                }
                else
                {
                    mailBody.AppendLine($@"{approver.ApproverName} ({approver.Status}), Date: {approver.StatusDate} <br>");
                }
                
            }

            var footer = $@" <br> <br> Thank you, <br>
                        SW Team <br>
                        I&I Department";

            mailBody.Append(footer);

            toMailList.Add(crInfo.Email);
            cCMailList.Add(ccUser);

            //cCMailList.Add("loton1984@gmail.com");
            //toMailList.Add("sanjib.dhar@link3.net");


            _mailSenderService.SendMail(crInfo.Subject, mailBody.ToString(), fromMailUser.ApproverEmail, toMailList, cCMailList, null, fromMailUser.ApproverName);
        }

        private async Task MailSendForDeveloper(ChangeRequestedInfo crInfo, List<DeveloperInformation> toUserEmail, List<SP_AssignEmployeeListResponse> assignEmp)
        {
            
            foreach(var sendMailAEmp in assignEmp)
            {
                var cCMailList = new List<string>();
                var toMailList = new List<string>();
                var mailBody = new StringBuilder();
                var fromMailUser = new CrApprovalFlow()
                {
                    ApproverEmail = _configuration.GetValue<string>("MailConfig:fromAddress"),
                    ApproverName = _configuration.GetValue<string>("MailConfig:displayName")
                };

                var aDeveloper = toUserEmail.FirstOrDefault(x => x.UserId == sendMailAEmp.EmpId);
                string srtTextBody = $@"Hello {sendMailAEmp.EmpName},<br><br>
                                        CR#{crInfo.Id} has been assigned to you with the following details: <br>
 
                                        CR#{crInfo.Id}  <br>
                                        CR Name : {crInfo.Subject} <br>
                                        Development Start Time: {sendMailAEmp.StartDate} <br>
                                        Development End Time: {sendMailAEmp.EndDate} <br>
                                        Number of Days for Development: {sendMailAEmp.TotalDay} <br>
 
                                        Thank you, <br>
                                        SW Team <br>
                                        I&I Department";

                mailBody.Append(srtTextBody);

                if (aDeveloper != null)
                {
                    toMailList.Add(aDeveloper.User_Email);
                }
                cCMailList.Add(crInfo.Email);

                //cCMailList.Add("loton1984@gmail.com");
                //toMailList.Add("sanjib.dhar@link3.net");

                _mailSenderService.SendMail(crInfo.Subject, mailBody.ToString(), fromMailUser.ApproverEmail, toMailList, cCMailList, null, fromMailUser.ApproverName);

            }


            //string srtTextBody = $@"<p>Hello {crInfo.RequestorName},<br><br>
            //                    We hope you are doing well.<br>The Software Team from I &amp;I Department are glad to inform you that 
            //                    your CR (CR#{crInfo.Id}) with the title &quot;{crInfo.Subject}&quot; has been approved with expected 
            //                    development completion date by {crInfo.CrDate}.&nbsp;<br><br>Please communicate with Mr.Moynul Haque or 
            //                    Mr.Faisal Khan if there are any further questions or concerns.<br><br>Please be prepared for a UAT 
            //                    session before the completion date, 
            //                    that will be communicated by the SW team.<br><br>SW team will work relentlessly to ensure a robust 
            //                    solution for you within the timeline 
            //                    mentioned.<br><br>Have a great day<br><br>Thank you<br>SW Team<br>I&amp;I Department</p>";




            
            

            
        }

        public async Task<ApiResponse> UpdateCRProcessStatus(UpdateCrStatusRequestModel model, string userId, string ip)
        {

            var methodName = "CrStatusService/UpdateCRProcessStatus";
            try
            {
                var allCrStatus = await _crStatusRepository.FindAsync(null);
                var getStatus = allCrStatus.FirstOrDefault(x => x.Status == model.CrStatus);

                var crInfo = await _changeRequestedInfoRepository.FindFirstOrDefaultAsync(x => x.Id == model.CrId);
                if (crInfo == null)
                {
                    throw new Exception($@"CR is not found.");
                }
                var getAllApprovalInfo = await _crApprovalFlowRepository.FindAsync(x => x.CrId == model.CrId && x.IsActive == true);
                var currentApprover = getAllApprovalInfo.FirstOrDefault(x => x.CrId == model.CrId && x.ApproverEmpId == userId);
                if (currentApprover != null)
                {
                    var topApprover = getAllApprovalInfo.FindAll(x => x.ApproverFlow < currentApprover.ApproverFlow).OrderBy(x => x.ApproverFlow).FirstOrDefault();
                    if (topApprover != null)
                    {
                        if (crInfo.LastModifiedBy == topApprover.ApproverEmpId)
                        {
                            if (crInfo.Status == AllStatus.Completed.ToString())
                            {
                                throw new Exception($@"You can't {getStatus.StatusDisplayName} yet. Because this CR already Completed by your senior officer.");
                            }
                            else if (crInfo.Status == AllStatus.Rejected.ToString())
                            {
                                throw new Exception($@"You can't {getStatus.StatusDisplayName} yet. Because this CR already Rejected by your senior officer.");
                            }
                            else if (crInfo.Status == AllStatus.OnHold.ToString())
                            {
                                throw new Exception($@"You can't {getStatus.StatusDisplayName} yet. Because this CR already OnHold by your senior officer.");
                            }
                            else if (crInfo.Status == AllStatus.InProgress.ToString())
                            {
                                throw new Exception($@"You can't {getStatus.StatusDisplayName} yet. Because this CR already OnProgress by your senior officer.");
                            }
                        }
                    }
                    currentApprover.Status = getStatus.Status;
                    currentApprover.StatusDate = DateTime.Now;
                    currentApprover.StatusDisplayName = getStatus.StatusDisplayName;
                    _crApprovalFlowRepository.Update(currentApprover);
                }

                crInfo.Status = getStatus.Status;
                crInfo.LastModifiedAt = DateTime.Now;
                crInfo.LastModifiedBy = userId;
                crInfo.StatusDisplayName = getStatus.StatusDisplayName;
                _changeRequestedInfoRepository.Update(crInfo);

                await _context.SaveChangesAsync();

                var requestDbModel = new AddChangeRequestLogReq()
                {
                    CrId = model.CrId,
                    CRChangeStatus = getStatus.Status
                };
                await ApplicatioinProcessLog(requestDbModel, userId, ip);

                var Message = "Change Request is " + getStatus.StatusDisplayName + " Successfully.";
                return await _cRRequestResponseService.CreateResponseRequest(model, null, ip, methodName, userId, Message);

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(model, ex, ip, methodName, userId, "Error", ex.Message.ToString());

            }
        }

        public async Task ApplicatioinProcessLog(AddChangeRequestLogReq model, string userId, string ip) {
            if(model != null)
            {
                if(model.CrId > 0) {
                    var getAllCrLogs = await _changeRequestLogRepository.FindAsync(x => x.CrId == model.CrId);
                    model.TaskFlow = (getAllCrLogs.Count > 0) ? getAllCrLogs.Count + 1 : 1;

                    await _changeRequestLogService.AddChangeRequestLog(model, userId, ip);
                }
            }            
        }


    }
}

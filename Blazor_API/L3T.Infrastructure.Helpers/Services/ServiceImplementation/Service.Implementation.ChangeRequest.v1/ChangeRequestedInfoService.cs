using Elasticsearch.Net;
using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Pagination;
using L3T.Infrastructure.Helpers.Pagination.Filter;
using L3T.Infrastructure.Helpers.Pagination.Helper;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Email;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using L3T.Utility.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.Service.Implementation.ChangeRequest.v1
{
    public class ChangeRequestedInfoService : IChangeRequestedInfoService
    {

        private readonly IChangeRequestedInfoRepository _changeRequestedInfoRepository;
        private readonly ITempChangeRequestedInfoRepository _tempChangeRequestedInfoRepository;
        private readonly ChangeRequestDataContext _context;
        private readonly ILogger<ChangeRequestedInfoService> _logger;
        private readonly IUriService _uriService;
        private readonly ICrDefaultApprovalFlowRepository _crDefaultApprovalFlowRepository;
        private readonly ICrApprovalFlowRepository _crApprovalFlowRepository;
        private readonly ICRRequestResponseService _cRRequestResponseService;
        private readonly INotificationDetailsService _notificationDetailsService;
        private readonly ICrAttatchedFilesRepository _crAttatchedFilesRepository;
        private readonly IConfiguration _configuration;
        private readonly IMailSenderService _mailSenderService;
        private readonly ICrStatusRepository _crStatusRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        public ChangeRequestedInfoService(
            IChangeRequestedInfoRepository changeRequestedInfoRepository,
            ITempChangeRequestedInfoRepository tempChangeRequestedInfoRepository,
            IUriService uriService,
            ChangeRequestDataContext context,
            ICrDefaultApprovalFlowRepository CrDefaultApprovalFlowRepository,
            ICrApprovalFlowRepository CrApprovalFlowRepository,
            ILogger<ChangeRequestedInfoService> logger,
            INotificationDetailsService notificationDetailsService,
            ICRRequestResponseService cRRequestResponseService,
            ICrAttatchedFilesRepository crAttatchedFilesRepository,
            IConfiguration configuration,
            IMailSenderService mailSenderService,
            ICrStatusRepository crStatusRepository,
            IHttpClientFactory httpClientFactory)
        {

            _changeRequestedInfoRepository = changeRequestedInfoRepository;
            _tempChangeRequestedInfoRepository = tempChangeRequestedInfoRepository;
            _logger = logger;
            _uriService = uriService;
            _context = context;
            _crDefaultApprovalFlowRepository = CrDefaultApprovalFlowRepository;
            _crApprovalFlowRepository = CrApprovalFlowRepository;
            _notificationDetailsService = notificationDetailsService;
            _cRRequestResponseService = cRRequestResponseService;
            _crAttatchedFilesRepository = crAttatchedFilesRepository;
            _configuration = configuration;
            _mailSenderService = mailSenderService;
            _crStatusRepository = crStatusRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> testMethod(string l3Id)
        {
            var methodName = "ThirdPartyService/GetAllDefaultApprovalFlow";
            try
            {
                var requestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, $@"account/testMethod?searchText=TestData");

                var httpClient = _httpClientFactory.CreateClient("apiGateway");
                var response = await httpClient.SendAsync(requestMessage);

                var responseStatusCode = response.StatusCode.ToString();
                var responseBody = await response.Content.ReadAsStringAsync();

                if (responseStatusCode.ToString() == "OK")
                {
                    
                    return responseBody;
                }

                throw new Exception("Something is wrong in third party api request.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                await _cRRequestResponseService.CreateResponseRequest(l3Id, ex, null, methodName, null, "Error", ex.Message.ToString());
                return ex.Message + "\n"+ ex.StackTrace;

            }
        }

        public async Task<ApiResponse> AddChangeRequest(string userId, string ip)
        {
             

            var methodName = "ChangeRequestedInfoService/AddChangeRequest";

            try
            {
                var allCrStatus = await _crStatusRepository.FindAsync(null);
                if(allCrStatus.Count is 0)
                {
                    throw new Exception("Status is not found.");
                }
                var res = await _tempChangeRequestedInfoRepository.FirstOrDefaultAsync(x => x.EmployeeId == userId);

                if (res == null)
                {
                    throw new Exception("Change request information is not found.");
                }
                if (!res.StepNo.Contains("4"))
                {
                    throw new Exception("Change request information all information is not submit.");
                }

                if (string.IsNullOrWhiteSpace(res.Subject))
                {
                    throw new Exception("Please enter details properly.");

                }
                var allFiles = await _crAttatchedFilesRepository.FindAsync(x => x.CrId == res.Id);

                var approverInfo = _crDefaultApprovalFlowRepository.QueryAll(x => (x.IsPrincipleApprover == true || x.Department == res.DepartName) && x.IsActive == true).ToList();

                var principleApprover = approverInfo.FindAll(x=> x.IsPrincipleApprover == true).OrderByDescending(x => x.ApproverFlow).ToList();

                var departmentWiseApprover = approverInfo.FindAll(x => x.IsPrincipleApprover == false).OrderByDescending(x => x.ApproverFlow).ToList();

                var curentApproverInfo = new CrDefaultApprovalFlow();


                if (departmentWiseApprover.Count == 0)
                {
					curentApproverInfo = principleApprover.OrderByDescending(x => x.ApproverFlow).FirstOrDefault();
                }
                else
                {
					curentApproverInfo = departmentWiseApprover.OrderByDescending(x => x.ApproverFlow).FirstOrDefault();
				}

                var crStatus = allCrStatus.FirstOrDefault(x => x.Status == AllStatus.Submitted.ToString());
                if (approverInfo != null)
                {
                    var newCrCreate = new ChangeRequestedInfo()
                    {
                        Subject = res.Subject,
                        RequestorName = res.RequestorName,
                        DepartName = res.DepartName,
                        EmployeeId = res.EmployeeId,
                        RequestorDesignation = res.RequestorDesignation,
                        ContactNumber = res.ContactNumber,
                        CurrentApprover = curentApproverInfo.ApproverEmpId,
                        CurrentApproverRole = curentApproverInfo.ApproverRole,
                        Email = res.Email,
                        CrDate = DateTime.UtcNow,
                        ChangeRequestFor = res.ChangeRequestFor,
                        ChangeFromExisting = res.ChangeFromExisting,
                        ChangeToAfter = res.ChangeToAfter,
                        ChangeImpactDescription = res.ChangeImpactDescription,
                        Justification = res.Justification,
                        LevelOfRisk = res.LevelOfRisk,
                        LevelOfRiskDescription = res.LevelOfRiskDescription,
                        AlternativeDescription = res.AlternativeDescription,
                        AddReference = res.AddReference,
                        AttachFile = res.AttachFile,
                        StatusDisplayName = crStatus.StatusDisplayName,
                        Status = crStatus.Status,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId
                    };

                    await _changeRequestedInfoRepository.CreateAsync(newCrCreate);
                    await _context.SaveChangesAsync();

                    var newApprovalFlowList = new List<CrApprovalFlow>();
                    var crApprovalFlowStatus = allCrStatus.FirstOrDefault(x => x.Status == AllStatus.Pending.ToString());
                    foreach (var item in principleApprover.OrderBy(x => x.ApproverFlow).ToList())
                    {                        
                        newApprovalFlowList.Add(await setApprovalFlowModelData(item, newCrCreate.Id, crApprovalFlowStatus.Status, crApprovalFlowStatus.StatusDisplayName, userId));
                    }
                    if(departmentWiseApprover.Count > 0)
                    {
                        var lastPrincipleFlowNumber = principleApprover.OrderByDescending(x => x.ApproverFlow).Select(x => x.ApproverFlow).FirstOrDefault();
                        foreach (var item in departmentWiseApprover.OrderBy(x=> x.ApproverFlow).ToList())
                        {
                            newApprovalFlowList.Add(await setApprovalFlowModelData(item, newCrCreate.Id, crApprovalFlowStatus.Status, crApprovalFlowStatus.StatusDisplayName, userId));
                        }
                    }

                    await _crApprovalFlowRepository.CreateListAsync(newApprovalFlowList);

                    if(allFiles.Count > 0)
                    {
                        allFiles.ForEach(x => x.CrId = newCrCreate.Id);

                        _crAttatchedFilesRepository.UpdateRange(allFiles);
                    }

                    await _tempChangeRequestedInfoRepository.Delete(res);

                    var crsub = await _context.SaveChangesAsync();

                    var Model = new NotificationDetails()
                    {
                        CrId = newCrCreate.Id,
                        ApproverEmpId = curentApproverInfo.ApproverEmpId,
                        ApproverRole = curentApproverInfo.ApproverRole,
                        Title = res.ChangeRequestFor,
                        Message = res.Subject+" "+"by"+" " + res.RequestorName,
                        Image = null,
                        Type = res.DepartName,
                        IsRead = false,
                        IsActive = true,
                        CreatedBy = userId
                    };

                    await _notificationDetailsService.AddNotificationDetails(Model, userId, ip);

                    var ccEmail = new List<string>();

                    if(newApprovalFlowList.Count > 0)
                    {
                        if (_configuration.GetValue<string>("MailConfig:AfterCreateACRMailGoseToAllApproverOrCurrentApprover").ToLower() == "all")
                        {
                            ccEmail = newApprovalFlowList.FindAll(null).Select(x => x.ApproverEmail).ToList();
                        }
                        else
                        {
                            ccEmail = newApprovalFlowList.FindAll(x => x.ApproverEmpId == curentApproverInfo.ApproverEmpId).Select(x => x.ApproverEmail).ToList();
                        }
                    }                    

                    await MailSend(newCrCreate, ccEmail);

                    if (crsub > 0)
                    {
                        var Message = "ChangeRequest save successfully!";

                        return await _cRRequestResponseService.CreateResponseRequest(res, null, ip, methodName, userId, Message);
                    }
                    throw new Exception("Change Request Save some problem.");
                }

                throw new Exception("Change Request Save some problem.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName,userId, ex.Message.ToString());
                 
            }
        }

        private async Task<CrApprovalFlow> setApprovalFlowModelData(CrDefaultApprovalFlow item, long crId, string status, string statusDisplayName, string userId)
        {
            var crApprovalFlow = new CrApprovalFlow()
            {
                CrId = crId,
                ApproverName = item.ApproverName,
                ApproverDesignation = item.ApproverDesignation,
                ApproverDepartment = item.ApproverDepartment,
                Department = item.Department,
                ApproverEmpId = item.ApproverEmpId,
                ApproverFlow = item.ApproverFlow,
                CrDefaultApproverFlowId = item.Id,
                ParentId = item.ParentId,
                Status = status,
                StatusDisplayName = statusDisplayName,
                ApproverRole = item.ApproverRole,
                IsPrincipleApprover = item.IsPrincipleApprover,
                IsActive = item.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };
            return crApprovalFlow;
        }

        private async Task MailSend(ChangeRequestedInfo crInfo, List<string> ccUser)
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
            //                    development completion date by {crInfo.CrDate}.&nbsp;<br><br>Please communicate with Mr. Moynul Haque Biswas or 
            //                    Mr. Faisal Khan if there are any further questions or concerns.<br><br>Please be prepared for a UAT 
            //                    session before the completion date, 
            //                    that will be communicated by the SW team.<br><br>SW team will work relentlessly to ensure a robust 
            //                    solution for you within the timeline 
            //                    mentioned.<br><br>Have a great day<br><br>Thank you<br>SW Team<br>I&amp;I Department</p>";

            string srtTextBody = $@"Hello {crInfo.RequestorName}, <br><br>
                                    We hope you are doing well. <br>
                                    The Software Team from I &I Department are glad to inform you that your CR (CR#{crInfo.Id}) with the title &quot;{crInfo.Subject}&quot;
                                    has been created and is pending approval. <br>
                                    Please communicate with Mr. Moynul Haque Biswas or Mr. Faisal Khan if there are any further questions or concerns. <br>
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

            _mailSenderService.SendMail(crInfo.Subject, mailBody.ToString(), fromMailUser.ApproverEmail, toMailList, cCMailList, null, fromMailUser.ApproverName);
        }

        public async Task<ApiResponse> GetAllChangeRequest(ChangeRequestListReqModel model, string route, string userId, string ip)
        {
             
            var methodName = "ChangeRequestedInfoService/GetAllChangeRequest";
            try
            {
                
                var pagFilterModel = new PaginationFilter()
                {
                    PageNumber = model.PageNumber == 0 ? 1 : model.PageNumber,
                    PageSize = model.PageSize == 0 ? 10 : model.PageSize,
                };

                model.Status = model.Status == "All" ? string.Empty : model.Status;

                string sql = "EXEC ChangeRequestListSP " +
                "'" + model.Subject + "'," +
                "'" + model.RequestorName + "'," +
                "'" + model.UserId + "'," +
                "'" + model.Status + "'," +
                "'" + model.ApproverEmpId + "'," +
                "'" + model.CrId + "'," +
                "'" + model.Department + "'," +
                "'" + model.PageNumber + "'," +
                "'" + model.PageSize + "'";

                var res = await _context.ChangeRequestListResponse.FromSqlRaw(sql).ToListAsync();

                var totalRecords = res.Count > 0 ? res[0].Total : 0;
                var pagedReponse = PaginationHelper.CreatePagedReponse<SP_ChangeRequestListResponse>(res, pagFilterModel, totalRecords, _uriService, route);
                
                return await _cRRequestResponseService.CreateResponseRequest(model, pagedReponse, ip, methodName, userId, "Ok");

                 

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> CrUserPersonalStatus(ChangeRequestListReqModel model, string route, string userId, string ip)
        {

            var methodName = "ChangeRequestedInfoService/CrUserPersonalStatus";
            try
            {

                var pagFilterModel = new PaginationFilter()
                {
                    PageNumber = model.PageNumber == 0 ? 1 : model.PageNumber,
                    PageSize = model.PageSize == 0 ? 10 : model.PageSize,
                };


                model.Status = model.Status == "All" ? string.Empty : model.Status;

                string sql = $@"select c.*, Total = COUNT(*) OVER() from [dbo].[ChangeRequestedInfo] c
                                Inner Join [dbo].[CrApprovalFlow] a ON a.CrId = c.Id
                                WHERE a.Status = '{model.ApproverStatus}' AND a.ApproverEmpId = '{userId}' ";

                if (!string.IsNullOrEmpty(model.Subject))
                {
                    //sql = $@" {sql} AND c.Subject LIKE '%{model.Subject}%'";
                    sql = $@" {sql} AND (CAST(c.Id as nvarchar) LIKE '%{model.Subject}%' OR c.Subject LIKE '%{model.Subject}%' OR c.RequestorName LIKE '%{model.Subject}%' OR c.EmployeeId LIKE '%{model.Subject}%' OR c.DepartName LIKE '%{model.Subject}%')";
                }
                if (!string.IsNullOrEmpty(model.Status))
                {
                    sql = $@" {sql} AND c.Status = '{model.Status}' ";
                }
                if (!string.IsNullOrEmpty(model.RequestorName))
                {
                    sql = $@" {sql} AND c.RequestorName LIKE '%{model.RequestorName}%' ";
                }
                if (!string.IsNullOrEmpty(model.RequestorName))
                {
                    sql = $@" {sql} AND c.EmployeeId LIKE '%{model.RequestorName}%' ";
                }
                if (model.CrId != null && model.CrId > 0)
                {
                    sql = $@" {sql} AND c.Id = '%{model.CrId}%' ";
                }

                sql = $@" {sql} ORDER BY Id DESC 
				OFFSET ({model.PageNumber}-1)*{model.PageSize} ROWS
				FETCH NEXT {model.PageSize} ROWS ONLY ";

                var res = await _context.ChangeRequestListResponse.FromSqlRaw(sql).ToListAsync();

                var totalRecords = res.Count > 0 ? res[0].Total : 0;
                var pagedReponse = PaginationHelper.CreatePagedReponse<SP_ChangeRequestListResponse>(res, pagFilterModel, totalRecords, _uriService, route);

                return await _cRRequestResponseService.CreateResponseRequest(model, pagedReponse, ip, methodName, userId, "Ok");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId, "Error", ex.Message.ToString());

            }
        }

        public async Task<ApiResponse> CrForDeveloperWise(ChangeRequestListReqModel model, string route, string userId, string ip)
        {

            var methodName = "ChangeRequestedInfoService/CrForDeveloperWise";
            try
            {

                var pagFilterModel = new PaginationFilter()
                {
                    PageNumber = model.PageNumber == 0 ? 1 : model.PageNumber,
                    PageSize = model.PageSize == 0 ? 10 : model.PageSize,
                };


                model.Status = model.Status == "All" ? string.Empty : model.Status;

                string sql = $@"select c.*, Total = COUNT(*) OVER() from [dbo].[ChangeRequestedInfo] c
                                Inner Join [dbo].[AssignToEmployeeInfo] a ON a.CrId = c.Id
                                WHERE a.EmpId = '{userId}' AND a.DeleteStatus = 0 ";
                
                if (!string.IsNullOrEmpty(model.Subject))
                {
                    //sql = $@" {sql} AND c.Subject LIKE '%{model.Subject}%'";
                    sql = $@" {sql} AND (CAST(c.Id as nvarchar) LIKE '%{model.Subject}%' OR c.Subject LIKE '%{model.Subject}%' OR c.RequestorName LIKE '%{model.Subject}%' OR c.EmployeeId LIKE '%{model.Subject}%' OR c.DepartName LIKE '%{model.Subject}%')";
                }
                if (!string.IsNullOrEmpty(model.Status))
                {
                    sql = $@" {sql} AND c.Status = '{model.Status}' ";
                }
                if (!string.IsNullOrEmpty(model.RequestorName))
                {
                    sql = $@" {sql} AND c.RequestorName LIKE '%{model.RequestorName}%' ";
                }
                if (!string.IsNullOrEmpty(model.RequestorName))
                {
                    sql = $@" {sql} AND c.EmployeeId LIKE '%{model.RequestorName}%' ";
                }
                if (model.CrId != null && model.CrId > 0)
                {
                    sql = $@" {sql} AND c.Id = '%{model.CrId}%' ";
                }

                sql = $@" {sql} ORDER BY Id DESC 
				OFFSET ({model.PageNumber}-1)*{model.PageSize} ROWS
				FETCH NEXT {model.PageSize} ROWS ONLY ";

                var res = await _context.ChangeRequestListResponse.FromSqlRaw(sql).ToListAsync();

                var totalRecords = res.Count > 0 ? res[0].Total : 0;
                var pagedReponse = PaginationHelper.CreatePagedReponse<SP_ChangeRequestListResponse>(res, pagFilterModel, totalRecords, _uriService, route);

                return await _cRRequestResponseService.CreateResponseRequest(model, pagedReponse, ip, methodName, userId, "Ok");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId, "Error", ex.Message.ToString());

            }
        }

        public async Task<List<GetAllTotalCrByCatagoryWise>> GetAllCrTotalByCategory(string userId, string ip)
        {
            var methodName = "ChangeRequestedInfoService/GetAllCrTotalByCatagory";
            try
            {
                var sql_query = $@"SELECT ChangeRequestFor, count(ChangeRequestFor) as TotalItem 
                                    FROM [dbo].[ChangeRequestedInfo] GROUP BY ChangeRequestFor 
                                    ORDER BY count(ChangeRequestFor) DESC";

                var getAllTotalCrByCatagory = await _context.GetAllTotalCrByCatagoryWise.FromSqlRaw(sql_query).AsNoTracking().ToListAsync();
               
                await _cRRequestResponseService.CreateResponseRequest(null, getAllTotalCrByCatagory, ip, methodName, userId, "Ok");
                return getAllTotalCrByCatagory;


			}
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
				await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId, "Error", ex.Message.ToString());
				throw new Exception(ex.Message); // this method dose not return api responce model.
            }
        }

        public async Task<List<ChangeRequestedInfo>> GetLastFiveCr(string userId, string ip)
        {
            var methodName = "ChangeRequestedInfoService/GetLastFiveCr";
            try
            {
                var lastFiveCr = await _changeRequestedInfoRepository.QueryAll().OrderByDescending(x => x.Id).Take(5).ToListAsync();
         
                await _cRRequestResponseService.CreateResponseRequest(null, lastFiveCr, ip, methodName, userId, "Ok");
                return lastFiveCr;

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                throw new Exception(ex.Message); // this method dose not return api responce model.
            }
        }

        public async Task<ApiResponse> GetPendingAllChangeRequest(ChangeRequestListReqModel model, string route, string userId, string ip)
        {
             
            var methodName = "ChangeRequestedInfoService/GetPendingAllChangeRequest";
            try
            {

                var pagFilterModel = new PaginationFilter()
                {
                    PageNumber = model.PageNumber == 0 ? 1 : model.PageNumber,
                    PageSize = model.PageSize == 0 ? 10 : model.PageSize,
                };

                model.Status = model.Status == "All" ? string.Empty : model.Status;

                string sql = "EXEC SP_PendingChangeRequestListSP " +
                "'" + model.Subject + "'," +
                "'" + model.RequestorName + "'," +
                "'" + userId + "'," +
                "'" + model.Status + "'," +
                "'" + model.PageNumber + "'," +
                "'" + model.PageSize + "'";

                var res = await _context.ChangeRequestListResponse.FromSqlRaw(sql).ToListAsync();

                var totalRecords = res.Count > 0 ? res[0].Total : 0;
                var pagedReponse = PaginationHelper.CreatePagedReponse<SP_ChangeRequestListResponse>(res, pagFilterModel, totalRecords, _uriService, route);
                             
                return await _cRRequestResponseService.CreateResponseRequest(model, pagedReponse, ip, methodName, userId, "Ok");
                 

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(model, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> GetStatusWiseTotalCR(string showFor, string department, string userId, string ip)
        {
             
            var methodName = "ChangeRequestedInfoService/GetStatusWiseTotalCR";
            try
            {
				string sql, Message = string.Empty;
                if (showFor == "StatusWiseTotalCR")
                {
                    string addDpt = "";
                    if (!string.IsNullOrEmpty(department))
                    {
                        addDpt = $@"Where DepartName = '{department}'";
                    }
                    sql = $@"select Status, StatusDisplayName, count(Status) as Total from [dbo].[ChangeRequestedInfo] 
                            {addDpt} group by Status, StatusDisplayName";
                }
                else if(showFor == "MyCrTotalStatus")
                {
                    sql = $@"select Status, StatusDisplayName, count(Status) as Total from [dbo].[ChangeRequestedInfo] 
                            where EmployeeId = '{userId}' group by Status, StatusDisplayName";
                }
                else if(showFor is "MyApprovedCrTotalStatus")
                {
                    sql = $@"select c.Status, c.StatusDisplayName, count(c.Status) as Total from [dbo].[ChangeRequestedInfo] c
                                Inner Join [dbo].[CrApprovalFlow] a ON a.CrId = c.Id
                                WHERE a.Status = '{AllStatus.Approved.ToString()}' AND a.ApproverEmpId = '{userId}'
                                group by c.Status, c.StatusDisplayName";                    
                }
                else if(showFor is "DeveloperCrTotalStatus")
                {
                    sql = $@"select c.Status, c.StatusDisplayName, count(c.Status) as Total from [dbo].[ChangeRequestedInfo] c
                                Inner Join [dbo].[AssignToEmployeeInfo] a ON a.CrId = c.Id
                                WHERE a.EmpId = '{userId}' group by c.Status, c.StatusDisplayName";
                }
                else
                {
                    sql = $@"select Status, c.StatusDisplayName, count(Status) as Total from [dbo].[ChangeRequestedInfo] 
                        where EmployeeId = '{userId}' group by Status, c.StatusDisplayName";
                }

                var res = await _context.StatusWiseTotalCrResponse.FromSqlRaw(sql).AsNoTracking().ToListAsync();

                if(res == null)
                {
                    Message = "Data is empty";
                }
                else
                {
                    if (res.Count > 0)
                    {
                        var totalCr = 0;
                        foreach (var item in res)
                        {
                            totalCr = totalCr + item.Total;
                        }
                        var newItem = new StatusWiseTotalCrResponse()
                        {
                            Status = "All",
                            StatusDisplayName = "All",
                            Total = totalCr
                        };
                        res.Insert(0, newItem);
                        Message = "Ok";
                    }
                }
                return await _cRRequestResponseService.CreateResponseRequest(null, res, ip, methodName, userId, Message);
                 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> GetAllFiles(long crid, string userId, string ip)
        {
            var methodName = "ChangeRequestedInfoService/GetAllFiles";
            try
            {
                var getCr = await _changeRequestedInfoRepository.FindFirstOrDefaultAsync(x => x.Id == crid);
                if(getCr == null)
                {
                    throw new Exception("Cr is not found");
                }
                var allFiles = await _crAttatchedFilesRepository.FindAsync(x => x.CrId == getCr.Id);
                
                return await _cRRequestResponseService.CreateResponseRequest(null, allFiles, ip, methodName, userId, "Ok");

                 

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }


    }
}

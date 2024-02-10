
using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Pagination;
using L3T.Infrastructure.Helpers.Pagination.Filter;
using L3T.Infrastructure.Helpers.Pagination.Helper;
using L3T.Infrastructure.Helpers.Repositories.Interface;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using L3T.Utility.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.Service.Implementation.ChangeRequest.v1
{
    public class AssignEmployeeService : IAssignEmployeeService
    {
        private readonly IAssignEmployeeRepository _assignEmployeeRepository;
        private readonly IAssignDeveloperRepository _assignDeveloperRepository;
        private readonly ChangeRequestDataContext _context;
        private readonly ILogger<AssignEmployeeService> _logger;
        private readonly IUriService _uriService;
        private readonly ICRRequestResponseService _cRRequestResponseService;
        private readonly IChangeRequestedInfoRepository _changeRequestedInfoRepository;
        private readonly ICommonRepository _commonRepository;

        public AssignEmployeeService(
            IAssignEmployeeRepository AssignEmployeeRepository,
            ChangeRequestDataContext context,
            ILogger<AssignEmployeeService> logger,
            IUriService uriService,
            IAssignDeveloperRepository AssignDeveloperRepository,
            ICRRequestResponseService cRRequestResponseService,
            IChangeRequestedInfoRepository changeRequestedInfoRepository,
            ICommonRepository commonRepository)
        {
            _assignEmployeeRepository = AssignEmployeeRepository;
            _assignDeveloperRepository = AssignDeveloperRepository;
            _context = context;
            _logger = logger;
            _uriService = uriService;
            _cRRequestResponseService = cRRequestResponseService;
            _changeRequestedInfoRepository = changeRequestedInfoRepository;
            _commonRepository = commonRepository;
        }

        public async Task<ApiResponse> AddAssignEmployee(AddAssignEmployeeReq requestModel, string getUserid, string ip)
        {
             
            var methodName = "AssignEmployeeService/AddAssignEmployee";

            try
            {
                if (string.IsNullOrWhiteSpace(requestModel.EmpName))
                {
                    throw new Exception("Please enter details properly.");
                }

                var haveThisEmployee = await _assignEmployeeRepository.FindFirstOrDefaultAsync(x => x.CrId == requestModel.CrId && x.EmpId == requestModel.EmpId && x.DeleteStatus == false);
                if( haveThisEmployee != null)
                {
                    throw new Exception("This Employee is already assigned");
                }

                var requestDbModel = new AssignToEmployeeInfo()
                {
                    EmpId = requestModel.EmpId,
                    EmpName = requestModel.EmpName,
                    CrId = requestModel.CrId,
                    StartDate = requestModel.StartDate,
                    TotalDay = requestModel.TotalDay,
                    Task = requestModel.Task,
                    CreatedAt = DateTime.Now,
                    CreatedBy = getUserid
                };


                if (requestModel.Id > 0)
                {
                    requestDbModel.DeleteStatus = false;
                    requestDbModel.LastModifiedBy = getUserid;
                    requestDbModel.LastModifiedAt = DateTime.Now;

                    _assignEmployeeRepository.Update(requestDbModel);
                }
                else
                {
                    await _assignEmployeeRepository.CreateAsync(requestDbModel);
                }

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await _cRRequestResponseService.CreateResponseRequest(requestModel, null, ip, methodName, getUserid, "Assigned developer save successfully!");
                     
                }
                throw new Exception("Assigned developer Save some problem.");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(requestModel, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());
                 
            }

        }

        public async Task<ApiResponse> AssignEmployeeDelete(long Id, string getUserid, string ip)
        {
             
            var methodName = "AssignEmployeeService/AssignEmployeeStatusChange";
            try
            {
                var getData = await _assignEmployeeRepository.FirstOrDefaultAsync(x => x.Id == Id);

                if (getData != null)
                {
                    getData.DeleteStatus = true;
                    getData.LastModifiedBy = getUserid;
                    getData.LastModifiedAt = DateTime.UtcNow;

                    _assignEmployeeRepository.UpdateLatest(getData);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var Message = "Assigned developer removed successfully!";
                        return await _cRRequestResponseService.CreateResponseRequest(Id, null, ip, methodName, getUserid, Message);
                         
                    }
                    throw new Exception("Something is wrong.");
                }
                throw new Exception("Assigned developer removed some problem.");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(Id, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> GetAllAssignEmployeeQuery(AssignEmployeeListReqModel FilterReqModel, string route, string getUserid, string ip)
        {
             
            var methodName = "AssignEmployeeService/GetAllAssignEmployeeQuery";
            try
            {
                int totalRecords = 0;
                var pagFilterModel = new PaginationFilter()
                {
                    PageNumber = FilterReqModel.PageNumber,
                    PageSize = FilterReqModel.PageSize,
                };

                var res = await _commonRepository.GetAllAssignEmployeeFromDB(FilterReqModel);

                if(res.Count > 0)
                {
                    totalRecords = res.Count > 0 ? res[0].Total : 0;
                }

                var pagedReponse = PaginationHelper.CreatePagedReponse<SP_AssignEmployeeListResponse>(res, pagFilterModel, totalRecords, _uriService, route);

                return await _cRRequestResponseService.CreateResponseRequest(FilterReqModel, pagedReponse, ip, methodName, getUserid, "OK");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> GetAssignEmployeeByCrId(long CrId, string getUserid, string ip)
        {
             
            var methodName = "AssignEmployeeService/GetAssignEmployeeByCrId";

            try
            {

                //string sql = "SELECT * FROM [dbo].[AssignToEmployeeInfo] where CrId = " + CrId;

                // var res = await _context.AssignEmployeeListResponse.FromSqlRaw(sql).ToListAsync();

                var res = await _assignEmployeeRepository.FindAsync(x => x.CrId == CrId && x.DeleteStatus == false);
                if (res != null)
                {
                    return await _cRRequestResponseService.CreateResponseRequest(null, res, ip, methodName, getUserid, "Ok");
                }
                throw new Exception("Data not found.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());
                 
            }

        }

        public async Task<ApiResponse> UpdateAssignEmployee(UpdateAssignEmployeeReq ReqModel, string getUserid, string ip)
        {
             
            var methodName = "AssignEmployeeService/UpdateAssignEmployee";

            try
            {
                if (string.IsNullOrWhiteSpace(ReqModel.EmpId))
                {
                    throw new Exception("Please enter details properly");
                     
                }

                var getData = await _assignEmployeeRepository.FirstOrDefaultAsync(x => x.Id == ReqModel.Id);

                if (getData != null)
                {

                    getData.EmpId = ReqModel.EmpId;
                    getData.EmpName = ReqModel.EmpName;
                    getData.CrId = ReqModel.CrId;

                    getData.DeleteStatus = ReqModel.DeleteStatus;
                    getData.LastModifiedBy = "system";
                    getData.LastModifiedAt = DateTime.UtcNow;

                    _assignEmployeeRepository.UpdateLatest(getData);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var Message = "Assigned developer update successfully!";
                        return await _cRRequestResponseService.CreateResponseRequest(ReqModel, null, ip, methodName, getUserid, Message);
                         
                    }
                    throw new Exception("Something is worng.");
                }
                else
                {
                    throw new Exception("Your requested data not updated");

                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> UpdateAssignEmployeeTaskStatusIsCompleteOrIncomplete(long crId, string getUserid, string ip)
        {

            var methodName = "AssignEmployeeService/UpdateAssignEmployeeTaskStatusIsCompleteOrIncomplete";

            try
            {
                var crInfo = await _changeRequestedInfoRepository.FirstOrDefaultAsync(x => x.Id == crId);
                if (crInfo is null)
                {
                    throw new Exception("CR Information is not found");
                }
                if(crInfo.Status != AllStatus.InProgress.ToString())
                {
                    throw new Exception("Cr status is not In Progress.");
                }
                var getData = await _assignEmployeeRepository.FirstOrDefaultAsync(x => x.CrId == crId && x.EmpId == getUserid && x.DeleteStatus == false);
                if (getData is null)
                {
                    throw new Exception("Developer Information is not found");

                }

                if(getData.Task == AllStatus.Completed.ToString())
                {
                    getData.Task = null;
                }
                else
                {
                    getData.Task = AllStatus.Completed.ToString();
                }
                
                getData.LastModifiedBy = getUserid;
                getData.LastModifiedAt = DateTime.UtcNow;

                _assignEmployeeRepository.UpdateLatest(getData);
                if (await _context.SaveChangesAsync() > 0)
                {
                    var Message = "Developer Task is update successfully!";
                    return await _cRRequestResponseService.CreateResponseRequest(crId, null, ip, methodName, getUserid, Message);

                }
                throw new Exception("Something is worng.");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(crId, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());

            }
        }

        public async Task<ApiResponse> GetAllAssignDeveloper(string userId, string ip)
        {
             
            var methodName = "AssignEmployeeService/GetAllAssignDeveloper";

            try
            {
                var res = _assignDeveloperRepository.QueryAll();
                if (res != null)
                {
                    return await _cRRequestResponseService.CreateResponseRequest(null, res, ip, methodName, userId, "Ok");
                }
                throw new Exception("Data not found.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }

        }

        public async Task<ApiResponse> UpdateAllAssignDeveloper(string userId, string ip)
        {
             
            var methodName = "AssignEmployeeService/UpdateAllAssignDeveloper";

            try
            {
                var sqlQuery = "EXEC UpDateDeveloperInformation";
                var res = await _context.Database.ExecuteSqlRawAsync(sqlQuery);
                if (res != null)
                {
                    return await _cRRequestResponseService.CreateResponseRequest(null, res, ip, methodName, userId, "Ok");
                }
                throw new Exception("Data not found.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

    }
}

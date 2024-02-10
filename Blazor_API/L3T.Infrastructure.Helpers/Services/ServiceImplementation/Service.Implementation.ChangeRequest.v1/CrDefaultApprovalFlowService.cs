using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.ThirdPartyModel;
using L3T.Infrastructure.Helpers.Pagination;
using L3T.Infrastructure.Helpers.Repositories.Implementation.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Repositories.Interface;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.ThirdParty;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.Service.Implementation.ChangeRequest.v1
{
    public class CrDefaultApprovalFlowService : ICrDefaultApprovalFlowService
    {
        private readonly ICrDefaultApprovalFlowRepository _crDefaultApprovalFlowRepository;
        private readonly ChangeRequestDataContext _context;
        private readonly ILogger<CrDefaultApprovalFlowRepository> _logger;
        private readonly IUriService _uriService;
        private readonly ICRRequestResponseService _cRRequestResponseService;
        private readonly IThirdPartyService _thirdPartyService;
        private readonly IConfiguration _configuration;


		public CrDefaultApprovalFlowService(
			ICrDefaultApprovalFlowRepository crDefaultApprovalFlowRepository,
			IUriService uriService,
			ChangeRequestDataContext context,
			ILogger<CrDefaultApprovalFlowRepository> logger,
			ICRRequestResponseService cRRequestResponseService,
			IThirdPartyService thirdPartyService,
			IConfiguration configuration)

		{
			_crDefaultApprovalFlowRepository = crDefaultApprovalFlowRepository;
			_logger = logger;
			_uriService = uriService;
			_context = context;
			_cRRequestResponseService = cRRequestResponseService;
			_thirdPartyService = thirdPartyService;
			_configuration = configuration;
		}

		public async Task<ApiResponse> AddCrDefaultApprovalFlow(AddCrDefaultApprovalFlowReq requestModel, string userId, string ip)
        {
            var methodName = "CrDefaultApprovalFlowService/AddCrDefaultApprovalFlow";
            try
            {
                int lastFlow = 0;
                if (requestModel.IsPrincipleApprover)
                {
                    requestModel.Department = _configuration.GetValue<string>("DefaultApproverDepartment:DepartmentName");
				}

				var userInfoList = await _thirdPartyService.GetAllDefaultApprovalFlow(requestModel.ApproverEmpId);
                if(userInfoList.StatusCode == 400 || userInfoList.Data == null)
                {
					throw new Exception(userInfoList.Message);
				}
				if (userInfoList.Data.Count == 0)
				{
					throw new Exception("User is not found.");
				}

				AppUserModel user = userInfoList.Data.FirstOrDefault();

                var primaryApprover = new List<CrDefaultApprovalFlow>();

				List<CrDefaultApprovalFlow> getAllPerson = await _crDefaultApprovalFlowRepository.FindAsync(x => x.IsPrincipleApprover == requestModel.IsPrincipleApprover && x.Department == requestModel.Department);
                
                if(getAllPerson != null && getAllPerson.Count > 0)
                {
                    var checkDuplicate = getAllPerson.FirstOrDefault(x => x.ApproverEmpId == requestModel.ApproverEmpId);
                    if (checkDuplicate != null)
                    {
                        throw new Exception("Your provided name already exist.");
                    }
                    lastFlow = getAllPerson.OrderByDescending(x => x.ApproverFlow).Select(x => x.ApproverFlow).FirstOrDefault();
                }

                if(requestModel.ParentId == 0)
                {
                    requestModel.ParentId = getAllPerson.FindAll(x => x.ParentId == 0).Select(x=> x.ParentId).FirstOrDefault();
                }

                var requestDbModel = new CrDefaultApprovalFlow()
                {
                    ApproverName = user.fullName,
                    ApproverDesignation = user.user_designation,
                    ApproverDepartment = user.department,
                    ApproverEmpId = requestModel.ApproverEmpId,
                    ApproverRole = user.RoleName,
                    ApproverFlow = (lastFlow + 1),
                    ParentId = requestModel.ParentId,
                    IsPrincipleApprover = requestModel.IsPrincipleApprover,
                    Department = requestModel.Department,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId

                };
                await _crDefaultApprovalFlowRepository.CreateAsync(requestDbModel);
                if (await _context.SaveChangesAsync() > 0)
                {                    
                    var Message = "CR Default Approval Flow save successfully!";
                    return await _cRRequestResponseService.CreateResponseRequest(requestModel, null, ip, methodName, userId, "OK");                     
                }
                _logger.LogInformation("Add Default CR ApprovalFlow : " + requestDbModel);
                throw new Exception("CR Default ApprovalFlow Save some problem.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(requestModel, ex, ip, methodName, userId,"Error", ex.Message.ToString());                 
            }
        }

        public async Task<ApiResponse> CrDefaultApprovalFlowDelete(long Id, string userId, string ip)
        {
             
            var methodName = "DefaultApprovalFlowService/CrDefaultApprovalFlowDelete";
            try
            {
                var res = await _crDefaultApprovalFlowRepository.FirstOrDefaultAsync(x => x.Id == Id);
                if (res != null)
                {
                    await _crDefaultApprovalFlowRepository.Delete(res);
                    await _context.SaveChangesAsync();
					var Message = "Data Deleted Successfully.";
					return await _cRRequestResponseService.CreateResponseRequest(Id, null, ip, methodName, userId, Message);
				}
                else
                {
                    _logger.LogInformation("CR Default ApprovalFlow Delete : " + Id);
                    throw new Exception("Data not found.");
                }
                 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(Id, ex, ip, methodName, userId,"Error", ex.Message.ToString());                 
            }
        }

        public async Task<ApiResponse> GetAllDefaultApprovalFlow(string userId, string ip)
        {
             
            var methodName = "CrDefaultApprovalFlowService/GetAllDefaultApprovalFlow";
            try
            {
                var res = await _crDefaultApprovalFlowRepository.FindAsync(null);
                return await _cRRequestResponseService.CreateResponseRequest(null, res, ip, methodName, userId, "Ok");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }
        public async Task<ApiResponse> ActiveInactiveApprover(UpdateCrDefaultApprovalFlowReq ReqModel, string userId, string ip)
        {
            var methodName = "CrDefaultApprovalFlowService/ActiveInactiveApprover";
            try
            {
                var getData = await _crDefaultApprovalFlowRepository.FirstOrDefaultAsync(x => x.Id == ReqModel.Id);
                if(getData.ParentId == 0 && getData.IsActive == true)
                {
                    throw new Exception("Inactive is not possible.");
                }
                if (getData != null)
                {
                    getData.IsActive = ReqModel.IsActive;

                    _crDefaultApprovalFlowRepository.UpdateLatest(getData);
                    _context.SaveChanges();
                    var Message = "CR Default ApprovalFlow update successfully!";

                    return await _cRRequestResponseService.CreateResponseRequest(ReqModel, getData, ip, methodName, userId, Message);

                }
                else
                {

                    _logger.LogInformation("CR Default ApprovalFlowUpdate : " + ReqModel);
                    throw new Exception("Data not found.");
                }


            }
            catch (Exception ex)
            {

                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId, "Error", ex.Message.ToString());

            }
        }
        public async Task<ApiResponse> UpdateCrDefaultApprovalFlow(UpdateCrDefaultApprovalFlowReq ReqModel, string userId, string ip)
        {             
            var methodName = "CrDefaultApprovalFlowService/UpdateCrDefaultApprovalFlow";
            try
            {
                var getData = await _crDefaultApprovalFlowRepository.FirstOrDefaultAsync(x => x.Id == ReqModel.Id);
                if (getData != null)
                { 
                   
                    getData.ApproverName = ReqModel.ApproverName;
                    getData.ApproverDesignation = ReqModel.ApproverDesignation;
                    getData.ApproverDepartment = ReqModel.ApproverDepartment;
                    getData.ApproverEmpId = ReqModel.ApproverEmpId;
                    getData.ApproverFlow = ReqModel.ApproverFlow;
                    getData.ParentId = ReqModel.ParentId;
                    getData.LastModifiedBy = userId;
                    getData.LastModifiedAt = DateTime.UtcNow;
                    getData.IsPrincipleApprover = ReqModel.IsPrincipleApprover;
                    getData.IsActive = ReqModel.IsActive;

                    _crDefaultApprovalFlowRepository.UpdateLatest(getData);
                    _context.SaveChanges();
					var Message = "CR Default ApprovalFlow update successfully!";

					return await _cRRequestResponseService.CreateResponseRequest(ReqModel, getData, ip, methodName, userId, Message);
					
                }
                else
                {

                    _logger.LogInformation("CR Default ApprovalFlowUpdate : " + ReqModel);
                    throw new Exception("Data not found.");
                }
                 

            }
            catch (Exception ex)
            {

                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

    }
}

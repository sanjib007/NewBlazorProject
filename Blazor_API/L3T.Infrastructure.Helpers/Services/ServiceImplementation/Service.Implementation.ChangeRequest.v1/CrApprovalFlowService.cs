using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Pagination;
using L3T.Infrastructure.Helpers.Repositories.Implementation.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using L3T.Utility.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.Service.Implementation.ChangeRequest.v1
{
    public class CrApprovalFlowService : ICrApprovalFlowService
    {
        private readonly ICrApprovalFlowRepository _crApprovalFlowRepository;
        private readonly ChangeRequestDataContext _context;
        private readonly ILogger<CrApprovalFlowRepository> _logger;
        private readonly ICRRequestResponseService _cRRequestResponseService;

        public CrApprovalFlowService(
            ICrApprovalFlowRepository crApprovalFlowRepository,
            ChangeRequestDataContext context,
            ILogger<CrApprovalFlowRepository> logger, 
            ICRRequestResponseService cRRequestResponseService)

        {
            _crApprovalFlowRepository = crApprovalFlowRepository;
            _logger = logger;
            _context = context;
            _cRRequestResponseService = cRRequestResponseService;
        }

        public async Task<ApiResponse> AddCrApprovalFlow(AddCrApprovalFlowReq requestModel, string userId, string ip)
        {
             

            var methodName = "CrApprovalFlowService/AddCrApprovalFlow";
            try
            {
                if (string.IsNullOrWhiteSpace(requestModel.ApproverName))
                {
                    throw new Exception("Please enter details properly.");

                }
                var checkName = await _crApprovalFlowRepository.FirstOrDefaultAsync(x => x.ApproverEmpId == requestModel.ApproverEmpId);

                if (checkName != null)
                {
                    throw new Exception("Your provided name already exist.");

                }

                
                var requestDbModel = new CrApprovalFlow()
                {
                    CrId                = requestModel.CrId,
                    ApproverName        = requestModel.ApproverName,
                    ApproverDesignation = requestModel.ApproverDesignation,
                    ApproverDepartment = requestModel.ApproverDepartment,
                    ApproverEmpId      = requestModel.ApproverEmpId,
                    ApproverFlow      = requestModel.ApproverFlow,
                    ParentId          = requestModel.ParentId,
                    Remark           = requestModel.Remark,
                    CreatedAt        = DateTime.UtcNow,
                    CreatedBy        = userId

                };
                await _crApprovalFlowRepository.CreateAsync(requestDbModel);
                if (await _context.SaveChangesAsync() > 0)
                {
                    var Message = "CR Approval Flow save successfully!";

                    return await _cRRequestResponseService.CreateResponseRequest(requestModel, requestDbModel, ip, methodName, userId, "Ok");

                     
                }
                _logger.LogInformation("Add CR ApprovalFlow : " + requestDbModel);

                throw new Exception("CR ApprovalFlow Save some problem.");
            }
            catch (Exception ex)
            {

                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> CrApprovalFlowDelete(long Id, string userId, string ip)
        {
             
            var methodName = "ApprovalFlowService/CrApprovalFlowDelete";
            try
            {

                var res = await _crApprovalFlowRepository.FirstOrDefaultAsync(x => x.Id == Id);
                if (res != null)
                {

                    await _crApprovalFlowRepository.Delete(res);
                    await _context.SaveChangesAsync();

					var Message = "Data Deleted Successfully.";
					return await _cRRequestResponseService.CreateResponseRequest(null, res, ip, methodName, userId, Message);
				}
                else
                {
                    _logger.LogInformation("CR ApprovalFlow Delete : " + Id);
					return await _cRRequestResponseService.CreateResponseRequest(Id, "CR ApprovalFlow Delete : " + Id, ip, methodName, userId, "Error", "CR ApprovalFlow Delete : " + Id);

				}
                 
            }
            catch (Exception ex)
            {

                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(Id, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> GetCrApprovalFlow(long CrId, string userId, string ip)
        {
             
            var methodName = "CrApprovalFlowService/GetCrApprovalFlow";
            try
            {

                var res = await _crApprovalFlowRepository.FindAsync(x=>x.CrId == CrId);
                if (res != null)
                {
                    return await _cRRequestResponseService.CreateResponseRequest(CrId, res, ip, methodName, userId, "Ok");
                }
                else
                {
                    _logger.LogInformation("GetCrApprovalFlow : " + CrId);
                    throw new Exception("Data not found.");
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(CrId, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }
        public async Task<ApiResponse> CrApprovedApproverList(long CrId,string userId, string ip)
        {
             
            var methodName = "CrApprovalFlowService/CrApprovedApproverList";
            try
            {

                var res = await _crApprovalFlowRepository.QueryAll(x => x.Status == AllStatus.Approved.ToString() && x.CrId == CrId).ToListAsync();
                if (res != null)
                {
                    var aData = new ApiResponse()
                    {
                        Status = "Success",
                        Message = "",
                        StatusCode = 200,
                        Data = res
                    };

                    return await _cRRequestResponseService.CreateResponseRequest(CrId, res, ip, methodName, userId, "Ok");
                }
                else
                {
                    _logger.LogInformation("CrApprovedApproverList : " + CrId);
                    throw new Exception("Data not found.");

                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(CrId, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> OnlyApprovedDataByCrId(long CrId, string userId, string ip)
        {
             
            var methodName = "CrApprovalFlowService/OnlyApprovedDataByCrId";
            try
            {

                var res = await _crApprovalFlowRepository.FindAsync(x => x.CrId == CrId && x.Status == AllStatus.Approved.ToString());
                
                return await _cRRequestResponseService.CreateResponseRequest(CrId, res, ip, methodName, userId, "Ok");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(CrId, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> UpdateCrApprovalFlow(UpdateCrApprovalFlowReq ReqModel, string userId, string ip)
        {
             
            var methodName = "CrApprovalFlowService/UpdateCrApprovalFlow";
            try
            {
                var getData = await _crApprovalFlowRepository.FirstOrDefaultAsync(x => x.Id == ReqModel.Id);
                if (getData != null)
                {
                    getData.ApproverFlow = ReqModel.ApproverFlow;
                    getData.ParentId = ReqModel.ParentId;
                    getData.Status = ReqModel.Status;
                    getData.StatusDate = ReqModel.StatusDate;
                    getData.Remark = ReqModel.Remark;
                    getData.LastModifiedBy = userId;
                    getData.LastModifiedAt = DateTime.UtcNow;

                    _crApprovalFlowRepository.UpdateLatest(getData);
                    _context.SaveChanges();

					var Message = "CR ApprovalFlow update successfully!";

					return await _cRRequestResponseService.CreateResponseRequest(ReqModel, getData, ip, methodName, userId, "Ok");
				}
                else
                {

                    _logger.LogInformation("CR ApprovalFlowUpdate : " + ReqModel);
                    throw new Exception("Data not found.");
                }
                 

            }
            catch (Exception ex)
            {

                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(ReqModel, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> AddRemark(AddRemarkRequestModel ReqModel, string userId, string ip)
        {
             
            var methodName = "CrApprovalFlowService/UpdateCrApprovalFlow";
            try
            {
                var getData = await _crApprovalFlowRepository.FirstOrDefaultAsync(x => x.CrId == ReqModel.CrId && x.ApproverEmpId == userId);
                if (getData != null)
                {
                    getData.Remark = ReqModel.Remark;
                    getData.LastModifiedBy = userId;
                    getData.LastModifiedAt = DateTime.UtcNow;

                    _crApprovalFlowRepository.UpdateLatest(getData);
                    _context.SaveChanges();
                    var Message = "CR ApprovalFlow update successfully!";

					return await _cRRequestResponseService.CreateResponseRequest(ReqModel, getData, ip, methodName, userId, Message);
				}
                else
                {
                    _logger.LogInformation("CR ApprovalFlowUpdate : " + ReqModel);
                    throw new Exception("Data not found.");
                }
                 

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(ReqModel, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

    }
}

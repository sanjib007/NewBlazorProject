using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.Service.Implementation.ChangeRequest.v1
{
    public class ChangeRequestLogService: IChangeRequestLogService
    {
        private readonly IChangeRequestLogRepository _changeRequestLogRepository;
        private readonly ChangeRequestDataContext _context;
        private readonly ILogger<ChangeRequestLogService> _logger;
        private readonly ICRRequestResponseService _cRRequestResponseService;
        public ChangeRequestLogService(IChangeRequestLogRepository changeRequestLogRepository, ChangeRequestDataContext context,
            ILogger<ChangeRequestLogService> logger, ICRRequestResponseService cRRequestResponseService)
        {
            _context = context;
            _changeRequestLogRepository = changeRequestLogRepository;
            _logger = logger;
            _cRRequestResponseService = cRRequestResponseService;
        }

        public async Task<ApiResponse> AddChangeRequestLog(AddChangeRequestLogReq requestModel, string getUserid, string ip)
        {

            var methodName = "ChangeRequestLogService/AddChangeRequestLog";

            try
            {
                if (string.IsNullOrWhiteSpace(requestModel.CRChangeStatus))
                {
                    throw new Exception("Please enter CR Change Status properly.");
                }

                var requestDbModel = new ChangeRequestLog()
                {
                    CrId = requestModel.CrId,
                    CRChangeStatus = requestModel.CRChangeStatus,
                    CrApprovalFlowId = requestModel.CrApprovalFlowId,
                    AEId = requestModel.AEId,
                    TaskFlow = requestModel.TaskFlow,
                    Remark = requestModel.Remark,
                    CreatedAt = DateTime.Now,
                    CreatedBy = getUserid
                };

                await _changeRequestLogRepository.CreateAsync(requestDbModel);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return await _cRRequestResponseService.CreateResponseRequest(requestModel, null, ip, methodName, getUserid, "Change Request Log save successfully!");
                }
                throw new Exception("Change Request Log save failed.");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(requestModel, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());
            }

        }
    }
}

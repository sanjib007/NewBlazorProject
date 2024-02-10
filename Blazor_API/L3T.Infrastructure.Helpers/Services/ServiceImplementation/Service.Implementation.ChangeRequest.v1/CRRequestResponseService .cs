using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Pagination;
using L3T.Infrastructure.Helpers.Pagination.Filter;
using L3T.Infrastructure.Helpers.Pagination.Helper;
using L3T.Infrastructure.Helpers.Repositories.Implementation.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.Service.Implementation.ChangeRequest.v1
{
    public class CRRequestResponseService : ICRRequestResponseService
    {
        private readonly ICRRequestResponseRepository _CRRequestResponseRepository;
        private readonly ChangeRequestDataContext _context;
        private readonly ILogger<CRRequestResponseRepository> _logger;
        private readonly IUriService _uriService;

        public CRRequestResponseService(
            ICRRequestResponseRepository CRRequestResponseRepository, 
            ILogger<CRRequestResponseRepository> logger, 
            ChangeRequestDataContext context, 
            IUriService uriService
            )
        {
            _context = context;
            _CRRequestResponseRepository = CRRequestResponseRepository;
            _logger = logger;
            _uriService = uriService;
        }

        public async Task<ApiResponse> CRRequestResponseList(NotificationListFilterReqModel notificationListFilterReqModel, string getUserid, string route)
        {
             
            try
            {
                var pagFilterModel = new PaginationFilter()
                {
                    PageNumber = notificationListFilterReqModel.PageNumber,
                    PageSize = notificationListFilterReqModel.PageSize,
                };

                var result = await _CRRequestResponseRepository.QueryAll().ToListAsync();
                var totalRecords = result.Count > 0 ? result.Count : 0;
                var pagedReponse = PaginationHelper.CreatePagedReponse(result, pagFilterModel, totalRecords, _uriService, route);

                return new ApiResponse()
                {
                    Status = "Success",
                    Message = "",
                    StatusCode = 200,
                    Data = pagedReponse
                };
                 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now}" + ex.Message.ToString());
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> AddCRRequestResponse(CRRequestResponseModel CRRequestResponse)
        {
            var response = new ApiResponse();
            try
            {
                var result = _CRRequestResponseRepository.CreateAsync(CRRequestResponse);

                if (await _CRRequestResponseRepository.SaveChangeAsync() > 0)
                {
                    response.Status = "Success";
                    response.StatusCode = 200;
                    response.Message = "Request Response saved successfully!";
                    return response;

				}

                _logger.LogInformation("Add Request Response : " + CRRequestResponse);

                throw new Exception("Notifications Save has some problem.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now}" + ex.Message.ToString());
                throw new Exception(ex.Message);
            }

        }

		public async Task<ApiResponse> CreateResponseRequest(object request, object response, string cusIp, string methodName, string userId, string successMess, string errorMessage = null)
        {
            var newResponse = new ApiResponse();
			if (!string.IsNullOrEmpty(errorMessage))
			{
				newResponse = new ApiResponse()
				{
					Status = "Error",
					StatusCode = 400,
					Message = errorMessage,
                    Data = response
                };
			}
			else
			{
				newResponse = new ApiResponse()
				{
					Status = "Success",
					StatusCode = 200,
					Message = successMess,
					Data = response
				};
			}

			var reqResModel = new CRRequestResponseModel()
            {
                Request = JsonConvert.SerializeObject(request),
                Response = Convert.ToString(JsonConvert.SerializeObject(newResponse)),
                MethodName = methodName,
                RequestedIP = cusIp,
                CreatedAt = DateTime.Now,
                UserId = userId,
                ErrorLog = errorMessage,
                CreatedBy = userId

            };
            await AddCRRequestResponse(reqResModel);

            return newResponse;

		}

        

    }
}

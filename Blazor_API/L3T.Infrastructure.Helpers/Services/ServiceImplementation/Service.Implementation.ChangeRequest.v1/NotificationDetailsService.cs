using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Pagination;
using L3T.Infrastructure.Helpers.Pagination.Filter;
using L3T.Infrastructure.Helpers.Pagination.Helper;
using L3T.Infrastructure.Helpers.Repositories.Implementation.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.Service.Implementation.ChangeRequest.v1
{
    public class NotificationDetailsService : INotificationDetailsService
    {
        private readonly INotificationDetailsRepository _notificationDetailsRepository;
        private readonly ICRRequestResponseService _cRRequestResponseService;
        private readonly ChangeRequestDataContext _context;
        private readonly ILogger<NotificationDetailsRepository> _logger;
        private readonly IUriService _uriService;
        public NotificationDetailsService(
            INotificationDetailsRepository notificationDetailsRepository, 
            ILogger<NotificationDetailsRepository> logger, 
            ChangeRequestDataContext context, 
            ICRRequestResponseService cRRequestResponseService,
            IUriService uriService)
        {
            _context = context;
            _notificationDetailsRepository = notificationDetailsRepository;
            _logger = logger;
            _cRRequestResponseService = cRRequestResponseService;
            _uriService = uriService;
        }

        public async Task<ApiResponse> NotificationDetailsList(NotificationListFilterReqModel notificationListFilterReqModel, string getUserid, string route, string ip)
        {
             
            var methodName = "NotificationDetailsService/NotificationDetailsList";
            try
            {
                var pagFilterModel = new PaginationFilter()
                {
                    PageNumber = notificationListFilterReqModel.PageNumber,
                    PageSize = notificationListFilterReqModel.PageSize,
                };

                var updateNotification_sql = $@"EXEC AllApprovedCRDoesNotShowInTheNotification '{getUserid}'";
                await _context.Database.ExecuteSqlRawAsync(updateNotification_sql);

                var result = await _notificationDetailsRepository.QueryAll(x => x.ApproverEmpId == notificationListFilterReqModel.ApproverEmpId && x.IsActive == true).OrderByDescending(x => x.Id).OrderByDescending(x=> x.IsRead)
                    .Skip(pagFilterModel.PageNumber).Take(pagFilterModel.PageSize).AsNoTracking().ToListAsync();
                
                var totalRecords = await _notificationDetailsRepository.QueryAll(x => x.ApproverEmpId == notificationListFilterReqModel.ApproverEmpId).CountAsync();
                var totalUnReadMessage = await _notificationDetailsRepository.QueryAll(x => x.ApproverEmpId == notificationListFilterReqModel.ApproverEmpId && x.IsRead == false && x.IsActive == true).CountAsync();
                var pagedReponse = PaginationHelper.CreatePagedReponse(result, pagFilterModel, totalRecords, _uriService, route);
                pagedReponse.PageSize = totalUnReadMessage;
                
                return await _cRRequestResponseService.CreateResponseRequest(notificationListFilterReqModel, pagedReponse, ip, methodName, getUserid, "Ok");
                 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(notificationListFilterReqModel, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> AddNotificationDetailsRange(List<NotificationDetails> notificationDetails, string getUserid, string ip)
        {
             
            var methodName = "NotificationDetailsService/AddNotificationDetailsRange";
            try
            {
                
                var result = _notificationDetailsRepository.CreateListAsync(notificationDetails);
               

                if (await _context.SaveChangesAsync() > 0)
                {
                    var Message = "Notifications saved successfully!";
					return await _cRRequestResponseService.CreateResponseRequest(notificationDetails, null, ip, methodName, getUserid, Message);
				}
                throw new Exception("Notification is not create");
                 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(notificationDetails, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());
                 
            }

        }

        public async Task<ApiResponse> AddNotificationDetails(NotificationDetails notificationDetails, string getUserid, string ip)
        {
             
            var methodName = "NotificationDetailsService/AddNotificationDetails";
            try
            {
                var Model = new NotificationDetails()
                {
                    CrId = notificationDetails.CrId,
                    ApproverEmpId = notificationDetails.ApproverEmpId,
                    Title = notificationDetails.Title,
                    Message = notificationDetails.Message,
                    Image = notificationDetails.Image,
                    Type = notificationDetails.Type,
                    IsRead = notificationDetails.IsRead,
                    IsActive = notificationDetails.IsActive,
                    CreatedBy = getUserid
                };
                var result = _notificationDetailsRepository.CreateAsync(Model);

                if (await _context.SaveChangesAsync() > 0)
                {
                    var Message = "Notifications saved successfully!";
                    return await _cRRequestResponseService.CreateResponseRequest(notificationDetails, null, ip, methodName, getUserid, Message);
                     
                }
                throw new Exception("Something is worng.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(notificationDetails, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());
                 
            }

        }
        
        public async Task<ApiResponse> UpdateNotificationDetails(NotificationDetails notificationDetails, string getUserid, string ip)
        {
             
            var methodName = "NotificationDetailsService/UpdateNotificationDetails";
            try
            {
                if (notificationDetails.CrId <= 0 && string.IsNullOrWhiteSpace(notificationDetails.ApproverEmpId))
                {
                    throw new Exception("Please enter details properly");
                }

                var getData = await _notificationDetailsRepository.FirstOrDefaultAsync(x => x.Id == notificationDetails.Id);

                if (getData != null)
                {
                    getData.Title = notificationDetails.Title;
                    getData.Image = notificationDetails.Image;
                    getData.Type = notificationDetails.Type;
                    getData.Message = notificationDetails.Message;
                    getData.IsRead = notificationDetails.IsRead;

                    getData.LastModifiedBy = getUserid;

                    _notificationDetailsRepository.UpdateLatest(getData);
                   
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var Message = "Notifications update successfully!";
                        return await _cRRequestResponseService.CreateResponseRequest(notificationDetails, null, ip, methodName, getUserid, Message);
                         
                    }
                    throw new Exception("Something is wrong.");
                }
                else
                {
                    throw new Exception("Your requested data not updated");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(notificationDetails, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());
                 
            }
        }
        
        public async Task<ApiResponse> DeleteNotificationDetails(long id,string getUserid, string ip)
        {
             
            var methodName = "NotificationDetailsService/DeleteNotificationDetails";
            try
            {
                var res = await _notificationDetailsRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (res != null)
                {

                    await _notificationDetailsRepository.Delete(res);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var Message = "Data Deleted Successfully.";
                        return await _cRRequestResponseService.CreateResponseRequest(id, null, ip, methodName, getUserid, Message);
                                               
                    }
                    throw new Exception("Something is worng.");
                }
                else
                {
                    throw new Exception("Data not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(id, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());
                 
              
            }
        }

        public async Task<ApiResponse> UpdateNotificationUnreadToRead(string getUserid, string ip)
        {
            string message = string.Empty;
            var methodName = "NotificationDetailsService/UpdateNotificationUnreadToRead";
            try
            {
                var getData = await _notificationDetailsRepository.FindAsync(x => x.ApproverEmpId == getUserid && x.IsRead == false);

                if (getData.Count() > 0)
                {
                    getData.ForEach(x => x.IsRead = true);

                    _notificationDetailsRepository.UpdateRange(getData);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        message = "Notifications update successfully!";
                    }
                }
                return await _cRRequestResponseService.CreateResponseRequest(null, null, ip, methodName, getUserid, message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> WhenApprovedCRUpdateNotificationUnreadToRead(string getUserid, long crId, string ip)
        {

            var methodName = "NotificationDetailsService/WhenApprovedCRUpdateNotificationUnreadToRead";
            try
            {
                var getData = await _notificationDetailsRepository.FindAsync(x => x.ApproverEmpId == getUserid && x.CrId == crId);

                if (getData.Count() > 0)
                {
                    getData.ForEach(x => x.IsRead = true);

                    _notificationDetailsRepository.UpdateRange(getData);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var Message = "Notifications update successfully!";
                        return await _cRRequestResponseService.CreateResponseRequest(null, null, ip, methodName, getUserid, Message);

                    }
                    throw new Exception("Something is worng.");
                }
                else
                {

                    throw new Exception("Notification is empty");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, getUserid, "Error", ex.Message.ToString());

            }
        }




    }
}

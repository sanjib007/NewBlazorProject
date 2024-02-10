using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1
{
    public interface INotificationDetailsService
    {
        Task<ApiResponse> NotificationDetailsList(NotificationListFilterReqModel notificationListFilterReqModel, string getUserid, string route, string ip);
        Task<ApiResponse> AddNotificationDetails(NotificationDetails notificationDetails, string getUserid, string ip);
        Task<ApiResponse> AddNotificationDetailsRange(List<NotificationDetails> notificationDetails, string getUserid, string ip);
        Task<ApiResponse> UpdateNotificationDetails(NotificationDetails notificationDetails, string getUserid, string ip);
        Task<ApiResponse> DeleteNotificationDetails(long id, string getUserid, string ip);
        Task<ApiResponse> UpdateNotificationUnreadToRead(string getUserid, string ip);
        Task<ApiResponse> WhenApprovedCRUpdateNotificationUnreadToRead(string getUserid, long crId, string ip);



    }
}

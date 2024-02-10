using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.RequestModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Services.ServiceInterface.Client
{
    public interface ITrackingIssueService
    {
        Task<ApiResponse> AllTicketInfo(int day, string getUserid, string ip);
        Task<ApiResponse> AllTicketInfoByCustomer(string customerId, string getUserid, string ip);

        Task<ApiResponse> AddTicketRsmOrMis(TicketCreateReqModel ReqModel, string getUserid, string ip);
        Task<ApiResponse> RSMComplainTicketLogs(string ticketNo, string getUserid, string ip);
        Task<ApiResponse> GetAllTicketsNature(string systemType, string getUserid, string ip);

        Task<ApiResponse> GetAssignedPackageByCustomer(string customerId, string getUserid, string ip);
        
    }
}

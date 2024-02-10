using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Interface.FieldForce
{
    public interface IRSMComplainTicketService
    {
        Task<ApiResponse> GetRSMComplainTicket(string userId, string ip);
        Task<ApiResponse> GetRSMSubcriberInfo(string customerId, string userId, string ip);
        Task<ApiResponse> GetRSMComplainTicketLogs(string ticketNo, string userId, string ip);
        Task<ApiResponse> GetBillInformationFromHydra(string customerId, string userId, string ip);
        Task<ApiResponse> GetTechnicalInfoFromHydra(string customerId, string userId, string ip);
        Task<ApiResponse> GetComplainTicektImportentData(string ticketId, string userId, string ip);
        Task<ApiResponse> CloseNatureList(int categoryId, string userId, string ip);
        Task<ApiResponse> CloseTicket(RSMCloseTicketRequestModel model, string userId, string ip);
        Task<ApiResponse> UpdateRSMComplainPostReplay(PostReplayRequestModel model, string userId, string ip);
    }
}

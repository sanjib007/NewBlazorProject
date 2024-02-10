using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Models.RequestModel.Client;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client.Hydra;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client.RSM;
using L3T.Infrastructure.Helpers.Repositories.Implementation.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Repositories.Interface.Client
{
    public interface ITrackingIssueRepository
    {
        Task<List<TicketListResponseModel>> GetAllTicketWithFilter(TicketListRequestModel model);
        Task<RsmProfileResModel> RsmProfileView(string subscriber_codeUpper);
        Task<ApiResponse> AddRepositoryTicketRsm(TicketCreateReqModel ReqModel);
        Task<ApiResponse> AddRepositoryTicketMis(TicketCreateReqModel ReqModel);
        Task<List<RSM_ComplainLogDetailsModel>> GetRSMComplainTicketLogs(string ticketNo);

        Task<List<RSM_NatureSetup>> GetRSM_NatureSetup();
        Task<List<Tbl_Com_Category>> GetMis_NatureSetup();
        Task<List<NetworkInformationResponseModel>> GetHydraNetworkInformation(string customerId);
        Task<List<MisNetworkInformationResponseModel>> GetMisNetworkInformationResponseModel(string customerId);

    }
}

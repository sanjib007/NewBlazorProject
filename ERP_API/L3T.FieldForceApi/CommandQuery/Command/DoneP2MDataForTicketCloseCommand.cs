using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class DoneP2MDataForTicketCloseCommand : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public MisInstP2MTicketCloseRequestModel requestModel { get; set; }


        public class DoneP2MDataForTicketCloseHandler : IRequestHandler<DoneP2MDataForTicketCloseCommand, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public DoneP2MDataForTicketCloseHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(DoneP2MDataForTicketCloseCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.DoneP2MDataForTicketCloseForNewGo(request.ip, request.user, request.requestModel.branchidOrCliCode,
                request.requestModel.slnoOrCustomerCodeSlNo, request.requestModel.customerName, request.requestModel.customerBranchName,
                request.requestModel.customerAddressline1, request.requestModel.cablnetwork_CablePathID, request.requestModel.typeofp2mlink_Typeofp2mlinkID,
                request.requestModel.splitterName, request.requestModel.fiberLaser, request.requestModel.cableNumberFiber, request.requestModel.nTTNID,
                request.requestModel.teamName, request.requestModel.comments, request.requestModel.ticketId, request.requestModel.teamId_CategorySetupId,
                request.requestModel.distributorId_From_ClientDataBasemain, request.requestModel.otcAmount_ClientDbMain,
                request.requestModel.serviceNarration_ClientDbMain, request.requestModel.monthlyAmount_Amount_ClientDbMain,
                request.requestModel.entityName_Hostname, request.requestModel.realIp_ClientTechnicalInfo, request.requestModel.nTTN_Name,
                request.requestModel.installationDate_ClientDbMain, request.requestModel.designationName, request.requestModel.departmentName,
                request.requestModel.billingDate, request.requestModel.installation_MktBilling_comment, request.requestModel.linkidp2m_SummitLinkId,
                request.requestModel.scridp2m_FiberAtHomeSCRID, request.requestModel.bahoncoreid, request.requestModel.bahonlinkid,
                request.requestModel.btsId_FIBERMEDIADETAILSP2M, request.requestModel.btsName_FIBERMEDIADETAILSP2M,
                request.requestModel.fiberPon_FiberMediaDetailsP2M, request.requestModel.fiberPort_FiberMediaDetailsP2M,
                request.requestModel.fiberoltbrand_FiberMediaDetailsP2M, request.requestModel.fiberoltname_FiberMediaDetailsP2M,
                request.requestModel.fiberlaser_FiberMediaDetailsP2M, request.requestModel.fiberPortCapacity_FiberMediaDetailsP2M,
                request.requestModel.linkpathFb_ConnectivityDetailsP2M, request.requestModel.remarksFb_ConnectivityDetailsP2M,
                request.requestModel.latitude, request.requestModel.longiTude);
                return response;
            }
        }
    }
}

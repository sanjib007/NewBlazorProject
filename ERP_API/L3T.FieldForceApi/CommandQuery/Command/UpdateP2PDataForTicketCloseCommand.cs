using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class UpdateP2PDataForTicketCloseCommand : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string refnoOrTicketId { get; set; }
        public string branchidOrCliCode { get; set; }
        public int slnoOrCustomerCodeSlNo { get; set; }
        public string emailBody { get; set; }
        public int cablePathID_DDLcablnetwork { get; set; }
        public string Typeofp2mlink_DDLtypeofp2mlinkText { get; set; }
        public string p2pSwitchRouIP { get; set; }
        public string p2pSwRouPortNew { get; set; }
        public string p2pLaserNew { get; set; }
        public string p2PMCTypeInfo { get; set; }
        public string btsSetupName { get; set; }
        public int btsSetupId { get; set; }
        public string customerName { get; set; }
        public string customerBranchName { get; set; }
        public string customerAddressline1 { get; set; }
        public string linkpathp2p_GooglePath { get; set; }
        public string remarksp2pText { get; set; }
        public int autoOFIID_IncrementID { get; set; }


        public class UpdateP2PDataForTicketCloseHandler : IRequestHandler<UpdateP2PDataForTicketCloseCommand, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public UpdateP2PDataForTicketCloseHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(UpdateP2PDataForTicketCloseCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.UpdateP2PDataForTicketCloseForNewGo(request.ip, request.user, request.refnoOrTicketId,
                    request.branchidOrCliCode, request.slnoOrCustomerCodeSlNo, request.emailBody, request.cablePathID_DDLcablnetwork,
                    request.Typeofp2mlink_DDLtypeofp2mlinkText, request.p2pSwitchRouIP, request.p2pSwRouPortNew, request.p2pLaserNew,
                    request.p2PMCTypeInfo, request.btsSetupName, request.btsSetupId, request.customerName, request.customerBranchName,
                    request.customerAddressline1, request.linkpathp2p_GooglePath, request.remarksp2pText, request.autoOFIID_IncrementID);
                return response;
            }
        }
    }
}

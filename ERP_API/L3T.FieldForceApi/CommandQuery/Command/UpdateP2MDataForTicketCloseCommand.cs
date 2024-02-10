using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class UpdateP2MDataForTicketCloseCommand : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public string splitterNameFiber { get; set; }
        public string fiberoltbrand { get; set; }
        public string fiberoltname { get; set; }
        public int fiberpon { get; set; }
        public int fiberport { get; set; }
        public int portcapfiber { get; set; }
        public string encloserno { get; set; }
        public string refnoOrTicketId { get; set; }
        public string branchidOrCliCode { get; set; }
        public int slnoOrCustomerCodeSlNo { get; set; }
        public string customerName { get; set; }
        public string customerBranchName { get; set; }
        public string customerAddressline1 { get; set; }
        public int btsSetupId { get; set; }
        public string fiberLaser { get; set; }
        public string btsName { get; set; }
        public int cableNumber { get; set; }
        public string linkPathFiber { get; set; }
        public string remarksFiber { get; set; }
        public string emailBody { get; set; }


        public class UpdateP2MDataForTicketCloseHandler : IRequestHandler<UpdateP2MDataForTicketCloseCommand, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public UpdateP2MDataForTicketCloseHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(UpdateP2MDataForTicketCloseCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.UpdateP2MDataForTicketCloseForNewGo(request.ip, request.user, request.splitterNameFiber,
                    request.fiberoltbrand, request.fiberoltname, request.fiberpon, request.fiberport, request.portcapfiber, request.encloserno,
                    request.refnoOrTicketId, request.branchidOrCliCode, request.slnoOrCustomerCodeSlNo, request.customerName,
                    request.customerBranchName, request.customerAddressline1, request.btsSetupId, request.fiberLaser, request.btsName,
                    request.cableNumber, request.linkPathFiber, request.remarksFiber, request.emailBody);
                return response;
            }
        }

    }
}

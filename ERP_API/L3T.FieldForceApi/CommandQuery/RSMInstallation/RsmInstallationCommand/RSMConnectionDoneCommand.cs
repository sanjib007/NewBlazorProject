using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels.RSM;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationCommand
{
    public class RSMNetworkConnectionDoneCommand : IRequest<ApiResponse>
    {
        public NetworkConnectionRequestModel model { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }
        public class RSMConnectionDoneCommandHandler : IRequestHandler<RSMNetworkConnectionDoneCommand, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public RSMConnectionDoneCommandHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(RSMNetworkConnectionDoneCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.NetworkConnectionDoneP2M(request.model, request.userId, request.ip);
                return response;
            }
        }
    }
}

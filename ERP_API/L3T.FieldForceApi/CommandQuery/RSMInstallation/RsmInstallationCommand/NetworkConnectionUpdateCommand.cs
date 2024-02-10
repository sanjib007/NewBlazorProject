using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels.RSM;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationCommand
{
    public class RSMNetworkConnectionUpdateCommand : IRequest<ApiResponse>
    {
        public NetworkConnectionRequestModel model { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }
        public class RSMNetworkConnectionUpdateCommandHandler : IRequestHandler<RSMNetworkConnectionUpdateCommand, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public RSMNetworkConnectionUpdateCommandHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(RSMNetworkConnectionUpdateCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.NetworkConnectionUpdateP2M(request.model, request.userId, request.ip);
                return response;
            }
        }
    }
}

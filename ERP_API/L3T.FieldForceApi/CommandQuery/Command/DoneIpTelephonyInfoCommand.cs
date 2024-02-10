using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{

    public class DoneIpTelephonyInfoCommand : IRequest<ApiResponse>
    {
        public IpTelephonyDoneRequestModel model { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class DoneIpTelephonyInfoCommandHandler : IRequestHandler<DoneIpTelephonyInfoCommand, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public DoneIpTelephonyInfoCommandHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(DoneIpTelephonyInfoCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.DoneIpTelephonyInfoData(request.model, request.user, request.ip);
                return reaponse;
            }
        }
    }
}

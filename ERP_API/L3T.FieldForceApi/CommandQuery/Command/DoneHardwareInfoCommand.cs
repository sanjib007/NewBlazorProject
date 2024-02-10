using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{

    public class DoneHardwareInfoCommand : IRequest<ApiResponse>
    {
        public HardwareInfoDoneRequestModel model { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class DoneIpTelephonyInfoCommandHandler : IRequestHandler<DoneHardwareInfoCommand, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public DoneIpTelephonyInfoCommandHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(DoneHardwareInfoCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.DoneHardwareInfoData(request.model, request.user, request.ip);
                return reaponse;
            }
        }
    }
}

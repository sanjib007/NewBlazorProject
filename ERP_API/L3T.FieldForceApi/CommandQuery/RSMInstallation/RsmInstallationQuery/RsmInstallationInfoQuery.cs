using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationQuery
{
    public class RsmInstallationInfoQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public string ticketId { get; set; }

        public class RsmInstallationInfoQueryHandler : IRequestHandler<RsmInstallationInfoQuery, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public RsmInstallationInfoQueryHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(RsmInstallationInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetInstallationInfoData(request.userId, request.ip, request.ticketId);
                return response;
            }
        }
    }
}

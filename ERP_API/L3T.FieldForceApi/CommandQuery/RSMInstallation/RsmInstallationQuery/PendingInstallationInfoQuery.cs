using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationQuery
{
    public class PendingInstallationInfoQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
      

        public class PendingInstallationInfoQueryHandler : IRequestHandler<PendingInstallationInfoQuery, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public PendingInstallationInfoQueryHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(PendingInstallationInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetPendingInstallation(request.userId, request.ip);
                return response;
            }
        }
    }
}

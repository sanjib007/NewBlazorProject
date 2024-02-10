using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationQuery
{
    public class P2mHomeScridInfoQuery : IRequest<ApiResponse>
    {
        public string prefixText { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }
      

        public class P2mHomeScridInfoQueryHandler : IRequestHandler<P2mHomeScridInfoQuery, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public P2mHomeScridInfoQueryHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(P2mHomeScridInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetP2mHomeSCRIDInfo(request.userId, request.ip, request.prefixText);
                return response;
            }
        }
    }
}

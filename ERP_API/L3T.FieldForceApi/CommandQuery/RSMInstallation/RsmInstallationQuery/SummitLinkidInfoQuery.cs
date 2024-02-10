using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationQuery
{
    public class SummitLinkidInfoQuery : IRequest<ApiResponse>
    {
        public string prefixText { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }
      

        public class SummitLinkidInfoQueryHandler : IRequestHandler<SummitLinkidInfoQuery, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public SummitLinkidInfoQueryHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(SummitLinkidInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetSummitLinkIDInfo(request.userId, request.ip, request.prefixText);
                return response;
            }
        }
    }
}

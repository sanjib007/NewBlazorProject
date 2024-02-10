using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationQuery
{
    public class SalesPersonsInfoQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string employeeId { get; set; }

        public class SalesPersonsInfoHandler : IRequestHandler<SalesPersonsInfoQuery, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public SalesPersonsInfoHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(SalesPersonsInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetSalesPersonsInfo(request.user, request.ip, request.employeeId);
                return response;
            }
        }
    }
}

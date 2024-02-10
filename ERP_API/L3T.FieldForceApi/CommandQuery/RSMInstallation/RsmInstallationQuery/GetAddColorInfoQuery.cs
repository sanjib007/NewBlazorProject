using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationQuery
{

    public class GetAddColorInfoQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public string ticketId { get; set; }

        public class GetAddColorInfoQueryHandler : IRequestHandler<GetAddColorInfoQuery, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public GetAddColorInfoQueryHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetAddColorInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetAddColorInfoData(request.ticketId, request.userId, request.ip);
                return response;
            }
        }
    }
}

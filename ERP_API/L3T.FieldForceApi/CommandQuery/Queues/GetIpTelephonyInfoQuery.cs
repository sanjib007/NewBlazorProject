using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetIpTelephonyInfoQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GetIpTelephonyInfoQueryHandler : IRequestHandler<GetIpTelephonyInfoQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GetIpTelephonyInfoQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetIpTelephonyInfoQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetIpTelephonyData(request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

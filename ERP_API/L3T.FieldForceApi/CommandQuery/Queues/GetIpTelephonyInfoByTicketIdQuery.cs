using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
  
    public class GetIpTelephonyInfoByTicketIdQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GetIpTelephonyInfoByTicketIdQueryHandler : IRequestHandler<GetIpTelephonyInfoByTicketIdQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GetIpTelephonyInfoByTicketIdQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetIpTelephonyInfoByTicketIdQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetIpTelephonyInfoByTicketId(request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

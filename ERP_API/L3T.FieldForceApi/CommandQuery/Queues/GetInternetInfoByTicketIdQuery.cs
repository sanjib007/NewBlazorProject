using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{

    public class GetInternetInfoByTicketIdQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GetInternetInfoByTicketIdQueryHandler : IRequestHandler<GetInternetInfoByTicketIdQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GetInternetInfoByTicketIdQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetInternetInfoByTicketIdQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetInternetInfoByTicketId(request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

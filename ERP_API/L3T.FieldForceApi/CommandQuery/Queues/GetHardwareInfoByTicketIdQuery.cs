using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
   

    public class GetHardwareInfoByTicketIdQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GetHardwareInfoByTicketIdQueryHandler : IRequestHandler<GetHardwareInfoByTicketIdQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GetHardwareInfoByTicketIdQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetHardwareInfoByTicketIdQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetHardwareInfoByTicketId(request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

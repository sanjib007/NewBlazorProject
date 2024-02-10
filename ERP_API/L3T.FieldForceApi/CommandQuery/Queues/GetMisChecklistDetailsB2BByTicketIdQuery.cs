using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetMisChecklistDetailsB2BByTicketIdQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GetMisChecklistDetailsB2BByTicketIdQueryHandler : IRequestHandler<GetMisChecklistDetailsB2BByTicketIdQuery, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetMisChecklistDetailsB2BByTicketIdQueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetMisChecklistDetailsB2BByTicketIdQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetMisChecklistDetailsB2BByTicketId(request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

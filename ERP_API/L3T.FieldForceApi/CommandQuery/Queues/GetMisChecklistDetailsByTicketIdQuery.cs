using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{

    public class GetMisChecklistDetailsByTicketIdQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GetMisChecklistDetailsByTicketIdQueryHandler : IRequestHandler<GetMisChecklistDetailsByTicketIdQuery, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetMisChecklistDetailsByTicketIdQueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetMisChecklistDetailsByTicketIdQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetMisChecklistDetailsByTicketId(request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

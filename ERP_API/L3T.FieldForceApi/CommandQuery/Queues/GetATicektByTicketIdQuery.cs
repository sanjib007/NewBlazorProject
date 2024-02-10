using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetATicektByTicketIdQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string userid { get; set; }
        public string ip { get; set; }
        public class GetATicektByTicketIdQueryHandler : IRequestHandler<GetATicektByTicketIdQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public GetATicektByTicketIdQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetATicektByTicketIdQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetATicketByTicketId(request.ticketId, request.userid, request.ip);
                return reaponse;
            }
        }
    }
}

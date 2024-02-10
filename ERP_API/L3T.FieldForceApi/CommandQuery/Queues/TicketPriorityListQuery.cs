using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class TicketPriorityListQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ticketId { get; set; }

        public class TicketPriorityListHandler : IRequestHandler<TicketPriorityListQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public TicketPriorityListHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(TicketPriorityListQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetTicketPriorityListByTicketId(request.ip, request.user, request.ticketId);
                return response;
            }
        }
    }
}

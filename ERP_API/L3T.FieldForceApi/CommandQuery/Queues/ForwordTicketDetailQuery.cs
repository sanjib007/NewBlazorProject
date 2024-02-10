using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ForwardTicketDetailQuery : IRequest<ApiResponse>
    {
        public string TicketId { get; set; }
        public string ip { get; set; }
        public string UserId { get; set; }
        public class ForwardTicketDetailQueryHandler : IRequestHandler<ForwardTicketDetailQuery, ApiResponse>
        {
            private readonly IForwardTicketService _context;

            public ForwardTicketDetailQueryHandler(IForwardTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(ForwardTicketDetailQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.ForwardTicketDetails(request.TicketId, request.ip, request.UserId);
                return reaponse;
            }
        }
    }
}

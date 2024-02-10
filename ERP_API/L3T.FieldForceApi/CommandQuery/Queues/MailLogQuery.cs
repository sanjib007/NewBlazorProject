using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class MailLogQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string ticketId { get; set; }

        public class MailLogHandler : IRequestHandler<MailLogQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public MailLogHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(MailLogQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetMailLogByTicketId(request.user, request.ip, request.ticketId);
                return response;
            }
        }
    }
}

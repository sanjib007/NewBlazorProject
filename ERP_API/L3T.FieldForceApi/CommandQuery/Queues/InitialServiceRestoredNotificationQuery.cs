using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class InitialServiceRestoredNotificationQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public class InitialServiceRestoredNotificationQueryHandler : IRequestHandler<InitialServiceRestoredNotificationQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public InitialServiceRestoredNotificationQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(InitialServiceRestoredNotificationQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.InitialServiceRestoredNotification(request.ticketId, request.user, request.ip);
                return reaponse;
            }
        }
    }
}

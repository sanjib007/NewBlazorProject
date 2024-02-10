using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{

    public class GetSubscriptionInfoByTicketIdQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }
        public class GetSubscriptionInfoByTicketIdQueryHandler : IRequestHandler<GetSubscriptionInfoByTicketIdQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GetSubscriptionInfoByTicketIdQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetSubscriptionInfoByTicketIdQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetSubscriptionInfoByTicketId(request.ticketId, request.userId, request.ip);
                return reaponse;
            }
        }
    }
}

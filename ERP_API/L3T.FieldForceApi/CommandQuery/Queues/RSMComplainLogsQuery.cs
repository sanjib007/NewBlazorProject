using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Implementation.FieldForce;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class RSMComplainLogsQuery : IRequest<ApiResponse>
    {
        public string TicketId { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }
        public class RSMComplainLogsQueryHandler : IRequestHandler<RSMComplainLogsQuery, ApiResponse>
        {
            private readonly IRSMComplainTicketService _rSMComplainTicketService;

            public RSMComplainLogsQueryHandler(IRSMComplainTicketService rSMComplainTicketService)
            {
                _rSMComplainTicketService = rSMComplainTicketService;
            }

            public async Task<ApiResponse> Handle(RSMComplainLogsQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = await _rSMComplainTicketService.GetRSMComplainTicketLogs(request.TicketId, request.userId, request.ip);
                return response;
            }
        }
    }
}

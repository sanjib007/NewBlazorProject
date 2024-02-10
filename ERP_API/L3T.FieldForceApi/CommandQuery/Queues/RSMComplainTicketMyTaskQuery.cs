using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class RSMComplainTicketMyTaskQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public class RSMComplainTicketMyTaskQueryHandler : IRequestHandler<RSMComplainTicketMyTaskQuery, ApiResponse>
        {
            private readonly IRSMComplainTicketService _rSMComplainTicketService;

            public RSMComplainTicketMyTaskQueryHandler(IRSMComplainTicketService rSMComplainTicketService)
            {
                _rSMComplainTicketService = rSMComplainTicketService;
            }

            public async Task<ApiResponse> Handle(RSMComplainTicketMyTaskQuery request, CancellationToken cancellationToken)
            {
                var response = await _rSMComplainTicketService.GetRSMComplainTicket(request.userId, request.ip);
                return response;
            }
        }
    }
}

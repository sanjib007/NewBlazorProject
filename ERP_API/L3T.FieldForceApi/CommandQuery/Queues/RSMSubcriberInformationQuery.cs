using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class RSMSubcriberInformationQuery :IRequest<ApiResponse>
    {
        public string customerId { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }

        public class RSMSubcriberInformationQueryHandler : IRequestHandler<RSMSubcriberInformationQuery, ApiResponse>
        {
            private readonly IRSMComplainTicketService _rSMComplainTicketService;

            public RSMSubcriberInformationQueryHandler(IRSMComplainTicketService rSMComplainTicketService)
            {
                _rSMComplainTicketService = rSMComplainTicketService;
            }
            public async Task<ApiResponse> Handle(RSMSubcriberInformationQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = await _rSMComplainTicketService.GetRSMSubcriberInfo(request.customerId, request.userId, request.ip);
                return response;
            }
        }
    }
}

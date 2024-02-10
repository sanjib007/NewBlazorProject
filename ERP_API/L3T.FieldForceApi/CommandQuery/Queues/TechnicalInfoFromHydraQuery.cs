using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class TechnicalInfoFromHydraQuery : IRequest<ApiResponse>
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string Ip { get; set; }
        public class TechnicalInfoFromHydraQueryHandler : IRequestHandler<TechnicalInfoFromHydraQuery, ApiResponse>
        {
            private readonly IRSMComplainTicketService _rSMComplainTicketService;

            public TechnicalInfoFromHydraQueryHandler(IRSMComplainTicketService rSMComplainTicketService)
            {
                _rSMComplainTicketService = rSMComplainTicketService;
            }

            public async Task<ApiResponse> Handle(TechnicalInfoFromHydraQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = await _rSMComplainTicketService.GetTechnicalInfoFromHydra(request.CustomerId, request.UserId, request.Ip);
                return response;
            }
        }
    }
}

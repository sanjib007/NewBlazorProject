using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class CloseNatureListQuery : IRequest<ApiResponse>
    {
        public int categoryId { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }
        public class CloseNatureListQueryHandler : IRequestHandler<CloseNatureListQuery, ApiResponse>
        {
            private readonly IRSMComplainTicketService _rSMComplainTicketService;

            public CloseNatureListQueryHandler(IRSMComplainTicketService rSMComplainTicketService)
            {
                _rSMComplainTicketService = rSMComplainTicketService;
            }

            public async Task<ApiResponse> Handle(CloseNatureListQuery request, CancellationToken cancellationToken)
            {
                var response = await _rSMComplainTicketService.CloseNatureList(request.categoryId, request.userId, request.ip);
                return response;
            }
        }
    }
}

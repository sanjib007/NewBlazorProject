using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetAllTicketListAndCountQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public int lastDays { get; set; }
        public string ip { get; set; }
        public class GetAllTicketListAndCountQueryHandler : IRequestHandler<GetAllTicketListAndCountQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public GetAllTicketListAndCountQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetAllTicketListAndCountQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetAllAssignTicketForUserWithCount(request.userId, request.ip, request.lastDays);
                return reaponse;
            }
        }
    }
}

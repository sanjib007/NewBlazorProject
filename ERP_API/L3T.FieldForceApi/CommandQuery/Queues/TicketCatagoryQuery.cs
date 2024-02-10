using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class TicketCategoryQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string UserId { get; set; }
        public class TicketCategoryQueryHandler : IRequestHandler<TicketCategoryQuery, ApiResponse>
        {
            private readonly IForwardTicketService _context;

            public TicketCategoryQueryHandler(IForwardTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(TicketCategoryQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.Category(request.ip, request.UserId);
                return reaponse;
            }
        }
    }
}

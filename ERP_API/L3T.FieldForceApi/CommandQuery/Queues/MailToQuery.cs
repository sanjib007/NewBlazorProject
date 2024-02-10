using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ResolvedDetailsMailQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public class ResolvedDetailsMailQueryHandler : IRequestHandler<ResolvedDetailsMailQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public ResolvedDetailsMailQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(ResolvedDetailsMailQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.ResolvedDetailsMail(request.ticketId, request.user, request.ip);
                return reaponse;
            }
        }
    }
}

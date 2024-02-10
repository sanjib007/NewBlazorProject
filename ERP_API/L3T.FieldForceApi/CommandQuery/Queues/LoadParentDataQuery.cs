using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class LoadParentDataQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string userId { get; set; }
        public string ticketId { get; set; }

        public class LoadParentDataHandler : IRequestHandler<LoadParentDataQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public LoadParentDataHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(LoadParentDataQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetParentData(request.ip, request.user, request.userId, request.ticketId);
                return response;
            }
        }
    }
}

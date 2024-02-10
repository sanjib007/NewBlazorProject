using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class NetworkTypeQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public class NetworkTypeQueryHandler : IRequestHandler<NetworkTypeQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public NetworkTypeQueryHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(NetworkTypeQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetNetworkTypeList(request.user, request.ip);
                return response;
            }
        }

    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class P2PAddressForNewGoQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string subscriberId { get; set; }

        public class P2PAddressForNewGoHandler : IRequestHandler<P2PAddressForNewGoQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public P2PAddressForNewGoHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(P2PAddressForNewGoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetP2PAddressForNewGo(request.ip, request.user, request.subscriberId);
                return response;
            }
        }

    }
}

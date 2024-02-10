using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ConnectivityPortListQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public class ConnectivityPortListHandler : IRequestHandler<ConnectivityPortListQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public ConnectivityPortListHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(ConnectivityPortListQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetConnectivityPortList(request.ip, request.user);
                return response;
            }
        }

    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ConnectivityTrayListQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public class ConnectivityTrayListHandler : IRequestHandler<ConnectivityTrayListQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public ConnectivityTrayListHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(ConnectivityTrayListQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetConnectivityTrayList(request.ip, request.user);
                return response;
            }
        }

    }
}

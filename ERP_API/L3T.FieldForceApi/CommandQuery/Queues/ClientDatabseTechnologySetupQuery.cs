using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ClientDatabseTechnologySetupQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }

        public class ClientDatabseTechnologySetupQueryHandler : IRequestHandler<ClientDatabseTechnologySetupQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public ClientDatabseTechnologySetupQueryHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(ClientDatabseTechnologySetupQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetClientDatabseTechnologySetupData(request.ip, request.user);
                return reaponse;
            }
        }
    }
}

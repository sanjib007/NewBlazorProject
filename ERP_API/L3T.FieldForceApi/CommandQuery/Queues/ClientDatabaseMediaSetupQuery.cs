using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ClientDatabaseMediaSetupQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }

        public class ClientDatabaseMediaSetupQueryHandler : IRequestHandler<ClientDatabaseMediaSetupQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public ClientDatabaseMediaSetupQueryHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(ClientDatabaseMediaSetupQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetClientDatabaseMediaSetupData(request.ip, request.user);
                return reaponse;
            }
        }
    }
}

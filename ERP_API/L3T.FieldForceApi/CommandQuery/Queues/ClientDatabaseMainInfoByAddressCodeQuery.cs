using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ClientDatabaseMainInfoByAddressCodeQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string clientAddressCode { get; set; }

        public class ClientDatabaseMainInfoByAddressCodeQueryHandler : IRequestHandler<ClientDatabaseMainInfoByAddressCodeQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public ClientDatabaseMainInfoByAddressCodeQueryHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(ClientDatabaseMainInfoByAddressCodeQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetClientDatabaseMainInfoByAddressCode(request.user, request.ip, request.clientAddressCode);
                return response;
            }
        }

    }
}

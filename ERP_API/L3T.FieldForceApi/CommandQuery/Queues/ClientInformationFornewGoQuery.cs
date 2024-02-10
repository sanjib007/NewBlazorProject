using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ClientInformationFornewGoQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string subscriberId { get; set; }
        public int serialNo { get; set; }

        public class ClientInformationFornewGoHandler : IRequestHandler<ClientInformationFornewGoQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public ClientInformationFornewGoHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(ClientInformationFornewGoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetClientInformationForNewGo(request.ip, request.user, request.subscriberId, request.serialNo);
                return response;
            }
        }
    }
}

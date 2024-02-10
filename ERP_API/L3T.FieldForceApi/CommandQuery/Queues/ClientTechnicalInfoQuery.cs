using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ClientTechnicalInfoQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string brClientCode { get; set; }
        public int brSerialNumber { get; set; }

        public class ClientTechnicalInfoQueryHandler : IRequestHandler<ClientTechnicalInfoQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public ClientTechnicalInfoQueryHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(ClientTechnicalInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetClientTechnicalInfo(request.brClientCode,
                    request.brSerialNumber, request.userId, request.ip);
                return response;
            }
        }
    }
}

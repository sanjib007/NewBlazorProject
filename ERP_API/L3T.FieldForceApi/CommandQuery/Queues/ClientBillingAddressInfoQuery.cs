using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ClientBillingAddressInfoQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string brClientCode { get; set; }
        public int brSerialNumber { get; set; }

        public class ClientBillingAddressInfoQueryHandler : IRequestHandler<ClientBillingAddressInfoQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public ClientBillingAddressInfoQueryHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(ClientBillingAddressInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetClientBillingAddressInfoByClientCodeAndSerialNo(request.userId, request.ip,
                    request.brClientCode, request.brSerialNumber);
                return response;
            }
        }
    }
}

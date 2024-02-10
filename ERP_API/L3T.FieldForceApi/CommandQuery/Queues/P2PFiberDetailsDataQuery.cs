using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class P2PFiberDetailsDataQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string brClientCode { get; set; }
        public int brSerialNumber { get; set; }

        public class P2PFiberDetailsDataHandler : IRequestHandler<P2PFiberDetailsDataQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public P2PFiberDetailsDataHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(P2PFiberDetailsDataQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetP2PFiberDetailsData(request.ip, request.user, request.brSerialNumber, request.brClientCode);
                return response;
            }
        }
    }
}

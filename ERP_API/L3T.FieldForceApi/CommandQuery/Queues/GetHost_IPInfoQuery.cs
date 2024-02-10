using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetHost_IPInfoQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string brClientCode { get; set; }
        public int brSerialNumber { get; set; }

        public class GetHost_IPInfoHandler : IRequestHandler<GetHost_IPInfoQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public GetHost_IPInfoHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetHost_IPInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetHost_IPInfoForNewGo(request.ip, request.user, request.brSerialNumber, request.brClientCode);
                return response;
            }
        }
    }
}

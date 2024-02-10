using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetIpTvInfoQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string brClientCode { get; set; }

        public class GetIpTvInfoHandler : IRequestHandler<GetIpTvInfoQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public GetIpTvInfoHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetIpTvInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetIpTvInfoForNewGo(request.ip, request.user, request.brClientCode);
                return response;
            }
        }
    }
}

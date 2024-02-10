using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetColorInfoQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class GetColorInfoHandler : IRequestHandler<GetColorInfoQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public GetColorInfoHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetColorInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetColorInfo(request.ip, request.user);
                return response;
            }
        }
    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetAllSpliterInfoQuery : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string prefixText { get; set; }
        public int count { get; set; }
        public string btsId { get; set; }
        public class GetAllSpliterInfoHandler : IRequestHandler<GetAllSpliterInfoQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public GetAllSpliterInfoHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(GetAllSpliterInfoQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetAllSpliterInfoForNewGo(request.ip, request.user, request.prefixText, request.count, request.btsId);
                return response;
            }
        }
    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetRsmRouterTypeQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string userId { get; set; }
        public class GetRsmRouterTypeQueryHandler : IRequestHandler<GetRsmRouterTypeQuery, ApiResponse>
        {
            private readonly IRsmChecklistService _context;

            public GetRsmRouterTypeQueryHandler(IRsmChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetRsmRouterTypeQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetRouterTypeData(request.ip, request.userId);
                return reaponse;
            }
        }
    }
}

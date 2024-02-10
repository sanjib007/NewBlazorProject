using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetRouterTypeQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public class GetRouterTypeQueryHandler : IRequestHandler<GetRouterTypeQuery, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetRouterTypeQueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetRouterTypeQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetRouterTypeData(request.ip);
                return reaponse;
            }
        }
    }
}

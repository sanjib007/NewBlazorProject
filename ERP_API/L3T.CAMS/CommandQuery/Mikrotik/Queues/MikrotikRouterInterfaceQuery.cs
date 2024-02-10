using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Queues
{
    public class MikrotikRouterInterfaceQuery : IRequest<ApiResponse>
    {
        public MikrotikRouterFilterParams model { get; set; }
        public class testQueryHandler : IRequestHandler<MikrotikRouterInterfaceQuery, ApiResponse>
        {
            private readonly ICamsService _context;

            public testQueryHandler(ICamsService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(MikrotikRouterInterfaceQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.MikrotikRouterInterfaceData(request.model);
                return reaponse;
            }
        }
    }
}

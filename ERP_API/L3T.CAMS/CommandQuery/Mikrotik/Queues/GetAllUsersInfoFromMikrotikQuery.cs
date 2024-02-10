using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Queues
{
    public class GetAllUsersInfoFromMikrotikQuery : IRequest<ApiResponse>
    {
        public MikrotikRouterFilterParams model { get; set; }

        public class GetAllUsersInfoFromMikrotikQueryHandler : IRequestHandler<GetAllUsersInfoFromMikrotikQuery, ApiResponse>
        {
            private readonly ICamsService _context;

            public GetAllUsersInfoFromMikrotikQueryHandler(ICamsService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetAllUsersInfoFromMikrotikQuery request, CancellationToken cancellationToken)
            {
                var reaponse = _context.GetAllUsersInfoFromMikrotikRouter(request.model);
                return reaponse;
            }
        }
    }
}

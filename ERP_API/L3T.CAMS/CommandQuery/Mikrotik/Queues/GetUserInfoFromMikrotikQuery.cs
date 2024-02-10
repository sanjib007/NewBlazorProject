using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Queues
{
    public class GetUserInfoFromMikrotikQuery : IRequest<ApiResponse>
    {
        public GetUserInfoFromMikrotikRequestModel model { get; set; }

        public class GetUserInfoFromMikrotikQueryHandler : IRequestHandler<GetUserInfoFromMikrotikQuery, ApiResponse>
        {
            private readonly ICamsService _context;

            public GetUserInfoFromMikrotikQueryHandler(ICamsService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetUserInfoFromMikrotikQuery request, CancellationToken cancellationToken)
            {
                 var reaponse = await _context.GetUserInfoFromMikrotikRouter(request.model);
                 return reaponse;
            }
        }
    }
}

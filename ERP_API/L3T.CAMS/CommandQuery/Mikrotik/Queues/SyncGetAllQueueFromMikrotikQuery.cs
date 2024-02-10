using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.Cams;
using L3T.Infrastructure.Helpers.Models.Mikrotik.RequestModel;
using MediatR;

namespace CAMS.CommandQuery.Mikrotik.Queues
{
    public class SyncGetAllQueueFromMikrotikQuery : IRequest<ApiResponse>
    {
        public GetAllQueueRequestModel model { get; set; }
        public class SyncGetAllQueueFromMikrotikQueryHandler : IRequestHandler<SyncGetAllQueueFromMikrotikQuery, ApiResponse>
        {
            private readonly ICamsService _context;

            public SyncGetAllQueueFromMikrotikQueryHandler(ICamsService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(SyncGetAllQueueFromMikrotikQuery request, CancellationToken cancellationToken)
            {
                ApiResponse reaponse = _context.GetAllQueueFromMikrotikRouter(request.model);
                return reaponse;
            }
        }
    }
}

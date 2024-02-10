using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetRsmChannelWidthAutoQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string userId { get; set; }
        public class GetRsmChannelWidthAutoQueryHandler : IRequestHandler<GetRsmChannelWidthAutoQuery, ApiResponse>
        {
            private readonly IRsmChecklistService _context;

            public GetRsmChannelWidthAutoQueryHandler(IRsmChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetRsmChannelWidthAutoQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetChannelWidthAutoData(request.ip, request.userId);
                return reaponse;
            }
        }
    }
}

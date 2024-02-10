using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetRsmChannelWidth20MHzQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string userId { get; set; }
        public class GetRsmChannelWidth20MHzQueryHandler : IRequestHandler<GetRsmChannelWidth20MHzQuery, ApiResponse>
        {
            private readonly IRsmChecklistService _context;

            public GetRsmChannelWidth20MHzQueryHandler(IRsmChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetRsmChannelWidth20MHzQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetChannelWidth20MHzData(request.ip, request.userId);
                return reaponse;
            }
        }
    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetChannelWidth20MHzQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public class GetChannelWidth20MHzQueryHandler : IRequestHandler<GetChannelWidth20MHzQuery, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetChannelWidth20MHzQueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetChannelWidth20MHzQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetChannelWidth20MHzData(request.ip);
                return reaponse;
            }
        }
    }
}

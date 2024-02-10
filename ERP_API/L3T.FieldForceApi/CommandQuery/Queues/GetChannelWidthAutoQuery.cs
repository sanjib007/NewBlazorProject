using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetChannelWidthAutoQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public class GetChannelWidthAutoQueryHandler : IRequestHandler<GetChannelWidthAutoQuery, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetChannelWidthAutoQueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetChannelWidthAutoQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetChannelWidthAutoData(request.ip);
                return reaponse;
            }
        }
    }
}

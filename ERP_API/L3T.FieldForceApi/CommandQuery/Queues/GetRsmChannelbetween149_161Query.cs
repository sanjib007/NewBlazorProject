using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{

    public class GetRsmChannelbetween149_161Query : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string userId { get; set; }
        public class GetRsmChannelbetween149_161QueryHandler : IRequestHandler<GetRsmChannelbetween149_161Query, ApiResponse>
        {
            private readonly IRsmChecklistService _context;

            public GetRsmChannelbetween149_161QueryHandler(IRsmChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetRsmChannelbetween149_161Query request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetChannelbetween149_161Data(request.ip, request.userId);
                return reaponse;
            }
        }
    }
}

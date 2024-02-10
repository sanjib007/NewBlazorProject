using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetRsmGhzEnabledQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string userId { get; set; }
        public class GetRsmGhzEnabledQueryHandler : IRequestHandler<GetRsmGhzEnabledQuery, ApiResponse>
        {
            private readonly IRsmChecklistService _context;

            public GetRsmGhzEnabledQueryHandler(IRsmChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetRsmGhzEnabledQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetGhzEnabledData(request.ip, request.userId);
                return reaponse;
            }
        }
    }
}

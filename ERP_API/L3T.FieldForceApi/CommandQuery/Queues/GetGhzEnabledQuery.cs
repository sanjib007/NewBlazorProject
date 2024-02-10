using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetGhzEnabledQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public class GetGhzEnabledQueryHandler : IRequestHandler<GetGhzEnabledQuery, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetGhzEnabledQueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetGhzEnabledQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetGhzEnabledData(request.ip);
                return reaponse;
            }
        }
    }
}

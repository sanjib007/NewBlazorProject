using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{

    public class GetRsmSingleApQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string userId { get; set; }
        public class GetRsmSingleApQueryHandler : IRequestHandler<GetRsmSingleApQuery, ApiResponse>
        {
            private readonly IRsmChecklistService _context;

            public GetRsmSingleApQueryHandler(IRsmChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetRsmSingleApQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetSingleApData(request.ip, request.userId);
                return reaponse;
            }
        }
    }
}

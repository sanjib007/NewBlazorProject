using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetRsmMultipleApQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string userId { get; set; }
        public class GetRsmMultipleApQueryHandler : IRequestHandler<GetRsmMultipleApQuery, ApiResponse>
        {
            private readonly IRsmChecklistService _context;

            public GetRsmMultipleApQueryHandler(IRsmChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetRsmMultipleApQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetMultipleApData(request.ip, request.userId);
                return reaponse;
            }
        }
    }
}

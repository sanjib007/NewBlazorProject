using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetMultipleApQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public class GetMultipleApQueryHandler : IRequestHandler<GetMultipleApQuery, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetMultipleApQueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetMultipleApQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetMultipleApData(request.ip);
                return reaponse;
            }
        }
    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetSingleApQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public class GetSingleApQueryHandler : IRequestHandler<GetSingleApQuery, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetSingleApQueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetSingleApQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetSingleApData(request.ip);
                return reaponse;
            }
        }
    }
}

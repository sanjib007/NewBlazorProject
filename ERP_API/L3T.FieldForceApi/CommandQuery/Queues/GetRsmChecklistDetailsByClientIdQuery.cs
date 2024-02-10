using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{

    public class GetRsmChecklistDetailsByClientIdQuery : IRequest<ApiResponse>
    {
        public string clientId { get; set; }
        public string ip { get; set; }
        public string userId { get; set; }
        public class GetRsmChecklistDetailsByClientIdQueryHandler : IRequestHandler<GetRsmChecklistDetailsByClientIdQuery, ApiResponse>
        {
            private readonly IRsmChecklistService _context;

            public GetRsmChecklistDetailsByClientIdQueryHandler(IRsmChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetRsmChecklistDetailsByClientIdQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetRsmChecklistDetailsByClientId(request.clientId, request.ip, request.userId);
                return reaponse;
            }
        }
    }
}

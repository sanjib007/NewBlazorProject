using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetReasonForOutageQuery : IRequest<ApiResponse>
    {
        public long closingNatureId { get; set; }
        public string ip { get; set; }
        public string UserId { get; set; }
        public class GetReasonForOutageQueryHandler : IRequestHandler<GetReasonForOutageQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public GetReasonForOutageQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetReasonForOutageQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetReasonForOutage(request.closingNatureId, request.UserId, request.ip);
                return reaponse;
            }
        }
    }
}

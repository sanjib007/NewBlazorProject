using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetSupportDelayReasonQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string UserId { get; set; }
        public class GetSupportDelayReasonQueryHandler : IRequestHandler<GetSupportDelayReasonQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public GetSupportDelayReasonQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetSupportDelayReasonQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetSupportDealyReason(request.UserId, request.ip);
                return reaponse;
            }
        }
    }
}

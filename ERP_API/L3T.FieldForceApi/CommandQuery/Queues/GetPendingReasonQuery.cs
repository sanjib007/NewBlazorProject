using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetPendingReasonQuery : IRequest<ApiResponse>
    {
        public string categoryId { get; set; }
        public string ip { get; set; }
        public class GetPendingReasonQueryHandler : IRequestHandler<GetPendingReasonQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GetPendingReasonQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetPendingReasonQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetPendingReasonList(request.categoryId, request.ip);
                return reaponse;
            }
        }
    }
}

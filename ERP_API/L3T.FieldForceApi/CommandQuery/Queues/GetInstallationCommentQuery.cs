using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetInstallationCommentQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GetInstallationCommentQueryHandler : IRequestHandler<GetInstallationCommentQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GetInstallationCommentQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetInstallationCommentQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetInstallationCommentData(request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

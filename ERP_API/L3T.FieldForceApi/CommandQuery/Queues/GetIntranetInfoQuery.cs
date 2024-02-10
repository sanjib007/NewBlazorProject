using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetIntranetInfoQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GetIntranetInfoQueryHandler : IRequestHandler<GetIntranetInfoQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GetIntranetInfoQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetIntranetInfoQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetIntranetInfoData(request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

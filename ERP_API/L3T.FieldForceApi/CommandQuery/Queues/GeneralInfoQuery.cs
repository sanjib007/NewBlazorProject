using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GeneralInfoQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GeneralInfoQueryHandler : IRequestHandler<GeneralInfoQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GeneralInfoQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GeneralInfoQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetGenaralInfoData(request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

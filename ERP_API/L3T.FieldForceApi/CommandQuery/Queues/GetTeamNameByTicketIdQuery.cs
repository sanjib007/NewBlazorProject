using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
 
    public class GetTeamNameByTicketIdQuery : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GetTeamNameByTicketIdQueryHandler : IRequestHandler<GetTeamNameByTicketIdQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GetTeamNameByTicketIdQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetTeamNameByTicketIdQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetAllTeamNameByTicketId(request.userId,request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

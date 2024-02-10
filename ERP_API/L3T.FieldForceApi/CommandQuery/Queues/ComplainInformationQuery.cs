using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class ComplainInformationQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip {  get; set; }
        public class ComplainInformationQueryHandler : IRequestHandler<ComplainInformationQuery, ApiResponse>
        {
            private readonly IForwardTicketService _context;

            public ComplainInformationQueryHandler(IForwardTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(ComplainInformationQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.ComplainInformation(request.ticketId, request.user, request.ip);
                return reaponse;
            }
        }
    }
}

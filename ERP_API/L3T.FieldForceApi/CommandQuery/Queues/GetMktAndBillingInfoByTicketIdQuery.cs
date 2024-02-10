using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{

    public class GetMktAndBillingInfoByTicketIdQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GetMktAndBillingInfoByTicketIdQueryHandler : IRequestHandler<GetMktAndBillingInfoByTicketIdQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GetMktAndBillingInfoByTicketIdQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetMktAndBillingInfoByTicketIdQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetMktAndBillingInfoByTicketId(request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

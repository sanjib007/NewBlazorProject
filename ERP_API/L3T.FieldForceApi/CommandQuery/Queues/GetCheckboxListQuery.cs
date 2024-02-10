
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
 
    public class GetCheckboxListQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GetCheckboxListQueryHandler : IRequestHandler<GetCheckboxListQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public GetCheckboxListQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetCheckboxListQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetServiceCheckboxListData(request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

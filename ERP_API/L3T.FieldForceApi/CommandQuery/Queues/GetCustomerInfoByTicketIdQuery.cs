using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetCustomerInfoByTicketIdQuery : IRequest<ApiResponse>
    {
        public string ticketId { get; set; }
        public string ip { get; set; }
        public class GetCustomerInfoByTicketIdQueryHandler : IRequestHandler<GetCustomerInfoByTicketIdQuery, ApiResponse>
        {
            private readonly IChecklistService _context;

            public GetCustomerInfoByTicketIdQueryHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetCustomerInfoByTicketIdQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetCustomerInfoByTicketId(request.ticketId, request.ip);
                return reaponse;
            }
        }
    }
}

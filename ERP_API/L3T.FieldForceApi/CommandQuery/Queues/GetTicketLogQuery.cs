using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class GetTicketLogQuery : IRequest<ApiResponse>
    {
        public string TicketId { get; set; }
        public string UserId { get; set; }
        public string Ip { get; set; }
        public class GetTicketLogQueryHandler : IRequestHandler<GetTicketLogQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public GetTicketLogQueryHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(GetTicketLogQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.GetTicketLog(request.TicketId, request.UserId, request.Ip);
                return reaponse;
            }
        }
    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class EmployeeTicketPriorityQuery : IRequest<ApiResponse>
    {
        public string ip { get; set; }
        public string userId { get; set; }
        public string ticketId { get; set; }

        public class EmployeeTicketPriorityHandler : IRequestHandler<EmployeeTicketPriorityQuery, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public EmployeeTicketPriorityHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(EmployeeTicketPriorityQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.GetEmployeeTicketPriorityInfo(request.ip, request.userId, request.ticketId);
                return response;
            }
        }
    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class UpdateTicketPriorityStatusCommand : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public string ticketId { get; set; } = string.Empty;
        public int priorityStatus { get; set; }
        public int pendingListSlNo { get; set; }
        public string serviceType { get; set; }

        public class UpdateTicketPriorityStatusHandler : IRequestHandler<UpdateTicketPriorityStatusCommand, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public UpdateTicketPriorityStatusHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(UpdateTicketPriorityStatusCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.UpdateTicketPriorityStatusByTcketId(request.userId, request.ip, request.ticketId,
                    request.priorityStatus, request.pendingListSlNo, request.serviceType);
                return response;
            }
        }
    }
}

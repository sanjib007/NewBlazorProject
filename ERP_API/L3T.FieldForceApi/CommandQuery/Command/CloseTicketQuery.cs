using L3T.FieldForceApi.CommandQuery.Queues;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class CloseTicketCommand : IRequest<ApiResponse>
    {
        public RSMCloseTicketRequestModel model { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }
        public class CloseTicketCommandHandler : IRequestHandler<CloseTicketCommand, ApiResponse>
        {
            private readonly IRSMComplainTicketService _rSMComplainTicketService;

            public CloseTicketCommandHandler(IRSMComplainTicketService rSMComplainTicketService)
            {
                _rSMComplainTicketService = rSMComplainTicketService;
            }

            public async Task<ApiResponse> Handle(CloseTicketCommand request, CancellationToken cancellationToken)
            {
                var response = await _rSMComplainTicketService.CloseTicket(request.model, request.userId, request.ip);
                return response;
            }
        }
    }
}

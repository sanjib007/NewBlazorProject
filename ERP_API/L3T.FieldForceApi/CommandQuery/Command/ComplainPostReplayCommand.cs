using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class ComplainPostReplayCommand : IRequest<ApiResponse>
    {
        public PostReplayRequestModel model { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }

        public class ComplainPostReplayCommandHandler : IRequestHandler<ComplainPostReplayCommand, ApiResponse>
        {
            private readonly IRSMComplainTicketService _rSMComplainTicketService;

            public ComplainPostReplayCommandHandler(IRSMComplainTicketService rSMComplainTicketService)
            {
                _rSMComplainTicketService = rSMComplainTicketService;
            }

            public async Task<ApiResponse> Handle(ComplainPostReplayCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = await _rSMComplainTicketService.UpdateRSMComplainPostReplay(request.model, request.userId, request.ip);
                return response;
            }
        }
    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class ForwardTicketCommand : IRequest<ApiResponse>
    {
        public ForwardTicketRequestModel model { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class ForwardTicketCommandHandler : IRequestHandler<ForwardTicketCommand, ApiResponse>
        {
            private readonly IForwardTicketService _context;

            public ForwardTicketCommandHandler(IForwardTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(ForwardTicketCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.ForwardTicket(request.model, request.user, request.ip);
                return reaponse;
            }
        }
    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class ResolvedTicketCommand : IRequest<ApiResponse>
    {
        public ResolvedTicketRequestModel model { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public PushNotificationRequestModel AssignedPersonsEmployee { get; internal set; }

        public class ResolvedTicketCommandHandler : IRequestHandler<ResolvedTicketCommand, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public ResolvedTicketCommandHandler(IFieldForceService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(ResolvedTicketCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.ResolvedTicket(request.model, request.user, request.ip);
                return reaponse;
            }
        }
    }
}

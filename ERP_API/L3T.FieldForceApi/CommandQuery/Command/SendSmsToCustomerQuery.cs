using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
  
    public class SendSmsToCustomerQuery : IRequest<ApiResponse>
    {
        public SendSmsRequestModel model { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class SendSmsToCustomerQueryHandler : IRequestHandler<SendSmsToCustomerQuery, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public SendSmsToCustomerQueryHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(SendSmsToCustomerQuery request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.SendSmsText(request.model, request.user, request.ip);
                return reaponse;
            }
        }
    }
}

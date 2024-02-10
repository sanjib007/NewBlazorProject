using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{

    public class RsmInstallationUpdateColorCommand : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public UpdateColorRequestModel model { get; set; }

        public class RsmInstallationUpdateColorCommandHandler : IRequestHandler<RsmInstallationUpdateColorCommand, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public RsmInstallationUpdateColorCommandHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(RsmInstallationUpdateColorCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.UpdateColorData(request.userId, request.ip, request.model);
                return response;
            }
        }
    }
}

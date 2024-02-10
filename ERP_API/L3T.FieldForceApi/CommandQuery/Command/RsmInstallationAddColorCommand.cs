using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class RsmInstallationAddColorCommand : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public AddColorRequestModel model { get; set; }

        public class RsmInstallationAddColorCommandHandler : IRequestHandler<RsmInstallationAddColorCommand, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public RsmInstallationAddColorCommandHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(RsmInstallationAddColorCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.AddColorData(request.userId, request.ip, request.model);
                return response;
            }
        }
    }
}

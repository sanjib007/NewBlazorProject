using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class InstallationSheduleUpdateCommand : IRequest<ApiResponse>
    {
        public string userId { get; set; }
        public string ip { get; set; }
        public InstallationSheduleUpdateRequestModel model { get; set; }

        public class InstallationSheduleUpdateCommandHandler : IRequestHandler<InstallationSheduleUpdateCommand, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public InstallationSheduleUpdateCommandHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(InstallationSheduleUpdateCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.SheduleDataUpdate(request.userId, request.ip, request.model);
                return response;
            }
        }
    }
}

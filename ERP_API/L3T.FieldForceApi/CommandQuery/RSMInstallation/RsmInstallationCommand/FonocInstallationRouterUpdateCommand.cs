using L3T.FieldForceApi.CommandQuery.Command;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationCommand
{
    public class FonocInstallationRouterUpdateCommand : IRequest<ApiResponse>
    {
        public RSMfonocRouterUpdateRequestModel model { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }
        public class FonocInstallationRouterUpdateCommandHandler : IRequestHandler<FonocInstallationRouterUpdateCommand, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public FonocInstallationRouterUpdateCommandHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(FonocInstallationRouterUpdateCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.UpdateFonocRouterInfo(request.model,request.userId, request.ip);
                return response;
            }
        }
    }
}

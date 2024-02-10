using L3T.FieldForceApi.CommandQuery.Command;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationCommand
{
    public class FonocInstallationUpdateCommand : IRequest<ApiResponse>
    {
        public RSMfonocUpdateRequestModel model { get; set; }
        public string userId { get; set; }
        public string ip { get; set; }
        public class FonocInstallationUpdateCommandHandler : IRequestHandler<FonocInstallationUpdateCommand, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public FonocInstallationUpdateCommandHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(FonocInstallationUpdateCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.UpdateFonocInfo(request.model,request.userId, request.ip);
                return response;
            }
        }
    }
}

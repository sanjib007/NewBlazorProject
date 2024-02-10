using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{

    public class UpdateGeneralInfoCommand : IRequest<ApiResponse>
    {
        public GeneralInfoUpdateModel model { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class UpdateGeneralInfoCommandHandler : IRequestHandler<UpdateGeneralInfoCommand, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public UpdateGeneralInfoCommandHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(UpdateGeneralInfoCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.UpdateGeneralInfoData(request.model, request.user, request.ip);
                return reaponse;
            }
        }
    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class DoneGeneralInfoCommand : IRequest<ApiResponse>
    {
        public GeneralInfoDoneRequestModel model { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class DoneGeneralInfoCommandHandler : IRequestHandler<DoneGeneralInfoCommand, ApiResponse>
        {
            private readonly IInstallationTicketService _context;

            public DoneGeneralInfoCommandHandler(IInstallationTicketService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(DoneGeneralInfoCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.DoneGeneralInfoData(request.model,request.user, request.ip);
                return reaponse;
            }
        }
    }
}

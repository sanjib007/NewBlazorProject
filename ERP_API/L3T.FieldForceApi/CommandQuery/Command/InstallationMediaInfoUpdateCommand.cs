using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class InstallationMediaInfoUpdateCommand : IRequest<ApiResponse>
    {
        public MediaInformationRequestModel model { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class InstallationMediaInfoUpdateCommandHandler : IRequestHandler<InstallationMediaInfoUpdateCommand, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public InstallationMediaInfoUpdateCommandHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(InstallationMediaInfoUpdateCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.UpdateMediaInfoDetails(request.model, request.user, request.ip);
                return response;
            }
        }
    }
}

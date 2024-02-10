using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class SupportofficeDataUpdateCommand : IRequest<ApiResponse>
    {
        public SupportOfficeInfoRequestModel model { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class SupportofficeDataUpdateCommandHandler : IRequestHandler<SupportofficeDataUpdateCommand, ApiResponse>
        {
            private readonly IFieldForceService _context;

            public SupportofficeDataUpdateCommandHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(SupportofficeDataUpdateCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.SupportofficeDataUpdateInClientDatabase(request.model, request.user, request.ip);
                return reaponse;
            }
        }
    }
}

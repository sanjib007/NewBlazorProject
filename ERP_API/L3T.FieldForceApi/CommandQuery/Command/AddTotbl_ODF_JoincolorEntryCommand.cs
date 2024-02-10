using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class AddTotbl_ODF_JoincolorEntryCommand : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public tbl_ODF_JoincolorEntryRequestModel model { get; set; }

        public class AddTotbl_ODF_JoincolorEntryHandler : IRequestHandler<AddTotbl_ODF_JoincolorEntryCommand, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public AddTotbl_ODF_JoincolorEntryHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(AddTotbl_ODF_JoincolorEntryCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.AddTotbl_ODF_JoincolorEntry(request.ip, request.user, request.model);
                return response;
            }
        }
    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class AddToODF_JoincolorEntryCommand : IRequest<ApiResponse>
    {
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public tbl_Splitter_JoincolorEntryRequestModel model { get; set; }

        public class AddToODF_JoincolorEntryHandler : IRequestHandler<AddToODF_JoincolorEntryCommand, ApiResponse>
        {
            private readonly IFieldForceService _context;
            public AddToODF_JoincolorEntryHandler(IFieldForceService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(AddToODF_JoincolorEntryCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.AddToODF_JoincolorEntryCommand(request.ip, request.user, request.model);
                return response;
            }
        }
    }
}

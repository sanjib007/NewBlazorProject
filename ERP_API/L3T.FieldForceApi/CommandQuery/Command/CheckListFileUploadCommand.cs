using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class CheckListFileUploadCommand : IRequest<ApiResponse>
    {
        public FileUploadModel model { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class CheckListFileUploadCommandHandler : IRequestHandler<CheckListFileUploadCommand, ApiResponse>
        {
            private readonly IChecklistService _context;

            public CheckListFileUploadCommandHandler(IChecklistService context)
            {
                _context = context;
            }
            public async Task<ApiResponse> Handle(CheckListFileUploadCommand request, CancellationToken cancellationToken)
            {
                var reaponse = await _context.UploadAndSaveChecklistFile(request.model, request.user, request.ip);
                return reaponse;
            }
        }
    }
}

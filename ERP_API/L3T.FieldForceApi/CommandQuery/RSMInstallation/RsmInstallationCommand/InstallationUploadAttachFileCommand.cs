using L3T.FieldForceApi.CommandQuery.Command;
using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.RSMInstallation.RsmInstallationCommand
{
    public class InstallationUploadAttachFileCommand : IRequest<ApiResponse>
    {
        public RSMInstallationFileUploadRequestModel model { get; set; }
       
        public string userId { get; set; }
        public string ip { get; set; }
        public class InstallationUploadAttachFileCommandHandler : IRequestHandler<InstallationUploadAttachFileCommand, ApiResponse>
        {
            private readonly IRsmInstallationTicketService _context;

            public InstallationUploadAttachFileCommandHandler(IRsmInstallationTicketService context)
            {
                _context = context;
            }

            public async Task<ApiResponse> Handle(InstallationUploadAttachFileCommand request, CancellationToken cancellationToken)
            {
                var response = await _context.InstallationFileUpload(request.model, request.userId, request.ip);
                return response;
            }
        }
    }
}

using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Queues
{
    public class InstallationUploadedFileRealatedQuery : IRequest<ApiResponse>
    {
        public string tki_number { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }

        public class InstallationUploadedFileRealatedQueryHandler : IRequestHandler<InstallationUploadedFileRealatedQuery, ApiResponse>
        {
            private readonly IInstallationTicketFileUploadService _serviceContext;
            public InstallationUploadedFileRealatedQueryHandler(IInstallationTicketFileUploadService serviceContext)
            {
                _serviceContext = serviceContext;
            }

            public Task<ApiResponse> Handle(InstallationUploadedFileRealatedQuery request, CancellationToken cancellationToken)
            {
                var response = _serviceContext.GetMisInstallationUploadedFileList(request.tki_number);
                return response;
            }
        }
    }
}

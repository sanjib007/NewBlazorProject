using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using MediatR;
using System.Security.Claims;

namespace L3T.FieldForceApi.CommandQuery.Command
{
    public class InstallationFileUploadCommand : IRequest<ApiResponse>
    {
        public FileUploadModel fileDetails { get; set; }
        public ClaimsPrincipal user { get; set; }
        public string ip { get; set; }
        public string file_header { get; set; }
        public string cli_code { get; set; }
        public int code_sl { get; set; }
        public string file_categry_tki_number { get; set; }
        public string upload_by { get; set; }
        public string ServiceID { get; set; }



        public class InstallationFileUploadCommandHandler : IRequestHandler<InstallationFileUploadCommand, ApiResponse>
        {
            private readonly IInstallationTicketFileUploadService _serviceContext;
            public InstallationFileUploadCommandHandler(IInstallationTicketFileUploadService serviceContext)
            {
                _serviceContext = serviceContext;
            }

            public async Task<ApiResponse> Handle(InstallationFileUploadCommand request,
                //string file_header,
                //string cli_code, Int32 code_sl, string file_categry_tki_number, string upload_by,
                //string ServiceID,

                CancellationToken cancellationToken)
            {
                var result = await _serviceContext.UploadAndSaveInstallationFile(request.fileDetails,
                    request.file_header, request.cli_code, request.code_sl, request.file_categry_tki_number,
                    request.upload_by, request.ServiceID, request.user, request.ip);
                return result;
            }
        }


    }
}

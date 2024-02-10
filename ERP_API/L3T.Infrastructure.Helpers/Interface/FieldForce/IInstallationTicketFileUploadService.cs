using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using System.Security.Claims;

namespace L3T.Infrastructure.Helpers.Interface.FieldForce
{
    public interface IInstallationTicketFileUploadService
    {
        Task<ApiResponse> UploadAndSaveInstallationFile(FileUploadModel requestModel,
            string file_header,
            string cli_code, Int32 code_sl, string file_categry_tki_number, string upload_by,
            string ServiceID,
            ClaimsPrincipal user, string ip);

        Task<ApiResponse> GetMisInstallationUploadedFileList(string tki_number);
    }
}

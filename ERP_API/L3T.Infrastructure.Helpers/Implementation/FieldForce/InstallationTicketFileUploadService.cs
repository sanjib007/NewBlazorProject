using L3T.Infrastructure.Helpers.CommonModel;
using L3T.Infrastructure.Helpers.DataContext.FieldForceDBContex;
using L3T.Infrastructure.Helpers.Interface.Email;
using L3T.Infrastructure.Helpers.Interface.FieldForce;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace L3T.Infrastructure.Helpers.Implementation.FieldForce
{
    public class InstallationTicketFileUploadService : IInstallationTicketFileUploadService
    {
        private readonly MisDBContext _misDBContext;
        private readonly ILogger<InstallationTicketFileUploadService> _logger;
        private readonly IMailSenderService _mailSenderService;

        public InstallationTicketFileUploadService(MisDBContext misDBContext,
            ILogger<InstallationTicketFileUploadService> logger, IMailSenderService mailSenderService)
        {
            _misDBContext = misDBContext;
            _logger = logger;
            _mailSenderService = mailSenderService;
        }

        public async Task<ApiResponse> UploadAndSaveInstallationFile(FileUploadModel requestModel,
            string file_header, string cli_code, Int32 code_sl, string file_categry_tki_number, string upload_by,
            string ServiceID, ClaimsPrincipal user, string ip)
        {
            var methodName = "InstallationTicketFileUploadService/UploadAndSaveInstallationFile";

            try
            {
                // need to transfer file to 203.76.110.134 server as per web page
                //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FieldForceFiles");
                string path = Path.Combine(Directory.GetCurrentDirectory(), @"D:\\MIS\\PresaleAttachfile");
                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);


                //get file extension and info
                FileInfo fileInfo = new FileInfo(requestModel.FileDetails.FileName);
                string fileName = file_categry_tki_number + "_" + requestModel.FileDetails.FileName;
                string fileExtension = fileInfo.Extension;
                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    requestModel.FileDetails.CopyTo(stream);
                }

                // need to insert data in database

                //string strSql = "insert into [dbo].[clientDatabaseFile_Test] (cli_code, code_sl, file_header, file_categry, upload_by, "
                //    + " upload_datetime, uploaded_filename, saved_filename, ServiceID, file_type) values('" + cli_code + "', " + code_sl + ", "
                //    + " '" + file_header + "', '" + file_categry_tki_number + "', '" + upload_by + "', '" + DateTime.Now + "', "
                //    + " '" + requestModel.FileDetails.FileName + "', '" + fileName + "', '" + ServiceID + "', '" + fileExtension + "')";

                string strSql = "insert into [dbo].[clientDatabaseFile] (cli_code, code_sl, file_header, file_categry, upload_by, "
                    + " upload_datetime, uploaded_filename, saved_filename, ServiceID, file_type) values('" + cli_code + "', " + code_sl + ", "
                    + " '" + file_header + "', '" + file_categry_tki_number + "', '" + upload_by + "', '" + DateTime.Now + "', "
                    + " '" + requestModel.FileDetails.FileName + "', '" + fileName + "', '" + ServiceID + "', '" + fileExtension + "')";

                int items = await _misDBContext.Database.ExecuteSqlRawAsync(strSql);


                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "File Uploaded successfully.",
                    Data = null
                };

                return response;

            }
            catch (Exception ex)
            {
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetMisInstallationUploadedFileList(string tki_number)
        {
            var methodName = "InstallationTicketFileUploadService/GetMisInstallationUploadedFileList";
            try
            {
                string strSql = " select ServiceID,file_header,uploaded_filename,saved_filename from clientDatabaseFile "
                    + " where file_categry = '" + tki_number + "' and status='1' "
                    + " group by ServiceID,file_header,uploaded_filename,saved_filename";
                //var GetDataByTicketRefno = await _misDBContext.TblComplainInfo.FromSqlRaw(tickitComplainInfoQuery).FirstOrDefaultAsync();

                var result = await _misDBContext.ClientDatabaseFiles.FromSqlRaw(strSql).ToListAsync();

                var response = new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Get File data successfully.",
                    Data = result
                };

                return response;
            }
            catch (Exception ex)
            {
                await _mailSenderService.ExceptionSendMail(ex.ToString(), methodName + " Error Message :" + ex.Message);
                throw new Exception(ex.Message);
            }


        }




    }
}

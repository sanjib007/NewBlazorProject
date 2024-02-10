using Microsoft.EntityFrameworkCore;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class clientDatabaseFileResponseModel
    {
        // ServiceID,file_header,uploaded_filename,saved_filename

        //public long? unique_id { get; set; }
        //public string? cli_code { get; set; }
        //public int? code_sl { get; set; }
        public string? file_header { get; set; }
        //public string? file_categry { get; set; }
        public string? ServiceID { get; set; }
        //public string? upload_by { get; set; }
        //public DateTime? upload_datetime { get; set; }
        //public string? file_type { get; set; }
        //public Int32? file_length { get; set; }
        public string? uploaded_filename { get; set; }
        public string? saved_filename { get; set; }
        //public string? uploaded_path { get; set; }
        //public string? download_link { get; set; }
        //public string? file_status { get; set; }
        //public Int32? status { get; set; }

    }
}

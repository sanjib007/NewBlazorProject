using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class Tbl_ClientDatabaseFile
    {
        [Key]
        public int ID { get; set; }
        public string? RefNO { get; set; }
        public string? SubscriberID { get; set; }
        public string? File_Type { get; set; }
        public string? File_Name { get; set; }
        public string? Saved_Filename { get; set; }
        public string? UploadBy { get; set; }
        public DateTime? UploadDate { get; set; }
        public int? status { get; set; }
    }
}

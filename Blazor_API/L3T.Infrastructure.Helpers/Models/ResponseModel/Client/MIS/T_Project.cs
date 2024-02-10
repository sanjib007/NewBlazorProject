using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS
{
   
    public class T_Project
    {
        [Key]
        public string project_Id { get; set; }
        public string? Project_Type { get; set; }
        public string? project_Title { get; set; }
        public string? project_category { get; set; }
        public string? project_cat { get; set; }
        public string? Team_id { get; set; }
        public string? project_Description { get; set; }
        public DateTime? project_StartTime { get; set; }
        public DateTime? project_EstimateTime { get; set; }
        public string? project_Client_ID { get; set; }
        public string? project_Status { get; set; }
        public string? comments { get; set; }
        public DateTime? projectActualCloseTime { get; set; }
        public string? Team_Leader { get; set; }
    }
}

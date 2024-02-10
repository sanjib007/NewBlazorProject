using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class DistributorInfoModel
    {
        [Key]
        public string brCliCode { get; set; }
        public string DistributorSubscriberID {  get; set; }
        public string DistributorName { get; set; }
        public string MqID { get; set; }
        public string CommisionRate { get; set; }
        public string DistributorID { get; set; }
    }
}

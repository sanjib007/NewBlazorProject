using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class GeneralInfoResponseModel
    {
        [NotMapped]
        public List<SupportOfficeModel>? supportOfficeList { get; set; }
        [NotMapped]
        public List<WireSetupModel>? wireSetupList { get; set; }
        [NotMapped]
        public List<TechnologySetupModel>? technologySetupList { get; set; }
        [NotMapped]
        public List<MediaSetupModel>? mediaSetupList { get; set; }

        public string? brDns1 { get; set; }
        public string? brDns2 { get; set; }
        public string? brsmtp { get; set; }
        public string? brpop3 { get; set; }
        public string? note_for_bts { get; set; }
    }
}

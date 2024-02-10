using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    [Keyless]
    public class FoNocResponseModel
    {
        public List<RouterBrandResponseModel> RouterBrands { get; set; }
        public int? RouterBrandsSelectedValue { get; set; }
        public List<RouterRebotSettingResponseModel> RouterRebotSettings { get; set; }
        public int? RouterRebotSettingsSelectedValue { get; set; }
        public List<RouterRebotTimeResponseModel> RouterRebotTimes { get; set; }
        public int? RouterRebootTimesSelectedValue { get; set; }
        public FiberInfrastractureResponseModel FiberInfrastracture { get; set; }
        public bool? FiberInfrastractureIsVisible { get; set; }
        public string? CustomerMac { get; set; }
        public string? OnuMac { get; set; }
        public string? OnuPort { get; set; }
        public string? OnuId { get; set; }  
        public int? RouteModelSelectedValue { get; set; }
    }
}

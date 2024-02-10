using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    public class AddColorInfoResponseModel
    {
        [Key]
        public string? SplitterLocation { get; set; }
        public List<CableTypeResponseModel> CableTypes { get; set; }
        public List<TubeColorResponseModel> TubeColor { get; set; }
        public List<TubeColorResponseModel> CoreColor { get; set; }
        public List<ViewColorResponseModel> ViewColors { get; set; }
    }
}

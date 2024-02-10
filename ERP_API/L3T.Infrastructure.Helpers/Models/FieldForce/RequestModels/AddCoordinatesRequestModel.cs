using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class AddCoordinatesRequestModel
    {
        public string DviceID {  get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Uid { get; set; }
    }
}

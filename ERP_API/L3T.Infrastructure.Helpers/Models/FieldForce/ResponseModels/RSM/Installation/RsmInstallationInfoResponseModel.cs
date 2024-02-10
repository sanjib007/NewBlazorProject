using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    [Keyless]
    public class RsmInstallationInfoResponseModel
    {
        public SalesPersonInfoResponseModel SalesPersonInfo { get; set; }
        public PersonalDetailsResponseModel PersonalDetails { get; set; }
        public ContactDetailsResponseModel ContactDetails { get; set; }
        public ConnectivityAddressResponseModel ConnectivityAddress { get; set; }
        public NewFormatAddressResponseModel NewFormatAddress { get; set; }
        public CableNetworkResponseModel CableNetwork { get; set; }
        public FiberMediaDetailsP2MResponseModel FiberMediaDetails { get; set; }
        public ConnectivityDetailsResponseModel ConnectivityDetails { get; set; }
        public FoNocResponseModel FoNoc { get; set; }
        public AddCommentsResponseModel AddComments { get; set; }   
        public List<UpdateHistroryResponseModel> UpdateHistrory { get; set; }
    }
}

using L3T.Infrastructure.Helpers.Models.RequestModel.Client;
using L3T.Infrastructure.Helpers.Models.ResponseModel;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client.RSM;
using L3T.Infrastructure.Helpers.Repositories.Implementation.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Repositories.Interface.Client
{
    public interface IMISCustomerInformationRepository
    {
        Task<List<MisCustomerInfoResModel>> CustomerPhoneNumber(string CustomerCode);
        Task<List<MisCustomerInfoResModel>> CustomerCode(string mobileNumber);
        Task<List<RsmCustomerInfoResModel>> RSMCustomerPhoneNumber(string CustomerCode);
        


    }
}

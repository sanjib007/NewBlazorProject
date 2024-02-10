using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.RequestModel.Client;
using L3T.Infrastructure.Helpers.Models.ResponseModel;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client;
using L3T.Infrastructure.Helpers.Models.ResponseModel.Client.RSM;
using L3T.Infrastructure.Helpers.Repositories.Interface.Client;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Repositories.Implementation.Client
{
    public class MISCustomerInformationRepository: IMISCustomerInformationRepository
    {
        private readonly MisDBContext _misDBContext;
        private readonly RsmDBContext _rsmcontext;

        public MISCustomerInformationRepository(MisDBContext misLiveDBContext, RsmDBContext rsmcontext)
        {
            _misDBContext = misLiveDBContext;
            _rsmcontext = rsmcontext;
        }

        public async Task<List<MisCustomerInfoResModel>> CustomerCode(string mobileNumber)
        {
            string sql = @$"select phone_no, brCliCode,brSlNo from [WFA2].[dbo].clientDatabaseMain WITH (NOLOCK) where phone_no LIKE  '%{mobileNumber}%'";
            var getResult = await _misDBContext.MisCustomerInfoResModel.FromSqlRaw(sql).ToListAsync();
            return getResult;
        }

        public async Task<List<MisCustomerInfoResModel>> CustomerPhoneNumber(string CustomerCode)
        {             
            string sql = @$"select DISTINCT brCliCode,phone_no,brSlNo from [WFA2].[dbo].clientDatabaseMain WITH (NOLOCK) where brCliCode = '{CustomerCode}'";
            var getResult = await _misDBContext.MisCustomerInfoResModel.FromSqlRaw(sql).ToListAsync();
            return getResult;            
        }


        public async Task<List<RsmCustomerInfoResModel>> RSMCustomerPhoneNumber(string CustomerCode)
        {
            string sql = @$"select DISTINCT brCliCode,phone_no,brSlNo from clientDatabaseMain WITH (NOLOCK) where brCliCode = '{CustomerCode}'";
            var getResult = await _rsmcontext.RsmCustomerInfoResModel.FromSqlRaw(sql).ToListAsync();
            return getResult;
        }




    }
}

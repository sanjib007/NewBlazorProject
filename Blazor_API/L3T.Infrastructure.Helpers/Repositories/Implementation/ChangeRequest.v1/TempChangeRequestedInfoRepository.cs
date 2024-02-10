using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Repositories.Interface;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Repositories.Implementation.ChangeRequest.v1
{
    public class TempChangeRequestedInfoRepository : BaseRepository<TempChangeRequestedInfo>, ITempChangeRequestedInfoRepository
    {
        public TempChangeRequestedInfoRepository(ChangeRequestDataContext dbContext) : base(dbContext)
        {

        }
    }
}

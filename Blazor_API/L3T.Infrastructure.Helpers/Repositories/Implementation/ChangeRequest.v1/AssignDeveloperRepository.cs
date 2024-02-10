﻿using L3T.Infrastructure.Helpers.DataContext;
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
    public class AssignDeveloperRepository : BaseRepository<DeveloperInformation>, IAssignDeveloperRepository
    {
        public AssignDeveloperRepository(ChangeRequestDataContext dbContext) : base(dbContext)
        {

        }
    }
}

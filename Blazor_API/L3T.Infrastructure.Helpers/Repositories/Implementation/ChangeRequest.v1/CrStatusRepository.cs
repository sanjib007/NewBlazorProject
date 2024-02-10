﻿using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Repositories.Implementation.ChangeRequest.v1
{
    public class CrStatusRepository : BaseRepository<CrStatus>, ICrStatusRepository
    {
        public CrStatusRepository(ChangeRequestDataContext dbContext) : base(dbContext)
        {
         
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class tbl_CasueOfDelayModel
    {
        [Key]
        public int ID { get; set; }
        public string CauseOfDelay { get; set; }
    }
}

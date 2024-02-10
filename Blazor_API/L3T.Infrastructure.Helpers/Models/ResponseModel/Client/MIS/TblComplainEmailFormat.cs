﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.ResponseModel.Client.MIS
{
    
    public class TblComplainEmailFormat
    {
        [Key]
        public string? CTID { get; set; }
        public string? Mailfrom { get; set; }
        public string? Mailto { get; set; }
        public string? MailCC { get; set; }
        public string? MailBcc { get; set; }
        public string? MailSubject { get; set; }
        public string? MailBody { get; set; }
        public string? Status { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    [Keyless]
    public class CheckListB2BResponseModel
    {       
        public int? Id { get; set; }
        public string? CheckList { get; set; }
        public string? ddlname { get; set; }
    }
}

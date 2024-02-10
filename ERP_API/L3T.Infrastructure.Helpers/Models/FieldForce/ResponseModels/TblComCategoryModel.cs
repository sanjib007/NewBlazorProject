using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class TblComCategoryModel
    {
        [Key]
        public decimal C_id { get; set; }
        public string Com_Category { get; set;}
    }
}

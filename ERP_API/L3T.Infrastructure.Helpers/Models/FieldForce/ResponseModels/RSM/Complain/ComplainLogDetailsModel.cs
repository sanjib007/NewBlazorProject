using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class ComplainLogDetailsModel
    {
        [Key]
        public DateTime CommentsDate { get; set; }
        public string CommentsBy {  get; set; }
        public string DegDepCell { get; set; }
        public string Comments { get; set; }
    }
}

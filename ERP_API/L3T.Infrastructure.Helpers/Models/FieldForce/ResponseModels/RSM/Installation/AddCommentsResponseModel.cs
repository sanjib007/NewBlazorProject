using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Installation
{
    [Keyless]
    public class AddCommentsResponseModel
    {
        public List<PendingReasonResponseModel> pendingReasons { get; set; }
        [NotMapped]
        public int? SelectedId { get; set; }
    }
}

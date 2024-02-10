using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Location
{
    public class Upazila
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? DistrictId { get; set; }
        public string UpazilaName { get; set; }

        #region Navigation Property

        [ForeignKey("DistrictId")]
        public virtual District District { get; set; } = null!;
        #endregion
    }
}

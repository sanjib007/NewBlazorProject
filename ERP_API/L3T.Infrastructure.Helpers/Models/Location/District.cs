using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Location
{
    public class District
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int DivisionId { get; set; }
        public string DistrictName { get; set; }

        #region Navigation Property

        [ForeignKey("DivisionId")]
        public virtual Division Division { get; set; } = null!;

        public virtual ICollection<Upazila> Upazila { get; set; }
        #endregion
    }
}

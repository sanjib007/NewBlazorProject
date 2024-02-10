using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.BTS
{
    public class RouterType
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string TypeName { get; set; }

        #region Audit field
        public string InsertedBy { get; set; }
        public DateTime InsertedDateTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        #endregion

        #region Navigation Property
        public long? BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public Brand Brand{ get; set; }
        public Router Router { get; set; }
        #endregion

    }
}

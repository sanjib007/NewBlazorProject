using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L3T.Infrastructure.Helpers.Models.BTS
{
    [Table("BTSType")]
    public class BTSType
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

        #endregion
    }
}

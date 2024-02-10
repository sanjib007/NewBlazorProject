using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.BTS
{
    public class Router
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public string IP { get; set; }
        public string SerialNumber { get; set; }
        //public string Model { get; set; }
        public string Remarks { get; set; }

        #region Audit field
        public string InsertedBy { get; set; }
        public DateTime InsertedDateTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        #endregion

        #region Navigation Property
        public long? BtsInfoId { get; set; }
        [ForeignKey(nameof(BtsInfoId))]
        public BtsInfo Bts { get; set; }
        public long? RouterTypeId { get; set; }
        [ForeignKey(nameof(RouterTypeId))]
        public RouterType RouterType { get; set; }

        public ICollection<Switch> Switch { get; set; }
        #endregion
    }
}

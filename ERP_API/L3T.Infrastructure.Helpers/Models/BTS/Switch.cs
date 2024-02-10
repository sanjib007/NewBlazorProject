using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Routing;

namespace L3T.Infrastructure.Helpers.Models.BTS
{
    public class Switch
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public string IP { get; set; }
        public int NumberofPort { get; set; }
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
        //public long? RouterId { get; set; }
        //[ForeignKey(nameof(RouterId))]
        //public Router Router { get; set; }
        //public long SwitchTypeId { get; set; }
        //[ForeignKey(nameof(SwitchTypeId))]
        //public SwitchType SwitchType { get; set; }
        #endregion
    }
}

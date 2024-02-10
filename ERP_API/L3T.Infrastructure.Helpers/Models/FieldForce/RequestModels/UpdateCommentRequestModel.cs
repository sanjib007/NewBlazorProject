using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class UpdateCommentRequestModel
    {
        [Key]
        public string TicketRefNo { get; set; }
        public string ReasonId { get; set; }
        public string CommentText { get; set; }
        public List<string> CheckboxList { get; set; }
        public string Mail { get; set; }
        public string AdditionalMail { get; set; }
        public DateTime CommentDate { get; set; }
    }
}

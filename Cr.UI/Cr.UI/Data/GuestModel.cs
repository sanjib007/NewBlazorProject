using System.ComponentModel.DataAnnotations;

namespace Cr.UI.Data
{
    public class GuestModel
    {
        public long Id { get; set; }
        [Required]
        public string name { get; set; }
        public string address { get; set; }
    }
}

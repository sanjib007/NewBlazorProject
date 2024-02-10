using System.ComponentModel.DataAnnotations;

namespace Cr.UI.Data
{
    public class AddAssignEmployeeReq
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "Employee Id is required")]
        public string EmpId { get; set; }

        [Required(ErrorMessage = "Employee Name is required")]
        public string EmpName { get; set; }

        [Required(ErrorMessage = "CR Id is required")]
        public long CrId { get; set; }

        [Required(ErrorMessage = "Start date Id is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Total working day is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a total working day bigger than 0")]
        public int TotalDay { get; set; }
        public string? Task { get; set; }
    }
}

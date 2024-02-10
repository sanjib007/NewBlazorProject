namespace Cr.UI.Data
{
    public class NotificationDetailsResponseModel : AuditableEntity
    {
        public long CrId { get; set; }
        public String ApproverEmpId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? Image { get; set; }
        public string? NotifyURL { get; set; }
        public string? Type { get; set; }
        public bool IsRead { get; set; }
        public bool IsActive { get; set; }
    }
}

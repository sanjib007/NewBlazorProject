namespace L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels
{
    public class PushNotificationRequestModel
    {
        public string? TicketId { get; set; }
        public string? Clientname { get; set; }
        public string? SubjectOrSuspectedCase { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public string[]? AssignedPersonsEmployeeId { get; set; }
        public string? RequiestedByUserId { get; set; }
    }
}

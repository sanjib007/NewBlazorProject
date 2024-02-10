using FluentValidation;

namespace L3TIdentityOAuth2Server.DataAccess.RequestModel
{
    public class PushNotificationRequestModel
    {
        public string TicketId { get; set; }
        public string Clientname { get; set; }
        public string? SubjectOrSuspectedCase { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public string[] AssignedPersonsEmployeeId { get; set; }
        public string? RequiestedByUserId { get; set; }

    }

    public class PushNotificationRequestModelValidator : AbstractValidator<PushNotificationRequestModel>
    {
        public PushNotificationRequestModelValidator()
        {
            RuleFor(x => x.TicketId).NotEmpty().NotNull().WithMessage("Request Must have a Ticket Id.!");
            RuleFor(x => x.SubjectOrSuspectedCase).NotEmpty().NotNull().WithMessage("Request Must have a Subject.!");
            RuleFor(x => x.Description).NotEmpty().NotNull().WithMessage("Request Must have a Description.!");
            RuleFor(x => x.RequiestedByUserId).NotEmpty().NotNull().WithMessage("Request Must have a RequiestedByUserId.!");
            //RuleFor(x => x.AssignedPersonsEmployeeId).NotNull().WithMessage("Request Must have an Assigned Person.!");
            //RuleFor(x => x.TicketId).NotEmpty().NotNull().WithMessage("Request Must have a Ticket Id.!");
            //RuleFor(x => x.Codes).Must(x => x == null || x.Any());
            RuleFor(x => x.AssignedPersonsEmployeeId).Must(x => x == null || x.Any());
        }
    }



}

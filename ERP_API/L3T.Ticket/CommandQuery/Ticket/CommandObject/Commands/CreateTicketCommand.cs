using L3T.Infrastructure.Helpers.Interface.Ticket;
using L3T.Infrastructure.Helpers.Models.TicketEntity;
using MediatR;

namespace L3T.Ticket.CommandQuery.Ticket.CommandObject.Commands
{
    public class CreateTicketCommand : IRequest<long>
    {
        public TicketEntry TicketEntry { get; set; }
        public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, long>
        {
            private readonly ITicketService _context;
            public CreateTicketCommandHandler(ITicketService context)
            {
                _context = context;
            }
            public async Task<long> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
            {
                var id=  await _context.TicketEntrySubmitAsync(command.TicketEntry);
                return id;
            }
        }
    }
}
